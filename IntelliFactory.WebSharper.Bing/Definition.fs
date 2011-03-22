﻿namespace IntelliFactory.WebSharper.BingExtension

open IntelliFactory.WebSharper.Dom

module Bing =
    open IntelliFactory.WebSharper.InterfaceGenerator

    ///////////////////////////////////////////////////////////////////
    // Ajax API

    let AltitudeReference = Type.New()

    let AltitudeReferenceClass =
        Class "Microsoft.Maps.AltitudeReference"
        |=> AltitudeReference
        |+> [
                "ground" =? AltitudeReference
                |> WithComment "The altitude is measured from the ground level."

                "ellipsoid" =? AltitudeReference
                |> WithComment "The altitude is measured from the WGS 84 ellipsoid of the Earth."

                "isValid" => AltitudeReference ^-> T<bool>
                |> WithComment "Determines if the specified reference is a supported AltitudeReference."
            ]

    let Location = Type.New()
    
    let LocationClass =
        Class "Microsoft.Maps.Location"
        |=> Location
        |+> [
                Constructor (T<float> * T<float> * T<float> * AltitudeReference)
                Constructor (T<float> * T<float> * T<float>)
                Constructor (T<float> * T<float>)

                "areEqual" => Location * Location ^-> T<bool>
                |> WithComment "Determines if the specified Location objects are equal."

                "normalizeLongitude" => T<float -> float>
                |> WithComment "Normalizes the specified longitude so that it is between -180 and 180."
            ]
        |+> Protocol
            [
                "altitude" =? T<float>
                |> WithComment "The altitude of the location."

                "altitudeMode" =? AltitudeReference
                |> WithComment "The reference from which the altitude is measured."

                "latitude" =? T<float>
                |> WithComment "The latitude of the location."

                "longitude" =? T<float>
                |> WithComment "The longitude of the location."

                "clone" => T<unit> ^-> Location
                |> WithComment "Returns a copy of the Location object."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Location object to a string."
            ]

    let LocationRect = Type.New()

    let LocationRectClass =
        Class "Microsoft.Maps.LocationRect"
        |=> LocationRect
        |+> [
                Constructor (Location * T<float> * T<float>)

                "fromCorners" => Location * Location ^-> LocationRect
                |> WithComment "Returns a LocationRect using the specified locations for the northwest and southeast corners."

                "fromEdges" => T<float> * T<float> * T<float> * T<float> * T<float> * AltitudeReference ^-> LocationRect
                |> WithComment "Returns a LocationRect using the specified northern and southern latitudes and western and eastern longitudes for the rectangle boundaries."

                "fromLocations" => Type.ArrayOf Location ^-> LocationRect
                |> WithComment "Returns a LocationRect using an array of locations."

                "fromString" => T<string> ^-> LocationRect
                |> WithComment "Creates a LocationRect from a string with the following format: \"north,west,south,east\". North, west, south and east specify the coordinate number values."
            ]
        |+> Protocol [
                "center" =? Location
                |> WithComment "The location that defines the center of the rectangle."

                "height" =? T<float>
                |> WithComment "The height, in degrees, of the rectangle."

                "width" =? T<float>
                |> WithComment "The width, in degrees, of the rectangle."

                "clone" => T<unit> ^-> LocationRect
                |> WithComment "Returns a copy of the LocationRect object."

                "contains" => Location ^-> T<bool>
                |> WithComment "Returns whether the specified Location is within the LocationRect."

                "getEast" => T<unit -> float>
                |> WithComment "Returns the longitude that defines the eastern edge of the LocationRect."
                "getWest" => T<unit -> float>
                |> WithComment "Returns the latitude that defines the western edge of the LocationRect."
                "getNorth" => T<unit -> float>
                |> WithComment "Returns the latitude that defines the northern edge of the LocationRect."
                "getSouth" => T<unit -> float>
                |> WithComment "Returns the latitude that defines the southern edge of the LocationRect."

                "getSouthEast" => T<unit> ^-> Location
                |> WithComment "Returns the Location that defines the southeast corner of the LocationRect."
                "getNorthWest" => T<unit> ^-> Location
                |> WithComment "Returns the Location that defines the northwest corner of the LocationRect."

                "intersects" => LocationRect ^-> T<bool>
                |> WithComment "Returns whether the specified LocationRect intersects with this LocationRect."

                "toString" => T<unit -> string>
                |> WithComment "Converts the LocationRect object to a string."
            ]

    let Waypoint = Type.New()
    let Point = Type.New()

    let PointClass =
        Class "Microsoft.Maps.Point"
        |=> Point
        |+> [
                Constructor (T<float> * T<float>)

                "areEqual" => Point * Point ^-> T<bool>
                |> WithComment "Determines if the specified points are equal."

                "clonePoint" => Point ^-> Point
                |> WithSourceName "clone"
                |> WithComment "Returns a copy of the Point object."
            ]
        |+> Protocol
            [
                "x" =? T<float>
                |> WithComment "The x value of the coordinate."

                "y" =? T<float>
                |> WithComment "The y value of the coordinate."

                "clone" => T<unit> ^-> Point
                |> WithComment "Returns a copy of the Point object."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Point object into a string."

                "toUrlString" => T<unit -> string>
                |> WithInline "($this.x+','+$this.y)"
            ]

    let LabelOverlay = Type.New()

    let LabelOverlayClass =
        Class "Microsoft.Maps.LabelOverlay"
        |=> LabelOverlay
        |+> [
                "hidden" =? LabelOverlay
                |> WithComment "Map labels are not shown on top of imagery."

                "visible" =? LabelOverlay
                |> WithComment "Map labels are shown on top of imagery."

                "isValid" => LabelOverlay ^-> T<bool>
                |> WithComment "Determines whether the specified labelOverlay is a supported LabelOverlay."
            ]

    let ViewOptions = Type.New()
    let MapOptions = Type.New()
    let MapViewOptions = Type.New()

    
    let MapTypeId = Type.New()

    let MapTypeIdClass =
        Class "Microsoft.Maps.MapTypeId"
        |=> MapTypeId
        |+> [
                "mercator" =? MapTypeId
                "aerial" =? MapTypeId
                "auto" =? MapTypeId
                "birdseye" =? MapTypeId
                "collinsBart" =? MapTypeId
                "ordnanceSurvey" =? MapTypeId
                "road" =? MapTypeId
            ]

    let private ViewOptionsFields =
        [
            "animate", T<bool>
            "bounds", LocationRect
            "center", Location
            "centerOffset", Point
            "heading", T<float>
            "labelOverlay", LabelOverlay
            "mapTypeId", MapTypeId
            "padding", T<int>
            "zoom", T<int>
        ]
    
    let private MapOptionsFields =
        [
            "credentials", T<string>
            "disableKeyboardInput", T<bool>
            "disableMouseInput", T<bool>
            "disableTouchInput", T<bool>
            "disableUserInput", T<bool>
            "enableClickableLogo", T<bool>
            "enableSearchLogo", T<bool>
            "height", T<int>
            "showCopyright", T<bool>
            "showDashboard", T<bool>
            "showMapTypeSelector", T<bool>
            "showScalebar", T<bool>
            "width", T<int>
        ]

    let ViewOptionsClass =
        Pattern.Config "Microsoft.Maps.ViewOptions" {
            Required = []
            Optional = ViewOptionsFields
        }
        |=> ViewOptions

    let MapOptionsClass =
        Pattern.Config "Microsoft.Maps.MapOptions" {
            Required = []
            Optional = MapOptionsFields
        }
        |=> MapOptions

    let MapViewOptionsClass =
        Pattern.Config "Microsoft.Maps.MapViewOptions" {
            Required = []
            Optional = MapOptionsFields @ ViewOptionsFields
        }
        |=> MapViewOptions

    let Size = Type.New()

    let SizeClass =
        Pattern.Config "Microsoft.Maps.Size" {
            Required =
                [
                    "height", T<float>
                    "width", T<float>
                ]
            Optional = []
        }
        |=> Size





    let Entity = Type.New()

    let EntityInterface =
        Interface "Microsoft.Maps.Entity"
        |=> Entity

    let EntityCollectionOptions = Type.New()

    let EntityCollectionOptionsClass =
        Pattern.Config "Microsoft.Maps.EntityCollectionOptions" {
            Required = []
            Optional =
                [
                    "visible", T<bool>
                    "zIndex", T<int>
                ]
        }

    let EntityCollection = Type.New()

    let EntityCollectionClass =
        Class "Microsoft.Maps.EntityCollection"
        |=> EntityCollection
        |=> Implements [EntityInterface]
        |+> [
                Constructor T<unit>
                Constructor EntityCollectionOptions
            ]
        |+> Protocol
            [
                "clear" => T<unit -> unit>
                |> WithComment "Removes all entities from the collection."

                "get" => T<int> ^-> Entity
                |> WithComment "Returns the entity at the specified index in the collection."

                "getLength" => T<unit -> int>
                |> WithComment "Returns the number of entities in the collection."

                "getVisible" => T<unit -> bool>
                |> WithComment "Returns whether the entity collection is visible on the map."

                "getZIndex" => T<unit -> int>
                |> WithComment "Gets the z-index of the entity collection with respect to other items on the map."

                "indexOf" => Entity ^-> T<int>
                |> WithComment "Returns the index of the specified entity in the collection. If the entity is not found in the collection, -1 is returned."

                "insert" => Entity * T<int> ^-> T<unit>
                |> WithComment "Inserts the specified entity into the collection at the given index."

                "pop" => T<unit> ^-> Entity
                |> WithComment "Removes the last entity from the collection and returns it."

                "push" => Entity ^-> T<unit>
                |> WithComment "Adds the specified entity to the last position in the collection."

                "remove" => Entity ^-> Entity
                |> WithComment "Removes the specified entity from the collection and returns it."

                "removeAt" => T<int> ^-> Entity
                |> WithComment "Removes the entity at the specified index from the collection and returns it."

                "setOptions" => EntityCollectionOptions ^-> T<unit>
                |> WithComment "Sets the options for the entity collection."

                "toString" => T<unit -> string>
                |> WithComment "Converts the EntityCollection object to a string."
            ]

    let KeyEventArgs = Type.New()

    let KeyEventArgsClass =
        Class "Microsoft.Maps.KeyEventArgs"
        |=> KeyEventArgs
        |+> Protocol
            [
                "altKey" =? T<bool>
                |> WithComment "A boolean indicating if the ALT key was pressed."

                "ctrlKey" =? T<bool>
                |> WithComment "A boolean indicating if the CTRL key was pressed."

                "eventName" =? T<string>
                |> WithComment "The event that occurred."

                "handled" =% T<bool>
                |> WithComment "A boolean indicating whether the event is handled. If this property is set to true, the default map control behavior for the event is cancelled."

                "keyCode" =? T<string>
                |> WithComment "The code that identifies the keyboard key that was pressed."

                "originalEvent" =? T<obj>
                |> WithComment "The original browser event."

                "shiftKey" =? T<bool>
                |> WithComment "A boolean indicating if the SHIFT key was pressed."
            ]

    let MouseEventArgs = Type.New()

    let MouseEventArgsClass =
        Class "Microsoft.Maps.MouseEventArgs"
        |=> MouseEventArgs
        |+> Protocol
            [
                "eventName" =? T<string>
                |> WithComment "The event that occurred."

                "handled" =% T<bool>
                |> WithComment "A boolean indicating whether the event is handled. If this property is set to true, the default map control behavior for the event is cancelled."

                "isPrimary" =? T<bool>
                |> WithComment "A boolean indicating if the primary button (such as the left mouse button or a tap on a touch screen) was used."

                "isSecondary" =? T<bool>
                |> WithComment "A boolean indicating if the secondary mouse button (such as the right mouse button) was used."

                "isTouchEvent" =? T<bool>
                |> WithComment "A boolean indicating whether the event that occurred was a touch event."

                "originalEvent" =? T<obj>
                |> WithComment "The original browser event."

                "pageX" =? T<int>
                |> WithComment "The x-value of the pixel coordinate on the page of the mouse cursor."

                "pageY" =? T<int>
                |> WithComment "The y-value of the pixel coordinate on the page of the mouse cursor."

                "target" =? Entity
                |> WithComment "The object that fired the event."

                "targetType" =? T<string>
                |> WithComment "The type of the object that fired the event. Valid values include the following: 'map', 'polygon', 'polyline', or 'pushpin'"

                "wheelData" =? T<int>
                |> WithComment "The number of units that the mouse wheel has changed."

                "getX" =? T<unit -> int>
                |> WithComment "Returns the x-value of the pixel coordinate, relative to the map, of the mouse."

                "getY" =? T<unit -> int>
                |> WithComment "Returns the y-value of the pixel coordinate, relative to the map, of the mouse."
            ]

    let KeyEvent = Type.New()

    let private ConstantStrings ty l =
        List.map (fun s -> (s =? ty |> WithGetterInline ("'" + s + "'")) :> CodeModel.IClassMember) l

    let KeyEventClass =
        Class "Microsoft.Maps.KeyEvent"
        |=> KeyEvent
        |+> ConstantStrings KeyEvent ["keydown"; "keyup"; "keypress"]

    let MouseEvent = Type.New()

    let MouseEventClass =
        Class "Microsoft.Maps.MouseEvent"
        |=> MouseEvent
        |+> ConstantStrings MouseEvent
            [
                "click"; "dblclick"; "rightclick"
                "mousedown"; "mouseup"; "mousemove"; "mouseover"; "mouseleave"; "mouseout"; "mousewheel"
            ]

    let UnitEvent = Type.New()

    let UnitEventClass =
        Class "Microsoft.Maps.UnitEvent"
        |=> UnitEvent
        |+> ConstantStrings UnitEvent
            [
                "copyrightchanged"; "imagerychanged"; "maptypechanged"; "targetviewchanged"; "tiledownloadcomplete"
                "viewchange"; "viewchangeend"; "viewchangestart"
                "entityadded"; "entitychanged"; "entityremoved"
            ]

    let EventHandler = Type.New()

    let EventHandlerClass =
        Class "Microsoft.Maps.EventHandler"
        |=> EventHandler

    let Events = Type.New()

    let EventsClass =
        Class "Microsoft.Maps.Events"
        |=> Events
        |+> [
                "addHandler" => Entity * KeyEvent * (KeyEventArgs ^-> T<unit>) ^-> EventHandler
                |> WithComment "Attaches the handler for the event that is thrown by the target."

                "addHandler" => Entity * MouseEvent * (MouseEventArgs ^-> T<unit>) ^-> EventHandler
                |> WithComment "Attaches the handler for the event that is thrown by the target."

                "addHandler" => Entity * UnitEvent * T<unit -> unit> ^-> EventHandler
                |> WithComment "Attaches the handler for the event that is thrown by the target."

                "addThrottledHandler" => Entity * T<string> * T<obj -> unit> * T<float> ^-> EventHandler
                |> WithComment "Attaches the handler for the event that is thrown by the target, where the minimum interval between events (in milliseconds) is specified in the ThrottleInterval parameter. The last occurrence of the event is called after the specified ThrottleInterval."

                "hasHandler" => Entity * T<string> ^-> T<bool>
                |> WithComment "Checks if the target has any attached event handler."

                "invoke" => Entity * T<string> ^-> T<unit>
                |> WithComment "Invokes an event on the target. This causes all handlers for the specified eventName to be called."

                "removeHandler" => EventHandler ^-> T<unit>
                |> WithComment "Detaches the specified handler from the event."
            ]

    let InfoboxAction = Type.New()

    let InfoboxActionClass =
        Pattern.Config "Microsoft.Maps.InfoboxAction" {
            Required =
                [
                    "label", T<string>
                    "eventHandler", MouseEventArgs ^-> T<unit>
                ]
            Optional = []
        }

    let InfoboxOptions = Type.New()

    let InfoboxOptionsClass =
        Pattern.Config "Microsoft.Maps.InfoboxOptions" {
            Required = []
            Optional =
                [
                    "actions", Type.ArrayOf InfoboxAction
                    "description", T<string>
                    "height", T<int>
                    "htmlContent", T<string>
                    "id", T<string>
                    "location", Location
                    "offset", Point
                    "showCloseButton", T<bool>
                    "showPointer", T<bool>
                    "title", T<string>
                    "titleClickHandler", MouseEventArgs ^-> T<unit>
                    "visible", T<bool>
                    "width", T<int>
                    "zIndex", T<int>
                ]
        }

    let Infobox = Type.New()

    let InfoboxClass =
        Class "Microsoft.Maps.Infobox"
        |=> Infobox
        |=> Implements [EntityInterface]
        |+> [
                Constructor Location
                Constructor (Location * InfoboxOptions)
            ]
        |+> Protocol
            [
                "getActions" => T<unit> ^-> Type.ArrayOf InfoboxAction
                |> WithComment "Returns a list of actions, where each item is a name-value pair indicating an action link name and the event name for the action that corresponds to that action link."

                "getAnchor" => T<unit> ^-> Point
                |> WithComment "Returns the point on the infobox which is anchored to the map. An anchor of (0,0) is the top left corner of the infobox."

                "getDescription" => T<unit -> string>
                |> WithComment "Returns the string that is printed inside the infobox."

                "getHeight" => T<unit -> int>
                |> WithComment "Returns the height of the infobox."

                "getHtmlContent" => T<unit -> string>
                |> WithComment "Returns the infobox as HTML."

                "getId" => T<unit -> string>
                |> WithComment "Returns the ID of the infobox."

                "getLocation" => T<unit> ^-> Location
                |> WithComment "Returns the location on the map where the infobox’s anchor is attached."

                "getOffset" => T<unit -> int>
                |> WithComment "Returns the amount the infobox pointer is shifted from the location of the infobox, or if showPointer is false, then it is the amount the infobox bottom left edge is shifted from the location of the infobox. The default value is (0,0), which means there is no offset."

                "getOptions" => T<unit> ^-> InfoboxOptions
                |> WithComment "Returns the infobox options."

                "getShowCloseButton" => T<unit -> bool>
                |> WithComment "Returns a boolean indicating whether the infobox close button is shown."

                "getShowPointer" => T<unit -> bool>
                |> WithComment "Returns a boolean indicating whether the infobox is drawn with a pointer."

                "getTitle" => T<unit -> string>
                |> WithComment "Returns a string that is the title of the infobox."

                "getTitleClickHandler" => T<unit -> string>
                |> WithComment "Returns the name of the function to call when the title of the infobox is clicked."

                "getVisible" => T<unit -> bool>
                |> WithComment "Returns whether the infobox is visible. A value of false indicates that the infobox is hidden, although it is still an entity on the map."

                "getWidth" => T<unit -> int>
                |> WithComment "Returns the width of the infobox."

                "getZIndex" => T<unit -> int>
                |> WithComment "Returns the z-index of the infobox with respect to other items on the map."

                "setHtmlContent" => T<string -> unit>
                |> WithComment "Sets the HTML content of the infobox. You can use this method to change the look of the infobox."

                "setLocation" => Location ^-> T<unit>
                |> WithComment "Sets the location on the map where the anchor of the infobox is attached."

                "setOption" => InfoboxOptions ^-> T<unit>
                |> WithComment "Sets options for the infobox."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Infobox object to a string."
            ]

    let Range = Type.New()

    let RangeClass =
        Class "Range"
        |=> Range
        |+> Protocol
            [
                "min" =? T<float>
                |> WithComment "The minimum value in the range."

                "max" =? T<float>
                |> WithComment "The maximum value in the range."
            ]

    let PixelReference = Type.New()

    let PixelReferenceClass =
        Class "Microsoft.Maps.PixelReference"
        |=> PixelReference

    let Map = Type.New()

    let MapClass =
        Class "Microsoft.Maps.Map"
        |=> Map
        |=> Implements [EntityInterface]
        |+> [
                Constructor (T<Node> * MapViewOptions)
                Constructor (T<Node> * MapOptions)
                Constructor (T<Node> * ViewOptions)
                Constructor (T<Node>)
            ]
        |+> Protocol
            [
                "entities" =? EntityCollection
                |> WithComment "The map’s entities. Use this property to add or remove entities from the map."

                "blur" => T<unit -> unit>
                |> WithComment "Removes focus from the map control so that it does not respond to keyboard events."

                "dispose" => T<unit -> unit>
                |> WithComment "Deletes the Map object and releases any associated resources."

                "focus" => T<unit -> unit>
                |> WithComment "Applies focus to the map control so that it responds to keyboard events."

                "getBounds" => T<unit> ^-> LocationRect
                |> WithComment "Returns the location rectangle that defines the boundaries of the current map view."

                "getCenter" => T<unit> ^-> Location
                |> WithComment "Returns the location of the center of the current map view."

                "getCopyrights" => T<unit> ^-> Type.ArrayOf T<string>
                |> WithComment "Gets the array of strings representing the attributions of the imagery currently displayed on the map."

                "getCredentials" => T<(string option -> unit) -> unit>
                |> WithComment "Gets the session ID. This method calls the callback function with the session ID as the first parameter."

                "getHeading" => T<unit -> float>
                |> WithComment "Returns the heading of the current map view."

                "getHeight" => T<unit -> int>
                |> WithComment "Returns the height of the map control."

                "getImageryId" => T<unit -> string>
                |> WithComment "Returns the string that represents the imagery currently displayed on the map."

                "getMapTypeId" => T<unit -> string>
                |> WithComment "Returns a string that represents the current map type displayed on the map."

                "getMetersPerPixel" => T<unit -> float>
                |> WithComment "Returns the current scale in meters per pixel of the center of the map."

                "getModeLayer" => T<unit -> Node>
                |> WithComment "Returns the map’s mode node."

                "getOptions" => T<unit> ^-> MapOptions
                |> WithComment "Returns the map options that have been set. Note that if an option is not set, then the default value for that option is assumed and getOptions returns undefined for that option."

                "getPageX" => T<unit -> int>
                |> WithComment "Returns the x coordinate of the top left corner of the map control, relative to the page."

                "getPageY" => T<unit -> int>
                |> WithComment "Returns the y coordinate of the top left corner of the map control, relative to the page."

                "getRootElement" => T<unit -> Node>
                |> WithComment "Returns the map’s root node."

                "getTargetBounds" => T<unit> ^-> LocationRect
                |> WithComment "Returns the location rectangle that defines the boundaries of the view to which the map is navigating."

                "getTargetCenter" => T<unit> ^-> Location
                |> WithComment "Returns the center location of the view to which the map is navigating."

                "getTargetHeading" => T<unit -> float>
                |> WithComment "Returns the heading of the view to which the map is navigating."

                "getTargetMetersPerPixel" => T<unit -> float>
                |> WithComment "Returns the scale in meters per pixel of the center of the view to which the map is navigating."

                "getTargetZoom" => T<unit -> int>
                |> WithComment "Returns the zoom level of the view to which the map is navigating."

                "getUserLayer" => T<unit -> Node>
                |> WithComment "Returns the map’s user node."

                "getViewportX" => T<unit -> int>
                |> WithComment "Returns the x coordinate of the viewport origin (the center of the map), relative to the page."

                "getViewportY" => T<unit -> int>
                |> WithComment "Returns the y coordinate of the viewport origin (the center of the map), relative to the page."

                "getWidth" => T<unit -> int>
                |> WithComment "Returns the width of the map control."

                "getZoom" => T<unit -> int>
                |> WithComment "Returns the zoom level of the current map view."

                "getZoomRange" => T<unit> ^-> Range
                |> WithComment "Returns the range of valid zoom levels for the current map view."

                "isMercator" => T<unit -> bool>
                |> WithComment "Returns whether the map is in a regular Mercator nadir mode."

                "isRotationEnabled" => T<unit -> bool>
                |> WithComment "Returns true if the current map type allows the heading to change; false if the display heading is fixed."

                "setMapType" => MapTypeId ^-> T<unit>
                |> WithComment "Sets the current map type. The specified mapTypeId must be a valid map type ID or a registered map type ID."

                "setOptions" => Size ^-> T<unit>
                |> WithComment "Sets the height and width of the map."

                "setView" => ViewOptions ^-> T<unit>
                |> WithComment "Sets the map view based on the specified options."

                "tryLocationToPixel" => Location * PixelReference ^-> Point
                |> WithComment "Converts a specified Location to a Point on the map relative to the specified PixelReference. If the map is not able to convert the Location, null is returned."

                "tryLocationToPixel" => Location ^-> Point
                |> WithComment "Converts a specified Location to a Point on the map relative to PixelReference.Viewport. If the map is not able to convert the Location, null is returned."

                "tryLocationToPixel" => Type.ArrayOf Location * PixelReference ^-> Type.ArrayOf Point
                |> WithComment "Converts an array of Locations relative to the specified PixelReference and returns an array of Points if all locations were converted. If any of the conversions fail, null is returned."

                "tryLocationToPixel" => Type.ArrayOf Location ^-> Type.ArrayOf Point
                |> WithComment "Converts an array of Locations relative to PixelReference.Viewport and returns an array of Points if all locations were converted. If any of the conversions fail, null is returned."

                "tryPixelToLocation" => Point * PixelReference ^-> Location
                |> WithComment "Converts a specified Point to a Location on the map relative to the specified PixelReference. If the map is not able to convert the Point, null is returned."

                "tryPixelToLocation" => Point ^-> Location
                |> WithComment "Converts a specified Point to a Location on the map relative to PixelReference.Viewport. If the map is not able to convert the Point, null is returned."

                "tryPixelToLocation" => Type.ArrayOf Point * PixelReference ^-> Type.ArrayOf Location
                |> WithComment "Converts an array of Points relative to the specified PixelReference and returns an array of Locations if all points were converted. If any of the conversions fail, null is returned."

                "tryPixelToLocation" => Type.ArrayOf Point ^-> Type.ArrayOf Location
                |> WithComment "Converts an array of Points relative to PixelReference.Viewport and returns an array of Locations if all points were converted. If any of the conversions fail, null is returned."
            ]

    let Color = Type.New()

    let ColorClass =
        Class "Microsoft.Maps.Color"
        |=> Color
        |+> [
                Constructor (T<int> * T<int> * T<int> * T<int>)
                |> WithComment "Initializes a new instance of the Color class. The a parameter represents opacity. The range of valid values for all parameters is 0 to 255."

                "cloneColor" => Color ^-> Color
                |> WithInline "clone"
                |> WithComment "Creates a copy of the Color object."

                "fromHex" => T<string> ^-> Color
                |> WithComment "Converts the specified hex string to a Color."
            ]
        |+> Protocol
            [
                "a" =? T<int>
                |> WithComment "The opacity of the color. The range of valid values is 0 to 255."

                "r" =? T<int>
                |> WithComment "The red value of the color. The range of valid values is 0 to 255."

                "g" =? T<int>
                |> WithComment "The green value of the color. The range of valid values is 0 to 255."

                "b" =? T<int>
                |> WithComment "The blue value of the color. The range of valid values is 0 to 255."

                "clone" => T<unit> ^-> Color
                |> WithComment "Returns a copy of the Color object."

                "getOpacity" => T<unit -> float>
                |> WithComment "Returns the opacity of the Color as a value between 0 (a=0) and 1 (a=255)."

                "toHex" => T<unit -> string>
                |> WithComment "Converts the Color into a 6-digit hex string. Opacity is ignored. For example, a Color with values (255,0,0,0) is returned as hex string #000000."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Color object to a string."
            ]

    let PolylineOptions = Type.New()

    let PolylineOptionsClass =
        Pattern.Config "Microsoft.Maps.PolylineOptions" {
            Required = []
            Optional =
                [
                    "strokeColor", Color
                    "strokeThickness", T<float>
                    "visible", T<bool>
                ]
        }

    let Polyline = Type.New()

    let PolylineClass =
        Class "Microsoft.Maps.Polyline"
        |=> Polyline
        |=> Implements [EntityInterface]
        |+> [
                Constructor (Type.ArrayOf Location)
                Constructor (Type.ArrayOf Location * PolylineOptions)
            ]
        |+> Protocol
            [
                "getLocations" => T<unit> ^-> Type.ArrayOf Location
                |> WithComment "Returns the locations that define the polyline."

                "getStrokeColor" => T<unit> ^-> Color
                |> WithComment "Returns the color of the polyline."

                "getStrokeThickness" => T<unit -> float>
                |> WithComment "Returns the thickness of the polyline."

                "getVisible" => T<unit -> bool>
                |> WithComment "Returns whether the polyline is visible. A value of false indicates that the polyline is hidden, although it is still an entity on the map."

                "setLocations" => Type.ArrayOf Location ^-> T<unit>
                |> WithComment "Sets the locations that define the polyline."

                "setOptions" => PolylineOptions ^-> T<unit>
                |> WithComment "Sets options for the polyline."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Polyline object to a string."
            ]

    let TileLayer = Type.New()
    let TileLayerOptions = Type.New()
    let TileSource = Type.New()
    let TileSourceOptions = Type.New ()

    let TileLayerOptionsClass =
        Pattern.Config "Microsoft.Maps.TileLayerOptions" {
            Required = []
            Optional =
                [
                    "mercator", TileSource
                    "opacity", T<float>
                    "visible", T<bool>
                    "zIndex", T<float>
                ]
        }
        |=> TileLayerOptions

    let TileSourceOptionsClass =
        Pattern.Config "Microsoft.Maps.TileSourceOptions" {
            Required = []
            Optional =
                [
                    "height", T<float>
                    "uriConstructor", T<string>
                    "width", T<float>
                ]
        }
        |=> TileLayerOptions

    let TileSourceClass =
        Class "Microsoft.Maps.TileSource"
        |=> TileSource
        |+> [
                Constructor TileSourceOptions
                |> WithComment "Initializes a new instance of the TileSource  class."
                
                "getHeight" => T<unit -> float>
                |> WithComment "Returns the pixel height of each tile in the tile source."

                "getUriConstructor" => T<unit -> string>
                |> WithComment "Returns a string that constructs tile URLs used to retrieve tiles for the tile layer."

                "getWidth" => T<unit -> float>
                |> WithComment "Returns the pixel width of each tile in the tile source."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Color object to a string."
        ]

    let TileLayerClass =
        Class "Microsoft.Maps.TileLayer"
        |=> TileLayer
        |+> [
                Constructor TileLayerOptions
                |> WithComment "Initializes a new instance of the TileLayer class."

                "getOpacity" => T<unit -> float>
                |> WithComment "Returns the opacity of the tile layer, defined as a double between 0 (not visible) and 1."

                "getTileSource" => T<string> ^-> TileSource
                |> WithComment "Returns the tile source of the tile layer. The projection parameter accepts the following values: mercator, enhancedBirdseyeNorthUp, enhancedBirdseyeSouthUp, enhancedBirdseyeEastUp, enhancedBirdseyeWestUp"

                "getZIndex" => T<unit -> float>
                |> WithComment "Returns the z-index of the tile layer with respect to other items on the map."

                "setOptions" => TileLayerOptions ^-> T<unit>
                |> WithComment "Sets options for the tile layer."

                "toString" => T<unit -> string>
                |> WithComment "Converts the TileLayer object to a string."
            ]
        |+> Protocol
            [
                "a" =? T<int>
                |> WithComment "The opacity of the color. The range of valid values is 0 to 255."
            ]


    let PolygonOptions = Type.New()

    let PolygonOptionsClass =
        Pattern.Config "Microsoft.Maps.PolygonOptions" {
            Required = []
            Optional =
                [
                    "fillColor", Color
                    "strokeColor", Color
                    "strokeThickness", T<float>
                    "visible", T<bool>
                ]
        }

    let Polygon = Type.New()

    let PolygonClass =
        Class "Microsoft.Maps.Polygon"
        |=> Polygon
        |=> Implements [EntityInterface]
        |+> [
                Constructor (Type.ArrayOf Location)
                Constructor (Type.ArrayOf Location * PolygonOptions)
            ]
        |+> Protocol
            [
                "getFillColor" => T<unit> ^-> Color
                |> WithComment "Returns the color of the inside of the polygon."

                "getLocations" => T<unit> ^-> Type.ArrayOf Location
                |> WithComment "Returns the locations that define the polygon."

                "getStrokeColor" => T<unit> ^-> Color
                |> WithComment "Returns the color of the polygon."

                "getStrokeThickness" => T<unit -> float>
                |> WithComment "Returns the thickness of the polygon."

                "getVisible" => T<unit -> bool>
                |> WithComment "Returns whether the polygon is visible. A value of false indicates that the polygon is hidden, although it is still an entity on the map."

                "setLocations" => Type.ArrayOf Location ^-> T<unit>
                |> WithComment "Sets the locations that define the polygon."

                "setOptions" => PolygonOptions ^-> T<unit>
                |> WithComment "Sets options for the Polygon."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Polygon object to a string."
            ]

    let PushpinOptions = Type.New()

    let PushpinOptionsClass =
        Pattern.Config "Microsoft.Maps.PushpinOptions" {
            Required = []
            Optional =
                [
                    "anchor", Point
                    "draggable", T<bool>
                    "icon", T<string>
                    "height", T<int>
                    "text", T<string>
                    "textOffset", Point
                    "typeName", T<string>
                    "visible", T<bool>
                    "width", T<int>
                    "zIndex", T<int>
                ]
        }

    let Pushpin = Type.New()

    let PushpinClass =
        Class "Microsoft.Maps.Pushpin"
        |=> Pushpin
        |=> Implements [EntityInterface]
        |+> [
                Constructor Location
                Constructor (Location * PushpinOptions)
            ]
        |+> Protocol
            [
                "getAnchor" => T<unit> ^-> Point
                |> WithComment "Returns the point on the pushpin icon which is anchored to the pushpin location. An anchor of (0,0) is the top left corner of the icon."

                "getIcon" => T<unit -> string>
                |> WithComment "Returns the pushpin icon."

                "getHeight" => T<unit -> int>
                |> WithComment "Returns the height of the pushpin, which is the height of the pushpin icon."

                "getLocation" => T<unit> ^-> Location
                |> WithComment "Returns the location of the pushpin."

                "getText" => T<unit -> string>
                |> WithComment "Returns the text associated with the pushpin."

                "getTextOffset" => T<unit> ^-> Point
                |> WithComment "Returns the amount the text is shifted from the pushpin icon."

                "getTypeName" => T<unit -> string>
                |> WithComment "Returns the type of the pushpin."

                "getVisible" => T<unit -> bool>
                |> WithComment "Returns whether the pushpin is visible. A value of false indicates that the pushpin is hidden, although it is still an entity on the map."

                "getWidth" => T<unit -> int>
                |> WithComment "Returns the width of the pushpin, which is the width of the pushpin icon."

                "getZIndex" => T<unit -> int>
                |> WithComment "Returns the z-index of the pushpin with respect to other items on the map."

                "setLocation" => Location ^-> T<unit>
                |> WithComment "Sets the location of the pushpin."

                "setOptions" => PushpinOptions ^-> T<unit>
                |> WithComment "Sets options for the pushpin."

                "toString" => T<unit -> string>
                |> WithComment "Converts the Pushpin object to a string."
            ]

    ///////////////////////////////////////////////////////////////////
    // REST Locations API

    let AuthenticationResultCode = Type.New()
    let RestResponse = Type.New()
    let ResourceSet = Type.New()
    let Address = Type.New()
    let LocationResource = Type.New()
    let Confidence = Type.New()
    let PointResource = Type.New()

    let RestResponseClass =
        Class "Microsoft.Maps.RestResponse"
        |=> RestResponse
        |+> Protocol
            [
                "statusCode" =? T<int>
                "statusDescription" =? T<string>
                "authenticationResultCode" =? AuthenticationResultCode
                "traceId" =? T<string>
                "copyright" =? T<string>
                "brandLogoUri" =? T<string>
                "resourceSets" =? Type.ArrayOf ResourceSet
                "errorDetails" =? Type.ArrayOf T<string>
            ]

    let AuthenticationResultCodeClass =
        Class "Microsoft.Maps.AuthenticationResultCode"
        |=> AuthenticationResultCode
        |+> ConstantStrings AuthenticationResultCode
            ["ValidCredentials"; "InvalidCredentials"; "CredentialsExpired"; "NotAuthorized"; "NoCredentials"]

    let ResourceSetClass =
        Class "Microsoft.Maps.ResourceSet"
        |=> ResourceSet
        |+> Protocol
            [
                "estimatedTotal" =? T<int>
                "resources" =? Type.ArrayOf T<obj>
            ]

    let LocationResourceClass =
        Class "Microsoft.Maps.LocationResource"
        |=> LocationResource
        |+> Protocol
            [
                "__type" =? T<string>
                "name" =? T<string>
                "point" =? PointResource
                "bbox" =? Type.ArrayOf T<float>
                "entityType" =? T<string>
                "address" =? Address
                "confidence" =? Confidence
            ]

    let PointResourceClass =
        Class "Microsoft.Maps.PointResource"
        |=> PointResource
        |+> Protocol
            [
                "type" =? T<string>
                "coordinates" =? Type.ArrayOf T<float>
            ]

    let ConfidenceClass =
        Class "Microsoft.Maps.Confidence"
        |=> Confidence
        |+> ConstantStrings Confidence ["High"; "Medium"; "Low"; "Unknown"]

    let AddressClass =
        Class "Microsoft.Maps.Address"
        |=> Address
        |+> Protocol
            [
                "adminDistrict" =? T<string>
                "locality" =? T<string>
                "postalCode" =? T<string>
                "addressLine" =? T<string>
                "countryRegion" =? T<string>
            ]

    ///////////////////////////////////////////////////////////////////
    // REST Routes API

    let RouteResource = Type.New()
    let RouteLeg = Type.New()
    let ItineraryItem = Type.New()
    let ItineraryDetail = Type.New()
    let ItineraryIcon = Type.New()
    let ItineraryInstruction = Type.New()
    let TransitLine = Type.New()
    let RoutePath = Type.New()
    let LineResource = Type.New()
    let RouteRequest = Type.New()
    let RouteOptimize = Type.New()
    let RouteAvoid = Type.New()
    let RoutePathOutput = Type.New()
    let DistanceUnit = Type.New()
    let TimeType = Type.New()
    let TravelMode = Type.New()

    let WaypointClass =
        Class "Microsoft.Maps.Waypoint"
        |=> Waypoint
        |+> [
                Constructor T<string>?s
                |> WithInline "$s"

                Constructor Point?p
                |> WithInline "($p.x+','+$p.y)"
            ]

    let RouteResourceClass =
        Class "Microsoft.Maps.RouteResource"
        |=> RouteResource
        |+> Protocol
            [
                "id" =? T<string>
                "bbox" =? Type.ArrayOf T<float>
                "distanceUnit" =? DistanceUnit
                "durationUnit" =? T<string>
                "travelDistance" =? T<float>
                "travelDuration" =? T<float>
                "routeLegs" =? Type.ArrayOf RouteLeg
                "routePath" =? RoutePath
            ]

    let RouteLegClass =
        Class "Microsoft.Maps.RouteLeg"
        |=> RouteLeg
        |+> Protocol
            [
                "travelDistance" =? T<float>
                "travelDuration" =? T<float>
                "actualStart" =? PointResource
                "actualEnd" =? PointResource
                "startLocation" =? Type.ArrayOf LocationResource
                "endLocation" =? Type.ArrayOf LocationResource
                "itineraryItems" =? Type.ArrayOf ItineraryItem
            ]

    let ItineraryItemClass =
        Class "Microsoft.Maps.ItineraryItem"
        |=> ItineraryItem
        |+> Protocol
            [
                "childItineraryItems" =? Type.ArrayOf ItineraryItem
                "compassDirection" =? T<string>
                "details" =? Type.ArrayOf ItineraryDetail
                "exit" =? T<string>
                "hints" =? T<string>
                "iconType" =? ItineraryIcon
                "instruction" =? ItineraryInstruction
                "maneuverPoint" =? PointResource
                "sideOfStreet" =? T<string>
                "signs" =? T<string>
                "time" =? T<string>
                "tollZone" =? T<string>
                "towardsRoadName" =? T<string>
                "transitLine" =? Type.ArrayOf TransitLine
                "transitStopId" =? T<string>
                "transitTerminus" =? T<string>
                "travelDistance" =? T<float>
                "travelDuration" =? T<float>
                "travelMode" =? T<string>
                "warnings" =? T<string>
            ]

    let ItineraryInstructionClass =
        Class "Microsoft.Maps.ItineraryInstruction"
        |=> ItineraryInstruction
        |+> Protocol
            [
                "maneuverType" =? T<string>
                "text" =? T<string>
            ]

    let ItineraryIconClass =
        Class "Microsoft.Maps.ItineraryIcon"
        |=> ItineraryIcon
        |+> ConstantStrings ItineraryIcon ["None"; "Airline"; "Auto"; "Bus"
                                           "Ferry"; "Train"; "Walk"; "Other"]

    let ItineraryDetailClass =
        Class "Microsoft.Maps.ItineraryDetail"
        |=> ItineraryDetail
        |+> Protocol
            [
                "compassDegrees" =? T<string>
                "maneuverType" =? T<string>
                "names" =? T<string>
                "startPathIndices" =? Type.ArrayOf T<int>
                "endPathIndices" =? Type.ArrayOf T<int>
                "roadType" =? T<string>
            ]

    let TransitLineClass =
        Class "Microsoft.Maps.TransitLine"
        |=> TransitLine
        |+> Protocol
            [
                "verboseName" =? T<string>
                "abbreviatedName" =? T<string>
                "agencyId" =? T<string>
                "agencyName" =? T<string>
                "lineColor" =? T<string>
                "lineTextColor" =? T<string>
                "uri" =? T<string>
                "phoneNumber" =? T<string>
                "providerInfo" =? T<string>
            ]

    let RoutePathClass =
        Class "Microsoft.Maps.RoutePath"
        |=> RoutePath
        |+> Protocol
            [
                "line" =? LineResource
                "point" =? PointResource
            ]

    let LineResourceClass =
        Class "Microsoft.Maps.LineResource"
        |=> LineResource
        |+> Protocol
            [
                "type" =? T<string>
                "coordinates" =? Type.ArrayOf (Type.ArrayOf T<float>)
            ]

    let RouteRequestClass =
        Pattern.Config "Microsoft.Maps.RouteRequest" {
            Required = []
            Optional =
                [
                    "waypoints", Type.ArrayOf Waypoint
                    "avoid", Type.ArrayOf RouteAvoid
                    "heading", T<int>
                    "optimize", RouteOptimize
                    "routePathOutput", RoutePathOutput
                    "distanceUnit", DistanceUnit
                    "dateTime", T<string>
                    "timeType", TimeType
                    "maxSolutions", T<int>
                    "travelMode", TravelMode
                ]
        }

    let RouteOptimizeClass =
        Class "Microsoft.Maps.RouteOptimize"
        |=> RouteOptimize
        |+> ConstantStrings RouteOptimize ["distance"; "time"; "timeWithTraffic"]

    let RouteAvoidClass =
        Class "Microsoft.Maps.RouteAvoid"
        |=> RouteAvoid
        |+> ConstantStrings RouteAvoid ["highways"; "tolls"; "minimizeHighways"; "minimizeTolls"]

    let RoutePathOutputClass =
        Class "Microsoft.Maps.RoutePathOutput"
        |=> RoutePathOutput
        |+> ConstantStrings RoutePathOutput ["Points"; "None"]

    let DistanceUnitClass =
        Class "Microsoft.Maps.DistanceUnit"
        |=> DistanceUnit
        |+> ConstantStrings DistanceUnit ["Mile"; "Kilometer"]

    let TimeTypeClass =
        Class "Microsoft.Maps.TimeType"
        |=> TimeType
        |+> ConstantStrings TimeType ["Arrival"; "Departure"; "LastAvailable"]

    let TravelModeClass =
        Class "Microsoft.Maps.TravelMode"
        |=> TravelMode
        |+> ConstantStrings TravelMode ["Driving"; "Walking"; "Transit"]

    ///////////////////////////////////////////////////////////////////
    // REST Imagery API

    let StaticMapRequest = Type.New()
    let ImagerySet = Type.New()
    let MapLayer = Type.New()
    let MapVersion = Type.New()
    let PushpinResource = Type.New()

    let StaticMapRequestClass =
        Pattern.Config "Microsoft.Maps.StaticMapRequest" {
            Required =
                [
                    "imagerySet", ImagerySet
                ]
            Optional =
                [
                    "avoid", Type.ArrayOf RouteAvoid
                    "centerPoint", Point
                    "dateTime", T<string>
                    "mapArea", Point * Point
                    "mapLayer", MapLayer
                    "mapSize", T<int> * T<int>
                    "mapVersion", MapVersion
                    "maxSolutions", T<int>
                    "optimize", RouteOptimize
                    "pushpin", Type.ArrayOf PushpinResource
                    "query", T<string>
                    "timeType", TimeType
                    "travelMode", TravelMode
                    "waypoints", Type.ArrayOf Waypoint
                    "zoomLevel", T<int>
                ]
        }
        |=> StaticMapRequest

    let ImagerySetClass =
        Class "Microsoft.Maps.ImagerySet"
        |=> ImagerySet
        |+> ConstantStrings ImagerySet ["Aerial"; "AerialWithLabels"; "Road"]

    let MapLayerClass =
        Class "Microsoft.Maps.MapLayer"
        |=> MapLayer
        |+> ConstantStrings MapLayer ["TrafficFlow"]

    let MapVersionClass =
        Class "Microsoft.Maps.MapVersion"
        |=> MapVersion
        |+> ConstantStrings MapVersion ["v0"; "v1"]

    let PushpinResourceClass =
        Pattern.Config "Microsoft.Maps.PushpinResource" {
            Required =
                [
                    "x", T<float>
                    "y", T<float>
                ]
            Optional =
                [
                    "iconStyle", T<int>
                    "label", T<string>
                ]
        }
        |=> PushpinResource

    let ImageryMetadataRequest = Type.New()
    let ImageryMetadataInclude = Type.New()
    let ImageryMetadataResource = Type.New()

    let ImageryMetadataRequestClass =
        Pattern.Config "Microsoft.Maps.ImageryMetadataRequest" {
            Required =
                [
                    "imagerySet", ImagerySet
                ]
            Optional =
                [
                    "centerPoint", Point
                    "include", ImageryMetadataInclude
                    "mapVersion", MapVersion
                    "orientation", T<float>
                    "zoomLevel", T<int>
                ]
        }
        |=> ImageryMetadataRequest

    let ImageryMetadataIncludeClass =
        Class "Microsoft.Maps.ImageryMetadataInclude"
        |=> ImageryMetadataInclude
        |+> ConstantStrings ImageryMetadataInclude ["ImageryProviders"]

    let ImageryMetadataResourceClass =
        Class "Microsoft.Maps.ImageryMetadataResource"
        |=> ImageryMetadataResource
        |+> Protocol
            [
                "__type" =? T<string>
                "imageHeight" =? T<int>
                "imageWidth" =? T<int>
                "imageUrl" =? T<string>
                "imageUrlSubdomains" =? Type.ArrayOf T<string>
                "imageryProviders" =? T<obj>
                "vintageBegin" =? T<string>
                "vintageEnd" =? T<string>
                "zoomMax" =? T<int>
                "zoomMin" =? T<int>
                "orientation" =? T<float>
                "tilesX" =? T<int>
                "tilesY" =? T<int>
            ]


    let Assembly =
        Assembly [
            Namespace "IntelliFactory.WebSharper.Bing" [
                AltitudeReferenceClass
                LocationClass
                LocationRectClass
                PointClass
                EventHandlerClass
                EventsClass
                KeyEventArgsClass
                MouseEventArgsClass
                KeyEventClass
                MouseEventClass
                UnitEventClass
                LabelOverlayClass
                MapOptionsClass
                ViewOptionsClass
                MapViewOptionsClass
                EntityInterface
                EntityCollectionOptionsClass
                EntityCollectionClass
                RangeClass
                MapTypeIdClass
                SizeClass
                MapClass
                ColorClass
                InfoboxActionClass
                InfoboxOptionsClass
                InfoboxClass
                TileLayerClass
                TileLayerOptionsClass
                TileSourceClass
                TileSourceOptionsClass
                PolylineOptionsClass
                PolylineClass
                PolygonOptionsClass
                PolygonClass
                PushpinOptionsClass
                PushpinClass

                // REST locations
                AuthenticationResultCodeClass
                RestResponseClass
                ResourceSetClass
                LocationResourceClass
                ConfidenceClass
                AddressClass
                PointResourceClass
                WaypointClass

                // REST Routes
                RouteResourceClass
                RouteLegClass
                ItineraryItemClass
                ItineraryDetailClass
                ItineraryIconClass
                ItineraryInstructionClass
                TransitLineClass
                RoutePathClass
                LineResourceClass
                RouteRequestClass
                RouteOptimizeClass
                RouteAvoidClass
                RoutePathOutputClass
                DistanceUnitClass
                TimeTypeClass
                TravelModeClass

                // REST Imagery
                StaticMapRequestClass
                ImagerySetClass
                MapLayerClass
                MapVersionClass
                PushpinResourceClass
                ImageryMetadataRequestClass
                ImageryMetadataIncludeClass
                ImageryMetadataResourceClass
            ]
        ]

