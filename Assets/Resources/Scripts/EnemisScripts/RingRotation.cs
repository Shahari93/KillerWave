using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rotate the ring of the enemy over time
/// </summary>
public class RingRotation : MonoBehaviour
{
    [SerializeField] private float ringRotationSpeed = 0f;
    void Update()
    {
        transform.Rotate(Vector3.left * Time.deltaTime * ringRotationSpeed);
    }
}
