using System;
using Identifier;
using UnityEngine;

namespace Trigger
{
    public class PlayerObjectTrigger : MonoBehaviour
    {
        private int _keyID;
        private int _doorID;
        private GameObject _bronzeBlock;
        private GameObject _silverBlock;
        private GameObject _goldBlock;

        private void Start()
        {
            _bronzeBlock = GameObject.Find("BronzeSquare");
            _silverBlock = GameObject.Find("SilverSquare");
            _goldBlock = GameObject.Find("GoldSquare");
            if (_bronzeBlock == null || _silverBlock == null || _goldBlock == null)
            {
                Debug.Log("Ignore This Message");
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Key"))
            {
                _keyID = col.gameObject.GetComponent<KeyID>().keyID;
                switch (_keyID)
                {
                    case 1:
                        _bronzeBlock.GetComponent<Animator>().SetBool("Broken", true);
                        Destroy(col.gameObject);
                        break;
                    case 2:
                        _silverBlock.GetComponent<Animator>().SetBool("Broken", true);
                        Destroy(col.gameObject);
                        break;
                    case 3:
                        _goldBlock.GetComponent<Animator>().SetBool("Broken", true);
                        Destroy(col.gameObject);
                        break;
                }
            }
        }
    }
}
