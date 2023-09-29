using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialMover : MonoBehaviour
{

    public float scrollSpeed = 0.5f;

    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = rend.material.mainTextureOffset;
    }

    void Update()
    {
        // Mesh offsetini sürekli olarak hareket ettir
        offset.y += scrollSpeed * Time.deltaTime;
        rend.material.mainTextureOffset = offset;
    }
}