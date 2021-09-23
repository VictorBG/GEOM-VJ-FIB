using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Entities
{
    public class Key : MonoBehaviour
    {
        [SerializeField] public VisualEffect lootVFX;
        [SerializeField] public UnityEvent collected;

        private void Start()
        {
            if (lootVFX == null)
            {
                lootVFX = GetComponentInChildren<VisualEffect>();
            }

            lootVFX.Play();
        }

        private void OnTriggerEnter(Collider co)
        {
            if (co.gameObject.CompareTag("Player"))
            {
                collected.Invoke();
                AudioSource.PlayClipAtPoint(GameManager.Instance.keyAudio, transform.position, 0.6f);
                lootVFX.Stop();
                Destroy(gameObject);
            }
        }
    }
}