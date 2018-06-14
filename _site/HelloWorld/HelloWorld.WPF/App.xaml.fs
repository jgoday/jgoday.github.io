namespace HelloWorld.WPF

open System
open System.Windows

open FsXaml
type AppBase = XAML<"App.xaml">

type App() =
    inherit AppBase()

    member this.AppStart (sender: Object) (args: StartupEventArgs) : unit =
        printfn "Application start: %A !!" args