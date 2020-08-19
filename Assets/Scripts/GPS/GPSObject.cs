using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenTK;
using UnityEngine.SceneManagement;

public class GPSObject : MonoBehaviour{
  private bool isObjectSpawned;
  public Text DebugText;
  public Vector3d GpsPosition;
  public GPSManager GPSManager;
  public float maxScale = 75;
  public float minScale = 5;

  void Start () {}

  //Calculates a value to scale by based off of ditance to the object
  private float ScaleOnDistance(Vector3d offset){
    double dist = (float)Mathf.Sqrt(Mathf.Pow(Mathf.Abs((float)offset.X), 2) + Mathf.Pow(Mathf.Abs((float)offset.Z), 2));
    float scale = (float)(dist / 5f);
    //end cases
    if(scale < minScale) scale = minScale;
    if(scale > maxScale) scale = maxScale;
    return scale;
  }

  private bool set = false;
  private float initHeading = -1f;
  void Update () {
     if (GPSManager.ServiceStatus == LocationServiceStatus.Running && !set && GPSManager.headingAccuracy >= 0 ){
       transform.position = Vector3.zero; //center
       transform.rotation = Quaternion.identity; // remove any rotation

       Vector3d offset = GPSManager.CalculateOffset(new Vector3d(GPSManager.position), GpsPosition);

       initHeading = -(GPSManager.heading); //idk why this was necesary but it was
       //initHeading = GPSManager.heading;
       /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        *  Basic idea here is that since we know the initial heading, we know
        * which way the user was facing the first time that this script was
        * called. This value is how much the AR coordinate system is from the
        * real worlds.
        *  We rotate the object by this value and then rotate it such that it
        * aligns with the real world and then apply a translation which is
        * relative to the objects rotation. Essentially if you roate 45deg and
        * then translate 1 unit +Z you will move based off of your LOCAL
        * coordinate system which is now 45deg off from the GLOBAL coordinate
        * system.
        * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
       transform.Rotate(0, initHeading, 0);

       transform.Translate(new Vector3((float)offset.Z, 50f, (float)offset.X));
       //float s = ScaleOnDistance(offset);
       //transform.localScale = new Vector3(s, s, s);

       set = !set;
     }

     if(DebugText != null) DebugText.text = "OBJECT:\n\tPOS " + transform.position + "\n\tGPS " + GpsPosition + "\n\tSET: " + set + "\n\tINIT HEADING: " + initHeading;
	}
}
