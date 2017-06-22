using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCounterClock : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.up, Time.deltaTime * speed);
        Collider collider = gameObject.GetComponent<Collider>();
        transform.RotateAround(collider.bounds.center, Vector3.down, speed * Time.deltaTime);

    }
}
