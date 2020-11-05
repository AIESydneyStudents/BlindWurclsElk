using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{

    void Start()
    {
        Invoke("Disable", 0.05f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}