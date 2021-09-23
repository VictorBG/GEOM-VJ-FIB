using TMPro;
using UnityEngine;

namespace Enemies.Basic
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private float timeToLive;
        [SerializeField] private float speed;
        [SerializeField] private Color critialColor;
        private TextMeshPro _textMeshPro;

        private float _ttl;
        private Color _color;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshPro>();
            _color = _textMeshPro.color;
        }

        public void Setup(int dmg, bool critical)
        {
            _textMeshPro.SetText(dmg.ToString());
            _ttl = timeToLive;

            if (critical)
            {
                _color = critialColor;
            }

            _textMeshPro.color = _color;
        }

        private void Update()
        {
            transform.position += new Vector3(speed, speed) * Time.deltaTime;

            _ttl -= Time.deltaTime;
            if (!(_ttl < 0)) return;
            _color.a -= Time.deltaTime;
            _textMeshPro.color = _color;

            if (_color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}