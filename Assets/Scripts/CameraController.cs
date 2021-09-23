using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    private Vector3 _destination;

    private Vector3 _startPos;

    private void Start()
    {
        _destination = transform.position;
        _startPos = transform.position;
    }

    private void Update()
    {
        if (transform.position == _destination) return;
        var position = transform.position;
        position = Vector3.Lerp(position, _destination, Time.deltaTime * speed);
        transform.position = position;
    }

    public void SetDestination(Vector3 point)
    {
        _destination = point;
    }

    public void Reset()
    {
        _destination = _startPos;
        transform.position = _startPos;
    }
}