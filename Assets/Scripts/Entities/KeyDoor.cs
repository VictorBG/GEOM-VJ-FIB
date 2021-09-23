using UnityEngine;
using UnityEngine.VFX;

namespace Entities
{
    public class KeyDoor : MonoBehaviour
    {
        [SerializeField] private Door door;
        [SerializeField] public VisualEffect lootVFX;
        [SerializeField] public GameObject[] associatedKeys;

        private void Start()
        {
            lootVFX.Play();
        }

        private void OnTriggerEnter(Collider co)
        {
            if (co.gameObject.CompareTag("Player"))
            {
                AudioSource.PlayClipAtPoint(GameManager.Instance.keyAudio, transform.position, 0.1f);
                door.Open();
                lootVFX.Stop();
                Destroy(gameObject);

                foreach (var key in associatedKeys)
                {
                    Destroy(key);
                }
            }
        }
    }
}