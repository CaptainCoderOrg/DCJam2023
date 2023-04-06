using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BirdsEyeCam : MonoBehaviour
{
    public Shader NoRender;
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        cam.RenderWithShader(NoRender, string.Empty);
    }
}
