using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    void Start()
    {
        // Get the Renderer component from the sphere
        Renderer sphereRenderer = GetComponent<Renderer>();
        
        // Set the color of the material to red
        sphereRenderer.material.color = Color.red;
    }
}