using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ModelSelector : MonoBehaviour
{
    [SerializeField] private Text currentModelText;
    [SerializeField] private List<GameObject> modelPrefabs;
    [SerializeField] private ARAnchorManager arAnchorManager;

    public Action<GameObject> OnCurrentModelChanged;
    
    public GameObject CurrentModel
    {
        get => arAnchorManager.anchorPrefab;
        set
        {
            arAnchorManager.anchorPrefab = value;
            currentModelText.text = $"Выбранная модель: {value.name}";
            OnCurrentModelChanged?.Invoke(value);
        }
    }
    
    private sbyte _currentModelIndex = 0;

    private sbyte CurrentModelIndex
    {
        get => _currentModelIndex;
        set
        {
            _currentModelIndex = value;
            if (_currentModelIndex >= modelPrefabs.Count)
            {
                _currentModelIndex = 0;
            }

            if (_currentModelIndex < 0)
            {
                _currentModelIndex = (sbyte)(modelPrefabs.Count - 1);
            }

            CurrentModel = modelPrefabs[_currentModelIndex];
        }
    }

    private void Start()
    {
        CurrentModelIndex = 0;
    }

    public void NextModel()
    {
        CurrentModelIndex++;
    }
    
    public void PreviousModel()
    {
        CurrentModelIndex--;
    }
}
