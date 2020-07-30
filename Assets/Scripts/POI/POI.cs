using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This file defines a POI object which is what is read in from the JSON file
 * to create the seamless system we all love and know.
 */
[System.Serializable]
public class POI{
  public double lat;
  public double lon;
  public string name;
  public string info;
  public double rad;
  public int type;
  public string prefab;
}
