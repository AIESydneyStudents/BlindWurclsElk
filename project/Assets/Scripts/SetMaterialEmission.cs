using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SetMaterialEmission : MonoBehaviour
{
    public Color color = Color.yellow;

    public Material material;


    void Awake()
    {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color);
    }
}