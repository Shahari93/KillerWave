using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0; 

    void Update()
    {
        transform.Rotate(Vector3.left * Time.deltaTime * rotateSpeed);
    }
}
