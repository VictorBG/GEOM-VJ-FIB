using System;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Vector3 cameraPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!(Camera.main is null))
                Camera.main.GetComponent<CameraController>().SetDestination(cameraPoint);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!(Camera.main is null))
                Camera.main.GetComponent<CameraController>().SetDestination(cameraPoint);
        }
    }
}