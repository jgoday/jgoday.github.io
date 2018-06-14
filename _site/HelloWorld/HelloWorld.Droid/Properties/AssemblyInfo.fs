namespace HelloWorld.Droid

open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open Android.App

[<assembly: AssemblyTitle("HelloWorld.Droid")>]
[<assembly: AssemblyDescription("")>]
[<assembly: AssemblyConfiguration("")>]
[<assembly: AssemblyCompany("")>]
[<assembly: AssemblyProduct("HelloWorld.Droid")>]
[<assembly: AssemblyCopyright("Copyright ©  2018")>]
[<assembly: AssemblyTrademark("")>]
[<assembly: AssemblyCulture("")>]
[<assembly: ComVisible(false)>]

[<assembly: AssemblyVersion("1.0.0.0")>]
[<assembly: AssemblyFileVersion("1.0.0.0")>]

// Android permissions
[<assembly: UsesPermission(Android.Manifest.Permission.Internet)>]
[<assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)>]

do ()