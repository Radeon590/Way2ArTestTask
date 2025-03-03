using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotationController : MonoBehaviour
{
    [SerializeField] private ModelSpawnManager modelSpawnManager;
    [SerializeField] private float rotationSpeed = 0.2f;

    void Update()
    {
        if (modelSpawnManager.CurrentArAnchor != null)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    if (modelSpawnManager.CurrentArAnchor.TryGetComponent(out RotatableModel rotatableModel))
                    {
                        rotatableModel.RotatableTransform.Rotate(Vector3.up,
                            -touch.deltaPosition.x * rotationSpeed, Space.World);
                    }
                    else
                    {
                        Debug.LogWarning("Trying rotate model without RotatableModel component");
                    }
                }
            }
        }
    }
}