using System;
using Controller;
using Identifier;
using Old.Manager;
using UnityEngine;

namespace Trigger
{
    public class PlayerObjectTrigger : MonoBehaviour
    {
        private int _objectID;
        private int _doorID;
        private GameObject _bronzeBlock;
        private GameObject _silverBlock;
        private GameObject _goldBlock;
        private PlayerController _playerController;

        private void Start()
        {
            _bronzeBlock = GameObject.Find("BronzeSquare");
            _silverBlock = GameObject.Find("SilverSquare");
            _goldBlock = GameObject.Find("GoldSquare");
            _playerController = gameObject.GetComponent<PlayerController>();
            if (_bronzeBlock == null || _silverBlock == null || _goldBlock == null)
            {
                
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Object"))
            {
                _objectID = col.gameObject.GetComponent<ObjectID>().objectID;
                switch (_objectID)
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
                    case 4:
                        _playerController.ModifyShiftCount(1);
                        Destroy(col.gameObject);
                        break;
                }
            }
            if (col.CompareTag("Interact"))
            {
                _objectID = col.gameObject.GetComponent<ObjectID>().objectID;
                switch (_objectID)
                {
                    case 1001:
                        _playerController.colliderID = 1001;
                        break;
                }
            }

            if (col.CompareTag("TriggerObject"))
            {
                _objectID = col.gameObject.GetComponent<ObjectID>().objectID;
                switch (_objectID)
                {
                    case 2001: //To Tutorial02
                        GameSaveManager.Instance.GoNextScene(4);
                        break;
                    case 2002: //To Scenes01
                        GameSaveManager.Instance.GoNextScene(5);
                        break;
                    case 2003: //To Scenes02
                        GameSaveManager.Instance.GoNextScene(6);
                        break;
                    case 2004: //To Scenes03
                        GameSaveManager.Instance.GoNextScene(7);
                        break;
                    case 2005: //To Scenes04
                        GameSaveManager.Instance.GoNextScene(8);
                        break;
                    case 3001: //Death
                        _playerController.ModifyAlive(false);
                        break;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Interact"))
            {
                _objectID = col.gameObject.GetComponent<ObjectID>().objectID;
                switch (_objectID)
                {
                    case 1000:
                        _playerController.colliderID = 0;
                        break;
                }
            }
        }
    }
}
