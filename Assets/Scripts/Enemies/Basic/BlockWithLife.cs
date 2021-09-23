using System.Collections;
using UnityEngine;

namespace Enemies.Basic
{
    public class BlockWithLife : MonoBehaviour
    {
        [SerializeField] private float life;
        [SerializeField] private float hit;
        [SerializeField] private float popupScale = 1f;

        [SerializeField] private Transform damagePopup;
        [SerializeField] private GameObject destroyed;

        [SerializeField] private bool showPopup;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Hit(new Vector3(0, 0, 2.8f), 1);
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
            var gm = Instantiate(destroyed, transform.position, transform.rotation);
            for (var i = 0; i < gm.transform.childCount; i++)
            {
                var gmChildren = gm.transform.GetChild(i);
                gmChildren.GetComponent<Rigidbody>().AddExplosionForce(10.0f, transform.position, 5.0f);
                StartCoroutine(Disappear(gmChildren.gameObject));
            }

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
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

            if (showPopup)
            {
                var tr = Instantiate(damagePopup, new Vector3(pos.x, pos.y, 2.0f), Quaternion.identity);
                tr.localScale = new Vector3(popupScale, popupScale, popupScale);
                var dmgPopup = tr.GetComponent<DamagePopup>();
                dmgPopup.Setup((int) h, h > hit * 2.5f);
            }

            if (!(life <= 0)) return;
            DestroyIntoPieces();
        }
    }
}