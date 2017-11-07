using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectPooling : MonoBehaviour {

    public GameObject gObjectPrefab;
    public Queue<GameObject> objectPooling;

    [NonSerialized]
    public Side sameColor;
    [NonSerialized]
    public int sameColorCount = 0;

    private void Start()
    {
        objectPooling = new Queue<GameObject>();
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject gObject = Instantiate(gObjectPrefab) as GameObject;
            AddObjectIntoPool(gObject);
        }
    }

    public void AddObjectIntoPool(GameObject gObject)
    {
        gObject.SetActive(false);
        gObject.transform.position = gObjectPrefab.transform.position;
        objectPooling.Enqueue(gObject);
    }

    public GameObject TakeObjectFromPool()
    {
        GameObject gObject;

        if (objectPooling.Count <= 0)
        {
            gObject = Instantiate(gObjectPrefab) as GameObject;
            AddObjectIntoPool(gObject);
        }
        
        gObject = objectPooling.Dequeue();

       

        gObject.SetActive(true);

        return gObject;

    }

}
