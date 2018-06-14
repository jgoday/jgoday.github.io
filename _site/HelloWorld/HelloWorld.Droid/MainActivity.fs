namespace HelloWorld.Droid

open System
open System.Diagnostics
open Android.App
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.OS

open HelloWorld

[<Activity(Label = "HelloWorld", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true)>]
type MainActivity() =
    inherit Xamarin.Forms.Platform.Android.FormsAppCompatActivity()

    override x.OnCreate (bundle: Bundle) =

        // Xamarin.Forms.Platform.Android.FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar
        // Xamarin.Forms.Platform.Android.FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar
        Debug.WriteLine "## XFF: A"
        base.OnCreate(bundle)
        Debug.WriteLine "## XFF: B"
        Xamarin.Forms.Forms.Init(x, bundle)
        let app = new App()
        Debug.WriteLine "## XFF: C"
        if app.MainPage = null then
            Debug.WriteLine "## XFF: MAINPAGE IS NULL"
        else
            Debug.WriteLine "## XFF: MAINPAGE IS NOT NULL"
        x.LoadApplication(app)
        Debug.WriteLine "## XFF: D"
