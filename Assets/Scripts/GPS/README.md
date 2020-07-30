# GPS
This folder contains all of the GPS scripts and some of the OpenTK classes to enable high resolution tracking with double vectors instead of float vectors.
## GPSManager.cs
**NOTE:** A lot of this code has been repurposed from an example that can be found [here](https://blog.anarks2.com/Geolocated-AR-In-Unity-ARFoundation/), and as such there are some elements left over that I have yet to clean out.
The parameters are as follows:
* **Position** - This value is used for faking location. I suggest using Xcode or the Android equivalent to fake location as this one will stop live compass updates when faking location which hinders all modes.
* **Position Accuracy** - This value is used for faking location. I suggest using Xcode or the Android equivalent to fake location as this one will stop live compass updates when faking location which hinders all modes.
* **Heading** - This value is used for faking location. I suggest using Xcode or the Android equivalent to fake location as this one will stop live compass updates when faking location which hinders all modes.
* **Heading Accuracy** - This value is used for faking location. I suggest using Xcode or the Android equivalent to fake location as this one will stop live compass updates when faking location which hinders all modes.
* **Use fake location** - This value is used for faking location. I suggest using Xcode or the Android equivalent to fake location as this one will stop live compass updates when faking location which hinders all modes.
* **Text Display** - This is a debug display that allows for quick debugging of GPS location services. It should be turned off for production releases.
* **Auto Reload** - This is a time based reload system which will tighten the users "error cone". This probably isn't totally required anymore as it would only be useful in the map scene, but I have not removed it yet. When checked the scene will be reloaded and all runtime settings will be reset.
* **Reload Time** - This is the time in seconds that the reload function will wait before forcing a scene reload. I have found some success setting it at 60-120, but again this is no longer really required in our current application.
## GPSObject.cs
This object is attached to objects in AR to ensure that their location is correct relative to the user. These scripts also preform distance based scaling so that as the user approaches the object it will get smaller and vice versa. Unless you are manually placing this object you shouldn't worry about these values as the ``MapManager`` will correctly set them based on the ``POI`` JSON. This script takes the following parameters:
* **Debug Text** - This is a general purpose text to get debug information out of the object. It should not be used in a production setting.
* **GPS Position** - This is the lat(x)/alt(y)/lon(z) of the object in the real world.
* **GPS Manager** - This is for the GPS Manager so that the object knows how to calculate its offset relative to the user.
* **Max Scale** - This is for the distance based scaling method and sets the maximum size for the object.
* **Min Scale** - This is for the distance based scaling method and sets the minimum size for the object.
## OpenTK
The OpenTK classes were included to provide higher resolution ``Vector3``s that use ``double``s instead of ``float``s. More can be found [here](https://github.com/opentk/opentk).
