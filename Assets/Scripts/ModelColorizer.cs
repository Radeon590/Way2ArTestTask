using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelColorizer : MonoBehaviour
{
    [SerializeField] private ModelSelector modelSelector;
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
        modelSelector.OnCurrentModelChanged += OnCurrentModelChanged; 
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

    private void OnCurrentModelChanged(GameObject currentModel)
    {
        UpdateModelColor();
    }
    
    private void UpdateModelColor()
    {
        if (modelSelector.CurrentModel.TryGetComponent(out ColorableModel colorableModel))
        {
            colorableModel.SetColor(_currentColor);
        }
        else if (modelSelector.CurrentModel.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.material.color = _currentColor;
        }
        else
        {
            Debug.LogError("No ColorableModel or MeshRenderer found on " + modelSelector.CurrentModel.name);
        }
    }
}
