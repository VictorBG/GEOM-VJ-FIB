using Enemies.Basic;
using Enemies.Triangles;
using UnityEngine;

namespace Entities
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private float life;
        [SerializeField] private float popupScale = 1f;
        [SerializeField] private float hit;
        [SerializeField] private GameObject damagePopup;

        private Player _player;
        private bool _isPlayerNotNull;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
            _isPlayerNotNull = _player != null;
        }

        private void Update()
        {
            if (!GameManager.Instance.playerAlive)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag($"Projectile"))
            {
                var shoot = other.GetComponent<Shoot>();
                if (shoot != null)
                {
                    shoot.DestroyShot();

                    life -= hit;
                    if (life <= 0)
                    {
                        if (_isPlayerNotNull)
                        {
                            _player.RemoveShield();
                        }

                        Destroy(gameObject);
                    }

                    var pos = other.gameObject.transform.position;
                    var tr = Instantiate(damagePopup, new Vector3(pos.x, pos.y, 2.0f), Quaternion.identity);
                    tr.transform.localScale = new Vector3(popupScale, popupScale, popupScale);
                    var dmgPopup = tr.GetComponent<DamagePopup>();
                    dmgPopup.Setup((int) hit, false);
                    AudioSource.PlayClipAtPoint(GameManager.Instance.shieldHitAudio, pos, 0.1f);
                }
            }
        }

        public void SetPopupScale(float scale)
        {
            popupScale = scale;
        }
    }
}