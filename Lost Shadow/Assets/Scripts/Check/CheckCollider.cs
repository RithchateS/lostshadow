using Old.Controller;
using UnityEngine;

namespace Controller
{
    public class CheckCollider : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
            {
                Debug.Log("true");
                player.GetComponent<PlayerController>().CheckIfInCollider(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
            {
                Debug.Log("false");
                player.GetComponent<PlayerController>().CheckIfInCollider(false);
            }
        }
    }
}