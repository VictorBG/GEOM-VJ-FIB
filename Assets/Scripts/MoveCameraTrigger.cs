using UnityEngine;

public class MoveCameraTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 point1;

    [SerializeField] private Vector3 point2;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //if (!(Camera.main is null)) Camera.main.GetComponent<CameraController>().SetDestination(point1, point2);
        }
    }
}