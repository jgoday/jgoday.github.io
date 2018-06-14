open System
open System.Windows


[<STAThread>]
[<EntryPoint>]
let main _ =
    let app = new HelloWorld.WPF.App()
    let mainWindow = new HelloWorld.WPF.MainWindow()
    app.Run(mainWindow)