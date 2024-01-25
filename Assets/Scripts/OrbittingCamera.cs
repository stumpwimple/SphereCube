using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbittingCamera : MonoBehaviour
{
    public SphereSpawner targetObject;
    public float speed_x;
    public float rotationSpeed;

    private Vector3 targetCenter;
    // Use this for initialization
    void Start()
    {
        float cubeLen = targetObject.cubeLength-1;
        targetCenter = targetObject.transform.position + new Vector3(cubeLen / 2f, cubeLen / 2f, cubeLen / 2f);
        transform.position = new Vector3(cubeLen+.5f, cubeLen+1f, cubeLen-.5f);
        transform.LookAt(targetCenter - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(targetCenter, Vector3.up, Input.GetAxis("Mouse X")*rotationSpeed);
            float xRotation = transform.localRotation.eulerAngles.x;
            if (xRotation > 180)
            {
                xRotation -= 360f;
            }
            
            
            float yMouseAxis = Input.GetAxis("Mouse Y");

            if (xRotation < 80 && xRotation > -80)
            {
                transform.RotateAround(targetCenter, -Camera.main.transform.right, yMouseAxis * rotationSpeed);
            }else if ((xRotation >= 80 && yMouseAxis>0)|| (xRotation <= -80 && yMouseAxis < 0))
            {
                transform.RotateAround(targetCenter, -Camera.main.transform.right, yMouseAxis * rotationSpeed);
            }
        }

        float zoomScroll = Input.GetAxis("Mouse ScrollWheel");
        if (zoomScroll!=0) {
            Camera.main.orthographicSize -= zoomScroll*2f;
        }
    }
}
