using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ModelSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject modelIsAbleToSpawnUiElements;
    [SerializeField] private GameObject modelSpawnDependentUiElements;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private ARAnchorManager arAnchorManager;
    [SerializeField] private ARRaycastManager arRaycastManager;

    public Action<ARAnchor> OnArAnchorChanged;
    private ARAnchor _currentArAnchor;
    public ARAnchor CurrentArAnchor
    {
        get => _currentArAnchor;
        set
        {
            if (_currentArAnchor != null)
            {
                Destroy(_currentArAnchor.gameObject);
            }
            if (value == null)
            {
                Destroy(_currentArAnchor.gameObject);
                modelSpawnDependentUiElements.SetActive(false);
            }
            else
            {
                modelSpawnDependentUiElements.SetActive(true);
            }
            _currentArAnchor = value;
            OnArAnchorChanged?.Invoke(_currentArAnchor);
            UpdateModelIsAbleToSpawnUiElements();
        }
    }

    private byte _planesCount = 0;

    private byte PlanesCount
    {
        get => _planesCount;
        set
        {
            _planesCount = value;
            UpdateModelIsAbleToSpawnUiElements();
        }
    }
    private bool IsPlanesDetected => PlanesCount > 0;
    
    void Start()
    {
        arPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs changes)
    {
        PlanesCount = (byte)(_planesCount + changes.added.Count - changes.removed.Count);
    }
    
    void Update()
    {
        if (IsPlanesDetected && CurrentArAnchor is null)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    if (arRaycastManager.Raycast(touch.position, hits, TrackableType.Planes))
                    {
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            Debug.LogWarning("Pointer is over game object");
                            return;
                        }
                        Pose hitPose = hits[0].pose;

                        ARPlane plane = hits[0].trackable as ARPlane;

                        if (plane != null)
                        {
                            CurrentArAnchor = arAnchorManager.AttachAnchor(plane, hitPose);
                        }
                    }
                }
            }
        }
    }

    public void SpawnNewModelToggle()
    {
        CurrentArAnchor = null;
    }

    private void UpdateModelIsAbleToSpawnUiElements()
    {
        if (IsPlanesDetected && CurrentArAnchor is null)
        {
            modelIsAbleToSpawnUiElements.SetActive(true);
        }
        else
        {
            modelIsAbleToSpawnUiElements.SetActive(false);
        }
    }
}
