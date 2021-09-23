using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Animations
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Animator transition;
        [SerializeField] private float transitionTime = 1f;
        private static readonly int Start = Animator.StringToHash("Start");

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(LoadLevel(2));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(LoadLevel(3));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(LoadLevel(4));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(LoadLevel(5));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartCoroutine(LoadLevel(6));
            }
        }

        private IEnumerator LoadLevel(int level)
        {
            transition.SetTrigger(Start);

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(level);
        }

        public void Load(int i)
        {
            StartCoroutine(LoadLevel(i));
        }
    }
}