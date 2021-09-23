using UnityEngine;

namespace Enemies.Basic
{
    public class Paddle : MonoBehaviour
    {
        [SerializeField] private float min;
        [SerializeField] private float max;
        [SerializeField] private float speed;
        [SerializeField] private GameObject player;

        [SerializeField] private bool horizontal;

        private Vector3 _destination;
        [SerializeField] private float destinationDelay = 0.5f;
        private float _destinationTime = -1f;

        private void Update()
        {
            if (Time.time - _destinationTime > destinationDelay)
            {
                _destinationTime = Time.time;
                var playerPosition = player.transform.position;
                var dest = Mathf.Clamp(horizontal ? playerPosition.x : playerPosition.y, min, max);
                var position = transform.position;
                _destination = new Vector3(horizontal ? dest : position.x, !horizontal ? dest : position.y, position.z);
            }

            if (transform.position == _destination) return;

            var distance = Vector3.Distance(transform.position, player.transform.position);
            var sp = speed;
            if (distance > 1)
            {
                sp /= 4;
            }

            var d = (_destination - transform.position) * (Time.deltaTime * sp);
            transform.Translate(new Vector3(
                horizontal ? d.x : 0.0f,
                !horizontal ? d.y : 0.0f,
                0.0f));
        }
    }
}