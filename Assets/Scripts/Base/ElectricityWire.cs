using System.Collections;
using UnityEngine;

namespace Base
{
    public class ElectricityWire : MonoBehaviour
    {
        [SerializeField] private Material electricityMaterial;
        [SerializeField] private Material normalMaterial;

        [SerializeField] [Range(0f, 1f)] private float randomness;
        [SerializeField] private float duration;

        private MeshRenderer _meshRenderer;
        private bool _running;
        private bool Active { get; set; }

        private void Start()
        {
            _running = false;
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = electricityMaterial;
            Active = true;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Active || _running) return;

            var random = Random.Range(0f, 1f);
            if (random <= randomness)
            {
                StartCoroutine(Animate());
            }
        }

        private IEnumerator Animate()
        {
            _running = true;
            var timeLeft = duration;
            var electric = true;
            while (timeLeft > 0)
            {
                var newState = Random.Range(0, 2) == 0;
                if (electric != newState)
                {
                    electric = newState;
                    _meshRenderer.material = electric ? electricityMaterial : normalMaterial;
                }

                timeLeft -= (Time.deltaTime + 0.05f);
                yield return new WaitForSeconds(0.05f);
            }


            _meshRenderer.material = electricityMaterial;


            yield return new WaitForSeconds(10f);
            _running = false;
        }
    }
}