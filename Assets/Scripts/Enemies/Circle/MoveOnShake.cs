using System.Collections;
using UnityEngine;

namespace Enemies.Circle
{
    public class MoveOnShake : MonoBehaviour
    {
        [SerializeField] private Vector3 destination1;
        [SerializeField] private Vector3 destination2;

        [SerializeField] private float speed;
        [SerializeField] private float rotation = 3f;

        private Coroutine _coroutine;

        // This should be called with a Unity event when the player collides with a wall
        public void OnShake()
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(MoveTo());
            }
        }

        private IEnumerator MoveTo()
        {
            var destination =
                Vector3.Distance(transform.position, destination1) > Vector3.Distance(transform.position, destination2)
                    ? destination1
                    : destination2;

            var rotationMultiplier = transform.position.magnitude > destination.magnitude ? 1 : -1;
            while (transform.position != destination)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                transform.Rotate(0f, rotation * rotationMultiplier, 0f);
                yield return new WaitForEndOfFrame();
            }

            _coroutine = null;
        }
    }
}