using System.Collections;
using UnityEngine;

namespace Enemies.Triangles
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private int maxCollisions;
        [SerializeField] private GameObject deathPrefab;

        private int _collisions;

        private void Start()
        {
            _collisions = 0;
            StartCoroutine(SelfDestructAfter(10f));
        }

        private IEnumerator SelfDestructAfter(float time)
        {
            yield return new WaitForSeconds(time);
            SelfDestruct();
        }

        private void OnCollisionEnter(Collision other)
        {
            // Debug.Log("Collided with " + other.gameObject.tag);
            if (other.gameObject.CompareTag($"Player"))
            {
                SelfDestruct();
                return;
            }

            _collisions++;
            if (_collisions >= maxCollisions)
            {
                SelfDestruct();
            }
        }
        
        public void DestroyShot()
        {
            SelfDestruct();
        }

        private void SelfDestruct()
        {
            Destroy(Instantiate(deathPrefab, transform.position, transform.rotation), 10f);
            Destroy(gameObject);
        }
    }
}