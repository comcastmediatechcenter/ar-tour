using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionFixer : MonoBehaviour{

  /*
   *  Hopefully this will fix the Android resource request problem. The #if is
   * a pre process directive to check if we are running on Android and if we are
   * to include the script bellow. I have reason to believe it would not enjoy
   * this additional code on iOS.
   */
  void Start(){
    StartCoroutine(PermissionForcer());
  }

  // Update is called once per frame
  void Update(){

  }


  private IEnumerator PermissionForcer(){
    #if PLATFORM_ANDROID
    if(!Permission.HasUserAuthorizedPermission(Permission.Camera)){
      Permission.RequestUserPermission(Permission.Camera);
    }
    yield return new WaitForSeconds(1f);
    if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation)){
      Permission.RequestUserPermission(Permission.FineLocation);
    }
    yield return new WaitForSeconds(1f);
    if(!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)){
      Permission.RequestUserPermission(Permission.CoarseLocation);
    }
    #endif
    yield return new WaitForSeconds(1f);
    Debug.Log("PERMISSIONS: \n\tCAMERA: " + Permission.HasUserAuthorizedPermission(Permission.Camera) + "\n\t LOC: " + (Permission.HasUserAuthorizedPermission(Permission.FineLocation) || Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)));
  }
}
