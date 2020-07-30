# Managers
Each one of these managers is responsible for one specific task within each scene. They were all designed to be as reusable as possible while still maintaining as many features as they could have.
## InfoManager.cs
As the name suggests this manager manages the info panel that is in both scenes. The values that are required regardless of mode are as follows:
* **Mode** - This lets the user select between ``Map Mode`` and ``Raycast Mode``. In ``Map Mode`` the only consideration for activating the info panel will be the distance between the user and the object, if their distance is less than the objects ``rad`` value then the info panel will be activated (if not already). However in ``Raycast Mode``, the manager does not take into account the distance between the user and the object opting to detect when an object has been tapped through raycasting.
* **POIS Json** - A link to the JSON file. This file contains all of the necessary information that we display to the screen along with the range values for the distance calculations.
* **Display Object** - This is the image object that the manager uses. This should be the root of the prefab for the info panel.
* **Name Text** - This is where the name of the currently selected object is displayed. When we do not have a selected POI this will not be displayed. This is already a component of the info panel prefab.
* **Info Text** - This is where the info of the currently selected object is display. When we do not have a selected POI this will not be displayed. This is already a component of the info panel prefab.
* **Prompt Text** - This is where the prompt to complete an action is displayed. When we have a selected POI this will not be displayed. This is already a component of the info panel prefab.
* **Exit Button** - This is the button to close an active info panel. When we do not have a selected POI this will not be displayed. This is already a component of the info panel prefab.
* **GPS Manager** - This is only required when running in ``Map Mode`` and allows for the manager to know the location of the user in order to calculate the distance between the user and the different objects.
## MapManager.cs
This manager is resposible for generating the objects in both ``Map Mode`` and ``AR Mode``. While its duties stop there in ``AR Mode``, in ``Map Mode`` the manager also handles all of the touch inputs to make sure that the map responds to touch input correctly. The values that it takes in are as follows:
* **POIS Json** - This is the link to the JSON file so that the manager knows where to place object and what prefab to load when placing them. This is required in both modes.
* **GPS Manager** - This is the link to the GPS Manager so that this manager knows where to place every object in relation to the user. This is required in both modes.
* **Debug Text** - This is to help with calibration of end zones and touch controls. This is not required and shouldn't be included in a production release.
* **touchScale** - This is the amount that the touch "speed" will be scaled by. A larger number means that the map will move faster when touched and a smaller number will make it move slower. The default for this value is ``0.5``. This is only required in ``Map Mode``.
* **UserMarker** - This is the game object that will be used for the user location marker. This is only required in ``Map Mode``.
* **MapOrigin** - This is the lat/lon (where lat = x and lon = y) location of the maps origin. This needs to be as accurate as possible as all of the translations are applied in relation to this setting. This is only required in ``Map Mode``.
* **Map** - This is the map game object. It is only required in ``Map Mode``.
## UIManager.cs
This super simple manager just provides a clean way for the UI buttons to activate reusable functions. It is set up with null detection so if a button is not required simply leave it set to ``None`` and the manager will handle it accordingly. The names are meant to be easy to tell what they do, but a list follows nonetheless:
* **To Map Button** - This button will change from whatever scene you are currently in to the map scene.
* **To AR Button** - This button will change from whatever scene you are currently in to the AR scene.
* **To CU Button** - This button will change from whatever scene you are currently in to the CU scene.
## GPS Manager
Info on the GPS Manager can be found under the GPS scripts folder.
