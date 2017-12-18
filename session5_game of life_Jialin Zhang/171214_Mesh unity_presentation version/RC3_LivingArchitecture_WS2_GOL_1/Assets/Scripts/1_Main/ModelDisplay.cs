using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDisplay : MonoBehaviour {

    Vector3 centerOfObject;

	// Use this for initialization
	void Start () {////////////////let the geometry spin by the center of the geometry, but not rotate by the other point
        centerOfObject = new Vector3(0, 0, 0);
        Transform[] childrenTransforms = gameObject.GetComponentsInChildren<Transform>();/////******************* create the vector of child unit
        foreach (Transform childTransform in childrenTransforms)//////////then add each child's vector (from original point to center of child voxel)
        {
            centerOfObject += childTransform.position;
        }
        centerOfObject = centerOfObject / childrenTransforms.Length;////////add all vector and then divide the vector by the numbers of childen,then we can get the vector (from original point to the center of the whole geometry)
    }
	
	// Update is called once per frame
	void Update () {        
        transform.RotateAround(centerOfObject, Vector3.up, 20 * Time.deltaTime);/////**************
    }
}
