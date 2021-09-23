using System.Collections;
using UnityEngine;

namespace Base
{
    public class SliderDoor : MonoBehaviour
    {
        [SerializeField] private Vector3 destination;
        [SerializeField] private float speed;

        private Vector3 _original;
        private bool _open;
        private Coroutine _moveCoroutine;

        private void Start()
        {
            _original = transform.position;
        }


        private void Update()
        {
            // For testing purposes
            if (Input.GetKeyDown(KeyCode.O))
            {
                OnOpen();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                OnClose();
            }
        }

        private IEnumerator Move(Vector3 to)
        {
            while (transform.position != to)
            {
                transform.position = Vector3.MoveTowards(transform.position, to, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            _moveCoroutine = null;
            _open = !_open;
        }

        public void OnOpen()
        {
            if (_open)
            {
                return;
            }

            StartMove(destination);
        }

        public void OnClose()
        {
            if (!_open)
            {
                return;
            }

            StartMove(_original);
        }

        private void StartMove(Vector3 to)
        {
            if (_moveCoroutine == null)
            {
                _moveCoroutine = StartCoroutine(Move(to));
            }
        }
    }
}