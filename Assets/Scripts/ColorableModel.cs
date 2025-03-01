using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableModel : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    
    public void SetColor(Color color)
    {
        meshRenderer.material.color = color;
    }
}
