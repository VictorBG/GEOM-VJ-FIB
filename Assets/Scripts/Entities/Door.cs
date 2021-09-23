using System.Collections;
using UnityEngine;

namespace Entities
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private float duration;

        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void Open()
        {
            StartCoroutine(Dissolve());
        }

        private IEnumerator Dissolve()
        {
            // TODO: update property id
            var id = Shader.PropertyToID("Vector1_4CB02DBB"); // ShaderTime
            var d = 0f;
            while (d < duration)
            {
                _renderer.material.SetFloat(id, d.Remap(0, duration, 0, 1));
                d += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}