using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Cube : MonoBehaviour  {

    public Side side;
    public float movementSpeed;
    public List<Material> materials;
    private ObjectPooling pool;

    void Start()
    {
        pool = Camera.main.GetComponent<ObjectPooling>();
        ChangeMaterial();
    }
    
    void Update()
    {
        Move();
    }

    public void ChangeMaterial(int materialIndex)
    {
        GetComponent<Renderer>().material = materials[materialIndex];
        side = (Side)materialIndex;
    }

    public void ChangeMaterial()
    {
        int materialIndex = RandomColor();
        ChangeMaterial(materialIndex);   
    }


    private int RandomColor()
    {
        int lastColorCount = pool.sameColorCount;
        Side lastColor = pool.sameColor;
        int index = 0;
        do
        {
            index = UnityEngine.Random.Range(0, materials.Count);

            if (pool.sameColorCount == 0)
            {
                pool.sameColorCount++;
                pool.sameColor = (Side)index;
            }
            else
            {
                if (pool.sameColor == (Side)index)
                {
                    pool.sameColorCount++;
                }
                else
                {
                    pool.sameColorCount = 1;
                    pool.sameColor = (Side)index;
                }

            }

        } while (pool.sameColorCount > 2);

        return index;
    }


    public void Move()
    {
        transform.Translate(new Vector3(0, -movementSpeed * Time.deltaTime, 0));
    }
    
}
