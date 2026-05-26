using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField] private float _wallSpeed;

    [SerializeField] private Renderer _bgRenderer;

    void Update()
    {
        _bgRenderer.material.mainTextureOffset += new Vector2(_wallSpeed * Time.deltaTime, 0); 
    }
}
