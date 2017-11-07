using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooling))]
public class Container : MonoBehaviour {

    public static Container Instance { get; set; }
    public float interval = 1f;
    private float startTime;
    private ObjectPooling pool;
    


    void Start ()
    {
        Instance = this;
        pool = GetComponent<ObjectPooling>();
        startTime = Time.time;
	}
	
	void Update ()
    {
        if (Time.time - startTime >= interval)
        {
            AddCube();
            startTime = Time.time;
        }	
	}

    public void AddCube()
    {
        GameObject gObject = pool.TakeObjectFromPool();
    }

    public void AddCube(Side color)
    {
        GameObject gObject = pool.TakeObjectFromPool();
        gObject.GetComponent<Cube>().ChangeMaterial((int)color);
    }

    public void RemoveCube(GameObject gObject)
    {
        pool.AddObjectIntoPool(gObject);
    }

}
