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
