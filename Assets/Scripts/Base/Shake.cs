using System.Collections;
using UnityEngine;

namespace Base
{
    public class Shake : MonoBehaviour
    {
        [SerializeField] private float duration = 0.05f;
        [SerializeField] [Range(0.0f, 1.0f)] private float strength = 0.05f;

        public IEnumerator ShakeCamera()
        {
            var original = transform.position;

            var timeLeft = duration;

            while (timeLeft > 0)
            {
                transform.position = GetRandomShakePosition(original);
                timeLeft -= Time.deltaTime;
                yield return null;
            }

            transform.position = original;
        }

        private Vector3 GetRandomShakePosition(Vector3 original)
        {
            return new Vector3(
                original.x + Random.Range(-1.0f, 1.0f) * strength,
                original.y + Random.Range(-1.0f, 1.0f) * strength,
                original.z);
        }
    }
}