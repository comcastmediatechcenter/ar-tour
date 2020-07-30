using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using OpenTK;

public class MapManager : MonoBehaviour{

  public enum Mode {MapMode, ARMode};
  public Mode OpMode;

  [Header("General Settings")]
  public TextAsset POISJson;
  public GPSManager gps;
  public Text debugText;

  [Header("Map Mode Settings")]
  public float touchScale = .5f;
  public GameObject UserMarker;
  public Vector2d MapOrigin;
  public GameObject Map;

  private bool snapped = false;

  [HideInInspector]
  public POIS pois;

  // Start is called before the first frame update
  void Start(){
    pois = JsonUtility.FromJson<POIS>(POISJson.text);
    //Debug.Log("POIS SIZE: " + (pois.data.Length));

    foreach (POI poi in pois.data) {
      if(poi.prefab != ""){
        if(OpMode == Mode.MapMode){
          GameObject obj = (GameObject)Instantiate(Resources.Load(poi.prefab));
          Vector3d offset = gps.CalculateOffset(new Vector3d(MapOrigin.X, 0 ,MapOrigin.Y), new Vector3d(poi.lat, 0, poi.lon));
          obj.transform.SetParent(Map.transform);
          obj.transform.localPosition = new Vector3(-(float)offset.Z, 10, -(float)offset.X);
          //Debug.Log("POI " + poi.name + ": " + obj.transform.localPosition);
        } else {
          if(poi.type == 1){
            GameObject obj = (GameObject)Instantiate(Resources.Load(poi.prefab));
            obj.AddComponent<GPSObject>();
            obj.GetComponent<GPSObject>().GpsPosition = new Vector3d(poi.lat, 0, poi.lon);
            obj.GetComponent<GPSObject>().GPSManager = gps;
            obj.transform.name = poi.name;
          }
        }
      }
    }
  }
  // Update is called once per frame
  private float lastZoom = 0f;
  void Update(){
    if(OpMode == Mode.MapMode){
      //manage the pin
      Vector3d offset = gps.CalculateOffset(new Vector3d(MapOrigin.X, 0 ,MapOrigin.Y), new Vector3d(gps.position));
      UserMarker.transform.localPosition = new Vector3(-(float)offset.Z, 10, -(float)offset.X);
      UserMarker.transform.rotation = Quaternion.Euler(0, gps.heading, 0);
      if(!snapped){
        transform.position = new Vector3(-(float)offset.Z, 1000, -(float) offset.X);
        snapped = true;
      }

      if(Input.touchCount == 1){
        if(Input.GetTouch(0).phase == TouchPhase.Moved){
          Vector2 delta = Input.GetTouch(0).deltaPosition;
          transform.Translate(-delta.x * touchScale, -delta.y * touchScale, 0);
          if(debugText != null){
            debugText.text = "TOUCH X: " + delta.x + " TOUCH Y: " + delta.y + " COUNT: " + Input.touchCount
                           + "\nMAPPOS: " + transform.position
                           + "\nUSER LOC: " + UserMarker.transform.localPosition
                           +"\nGPS: " + gps.position;
          }
        }
      }

      if(Input.touchCount >= 2){
        if(Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved){
          Vector2 d1 = Input.GetTouch(0).position;
          Vector2 d2 = Input.GetTouch(1).position;
          float zoom = Vector2.Distance(d1, d2) * touchScale * .1f;

          if(lastZoom == 0f){
            lastZoom = zoom;
          }

          //transform.Translate(0, 0, zoom);
          if((lastZoom - zoom) < 25) Camera.main.fieldOfView += (lastZoom - zoom);
          if(debugText != null){
            debugText.text = "FOV: " + Camera.main.fieldOfView + " COUNT: " + Input.touchCount
                            + "\nMAPPOS: " + transform.position
                            + "\nUSER LOC: " + UserMarker.transform.localPosition
                            +"\nGPS: " + gps.position;
          }
          lastZoom = zoom;
        }
        if(Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(1).phase == TouchPhase.Ended){
          lastZoom = 0f;
        }
      }

      //Ensure within Bounds
      if(transform.position.x > 775 - Camera.main.fieldOfView) transform.position = new Vector3(775 - Camera.main.fieldOfView, transform.position.y, transform.position.z);
      if(transform.position.x < -900 + Camera.main.fieldOfView) transform.position = new Vector3(-900 + Camera.main.fieldOfView, transform.position.y, transform.position.z);
      if(transform.position.z > 775 - Camera.main.fieldOfView) transform.position = new Vector3(transform.position.x, transform.position.y, 775 - Camera.main.fieldOfView);
      if(transform.position.z < -800 + Camera.main.fieldOfView) transform.position = new Vector3(transform.position.x, transform.position.y, -800 + Camera.main.fieldOfView);
      if(Camera.main.fieldOfView < 5) Camera.main.fieldOfView = 5;
      if(Camera.main.fieldOfView > 65) Camera.main.fieldOfView = 65;
      //if(transform.position.y > 1500) transform.position = new Vector3(transform.position.x, 1500, transform.position.y);
      //if(transform.position.y < 500) transform.position = new Vector3(transform.position.x, 500, transform.position.y);
    }
  }
}
