---
layout: post
title:  "Xamarin Forms with F#: Part 2 (WPF)"
date:   2018-05-28 22:32:52
categories: [xamarin.forms, f#]
---

The last post, [Hello world GTK](http://jgoday.github.io/xamarin.forms/f%23/2018/05/21/xamarin_forms.html), was about writting a GTK application with Xamarin.Forms and F# on linux.

Now, it's time for the WPF Backend, without any tooling support.

I tested it with Visual Studio 2017 Community Preview ('Desktop Development with .NET' workload installed).

![Image of Desktop Development with .NET](/assets/dotNetDesktopDevelopment.png)

I have used the following aliases in PowerShell
```bash

Set-Alias nuget ~/Apps/nuget.exe
Set-Alias msbuild 'C:\Program Files (x86)\Microsoft Visual Studio\Preview\Community\MSBuild\15.0\Bin\MSBuild'

```
The full code is on github [https://github.com/jgoday/xamarinForms-FSharpTemplate](https://github.com/jgoday/xamarinForms-FSharpTemplate).

The main problem here, is the lack of **'Partial classes'** in FSharp, to define the logic and the xaml presentation declaration in different files (the XAML definition and the 'Code Behind' file).
So you have to load the xaml manually (without x:Class attribute) and make the bindings yourself,
or use something like FsXaml [F# Tools for working with XAML Projects](https://github.com/fsprojects/FsXaml).


### Load Xaml files

To load manually a Xaml file:
  * The Xaml file must be marked as '**Embed resource**' in the fsproj ile.
  * **System.Xml.XmlReader** and **System.Windows.Markup.XamlReader** will be used to load the Xaml file and casted (:?> 'a) to a WPF control type.
  * You can reference the Xaml file directly or use '[Pack URIs](https://docs.microsoft.com/en-us/dotnet/framework/wpf/app-development/pack-uris-in-wpf)' (if the Xaml file is in another assembly)

```fsharp
let loadXaml<'a>(path: string) =
    let streamInfo = Application.GetResourceStream(new Uri(path, UriKind.RelativeOrAbsolute))
    use stream = streamInfo.Stream
    use xaml = XmlReader.Create(stream)
    XamlReader.Load(xaml) :?> 'a

let mainWindow = loadXaml<System.Windows.Window>(new Uri("pack://application:,,,/MainWindow.xaml"))
// or simply
let mainWindow = loadXaml<System.Windows.Window>(new Uri("MainWindow.xaml"))
```



### Application lifecycle events

**FsXaml** takes advantage of F# type providers in compile time, to load the markup definition and generate a base class that you can extend later to customize his behaviour.

For instance, If you want to control the application lifecycle events.

* Define the event and the handler in the Xaml file

```fsharp
<Application xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             Startup="AppStart">
    <Application.Resources>
        <ResourceDictionary>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

*  Use FsXaml

```fsharp
namespace HelloWorld.WPF

open System
open System.Windows

open FsXaml
type AppBase = XAML<"App.xaml">

type App() =
    inherit AppBase()

    member this.AppStart (sender: Object) (args: StartupEventArgs) : unit =
        printfn "Application start: %A !!" args

```

### WPF and F# Files

* Program.fs

```fsharp
open System
open System.Windows

[<STAThread>]
[<EntryPoint>]
let main _ =
    let app = XamlUtils.loadXaml<Application>("App.xaml")
    let mainWindow = new HelloWorld.WPF.MainWindow()
    app.Run(mainWindow)
```
<div class="code-comment">
Load the WPF application and main window.
</div>


* App.xaml

```xml
<Application xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <ResourceDictionary>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```
<div class="code-comment">
WPF application common resources and lifecycle events.
</div>

* App.xaml.fs

```fsharp
namespace HelloWorld.WPF

open System.Windows

type App() =
    inherit Application()
```
<div class="code-comment">
This code begind class is not really needed (in fact, is not even used if you load the App.xaml manually), unless you want to control application events (start, finish,...), in this case, you have to use FsXaml.
</div>

* MainWindow.xaml

```xml
<wpf:FormsApplicationPage
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:wpf="clr-namespace:Xamarin.Forms.Platform.WPF;assembly=Xamarin.Forms.Platform.WPF"
        mc:Ignorable="d"
        WindowState="Normal">
</wpf:FormsApplicationPage>
```
<div class="code-comment">
</div>

* MainWindow.xaml.fs

```fsharp
namespace HelloWorld.WPF

open Xamarin.Forms.Platform.WPF

open FsXaml
type MainWindowBase = XAML<"MainWindow.xaml">

type MainWindow() =
    inherit MainWindowBase()

    do
        Xamarin.Forms.Forms.Init()
        base.LoadApplication(new HelloWorld.App())
```
<div class="code-comment">
The WPF mainWindow will load the common app after initialize Xamarin.Forms.
</div>

* Compile and run it.

```bash
cd HelloWorld\HelloWorld
dotnet build
cd ..\HelloWorld\HelloWorld.WPF
nuget restore -PackagesDirectory ..\packages
msbuild /t:Build
bin\Debug\HelloWorld.WPF.exe
```