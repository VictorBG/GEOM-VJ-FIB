using System.Collections;
using UnityEngine;

namespace Enemies.Circle
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject pivotOrientation;
        [SerializeField] private Vector3 pivot;
        [SerializeField] private GameObject turretCanyon;
        [SerializeField] private float speed;
        [SerializeField] private Vector3 shotDirection;
        [SerializeField] private bool useShotDirection;
        [SerializeField] private bool ignoreAim;

        [SerializeField] private GameObject canyonShotPoint;
        [SerializeField] private GameObject canyon2ShotPoint;

        [SerializeField] private bool inverseAim;
        [Header("Debug")] [SerializeField] private bool log;

        [Header("Time between shoots")] [SerializeField]
        private float min;

        [SerializeField] private float max;

        [SerializeField] private bool activeAtStart = false;

        public bool Activated { get; set; }

        private float _nextShot;
        private Vector3 _startPos;

        private void Start()
        {
            Activated = activeAtStart;
            _nextShot = 0f;
            _startPos = pivotOrientation.transform.position;
            // Time.timeScale = 0.2f;
        }

        // Update is called once per frame
        private void Update()
        {
            _nextShot -= Time.deltaTime;

            if (!GameManager.Instance.playerAlive && Activated)
            {
                Activated = false;
                StartCoroutine(RestTurret());
            }

            if (Activated)
            {
                AimTurret(player.transform.position);

                if (_nextShot <= 0)
                {
                    var canyonPosition = canyonShotPoint.transform.position;
                    var canyonPosition2 = canyon2ShotPoint.transform.position;
                    var dir1 = (player.transform.position - canyonPosition).normalized;
                    var dir2 = (player.transform.position - canyonPosition2).normalized;


                    var projectile1 = Instantiate(projectile, new Vector3(canyonPosition.x, canyonPosition.y, 2.9f),
                        transform.rotation);
                    var projectile2 = Instantiate(projectile, new Vector3(canyonPosition2.x, canyonPosition2.y, 2.9f),
                        transform.rotation);
                    
                    AudioSource.PlayClipAtPoint(GameManager.Instance.shotAudio, canyonPosition, 0.2f);
                    
                    var rb1 = projectile1.GetComponent<Rigidbody>();
                    var rb2 = projectile2.GetComponent<Rigidbody>();

                    rb1.AddForce((useShotDirection ? shotDirection : dir1) * speed, ForceMode.VelocityChange);
                    rb2.AddForce((useShotDirection ? shotDirection : dir2) * speed, ForceMode.VelocityChange);

                    _nextShot = Random.Range(min, max);
                }
            }
        }

        private bool AimTurret(Vector3 destination)
        {
            if (ignoreAim)
            {
                return true;
            }

            if (log)
            {
                Debug.DrawLine(pivot, player.transform.position, Color.red);
                Debug.DrawLine(pivot, pivotOrientation.transform.position, Color.yellow);
                Debug.Log(player.transform.position.x + "   " + transform.position.x);
            }


            if (inverseAim)
            {
                if (player.transform.position.x >= transform.position.x)
                {
                    return false;
                }
            }
            else
            {
                if (player.transform.position.x <= transform.position.x)
                {
                    return false;
                }
            }

            var lineToPlayer = destination - pivot;
            var lineToPivotOrientation = pivotOrientation.transform.position - pivot;
            //SignAngle
            // var ang = Vector3.SignedAngle(lineToPlayer, lineToPivotOrientation)
            var angle = Mathf.Min(Vector3.Angle(lineToPlayer, lineToPivotOrientation), 2f);

            if (inverseAim)
            {
                if (lineToPlayer.normalized.y >= lineToPivotOrientation.normalized.y)
                {
                    // If not the object will rotate onto the other direction thus doing a 360
                    angle *= -1;
                }
            }
            else
            {
                if (lineToPlayer.normalized.y <= lineToPivotOrientation.normalized.y)
                {
                    // If not the object will rotate onto the other direction thus doing a 360
                    angle *= -1;
                }
            }

            if (log)
            {
                Debug.Log("Angle: " + angle);
            }

            if (Mathf.Abs(angle) > 0.5f)
            {
                turretCanyon.transform.RotateAround(pivot, Vector3.forward, angle);
            }
            else
            {
                return false;
            }

            return true;
        }

        private IEnumerator RestTurret()
        {
            while (AimTurret(_startPos))
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}