namespace HelloWorld

open Xamarin.Forms
open Xamarin.Forms.Xaml

type App() =
    inherit Application(MainPage = MainPage())

    do base.LoadFromXaml(typeof<App>) |> ignore
    let hello name =
        printfn "Hello %s" name
