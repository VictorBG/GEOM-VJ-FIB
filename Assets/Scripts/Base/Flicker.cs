using System.Collections;
using UnityEngine;

namespace Base
{
    // https://answers.unity.com/questions/742466/camp-fire-light-flicker-control.html
    public class Flicker : MonoBehaviour
    {
        [SerializeField] private float MaxReduction;
        [SerializeField] private float MaxIncrease;
        [SerializeField] private float rangeIncrease;
        [SerializeField] private float RateDamping;
        [SerializeField] private float Strength;
        [SerializeField] private bool StopFlickering;

        private Light _lightSource;
        private float _baseIntensity;
        private float _baseRange;
        private bool _flickering;

        public void Reset()
        {
            MaxReduction = 0.2f;
            MaxIncrease = 0.2f;
            RateDamping = 0.1f;
            Strength = 300;
        }

        public void Start()
        {
            _lightSource = GetComponent<Light>();
            if (_lightSource == null)
            {
                Debug.LogError("Flicker script must have a Light Component on the same GameObject.");
                return;
            }

            _baseIntensity = _lightSource.intensity;
            _baseRange = _lightSource.range;
            StartCoroutine(DoFlicker());
        }

        private void Update()
        {
            if (!StopFlickering && !_flickering)
            {
                StartCoroutine(DoFlicker());
            }
        }

        private IEnumerator DoFlicker()
        {
            _flickering = true;
            while (!StopFlickering)
            {
                _lightSource.intensity = Mathf.Lerp(_lightSource.intensity,
                    Random.Range(_baseIntensity - MaxReduction, _baseIntensity + MaxIncrease),
                    Strength * Time.deltaTime);

                _lightSource.range = Mathf.Lerp(_lightSource.range,
                    Random.Range(_baseRange - rangeIncrease, _baseRange + rangeIncrease),
                    Strength * Time.deltaTime);

                yield return new WaitForSeconds(RateDamping);
            }

            _flickering = false;
        }
    }
}