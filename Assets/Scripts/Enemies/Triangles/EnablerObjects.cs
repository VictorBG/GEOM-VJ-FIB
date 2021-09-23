using UnityEngine;

namespace Enemies.Triangles
{
    public class EnablerObjects : MonoBehaviour
    {
        [SerializeField] private TriangleShoot triangleShoot;
        [SerializeField] private bool enable;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                triangleShoot.Activated = enable;
            }
        }
    }
}