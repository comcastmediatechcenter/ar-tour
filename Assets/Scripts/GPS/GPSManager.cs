using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using OpenTK;

public class GPSManager : MonoBehaviour{
  public static GPSManager Instance { set; get; }
  public Vector3 position;
  public Vector3 positionAccuracy;
  public float heading;
  public float headingAccuracy;
  public bool UseFakeLocation;
  public Text TextDisplay;

  public bool autoReload = false;
  public int reloadTime = 0;

  [HideInInspector]
  public bool isRunning = true;

  [HideInInspector]
  public LocationServiceStatus ServiceStatus = LocationServiceStatus.Stopped;

  private float degreesLatitudeInMeters = 111132;
  private float degreesLongitudeInMetersAtEquator = 111319.9f;
  private bool active = true;

  private void Start(){
    Instance = this;
    DontDestroyOnLoad(gameObject);
    StartCoroutine(StartLocationService());
    if(autoReload) StartCoroutine(TimedReload(reloadTime));
  }

  private IEnumerator StartLocationService(){
    while (isRunning){
      UpdateGPS();
      yield return new WaitForSeconds(0.1f);
    }
  }

  private IEnumerator TimedReload(int t){
    yield return new WaitForSeconds(t);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  private void UpdateGPS(){
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * Most of this code came from here: https://blog.anarks2.com/Geolocated-AR-In-Unity-ARFoundation/.
     * Some changes to it allowed for us to reuse it app wide.
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    if (Input.location.status == LocationServiceStatus.Running && !UseFakeLocation){
      var latitude = Input.location.lastData.latitude;
      var longitude = Input.location.lastData.longitude;
      var altitude = Input.location.lastData.altitude;
      position = new Vector3(latitude, altitude, longitude);

      var hAcc = Input.location.lastData.horizontalAccuracy;
      var vAcc = Input.location.lastData.verticalAccuracy;
      positionAccuracy = new Vector3(hAcc, vAcc, hAcc);
      heading = Input.compass.trueHeading;
      headingAccuracy = Input.compass.headingAccuracy;

      ServiceStatus = Input.location.status;
      //Debug.Log(string.Format("Lat: {0} Long: {1} Alt: {2}\nDir: {3}", position.x, position.z, position.y, heading));

      if (TextDisplay){
        TextDisplay.text = "USER\n\tGPS: " + position + "\n\tHEADING: " + heading + " ACC: " + headingAccuracy;
      }
    }
  }

  //Used to caluclate the delta between two real world objects in Unity coordinates
  //For whatever reason X and Z are flipped. Y is altitude.
  public Vector3d CalculateOffset(Vector3d origin, Vector3d target){
    return (target - origin) * new Vector3d(degreesLatitudeInMeters, target.Y, GetLongitudeDegreeDistance(origin.X));
  }

  //From GPSTrackedObject
  public double GetLongitudeDegreeDistance(double latitude){
    return degreesLongitudeInMetersAtEquator * Mathf.Cos(((float)latitude) * (Mathf.PI / 180));
  }

  void OnApplicationPause(bool paused){
    //paused == true when app is sent to Background
    //paused == false when app is brought from background

    //If we were only paused no need to reset everything yet but we can save some battery, however if were coming back its time to reload!
    if(paused){
      //Pause the location gathering
      isRunning = false;
    } else {
      //Refresh the scene. This will reload everything attached.
      StartCoroutine(TimedReload(1));
    }
  }
}
