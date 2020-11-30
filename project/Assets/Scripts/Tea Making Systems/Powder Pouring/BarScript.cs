using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarScript : MonoBehaviour
{
    public RectTransform indicator;

    // Size of the bar to move along
    public float length;
    // How many times to move per second
    public float period = 1;

    [HideInInspector]
    public float scaledPos;


    void Update()
    {
        // PingPong moves between 0 and 1
        scaledPos = Mathf.PingPong(Time.time * period, 1);

        // Move indicator over the length of the bar
        indicator.localPosition = new Vector3(scaledPos * length - (length*0.5f), 0, 0);
    }
}