using UnityEngine;

namespace Enemies.Triangles
{
    public class TriangleShoot : MonoBehaviour
    {
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 target;
        [SerializeField] private bool useTarget;
        [SerializeField] private bool onlyHorizontal;
        [SerializeField] private bool onlyVertical;
        [SerializeField] private bool inverse;

        [SerializeField] private float speed;

        [Header("Time between shoots")] [SerializeField]
        private float min;

        [SerializeField] private float max;

        public bool Activated { get; set; }

        private float _nextShot;

        private void Start()
        {
            Activated = false;
            _nextShot = 0;
        }

        private void Update()
        {
            _nextShot -= Time.deltaTime;

            if (!GameManager.Instance.playerAlive)
            {
                Activated = false;
            }

            if (Activated && _nextShot <= 0)
            {
                var dir = ((useTarget ? target : player.transform.position) - transform.position).normalized;

                var instantiate = Instantiate(projectile,
                    new Vector3(transform.position.x, transform.position.y, 2.9f),
                    transform.rotation);
                AudioSource.PlayClipAtPoint(GameManager.Instance.shotAudio, transform.position, 0.2f);
                instantiate.GetComponent<Rigidbody>()
                    .AddForce(new Vector3(onlyVertical ? 0.0f : dir.x,
                            onlyHorizontal ? 0.0f : dir.y,
                            0.0f) * (speed * (inverse ? -1 : 1)),
                        ForceMode.VelocityChange);

                _nextShot = Random.Range(min, max);
            }
        }
    }
}