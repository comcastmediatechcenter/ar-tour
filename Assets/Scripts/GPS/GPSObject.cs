using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSObject : MonoBehaviour{
  private bool isObjectSpawned;
  public Text DebugText;
  public Vector3 GpsPosition;
  public GPSManager GPSManager;

  private float degreesLatitudeInMeters = 111132;
  private float degreesLongitudeInMetersAtEquator = 111319.9f;

  // Use this for initialization
  void Start () {}

  //From GPSTrackedObject
  private float GetLongitudeDegreeDistance(float latitude){
    return degreesLongitudeInMetersAtEquator * Mathf.Cos(((float)latitude) * (Mathf.PI / 180));
  }

	// Update is called once per frame
  private bool set = false;
  private float initHeading = -1f;
  void Update () {
     if (GPSManager.ServiceStatus == LocationServiceStatus.Running && !set){
       transform.position = Vector3.zero;
       transform.rotation = Quaternion.identity;

       Vector3 gpsPosition = GPSManager.position;
       Vector3 objectPosition = GpsPosition;
       Vector3 offset = Vector3.Scale((objectPosition - gpsPosition), new Vector3(degreesLatitudeInMeters, 1, GetLongitudeDegreeDistance(gpsPosition.x)));

       //var heading = MathHelper.DegreesToRadians(GPSManager.heading);

       initHeading = 360 - GPSManager.heading;
       //var t = Quaterniond.FromEulerAngles(0, initHeading, 0);
       //var rotatedOffset = t * offset;

       transform.Rotate(0, initHeading, 0);
       transform.Translate(new Vector3((float)offset.z, 15f, (float)offset.x));

       set = !set;
     }
     if(DebugText != null) DebugText.text = "OBJECT:\n\tPOS " + transform.position + "\n\tGPS " + GpsPosition + "\n\tSET: " + set + "\n\tINIT HEADING: " + initHeading;
	}
}
