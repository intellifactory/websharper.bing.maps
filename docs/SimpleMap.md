# A Simple Map

Creating a map typically involves 3 steps:

  * Creating a container, an `Html` element for embedding the map object.

  * Initializing the map and attaching it to the container.

  * Setting properties, wiring events and adding shapes or controls to the
    attached map.

To initialize the map you need to supply your bing map key. More information on
how to retrieve a Bing Maps key is found at http://msdn.microsoft.com/en-us/library/ff428642.aspx.


The following example creates and initializes a simple map:

```fsharp
[<JavaScript>]
let MyCredentials = "Your Bing Maps Key"

[<JavaScript>]
let Test (f) =
    Div []
    |>! OnAfterRender (fun el ->
        let options =
            MapOptions(
                Credentials = MyCredentials,
                Width = 400,
                Height = 400
            )
        let map = Map(el.Body, options)
        f map
    )

    [<JavaScript>]
    let MapElement () =
    Test (fun map ->
         map.SetMapType(MapTypeId.Birdseye))
```

It is necessary to initialize the map using the `OnAfterRender` method since the
Bing Maps API requires that the container element is attached to the DOM before
the map is initialized. Doing the Map initialization after the widget rendering makes
sure that this condition holds.


![SimpleMap1](SimpleMap1.png)

The method `SetView` allows you to change the view parameters for the map. For example,
you can set the default location of the map by specifying a different latitude and
longitude. This is done through the `Location` class.

```fsharp 
[<JavaScript>]
let SetLocationProperties(map) =
    let location = Location(47.6, -122.33)
            location.Latitude <- 46.83
            let view = ViewOptions(Location = location,
                                   Zoom = 10,
                                   MapTypeId = Bing.MapTypeId.Birdseye)
    map.SetView(view)
```

This example shows how you can initialize the Location components directly from the
constructor or using mutation. This time we also used a 10x zoom and a `Birdseye` style
for the map.

Alternatively, you can also set view options when creating the map by passing a
`MapViewOptions` object to the map constructor. This object combines the properties of
`MapOptions` and `ViewOptions` for convenience.

![SimpleMap2](SimpleMap2.png)