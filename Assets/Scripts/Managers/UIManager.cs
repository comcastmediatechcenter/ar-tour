using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour{
  //Buttons
  public Button ToMapButton;
  public Button ToARButton;
  public Button ToCUButton;

  //Initialize all the listeners
  void Start(){
    //Essentially if the button has not been initialized in the inspector dont attach a function to it
    if(ToMapButton != null) ToMapButton.onClick.AddListener(GoToMapScene);
    if(ToARButton != null) ToARButton.onClick.AddListener(GoToARScene);
    if(ToCUButton != null) ToCUButton.onClick.AddListener(GoToCUScene);
  }

void Update(){/* nothing to update, only gotta listen! */}

  //Button functions
  public void GoToARScene(){
    new WaitForSeconds(.1f); //Buffer for shaky hands?
    SceneManager.LoadScene("ARScene");
  }
  public void GoToMapScene(){
    SceneManager.LoadScene("MapScene");
  }
  public void GoToCUScene(){
    SceneManager.LoadScene("CUScene");
  }
}
