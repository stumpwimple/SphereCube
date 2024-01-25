using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

    public GameObject[] sphereArray;
    public GameObject sphereExplosion;

    //public float floatCalibration;

    private GameObject hitObject;
    private MCFace hitFace;
    private bool sphereRowMatching;

    public GameObject matchFinderCollider;


    private GameObject[] spheresToDestroy = new GameObject[1000];
    private int spheresToDestroyCount = 0;

    // Use this for initialization
    void Start ()
    {
        sphereArray = GameObject.FindGameObjectsWithTag("Sphere");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0)) { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit))
            {
                hitObject = rayHit.collider.gameObject;
                hitFace = GetHitFace(rayHit);
            }
        }
        if (Input.GetMouseButton(0))
        {

            sphereArray = GameObject.FindGameObjectsWithTag("Sphere");
            float xzCameraScaler = Camera.main.orthographicSize / 15;
            float yCameraScaler = Camera.main.orthographicSize / 10;

            int yInvert = 1;
            if (Camera.main.transform.eulerAngles.x >180)
            {
                yInvert = -1;
            }

            if (hitFace == MCFace.X)
            {
                foreach (GameObject sph in sphereArray)
                {
                    if (sph.transform.position.y == hitObject.transform.position.y && sph.transform.position.z == hitObject.transform.position.z)
                    {
                        if (Camera.main.transform.eulerAngles.y >=180 && Camera.main.transform.eulerAngles.y <270)
                        {
                            Vector3 translateVector = new Vector3(-Input.GetAxis("Mouse X") - yInvert*Input.GetAxis("Mouse Y"),0f,0f);
                            sph.transform.Translate(translateVector*xzCameraScaler);
                        }
                        if (Camera.main.transform.eulerAngles.y >= 270 && Camera.main.transform.eulerAngles.y < 360)
                        {
                            Vector3 translateVector = new Vector3(Input.GetAxis("Mouse X") - yInvert * Input.GetAxis("Mouse Y"),0f,0f);
                            sph.transform.Translate(translateVector * xzCameraScaler);
                        }
                        if (Camera.main.transform.eulerAngles.y >= 0 && Camera.main.transform.eulerAngles.y < 90)
                        {
                            Vector3 translateVector = new Vector3(Input.GetAxis("Mouse X") + yInvert * Input.GetAxis("Mouse Y"),0f,0f);
                            sph.transform.Translate(translateVector * xzCameraScaler);
                        }
                        if (Camera.main.transform.eulerAngles.y >= 90 && Camera.main.transform.eulerAngles.y < 180)
                        {
                            Vector3 translateVector = new Vector3(-Input.GetAxis("Mouse X") + yInvert * Input.GetAxis("Mouse Y"),0f,0f);
                            sph.transform.Translate(translateVector * xzCameraScaler);
                        }
                    }
                }
            }

            if (hitFace == MCFace.Y)
            {
                foreach (GameObject sph in sphereArray)
                {
                    if (sph.transform.position.x == hitObject.transform.position.x && sph.transform.position.z == hitObject.transform.position.z)
                    {
                        sph.transform.Translate(new Vector3(0f, Input.GetAxis("Mouse Y")*yCameraScaler, 0f));

                    }
                }
            }

            if (hitFace == MCFace.Z)
            {
                foreach (GameObject sph in sphereArray)
                {
                    if (sph.transform.position.x == hitObject.transform.position.x && sph.transform.position.y == hitObject.transform.position.y)
                    {
                        if (Camera.main.transform.eulerAngles.y >= 180 && Camera.main.transform.eulerAngles.y < 270)
                        {
                            Vector3 translateVector = new Vector3(0f, 0f, Input.GetAxis("Mouse X") - yInvert * Input.GetAxis("Mouse Y"));
                            sph.transform.Translate(translateVector*xzCameraScaler);
                        }
                        if (Camera.main.transform.eulerAngles.y >= 270 && Camera.main.transform.eulerAngles.y < 360)
                        {
                            Vector3 translateVector = new Vector3(0f, 0f, Input.GetAxis("Mouse X") + yInvert * Input.GetAxis("Mouse Y"));
                            sph.transform.Translate(translateVector * xzCameraScaler);
                        }
                        if (Camera.main.transform.eulerAngles.y >= 0 && Camera.main.transform.eulerAngles.y < 90)
                        {
                            Vector3 translateVector = new Vector3(0f, 0f, -Input.GetAxis("Mouse X") + yInvert * Input.GetAxis("Mouse Y"));
                            sph.transform.Translate(translateVector * xzCameraScaler);
                        }
                        if (Camera.main.transform.eulerAngles.y >= 90 && Camera.main.transform.eulerAngles.y < 180)
                        {
                            Vector3 translateVector = new Vector3(0f, 0f, -Input.GetAxis("Mouse X") - yInvert * Input.GetAxis("Mouse Y"));
                            sph.transform.Translate(translateVector * xzCameraScaler);
                        }
                    }
                }
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            sphereArray = GameObject.FindGameObjectsWithTag("Sphere");

            for (int i = 0; i < spheresToDestroy.Length; i++)
            {
                spheresToDestroy[i] = null;
            }



            foreach (GameObject sph in sphereArray)
            {
                sph.transform.position = new Vector3(Mathf.RoundToInt(sph.transform.position.x), Mathf.RoundToInt(sph.transform.position.y), Mathf.RoundToInt(sph.transform.position.z));
            }

            //TODO CHECK PUZZLE STATUS HERE

            foreach (GameObject sph in sphereArray)
            {
                Vector3 center1 = sph.transform.position;
                Vector3 centerX = sph.transform.position + Vector3.right * 2;
                Vector3 centerY = sph.transform.position + Vector3.up * 2;
                Vector3 centerZ = sph.transform.position + Vector3.forward * 2;
                
                Collider[] checkSpheresX = Physics.OverlapCapsule(center1, centerX, .25f);
                Collider[] checkSpheresY = Physics.OverlapCapsule(center1, centerY, .25f);
                Collider[] checkSpheresZ = Physics.OverlapCapsule(center1, centerZ, .25f);

                //DEBUG HERE, CURRENTLY ALL SPHERES TURN YELLOW
                //Debug.Log("Checking on sphere:"+sph);

                FindMatchingSpheres(checkSpheresX);
                FindMatchingSpheres(checkSpheresY);
                FindMatchingSpheres(checkSpheresZ);
            }

            foreach (GameObject sph in spheresToDestroy)
            {
                if (sph)
                {
                    Instantiate(sphereExplosion, sph.transform.position, Quaternion.identity);
                    Destroy(sph.gameObject);
                }
            }


        }

	}
    public void FindMatchingSpheres(Collider[] checkSpheres)
    {
        sphereRowMatching = true;
        if (checkSpheres.Length == 3)
        {
            foreach (Collider sphereToCheck in checkSpheres)
            {
                if (sphereToCheck.gameObject.GetComponentInChildren<MeshRenderer>().material.color != Color.red)
                {
                    sphereRowMatching = false;
                }
            }

            Debug.Log(sphereRowMatching);

            if (sphereRowMatching)
            {
                foreach (Collider sphereToCheck in checkSpheres)
                {

                    spheresToDestroy[spheresToDestroyCount++] = sphereToCheck.gameObject;
                    //sphereToCheck.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
                }
            }
        }
    }


    //Detect "Face" of Sphere Selected
    public enum MCFace
    {
        None,
        X,
        Y,
        Z
    }

    public MCFace GetHitFace(RaycastHit hit)
    {
        Vector3 incomingVec = hit.normal;
        incomingVec = new Vector3(Mathf.RoundToInt(incomingVec.x), Mathf.RoundToInt(incomingVec.y), Mathf.RoundToInt(incomingVec.z));

        if (incomingVec == new Vector3(-1, 0, 0))
            return MCFace.X;

        if (incomingVec == new Vector3(1, 0, 0))
            return MCFace.X;

        if (incomingVec == new Vector3(0, -1, 0))
            return MCFace.Y;

        if (incomingVec == new Vector3(0, 1, 0))
            return MCFace.Y;

        if (incomingVec == new Vector3(0, 0,-1))
            return MCFace.Z;

        if (incomingVec == new Vector3(0, 0, 1))
            return MCFace.Z;

        return MCFace.None;
    }
}
