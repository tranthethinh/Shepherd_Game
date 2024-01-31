using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime = 30f; 

    void Start()
    {
        
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
