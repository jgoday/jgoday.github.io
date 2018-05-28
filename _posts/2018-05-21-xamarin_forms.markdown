---
layout: post
title:  "Xamarin Forms with F#: Part 1 (GTK)"
date:   2018-05-21 22:15:52
categories: [xamarin.forms, f#]
---

I'm very interested in the multiplatform possibilities that comes with Xamarin.Forms (v >= 3), specially the android, WPF and [GTK](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/platform/gtk?tabs=vswin) backends.

I really like [F#](https://fsharp.org/). His clean, concise and functional approach, make it a nice csharp alternative in dotnet.

The first problem that I found, was the lack of tooling support for *Xamarin.Forms* and *F#*, both in Linux and Windows.

In Windows, the Xamarin tooling is ok, with a downside. There is no template for creating a solution with *Xamarin.Forms* in F#.

In Linux, there is no Xamarin tooling support at all. So you have to manually create the solution, and build it with **msbuild**.

I will focus here in creating a solution with *F#* and a *GTK Backend* in Linux.

Here are the tools, versions and steps:

  * **dotnet** (2.1.300-preview1-008174)
  * **mono** (5.12)
  * **gtk-sharp** 2.12


<div class="clearfix"></div>


### Solution
* **Create the solution**

```
cd /tmp
mkdir HelloWorld
cd HelloWorld
dotnet new sln
```

+ **Create the common netstandard Xamarin.Forms project**

```
dotnet new classlib -lang f# -o HelloWorld
```
+ **Add the Xamarin.Forms project to the solution**

```bash
dotnet sln add HelloWorld
```
+ **Create the GTK application**

```
dotnet new console -lang f# -o HelloWorld.GTK
```
+ **Add the GTK application to the solution**

```bash
dotnet sln add HelloWorld.GTK
```

### Configure the xamarin.forms common project

* **Add Xamarin.Forms dependency**

```bash
cd HelloWorld
dotnet add package Xamarin.Forms
```
* **Create App xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Application xmlns="http://xamarin.com/schemas/2014/forms"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            x:Class="HelloWorld.App">
<Application.Resources>
  </Application.Resources>
</Application>
```
* **Create App.fs**

```fsharp
namespace Innt

open Xamarin.Forms
open Xamarin.Forms.Xaml

type App() =
    inherit Application(MainPage = MainPage())

    do base.LoadFromXaml(typeof<App>) |> ignore
```

* **Create the MainPage.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        x:Class="HelloWorld.MainPage">
  <ContentPage.Content>
    <StackLayout Orientation="Vertical"
                 VerticalOptions="Center">
      <BoxView Color="Blue"
                WidthRequest="160"
                HeightRequest="160"
                VerticalOptions="Center"
                HorizontalOptions="Center" />
    </StackLayout>
  </ContentPage.Content>
</ContentPage>
```

* **Create the MainPage.fs**

```fsharp
namespace Innt

open Xamarin.Forms
open Xamarin.Forms.Xaml

type MainPage() =
    inherit ContentPage()

    do base.LoadFromXaml(typeof<MainPage>) |> ignore
```

* **Configure the project**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="MainView.fs" />
    <Compile Include="App.fs" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Xamarin.Forms" Version="3.0.0.482510" />
  </ItemGroup>
</Project>
```

* **Build it (you can use dotnet cli or msbuild since it's a netstandard project)**

```
# with dotnet cli
dotnet build
# with msbuild
msbuild /t:Build
```
<div class="code-comment">
you can use dotnet cli or msbuild since it's a netstandard project
</div>

### Create the Gtk application

* **Change the project definition**

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project DefaultTargets="Build" ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
      <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
      <Platform Condition=" '$(Platform)' == 'x86' "></Platform>
      <OutputType>Exe</OutputType>
      <RootNamespace>Innt</RootNamespace>
      <AssemblyName>Innt.Gtk</AssemblyName>
      <TargetFrameworkVersion>v4.7</TargetFrameworkVersion>
      <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
      <ProjectGuid>{0076E36A-1998-48A5-9F45-2BDE5920204F}</ProjectGuid>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|x86' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug</OutputPath>
    <DefineConstants>DEBUG;</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <PlatformTarget>x86</PlatformTarget>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="Program.fs" />
  </ItemGroup>

  <PropertyGroup>
      <FSharpTargetsPath>$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)\FSharp\Microsoft.FSharp.Targets</FSharpTargetsPath>
  </PropertyGroup>
  <Import Project="$(FSharpTargetsPath)" />
</Project>
```
<div class="code-comment">
  The GTK application must be compiled and executed with <strong>mono</strong>, so we have to change the project definition. You can create a <strong>fsharp</strong> project with <strong>monodevelop</strong> or paste/change the following xml.
</div>

* **Add nuget dependencies and restore them**

```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Xamarin.Forms" version="3.0.0.482510" targetFramework="net461" />
  <package id="Xamarin.Forms.Platform.GTK" version="3.0.0.482510" targetFramework="net461" />
</packages>
```
<div class="code-comment">
Create the nuget <strong>packages.config</strong> (in HelloWorld.GTK directory)
</div>

```
cd HelloWorld.GTK
nuget restore
```
<div class="code-comment">
Restore the nuget packages
</div>

* **Add a reference to the Xamarin.Forms common project**

```xml
<ItemGroup>
  <ProjectReference Include="..\HelloWorld\HelloWorld.fsproj">
    <Name>HelloWorld</Name>
  </ProjectReference>
</ItemGroup>
```
<div class="code-comment">
Paste the following <i>ItemGroup</i> to <strong>HelloWorld.GTK.fsproj</strong>
</div>

* **Add the dependencies to the project definition (HelloWorld.GTK.fsproj)**

```xml
<ItemGroup>
  <Reference Include="mscorlib" />
  <Reference Include="System" />
  <Reference Include="/usr/lib/mono/4.5/FSharp.Core.dll" />
  <Reference Include="gtk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f" />
  <Reference Include="glib-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f">
    <SpecificVersion>False</SpecificVersion>
  </Reference>
  <Reference Include="atk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f">
    <SpecificVersion>False</SpecificVersion>
  </Reference>
  <Reference Include="gdk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f">
    <SpecificVersion>False</SpecificVersion>
  </Reference>
  <Reference Include="Xamarin.Forms.Core.Design">
    <HintPath>.\packages\Xamarin.Forms.3.0.0.482510\lib\netstandard2.0\Design\Xamarin.Forms.Core.Design.dll</HintPath>
  </Reference>
  <Reference Include="Xamarin.Forms.Xaml.Design">
    <HintPath>.\packages\Xamarin.Forms.3.0.0.482510\lib\netstandard2.0\Design\Xamarin.Forms.Xaml.Design.dll</HintPath>
  </Reference>
  <Reference Include="Xamarin.Forms.Core">
    <HintPath>.\packages\Xamarin.Forms.3.0.0.482510\lib\netstandard2.0\Xamarin.Forms.Core.dll</HintPath>
  </Reference>
  <Reference Include="Xamarin.Forms.Platform">
    <HintPath>.\packages\Xamarin.Forms.3.0.0.482510\lib\netstandard2.0\Xamarin.Forms.Platform.dll</HintPath>
  </Reference>
  <Reference Include="Xamarin.Forms.Xaml">
    <HintPath>.\packages\Xamarin.Forms.3.0.0.482510\lib\netstandard2.0\Xamarin.Forms.Xaml.dll</HintPath>
  </Reference>
  <Reference Include="Xamarin.Forms.Platform.GTK">
    <HintPath>.\packages\Xamarin.Forms.Platform.GTK.3.0.0.482510\lib\net45\Xamarin.Forms.Platform.GTK.dll</HintPath>
  </Reference>
</ItemGroup>
```

* **Program.fs**

```fsharp
  open System
  open Xamarin.Forms
  open Xamarin.Forms.Platform.GTK

  [<EntryPoint>]
  [<STAThread>]
  let main argv =
    Gtk.Application.Init()
    Forms.Init()

    let app = new Innt.App()
    let window = new FormsWindow()
    window.LoadApplication(app)
    window.SetApplicationTitle("Sample")
    window.Show()

    Gtk.Application.Run()
    0
```

* **Build it and test it**

```
msbuild /t:Build /p:Platform="x86"
mono bin/Debug/HelloWorld.Gtk.exe
```
