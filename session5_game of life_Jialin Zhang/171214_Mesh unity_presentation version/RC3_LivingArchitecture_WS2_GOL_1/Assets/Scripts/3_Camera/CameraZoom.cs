using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes
 */ 

public class CameraZoom : MonoBehaviour
{
    //public float OrbitSensitivity = 5.0f;
    //public float OrbitStiffness = 10.0f;

    //public float ZoomSensitivity = 1.0f;
    //public float ZoomStiffness = 5.0f;

    //private Transform _pivot;
    //private Vector3 _rotation;
    //private float _distance = 50.0f;

    public int cameraCurrentZoom = 8;
    public int cameraZoomMax = 20;
    public int cameraZoomMin = 5;
    public int Sensitivity=1;
    // Use this for initialization
    void Start ()
    {
        Camera.main.orthographicSize = cameraCurrentZoom;
    }


    /// <summary>
    /// 
    /// </summary>
    private void LateUpdate()
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            if (cameraCurrentZoom < cameraZoomMax)
            {
                cameraCurrentZoom += Sensitivity;
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize + Sensitivity);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            if (cameraCurrentZoom > cameraZoomMin)
            {
                cameraCurrentZoom -= Sensitivity;
                Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize - Sensitivity);
            }
        }
    }
}
