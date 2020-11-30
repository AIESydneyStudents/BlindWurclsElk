using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObjectScript : MonoBehaviour
{
    public float spinSpeed;
    public float speed;
    public float height;

    private Vector3 basePos;

    void Start()
    {
        basePos = transform.localPosition;
    }

    void Update()
    {
        transform.localRotation *= Quaternion.Euler(0, spinSpeed * Time.deltaTime, 0);
        transform.localPosition = basePos + new Vector3(0, Mathf.Sin(Time.time * speed) * height, 0);
    }
}