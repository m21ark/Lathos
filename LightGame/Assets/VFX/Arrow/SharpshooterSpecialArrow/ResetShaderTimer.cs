using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I am using this script to reset the shader timer on the shader that I am using.
// The shader is called "SharpShooterShaderGraph" and it is a custom shader that I made.
// Update the variable _Phase

public class ResetShaderTimer : MonoBehaviour
{
    public Material material;
    public float timer = 0.0f;
    void Start()
    {
        //material.SetFloat("_Phase", timer);
    }

    void Update()
    {
        timer += Time.deltaTime;
        material.SetFloat("_Timer", timer);
        material.SetFloat("_Phase", timer);
    }
}
