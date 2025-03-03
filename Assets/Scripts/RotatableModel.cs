using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableModel : MonoBehaviour
{
    [SerializeField] private Transform rotatableTransform;
    public Transform RotatableTransform => rotatableTransform;
}
