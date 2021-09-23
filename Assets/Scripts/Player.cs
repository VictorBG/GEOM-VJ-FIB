using System.Collections;
using Base;
using Enemies.Triangles;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable All

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject dieExplosionPrefab;

    [Header("Player control")] [SerializeField]
    private bool bounceX = false;

    [SerializeField] private bool bounceY = true;
    [SerializeField] private float minimumBounciness = 1.0f;

    [SerializeField] private Vector3 startPos;

    [SerializeField] private CanvasGroup spaceToStartCanvas;
    [SerializeField] private CanvasGroup youDiedCanvas;
    [SerializeField] private CanvasGroup godModeCanvas;

    [SerializeField] private AudioClip youDiedAudio;
    [SerializeField] private AudioClip collisionAudio;

    [SerializeField] private UnityEvent shaked;

    [SerializeField] private GameObject shield;

    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    private float _lastTimeKey = -1;

    private bool _overrideBounce = true;
    private bool _dead;

    // Moves the player alongside a line, space changes the direction
    private bool _isLineActivatedX;
    private bool _isLineActivatedY;

    private bool _godmode;
    private Vector3 _cameraStartPos;

    private bool _hasShield;
    private bool _canPlayCollisionAudio;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        transform.position = startPos;
        if (!(Camera.main is null))
        {
            _cameraStartPos = Camera.main.transform.position;
        }
        else
        {
            _cameraStartPos = new Vector3(startPos.x, startPos.y, 0.0f);
        }

        spaceToStartCanvas.alpha = 1;
        youDiedCanvas.alpha = 0;
        youDiedCanvas.interactable = false;
        youDiedCanvas.blocksRaycasts = false;
        godModeCanvas.alpha = 0;
        godModeCanvas.interactable = false;
        godModeCanvas.blocksRaycasts = false;
        _hasShield = false;
        _canPlayCollisionAudio = true;
    }

    private Vector3 lastVelocity;

    private void Update()
    {
        if (_dead)
        {
            return;
        }

        _velocity = _rigidbody.velocity;
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X)) && Time.time - _lastTimeKey > 0.10)
        {
            GameManager.Instance.Playing();
            spaceToStartCanvas.alpha = 0;
            spaceToStartCanvas.interactable = false;
            spaceToStartCanvas.blocksRaycasts = false;
            youDiedCanvas.alpha = 0;
            youDiedCanvas.interactable = false;
            youDiedCanvas.blocksRaycasts = false;

            if (_isLineActivatedX)
            {
                _rigidbody.velocity = new Vector3(-_rigidbody.velocity.x, 0.0f, 0.0f);
            }
            else if (_isLineActivatedY)
            {
                _rigidbody.velocity = new Vector3(0.0f, -_rigidbody.velocity.y, 0.0f);
            }
            else
            {
                _lastTimeKey = Time.time;
                var x = bounceX || _overrideBounce || Mathf.Abs(_velocity.x) < 1f
                    ? -Mathf.Max(minimumBounciness, Mathf.Abs(_velocity.x)) * Mathf.Sign(_velocity.x)
                    : Mathf.Max(minimumBounciness, Mathf.Abs(_velocity.x)) * Mathf.Sign(_velocity.x);
                var y = bounceY || _overrideBounce || Mathf.Abs(_velocity.y) < 1f
                    ? -Mathf.Max(minimumBounciness, Mathf.Abs(_velocity.y)) * Mathf.Sign(_velocity.y)
                    : Mathf.Max(minimumBounciness, Mathf.Abs(_velocity.y)) * Mathf.Sign(_velocity.y);

                _rigidbody.velocity = new Vector3(x, y, 0.0f);
            }

            var gm = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gm, 2.5f);

            if (_overrideBounce)
            {
                _overrideBounce = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            InternalRestart();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _godmode = !_godmode;
            godModeCanvas.alpha = _godmode ? 1 : 0;
        }

        if (!_isLineActivatedX && !_isLineActivatedY)
        {
            if (spaceToStartCanvas.alpha == 0f
                && (Mathf.Abs(_rigidbody.velocity.x) < 0.5f || Mathf.Abs(_rigidbody.velocity.y) < 0.5f))
            {
                var x = Mathf.Abs(_rigidbody.velocity.x) >= 0.5f
                    ? _rigidbody.velocity.x
                    : -lastVelocity.x;

                var y = Mathf.Abs(_rigidbody.velocity.y) >= 0.5f
                    ? _rigidbody.velocity.y
                    : -lastVelocity.y;

                Debug.Log(_rigidbody.velocity);
                _rigidbody.velocity = new Vector3(x, y, 0f);
            }
            else
            {
                lastVelocity = _rigidbody.velocity;
            }
        }
        else
        {
            lastVelocity = _rigidbody.velocity;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 2.9f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_dead)
        {
            return;
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Die();
            return;
        }


        if (collision.gameObject.tag.Equals("Projectile"))
        {
            // Collisions should be ignored if the user has a shield (the layer is set to avoidprojectiles
            // which does not interact with projectiles), so this code is redundant as it should
            // never runs, but just in case.
            if (_hasShield)
            {
                var shoot = collision.gameObject.GetComponent<Shoot>();
                if (shoot != null)
                {
                    shoot.DestroyShot();
                }
                else
                {
                    Destroy(shoot);
                }

                return;
            }

            Die();
            return;
        }

        if (collision.gameObject.tag.Equals("Shakeable"))
        {
            StartCoroutine(cameraObject.GetComponent<Shake>().ShakeCamera());
            if (shaked != null)
            {
                shaked.Invoke();
            }
        }

        if (_canPlayCollisionAudio)
        {
            AudioSource.PlayClipAtPoint(collisionAudio, transform.position, 0.15f);
            _canPlayCollisionAudio = false;
            StartCoroutine(ResetCanPlayCollisionAudio());
        }

        var reflection = Vector3.Reflect(_velocity, collision.contacts[0].normal).normalized * Time.deltaTime * speed;
        var y = Mathf.Max(minimumBounciness, Mathf.Abs(reflection.y)) * Mathf.Sign(reflection.y);
        var x = Mathf.Max(minimumBounciness, Mathf.Abs(reflection.x)) * Mathf.Sign(reflection.x);
        _rigidbody.velocity = new Vector3(x, y, 0.0f);
        var gm = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gm, 5f);
    }

    private IEnumerator ResetCanPlayCollisionAudio()
    {
        yield return new WaitForSeconds(0.1f);
        _canPlayCollisionAudio = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Enemy") && !_dead)
        {
            Die();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag($"LineBlockY"))
        {
            _isLineActivatedY = true;
            transform.position = new Vector3(other.gameObject.transform.position.x, transform.position.y,
                transform.position.z);
            _rigidbody.velocity = new Vector3(0.0f, Mathf.Min(10, _rigidbody.velocity.y), 0.0f);
            
        }

        if (other.CompareTag($"LineBlockX"))
        {
            _isLineActivatedX = true;
            transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y,
                transform.position.z);
            _rigidbody.velocity = new Vector3(Mathf.Min(10, _rigidbody.velocity.x), 0.0f, 0.0f);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isLineActivatedY = false;
        _isLineActivatedX = false;
    }


    private void Die()
    {
        if (_godmode)
        {
            return;
        }

        GameManager.Instance.PlayerDead();
        youDiedCanvas.alpha = 1f;
        youDiedCanvas.interactable = true;
        youDiedCanvas.blocksRaycasts = true;
        if (!(Camera.main is null)) AudioSource.PlayClipAtPoint(youDiedAudio, Camera.main.transform.position, 0.1f);
        _dead = true;
        StartCoroutine(Restart());
        _rigidbody.velocity = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        var gm = Instantiate(dieExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gm, 5f);
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(7f);
        InternalRestart();
    }

    private void InternalRestart()
    {
        _dead = false;
        GameManager.Instance.Playing();
        youDiedCanvas.alpha = 0f;
        youDiedCanvas.interactable = false;
        youDiedCanvas.blocksRaycasts = false;
        spaceToStartCanvas.alpha = 1f;
        spaceToStartCanvas.interactable = true;
        spaceToStartCanvas.blocksRaycasts = true;
        transform.position = new Vector3(0f, 0f, 2.9f);
        _rigidbody.velocity = new Vector3(0, 0);
        GetComponent<MeshRenderer>().enabled = true;
        cameraObject.GetComponent<CameraController>().Reset();
    }

    public void AddShield()
    {
        if (_hasShield)
        {
            return;
        }

        Instantiate(shield);
        AudioSource.PlayClipAtPoint(GameManager.Instance.shieldAppearAudio, transform.position, 1f);
        _hasShield = true;
        gameObject.layer = 12;
    }

    public void RemoveShield()
    {
        if (!_hasShield)
        {
            return;
        }

        _hasShield = false;
        gameObject.layer = 0;
    }
}