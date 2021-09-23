using UnityEngine;

namespace Enemies.Triangles
{
    public class Spike : MonoBehaviour
    {
        [SerializeField] private bool moveY;
        [SerializeField] private bool moveX;
        [SerializeField] private float displacement = 5f;
        private float _originalY;
        private float _originalX;

        private bool Enabled { get; set; }

        private void Start()
        {
            var position = transform.position;
            _originalY = position.y;
            _originalX = position.x;
            Enabled = true;
        }

        private void Update()
        {
            if (!Enabled) return;
            var position = transform.position;
            var perlin = Mathf.PerlinNoise(position.x / displacement + Time.time,
                position.y / displacement + Time.time);
            transform.position = new Vector3(
                moveX ? perlin.Remap(0f, 1f, 0f, .5f) + _originalX : position.x,
                moveY ? perlin.Remap(0f, 1f, 0f, .5f) + _originalY : position.y,
                position.z);
        }
    }
}