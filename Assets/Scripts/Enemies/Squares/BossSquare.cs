using System.Collections;
using Enemies.Basic;
using UnityEngine;

namespace Enemies.Squares
{
    public class BossSquare : MonoBehaviour
    {
        [SerializeField] private Material evilMaterial;
        [SerializeField] private Material normalMaterial;

        [SerializeField] private float life;
        [SerializeField] private float hit;
        [SerializeField] private float rotationSpeed = 10f;

        [SerializeField] private Transform damagePopup;
        [SerializeField] private GameObject destroyed;

        private bool _evil;
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            StartCoroutine(ChangeBand());
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Hit(new Vector3(0, 0, 2.8f), 1);
            }
        }

        private IEnumerator ChangeBand()
        {
            while (true)
            {
                var diff = 90f;
                while (diff > 0)
                {
                    var degrees = Mathf.Min(Time.deltaTime * rotationSpeed, diff);
                    transform.Rotate(0, 0, degrees);
                    diff -= degrees;
                    yield return null;
                }

                //_evil = !_evil;
                //_meshRenderer.material = _evil ? evilMaterial : normalMaterial;

                yield return new WaitForSeconds(Random.Range(3f, 12f));
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                Hit(other.contacts[0].point, other.impulse.magnitude * 2);
            }
        }

        private void DestroyIntoPieces()
        {
            var gm = Instantiate(destroyed);
            for (var i = 0; i < gm.transform.childCount; i++)
            {
                var gmChildren = gm.transform.GetChild(i);
                gmChildren.GetComponent<Rigidbody>().AddExplosionForce(10.0f, transform.position, 5.0f);
                StartCoroutine(Disappear(gmChildren.gameObject));
            }

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 15f);
        }

        private static IEnumerator Disappear(Object gameObject)
        {
            var random = Random.Range(2, 5);
            yield return new WaitForSeconds(random);
            Destroy(gameObject);
        }

        private void Hit(Vector3 pos, float magnitude)
        {
            var h = hit * magnitude;
            life -= h;
            
            var tr = Instantiate(damagePopup, new Vector3(pos.x, pos.y, 2.7f), Quaternion.identity);
            var dmgPopup = tr.GetComponent<DamagePopup>();
            dmgPopup.Setup((int) h, h > 25);

            if (!(life <= 0)) return;
            DestroyIntoPieces();
        }
    }
}