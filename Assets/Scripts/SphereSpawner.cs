using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour {
    public GameObject spherePrefab;
    public int cubeLength;

	// Use this for initialization
	void Start () {
        for (int x = 0; x < cubeLength; x++)
        {
            for (int y = 0; y < cubeLength; y++)
            {
                for (int z = 0; z < cubeLength; z++)
                {
                    GameObject sphereGO = Instantiate(spherePrefab,new Vector3 (x,y,z), Quaternion.identity);
                    sphereGO.transform.parent = this.transform;
                    sphereGO.transform.name = "Sphere_" + x + "." + y + "." + z;

                    if (Random.value > .8)
                    {
                        sphereGO.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                    }

                }
            }
        }

        Debug.Log("SphereCubeMade@Time:" + Time.time);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
