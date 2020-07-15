using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CameraPositionTracker : MonoBehaviour {
  public Text text;
  public Camera camera;
  public ARSessionOrigin origin;

	// Use this for initialization
	void Start () {}

	void Update () {
    text.text = "CAMERA:\n\tPOS: " + camera.transform.position +"\n\tORIGIN: " + origin.transform.position;
	}
}
