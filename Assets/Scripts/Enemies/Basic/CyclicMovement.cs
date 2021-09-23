using UnityEngine;

namespace Enemies.Basic
{
    public class CyclicMovement : MonoBehaviour
    {
        [SerializeField] private bool horizontal;
        [SerializeField] private bool inverse;
        [SerializeField] private float min;
        [SerializeField] private float max;
        [SerializeField] private float speed;

        private Vector3 _start;

        private void Start()
        {
            _start = transform.position;
        }

        private void Update()
        {
            var d = Mathf.Sin(Time.time * speed).Remap(-1, 1, min, max);

            if (inverse)
            {
                d = Mathf.Sin(Time.time * speed).Remap(-1, 1, max, min);
            }

            transform.localPosition = new Vector3(
                horizontal ? d : _start.x,
                !horizontal ? d : _start.y,
                _start.z);
        }
    }
}