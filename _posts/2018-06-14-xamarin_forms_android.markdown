---
layout: post
title:  "Xamarin Forms with F#: Part 3 (Android)"
date:   2018-06-14 22:40:52
categories: [xamarin.forms, f#]
---

In this post, I will test an <strong>Android</strong> <strong>Xamarin.Forms</strong> application with <strong>F#</strong> in <strong>linux</strong>.

In theory there is no official <strong>Xamarin</strong> support for this platform,
but there are some prebuilt packages published for some distros.

After install **xamarin-android** package, you can use **MSBuild** to compile, generate and sign Android apks.

It will install both *Xamarin.Android.FSharp.targets* and *Xamarin.Android.CSharp.targets*.

See [https://github.com/0xFireball/xamarin-android-linux](https://github.com/0xFireball/xamarin-android-linux) for more information.

So, first step is download the prebuilt packages:

# Install xamarin android on linux

* Download prebuilt releases
from
[xamarin-android-linux](https://jenkins.mono-project.com/view/Xamarin.Android/job/xamarin-android-linux/lastSuccessfulBuild/Azure/).

    ```bash
    sudo dpkg -i xamarin.android-oss_8.3.99.189_amd64.deb

    /usr/lib/xamarin.android/
    ```

* Configure Android SDK paths.

    ```bash

    export ANDROID_SDK_PATH=$HOME/Android/Sdk
    export ANDROID_NDK_PATH=$HOME/Android/Sdk/ndk-bundle

    ```

# Define Xamarin Android project
* Xamarin Android will run with [mono](https://www.mono-project.com/), so we cannot use <i>dotnet cli</i> to generate the project, but the <strong>[MSBuild](https://msdn.microsoft.com/en-us/library/dd576348.aspx)</strong> definition is pretty simple.

    Let's follow the <i>Project Configuration</i> step by step:

1. Project Root with **'Build'** as default target.
    ```xml
    <Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003" DefaultTargets="Build">
    </Project>
    ```
2. F# support.
    ```xml
    <PropertyGroup>
        <FSharpTargetsPath>$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)\FSharp\Microsoft.FSharp.Targets</FSharpTargetsPath>
    </PropertyGroup>
    <Import Project="$(FSharpTargetsPath)" />
    ```
3. Program sources. Remember that order matters in F#.
    ```xml
    <ItemGroup>
        <Compile Include="Program.fs" />
        <Compile Include="Properties\AssemblyInfo.fs" />
    </ItemGroup>
    ```
4. Add a configuration file to store versions in **../Configuration/Versions.xml**.
    ```xml
    <Project>
        <PropertyGroup>
            <XamarinFormsVersion>3.1.0.530888-pre2</XamarinFormsVersion>
            <AndroidSupportVersion>26.1.0.1</AndroidSupportVersion>
        </PropertyGroup>
    </Project>
    ```
5. Import version numbers in the **Android** project.
    ```xml
    <Import Project="../Configuration/Versions.xml" />
    ```
6. Add **Xamarin** dependencies.
    ```xml
    <ItemGroup>
        <PackageReference Include="Xamarin.Android.Support.Design" Version="$(AndroidSupportVersion)" />
        <PackageReference Include="Xamarin.Android.Support.v4" Version="$(AndroidSupportVersion)" />
        <PackageReference Include="Xamarin.Android.Support.v7.AppCompat" Version="$(AndroidSupportVersion)" />
        <PackageReference Include="Xamarin.Android.Support.v7.CardView" Version="$(AndroidSupportVersion)" />
        <PackageReference Include="Xamarin.Android.Support.v7.MediaRouter" Version="$(AndroidSupportVersion)" />
        <PackageReference Include="Xamarin.Forms" Version="$(XamarinFormsVersion)" />
    </ItemGroup>
    ```
7. System & Framework dependencies.
    ```xml
    <ItemGroup>
        <Reference Include="Mono.Android" />
        <Reference Include="System" />
        <Reference Include="System.Core" />
        <Reference Include="System.ObjectModel" />
        <Reference Include="System.Xml.Linq" />
        <Reference Include="System.Xml" />
    </ItemGroup>
    ```
8. Reference to common **Xamarin.Forms** project (The &lt;Project&gt; guid is optional).
    ```xml
    <ItemGroup>
        <ProjectReference Include="..\HelloWorld\HelloWorld.fsproj">
            <Project>{F2A71F9B-5D33-465A-A702-920D77279786}</Project>
            <Name>HelloWorld</Name>
        </ProjectReference>
    </ItemGroup>
    ```
9. Import **Android** Targets.
    ```xml
    <Import Project="$(MSBuildExtensionsPath)\Xamarin\Android\Xamarin.Android.FSharp.targets" />
    ```
10. Common project properties (debug or release).
    ```xml
    <PropertyGroup>
        <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
        <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
        <ProductVersion>8.0.30703</ProductVersion>
        <SchemaVersion>2.0</SchemaVersion>
        <ProjectGuid>{42F9F6D1-9DF3-42BC-84FC-1A593A2D4EC9}</ProjectGuid>
        <ProjectTypeGuids>{EFBA0AD7-5A72-4C68-AF49-83D382785DCF};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
        <OutputType>Library</OutputType>
        <AppDesignerFolder>Properties</AppDesignerFolder>
        <RootNamespace>HelloWorld.Droid</RootNamespace>
        <AssemblyName>HelloWorld.Android</AssemblyName>
        <FileAlignment>512</FileAlignment>
        <AndroidApplication>true</AndroidApplication>
        <AndroidResgenFile>Resources\Resource.Designer.cs</AndroidResgenFile>
        <AndroidManifest>Properties\AndroidManifest.xml</AndroidManifest>
        <AndroidUseLatestPlatformSdk>true</AndroidUseLatestPlatformSdk>
        <TargetFrameworkVersion>v8.1</TargetFrameworkVersion>
        <JavaMaximumHeapSize />
        <NuGetPackageImportStamp />
    </PropertyGroup>
    ```
11. Debug properties (I deliverately omit some configurations like Release ...).
    ```xml
    <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
        <DebugSymbols>true</DebugSymbols>
        <DebugType>full</DebugType>
        <Optimize>false</Optimize>
        <OutputPath>bin\Debug\</OutputPath>
        <DefineConstants>DEBUG;TRACE</DefineConstants>
        <ErrorReport>prompt</ErrorReport>
        <WarningLevel>4</WarningLevel>
        <AndroidUseSharedRuntime>True</AndroidUseSharedRuntime>
        <AndroidLinkMode>None</AndroidLinkMode>
    </PropertyGroup>
    ```
12. **Android** Resources (drawable icons, Manifest and styles).
    ```xml
    <ItemGroup>
        <AndroidResource Include="Resources\drawable\icon.png" />
        <AndroidResource Include="Resources\drawable-hdpi\icon.png" />
        <AndroidResource Include="Resources\drawable-xhdpi\icon.png" />
        <AndroidResource Include="Resources\drawable-xxhdpi\icon.png" />
    </ItemGroup>
    <ItemGroup>
        <None Include="Properties\AndroidManifest.xml" />
    </ItemGroup>
    <ItemGroup>
        <AndroidResource Include="Resources\layout\Tabbar.axml" />
        <AndroidResource Include="Resources\layout\Toolbar.axml" />
        <AndroidResource Include="Resources\values\styles.xml" />
    </ItemGroup>
    ```
13. Complete Project definition (HelloWorld.Droid.fsproj).
    ```xml
    <Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003" DefaultTargets="Build">
        <Import Project="../Configuration/Versions.xml" />
        <!-- Common properties -->
        <PropertyGroup>
            <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
            <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
            <ProductVersion>8.0.30703</ProductVersion>
            <SchemaVersion>2.0</SchemaVersion>
            <ProjectGuid>{42F9F6D1-9DF3-42BC-84FC-1A593A2D4EC9}</ProjectGuid>
            <ProjectTypeGuids>{EFBA0AD7-5A72-4C68-AF49-83D382785DCF};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
            <OutputType>Library</OutputType>
            <AppDesignerFolder>Properties</AppDesignerFolder>
            <RootNamespace>HelloWorld.Droid</RootNamespace>
            <AssemblyName>HelloWorld.Droid</AssemblyName>
            <FileAlignment>512</FileAlignment>
            <AndroidApplication>true</AndroidApplication>
            <AndroidResgenFile>Resources\Resource.Designer.cs</AndroidResgenFile>
            <AndroidManifest>Properties\AndroidManifest.xml</AndroidManifest>
            <AndroidUseLatestPlatformSdk>true</AndroidUseLatestPlatformSdk>
            <TargetFrameworkVersion>v8.1</TargetFrameworkVersion>
            <JavaMaximumHeapSize />
            <NuGetPackageImportStamp />
        </PropertyGroup>
        <!-- DEBUG properties -->
        <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
            <DebugSymbols>true</DebugSymbols>
            <DebugType>full</DebugType>
            <Optimize>false</Optimize>
            <OutputPath>bin\Debug\</OutputPath>
            <DefineConstants>DEBUG;TRACE</DefineConstants>
            <ErrorReport>prompt</ErrorReport>
            <WarningLevel>4</WarningLevel>
            <AndroidUseSharedRuntime>True</AndroidUseSharedRuntime>
            <AndroidLinkMode>None</AndroidLinkMode>
        </PropertyGroup>

        <!-- SOURCES -->
        <ItemGroup>
            <Compile Include="MainActivity.fs" />
            <Compile Include="Properties\AssemblyInfo.fs" />
        </ItemGroup>

        <!-- RESOURCES -->
        <ItemGroup>
            <AndroidResource Include="Resources\drawable\icon.png" />
        </ItemGroup>
        <ItemGroup>
            <None Include="Properties\AndroidManifest.xml" />
        </ItemGroup>
        <ItemGroup>
            <AndroidResource Include="Resources\layout\Tabbar.axml" />
            <AndroidResource Include="Resources\layout\Toolbar.axml" />
            <AndroidResource Include="Resources\values\styles.xml" />
        </ItemGroup>

        <!-- common xamarin forms project -->
        <ItemGroup>
            <ProjectReference Include="..\HelloWorld\HelloWorld.fsproj">
            <Project>{F2A71F9B-5D33-465A-A702-920D77279786}</Project>
            <Name>HelloWorld</Name>
            </ProjectReference>
        </ItemGroup>

        <!-- dependencies -->
        <ItemGroup>
            <PackageReference Include="Xamarin.Android.Support.Design" Version="$(AndroidSupportVersion)" />
            <PackageReference Include="Xamarin.Android.Support.v4" Version="$(AndroidSupportVersion)" />
            <PackageReference Include="Xamarin.Android.Support.v7.AppCompat" Version="$(AndroidSupportVersion)" />
            <PackageReference Include="Xamarin.Android.Support.v7.CardView" Version="$(AndroidSupportVersion)" />
            <PackageReference Include="Xamarin.Android.Support.v7.MediaRouter" Version="$(AndroidSupportVersion)" />
            <PackageReference Include="Xamarin.Forms" Version="$(XamarinFormsVersion)" />
        </ItemGroup>
        <ItemGroup>
            <Reference Include="Mono.Android" />
            <Reference Include="System" />
            <Reference Include="System.Core" />
            <Reference Include="System.Numerics" />
            <Reference Include="System.ObjectModel" />
            <Reference Include="System.Xml.Linq" />
            <Reference Include="System.Xml" />
        </ItemGroup>

        <!-- imports -->
        <Import Project="$(MSBuildExtensionsPath)\Xamarin\Android\Xamarin.Android.FSharp.targets" />
    </Project>
    ```

# Sources

You can check full source code [here](https://github.com/jgoday/xamarinForms-FSharpTemplate).

1. **Properties/AssemblyInfo.fs**

    Contains attributes about the program and Android permissions.

    [How to write an assembly info in fsharp](http://mike-ward.net/2012/05/26/learning-fndashassembly-level-attributes/).

    ```fsharp
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
    ```
2. **MainActivity.fs**
    ```fsharp
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
            base.OnCreate(bundle)
            Xamarin.Forms.Forms.Init(x, bundle)
            let app = new App()
            x.LoadApplication(app)

    ```
3. **Properties/AndroidManifest.xml**
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <manifest xmlns:android="http://schemas.android.com/apk/res/android">
        <uses-sdk android:minSdkVersion="15" />
        <application android:label="HelloWorld.Droid"></application>
    </manifest>
    ```
4. **Resources/values/styles.xml**
    ```xml
    <?xml version="1.0" encoding="utf-8" ?>
    <resources>

    <style name="MainTheme" parent="MainTheme.Base">
    </style>
    <!-- Base theme applied no matter what API -->
    <style name="MainTheme.Base" parent="Theme.AppCompat.Light.DarkActionBar">
        <!--If you are using revision 22.1 please use just windowNoTitle. Without android:-->
        <item name="windowNoTitle">true</item>
        <!--We will be using the toolbar so no need to show ActionBar-->
        <item name="windowActionBar">false</item>
        <!-- Set theme colors from http://www.google.com/design/spec/style/color.html#color-color-palette -->
        <!-- colorPrimary is used for the default action bar background -->
        <item name="colorPrimary">#2196F3</item>
        <!-- colorPrimaryDark is used for the status bar -->
        <item name="colorPrimaryDark">#1976D2</item>
        <!-- colorAccent is used as the default value for colorControlActivated
            which is used to tint widgets -->
        <item name="colorAccent">#FF4081</item>
        <!-- You can also set colorControlNormal, colorControlActivated
            colorControlHighlight and colorSwitchThumbNormal. -->
        <item name="windowActionModeOverlay">true</item>

        <item name="android:datePickerDialogTheme">@style/AppCompatDialogStyle</item>
    </style>

    <style name="AppCompatDialogStyle" parent="Theme.AppCompat.Light.Dialog">
        <item name="colorAccent">#FF4081</item>
    </style>
    </resources>
    ```

# Compile and test
1. Restore project dependencies.
```bash
nuget restore HelloWorld.Droid.fsproj
```
2. Build it.
```bash
msbuild /t:Build
```
3. Build **Android APK file**.
```bash
msbuild /t:BuildApk
```
4. Sign **Android APK**.
```bash
msbuild /t:SignAndroidPackage
```
5. Install it on a device or emulator.
```bash
adb install -r bin/Debug/HelloWorld.Droid-Signed.apk
```