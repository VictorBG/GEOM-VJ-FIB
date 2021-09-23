using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemies.Triangles
{
    public class TrianglesFirstKeyDoor : MonoBehaviour
    {
        [SerializeField] private GameObject[] movableObjects;
        [SerializeField] private float amount;
        [SerializeField] private Vector3 direction;
        [SerializeField] public GameObject lootObject;
        [SerializeField] public VisualEffect lootVFX;

        private void Start()
        {
            lootVFX.Play();
        }

        private void OnTriggerEnter(Collider co)
        {
            if (!co.gameObject.CompareTag("Player")) return;
            lootVFX.Stop();
            lootObject.SetActive(false);
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            foreach (var movableObject in movableObjects)
            {
                var spike = movableObject.GetComponent<Spike>();
                if (spike != null)
                {
                    spike.enabled = false;
                }
            }

            var duration = 1.5f;
            var totalDuration = duration;
            while (duration > 0f)
            {
                foreach (var movableObject in movableObjects)
                {
                    movableObject.transform.position += Vector3.down * (Time.deltaTime * amount * 1f / totalDuration);
                }

                duration -= Time.deltaTime;
                yield return null;
            }

            foreach (var movableObject in movableObjects)
            {
                Destroy(movableObject);
            }

            Destroy(gameObject);
        }
    }
}