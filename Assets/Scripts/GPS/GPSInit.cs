using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GPSInit : MonoBehaviour{

  public Text DebugText;
  public string TargetScene;

  [HideInInspector]
  public LocationServiceStatus ServiceStatus = LocationServiceStatus.Stopped;

  // Start is called before the first frame update
  void Start() {
    StartCoroutine(StartLocationService());
  }

  private IEnumerator StartLocationService(){
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * Most of this code came from here: https://blog.anarks2.com/Geolocated-AR-In-Unity-ARFoundation/.
     * Some changes to it allowed for us to reuse it app wide.
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    ServiceStatus = LocationServiceStatus.Initializing;

    Input.compass.enabled = true;

    yield return new WaitForSeconds(5);

    DebugText.text = "Starting GPSManager . . . ";
    if (Input.location.isEnabledByUser){
      Input.location.Start();
      //yield return new WaitForSeconds(5);

      int maxWait = 20;
      while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0){
        yield return new WaitForSeconds(1);
        DebugText.text += ". ";
        maxWait--;
      }

      if (maxWait <= 0){
        DebugText.text += "FAILED: TIME OUT\n";
        //yield break;
      } else {
        ServiceStatus = Input.location.status;
        if (Input.location.status == LocationServiceStatus.Failed){
          //Debug.Log("Unable to dtermine device location");
          DebugText.text += " FAILED: UNKNOWN\n";
          yield break;
        } else {
          DebugText.text += " SUCSESS!\n";
          yield return new WaitForSeconds(1);
          SceneManager.LoadScene(TargetScene);
        }
      }

    } else {
      DebugText.text += "\nFAILED! User must enable location!";
    }
  }

  // Update is called once per frame
  void Update(){}
}
