using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using OpenTK;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class InfoManager : MonoBehaviour{
  public enum Mode {RangeMode, RaycastMode};
  public Mode DetectionMode;

  [Header("General")]
  public TextAsset POISJson;
  public GameObject displayObject;
  public Text NameText;
  public Text InfoText;
  public Text PromptText;
  public Button ExitButton;

  //[Header("Raycast Mode")]
  //public Camera ARCamera;

  [Header("Map Mode")]
  public GPSManager GPSManager;


  private POIS pois;
  private Sprite[] sprites;

  // Start is called before the first frame update
  void Start(){
    pois = JsonUtility.FromJson<POIS>(POISJson.text);
    object[] files = Resources.LoadAll("desc", typeof(Sprite));
    sprites = new Sprite[files.Length];

    for(int i = 0; i < files.Length; i++){
      sprites[i] = (Sprite) files[i];
    }

    displayObject.GetComponent<Image>().sprite = sprites[0];
    NameText.gameObject.SetActive(false);
    InfoText.gameObject.SetActive(false);
    ExitButton.gameObject.SetActive(false);

    ExitButton.onClick.AddListener(exitButtonClicked);

    if(DetectionMode == Mode.RangeMode) StartCoroutine(CheckRanges());
    else if(DetectionMode == Mode.RaycastMode) StartCoroutine(RaycastCheck());
  }

  void Update(){
  }

  private int activePoi = 0;
  private bool[] lastInSphere; //= new bool[pois.data.Length];
  private bool set = false, running = true;
  private IEnumerator CheckRanges(){
    lastInSphere = new bool[pois.data.Length];
    for(int i = 0; i < lastInSphere.Length; i++) lastInSphere[i] = false;
    yield return new WaitForSeconds(0.5f);

    while(running){
      for(int i = 0; i < pois.data.Length; i++){
        Vector3d offset = GPSManager.CalculateOffset(new Vector3d(GPSManager.position), new Vector3d(pois.data[i].lat, 0, pois.data[i].lon));
        double dist = (float)Mathf.Sqrt(Mathf.Pow(Mathf.Abs((float)offset.X), 2) + Mathf.Pow(Mathf.Abs((float)offset.Z), 2));

        if((lastInSphere[i] != (dist < pois.data[i].rad)) && !set){
          /*
           *  We should have just entered a sphere for the first time in this scenario
           * since set is false, and the lastInSphere is not what we just checked.
           */
          Debug.Log("Open " + pois.data[i].name);
          lastInSphere[i] = true;
          activePoi = i;
          yield return AnimateTransition(true, pois.data[i]);
          set = true;
        }
        if((lastInSphere[i] != (dist < pois.data[i].rad)) && set){
          /*
           * In this scenario we already have the info pane out but another event has
           * triggered. This can be one of two things, either we exited the radius
           * of this object OR we have entered the radius of another object.
           */
          if(!lastInSphere[i]){
            /*
             *  This event is (currently) false. That means that we have just entered
             * the radius of this item, so we need to close what is open (since
             * set is true) and then reopen with this new POI.
             */
            Debug.Log("Closed what was open, and opened " + pois.data[i].name);
            lastInSphere[i] = true;
            activePoi = i;
            yield return AnimateTransition(false, pois.data[i]);
            yield return AnimateTransition(true, pois.data[i]);
            set = true;
           } else {
            /*
             *  This event is (currently) true, so we have just exited the radius
             * of that target object. We need to close the info pane and set everything
             * accordingly.
             */
            Debug.Log("Closed " + pois.data[i].name);
            lastInSphere[i] = false;
            activePoi = 0;
            yield return AnimateTransition(false, pois.data[i]);
            set = false;
          }
        }
      }

      //Always have to yield
      yield return new WaitForSeconds(0.1f);
    }
  }

  private IEnumerator RaycastCheck(){
    List<ARRaycastHit> raycast_hits = new List<ARRaycastHit>();
    PromptText.text = "Tap on an icon to learn more!";
    while(running){
      if(Input.touchCount > 0){

        /*
         *  For whatever reason you cannot do this on the ARCamera, and when the
         * ARCamera is being used you cannot access Camera.main. This work around,
         * as weird as it is, is the only way I could get this to work.
         */
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
          Debug.Log("HIT: " + hit.transform.gameObject);
          for(int i = 0; i < pois.data.Length; i++){
            if(pois.data[i].name == hit.transform.gameObject.name){
              if(set){
                /*
                 * Were set, so close then open!
                 */
                yield return AnimateTransition(false, pois.data[i]);
                yield return AnimateTransition(true, pois.data[i]);
                set = true;
              } else {
                /*
                 * Were not set so just open!
                 */
                yield return AnimateTransition(true, pois.data[i]);
                set = true;
              }
            }
          }
        }
      }

      //Always yield
      yield return new WaitForSeconds(.1f);
    }
  }

  private void exitButtonClicked(){
    //lastInSphere[activePoi] = false;
    //yield return AnimateTransition(false, pois.data[activePoi]);
    StartCoroutine(AnimateTransition(false, pois.data[activePoi]));
    set = false;
  }

  private IEnumerator AnimateTransition(bool closed, POI poi){
    if(closed){
      PromptText.gameObject.SetActive(false);

      for(int i = 0; i < sprites.Length; i++){
        displayObject.GetComponent<Image>().sprite = sprites[i];
        yield return new WaitForSeconds(1/30);
      }

      //paranoid
      if(poi != null){
        NameText.text = poi.name;
        InfoText.text = poi.info;
        NameText.gameObject.SetActive(true);
        InfoText.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
      }
    } else {
      NameText.gameObject.SetActive(false);
      InfoText.gameObject.SetActive(false);
      ExitButton.gameObject.SetActive(false);

      for(int i = 0; i < sprites.Length; i++){
        displayObject.GetComponent<Image>().sprite = sprites[sprites.Length - i - 1];
        yield return new WaitForSeconds(1/30);
      }

      PromptText.gameObject.SetActive(true);
    }
  }
}
