using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This file exists as a wrapper for easy serialization of POI objects. Basically,
 * this class is just a wrapper for that POI array which then gets written and
 * read from the JSON.
 */
[System.Serializable]
public class POIS{
  public POI[] data;
}
