module XamlUtils

open System
open System.Windows
open System.Windows.Markup
open System.Xml

let loadXaml<'a>(name: string) =
    let uri = new Uri(name, UriKind.Relative)
    Application.LoadComponent(uri) :?> 'a

let loadXaml'<'a>(path: string) =
    let streamInfo = Application.GetResourceStream(new Uri(path, UriKind.RelativeOrAbsolute))
    use stream = streamInfo.Stream
    use xaml = XmlReader.Create(stream)
    XamlReader.Load(xaml) :?> 'a