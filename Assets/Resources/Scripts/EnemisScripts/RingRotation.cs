using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotation : MonoBehaviour
{
    [SerializeField] private float ringRotationSpeed = 0f;
    void Update()
    {
        transform.Rotate(Vector3.left * Time.deltaTime * ringRotationSpeed);
    }
}
