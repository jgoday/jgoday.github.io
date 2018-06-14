* Download nuget from https://www.nuget.org/downloads


* msbuild
from visual studio preview 2017 (15)
& 'C:\Program Files (x86)\Microsoft Visual Studio\Preview\Community\MSBuild\15.0\Bin\MSBuild'

* Alias
Set-Alias nuget D:\Apps\nuget.exe
Set-Alias msbuild 'C:\Program Files (x86)\Microsoft Visual Studio\Preview\Community\MSBuild\15.0\Bin\MSBuild'

* Restore packages
nuget restore  -PackagesDirectory ..\packages

* en windows visual -> tener instalado desarrollo WPF && .net sdk


* xaml
** FsXaml
** Manually

//        let mainWindow = loadXamlFromSource<Window>("pack://application:,,,/MainWindow.xaml")
        // let app = new HelloWorld.WPF.App()


    with e ->
        printfn "%A" e.StackTrace
        0
    // https://docs.microsoft.com/en-us/dotnet/framework/wpf/app-development/pack-uris-in-wpf
    //let mainWindow = loadXaml(new Uri("pack://application:,,,/MainWindow.xaml"))
    //app.Run(mainWindow)