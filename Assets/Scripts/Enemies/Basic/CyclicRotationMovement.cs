using UnityEngine;

namespace Enemies.Basic
{
    public class CyclicRotationMovement : MonoBehaviour
    {
        [SerializeField] private float rotation;
        [SerializeField] private float speed;

        private void Update()
        {
            transform.Rotate(0.0f, 0.0f,  rotation * speed * Time.deltaTime);
        }
    }
}