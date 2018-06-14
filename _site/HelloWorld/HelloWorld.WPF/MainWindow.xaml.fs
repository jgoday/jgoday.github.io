namespace HelloWorld.WPF

open Xamarin.Forms.Platform.WPF

open FsXaml
type MainWindowBase = XAML<"MainWindow.xaml">

type MainWindow() =
    inherit MainWindowBase()

    do
        Xamarin.Forms.Forms.Init()
        base.LoadApplication(new HelloWorld.App())