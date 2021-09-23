using UnityEngine;
using UnityEngine.Events;

namespace Base
{
    public class PlayerTriggerEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent triggerEnter;
        [SerializeField] private UnityEvent triggerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                triggerEnter?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                triggerExit?.Invoke();
            }
        }
    }
}