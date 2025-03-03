using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ColorableModel : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<Material> materials;

    public Color Color
    {
        get
        {

            if (meshRenderer != null)
            {
                return meshRenderer.material.color;
            }
            if (materials is { Count: > 0 })
            {
                return materials[0].color;
            }
            
            Debug.LogError("No MeshRenderer or materials found on " + gameObject.name);
            return Color.red;
        }
    }
    
    public void SetColor(Color color)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = color;
            
        }
        else if (materials is { Count: > 0 })
        {
            foreach (var material in materials)
            {
                material.color = color;
            }
        }
        else
        {
            Debug.LogError("No MeshRenderer or materials found on " + gameObject.name);
        }
    }
}
