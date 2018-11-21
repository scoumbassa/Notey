using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

    public GameObject objectToDestroy;

    public void DestroyGameObject()
    {
        Destroy(objectToDestroy);
    }
}
