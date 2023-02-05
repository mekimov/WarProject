using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera camera;
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            camera.transform.position += camera.transform.forward;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            camera.transform.position -= camera.transform.forward;
        }
    }

}
