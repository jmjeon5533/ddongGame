using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    float rotation;
    public float addrotation = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation += addrotation;
        transform.eulerAngles = new Vector3(rotation, 0, 0);
    }
}
