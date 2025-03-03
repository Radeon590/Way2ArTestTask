using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ModelColorizer : MonoBehaviour
{
    [SerializeField] private ModelSpawnManager modelSpawnManager;
    [SerializeField] private Slider sliderR;
    [SerializeField] private Slider sliderG;
    [SerializeField] private Slider sliderB;
    
    private Color _currentColor = Color.white;

    private Color CurrentColor
    {
        get => _currentColor;
        set
        {
            _currentColor = value;
            UpdateModelColor();
        }
    }

    private void Start()
    {
        modelSpawnManager.OnArAnchorChanged += OnCurrentArAnchorChanged; 
        sliderR.onValueChanged.AddListener(UpdateColorR);
        sliderG.onValueChanged.AddListener(UpdateColorG);
        sliderB.onValueChanged.AddListener(UpdateColorB);
    }

    private void UpdateColorR(float value)
    {
        CurrentColor = new Color(value, _currentColor.g, _currentColor.b);
    }

    private void UpdateColorG(float value)
    {
        CurrentColor = new Color(_currentColor.r, value, _currentColor.b);
    }

    private void UpdateColorB(float value)
    {
        CurrentColor = new Color(_currentColor.r, _currentColor.g, value);
    }

    private void OnCurrentArAnchorChanged(ARAnchor currentAnchor)
    {
        if (currentAnchor is null)
        {
            return;
        }
        if (currentAnchor.TryGetComponent(out ColorableModel colorableModel))
        {
            CurrentColor = colorableModel.Color;
        }
        else if (currentAnchor.TryGetComponent(out MeshRenderer meshRenderer))
        {
            CurrentColor = meshRenderer.material.color;
        }
        else
        {
            Debug.LogError("No ColorableModel or MeshRenderer found on " + currentAnchor.name);
        }
        
        UpdateSliders();
    }
    
    private void UpdateModelColor()
    {
        if (modelSpawnManager.CurrentArAnchor.TryGetComponent(out ColorableModel colorableModel))
        {
            colorableModel.SetColor(_currentColor);
        }
        else if (modelSpawnManager.CurrentArAnchor.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.material.color = _currentColor;
        }
        else
        {
            Debug.LogError("No ColorableModel or MeshRenderer found on " + modelSpawnManager.CurrentArAnchor.name);
        }
    }

    private void UpdateSliders()
    {
        Debug.Log(JsonUtility.ToJson(CurrentColor));
        sliderR.value = CurrentColor.r;
        sliderG.value = CurrentColor.g;
        sliderB.value = CurrentColor.b;
    }
}
