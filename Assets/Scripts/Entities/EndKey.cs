using UnityEngine;
using UnityEngine.VFX;

namespace Entities
{
    public class EndKey : MonoBehaviour
    {
        [SerializeField] public VisualEffect lootVFX;

        private void Start()
        {
            lootVFX.Play();
        }

        private void OnTriggerEnter(Collider co)
        {
            if (co.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.EndLevel();
                lootVFX.Stop();
                Destroy(gameObject);
            }
        }
    }
}