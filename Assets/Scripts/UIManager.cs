using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour{
  //Buttons
  public Button ExitButton;
  public Button ARButton;

  //Initialize all the listeners
  void Start(){
    //Essentially if the button has not been initialized in the inspector dont attach a function to it
    if(ExitButton != null) ExitButton.onClick.AddListener(GoToMapScene);
    if(ARButton != null) ARButton.onClick.AddListener(GoToARScene);
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
}
