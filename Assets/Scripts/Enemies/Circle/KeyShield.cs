using UnityEngine;
using UnityEngine.VFX;

namespace Enemies.Circle
{
    public class KeyShield : MonoBehaviour
    {
        [SerializeField] public VisualEffect lootVFX;
        [SerializeField] public GameObject shield;

        private Player _player;
        private bool _isPlayerNotNull;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
            _isPlayerNotNull = _player != null;
        }

        private void Start()
        {
            lootVFX.Play();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) && _isPlayerNotNull)
            {
                _player.AddShield();
            }
        }

        private void OnTriggerEnter(Collider co)
        {
            if (co.gameObject.CompareTag("Player"))
            {
                if (_isPlayerNotNull)
                {
                    _player.RemoveShield();
                    _player.AddShield();
                }
                AudioSource.PlayClipAtPoint(GameManager.Instance.keyAudio, transform.position, 0.3f);
                lootVFX.Stop();
                Destroy(gameObject);
            }
        }
    }
}