using System.Collections;
using UnityEngine;

namespace Enemies.Hexagon
{
    public class TemporalLight : MonoBehaviour
    {
        [SerializeField] private float time;

        [SerializeField] private float offset;
        [SerializeField] private bool activeAtStart = true;
        [SerializeField] private GameObject particles;
        [SerializeField] private GameObject trigger;

        private bool _active;

        private MeshRenderer _renderer;

        private IEnumerator Start()
        {
            _active = activeAtStart;
            _renderer = GetComponent<MeshRenderer>();
            yield return new WaitForSeconds(offset);
            yield return SwitchState();
        }

        private IEnumerator SwitchState()
        {
            while (true)
            {
                yield return new WaitForSeconds(time);
                _active = !_active;
                _renderer.enabled = _active;
                trigger.SetActive(_active);
                particles.SetActive(_active);
            }

            // ReSharper disable once IteratorNeverReturns
        }
    }
}