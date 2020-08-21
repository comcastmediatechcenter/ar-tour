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
  public float maxHeight = 350;
  public float minHeight = 100;

  void Start () {}

  //Calculates the target height based off of distance to the object
  private float HeightOnDistance(Vector3d offset){
    double dist = (float)Mathf.Sqrt(Mathf.Pow(Mathf.Abs((float)offset.X), 2) + Mathf.Pow(Mathf.Abs((float)offset.Z), 2));
    float height = (float)(dist / 10f);
    //end cases
    if(height < minHeight) height = minHeight;
    if(height > maxHeight) height = maxHeight;
    return height;
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

     transform.position = new Vector3(transform.position.x, HeightOnDistance(new Vector3d(transform.position)), transform.position.z);

     if(DebugText != null) DebugText.text = "OBJECT:\n\tPOS " + transform.position + "\n\tGPS " + GpsPosition + "\n\tSET: " + set + "\n\tINIT HEADING: " + initHeading;
	}
}
