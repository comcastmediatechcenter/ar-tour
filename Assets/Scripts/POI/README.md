# POI Framework
## POIS.cs
This file is a wrapper for the POI objects. For whatever reason Unity cannot read in an array from JSON unless it can be attached to an object directly. Since this object only has one value (which is an array) we can use it to read and write the array as a single object. It's by far the least pretty part of this system.
## POI.cs
This file defines what a POI object is and all of the attributes that are required for it. I tried to keep each one of these objects as simple as possible to make adding and removing them easier later down the road.
Bellow is a short example of each attribute.
* **lat** - This is the latitude of the object. The more values after the decimal place the more accurate the placement of the final object.
* **lon** - This is the longitude of the object. The more values after the decimal place the more accurate the placement of the final object.
* **name** - This is a string that defines what the objects name is. This is the value that will appear in the info screen when the object is approached or tapped on.
* **info** - This is the info that appears in the info panel. **NOTE:** You can use formatting within this field like ``\n`` to produce a new line or ``\t`` to produce an indentation.
* **rad** - This is the radius of the objects "sphere of influence". For high priority objects this value controls at what range the info panel will open with information about this specific object.
* **type** - This is the type or importance level of the object. Currently there are only two types: ``1`` where the object will be rendered in both AR and map view, and ``0`` where the object is only rendered in map view.
* **prefab** - This is a string field that references which prefab will instantiated for this object. Its is important to note that the object **MUST** be within the ``Resources`` folded and that string should not end in ``.prefab``. Example: ``TivoliMascotPrefab`` not ``TivoliMascotPrefab.prefab``.
## pois.json
This is the raw JSON file that contains all of the information for generating all of the POIs in both views.
