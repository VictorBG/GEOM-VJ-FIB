using UnityEngine;

namespace Enemies.Hexagon
{
    public class ElectricitySound : MonoBehaviour
    {
        [SerializeField] private float minimumDistance = 3f;
        [SerializeField] private float maxVolume = 0.3f;
        [SerializeField] private GameObject audioParent;
        private AudioSource _audioSource;

        private GameObject _player;
        private bool _isPlayerNotNull;


        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _isPlayerNotNull = _player != null;
        }

        private void Start()
        {
            _audioSource = (AudioSource) audioParent.AddComponent(typeof(AudioSource));
            _audioSource.clip = GameManager.Instance.electricityAudio;
            _audioSource.spatialBlend = 0f;
            _audioSource.loop = true;
            _audioSource.volume = 0f;
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameStatus != GameManager.GameStatus.PLAYING)
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }

                return;
            }

            var distance = Vector3.Distance(audioParent.transform.position, _player.transform.position);
            if (_isPlayerNotNull &&
                distance <= minimumDistance &&
                !_audioSource.isPlaying
                && audioParent.activeSelf)
            {
                _audioSource.volume = (minimumDistance - distance).Remap(0, minimumDistance, 0, maxVolume);
                _audioSource.Play();
            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }
            }
        }
    }
}