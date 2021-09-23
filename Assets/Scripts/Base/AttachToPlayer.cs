using UnityEngine;

namespace Base
{
    public class AttachToPlayer : MonoBehaviour
    {
        private GameObject _player;
        private bool _isPlayerNotNull;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _isPlayerNotNull = _player != null;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isPlayerNotNull)
            {
                transform.position = _player.transform.position;
            }
        }
    }
}