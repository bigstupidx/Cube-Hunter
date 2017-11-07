using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSensor : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        Container.Instance.RemoveCube(other.gameObject);
    }
}
