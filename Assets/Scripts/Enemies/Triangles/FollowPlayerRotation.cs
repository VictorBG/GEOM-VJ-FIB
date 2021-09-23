using UnityEngine;

namespace Enemies.Triangles
{
    public class FollowPlayerRotation : MonoBehaviour
    {
        [SerializeField] private GameObject player;


        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        }
    }
}