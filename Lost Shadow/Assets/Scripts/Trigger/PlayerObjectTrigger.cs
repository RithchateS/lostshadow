using System.Collections;
using Controller;
using Identifier;
using Manager;
using Old.Manager;
using UnityEngine;

namespace Trigger
{
    public class PlayerObjectTrigger : MonoBehaviour
    {
        private int _objectID;
        private bool _isShadow;
        private GameObject _bronzeBlock;
        private GameObject _silverBlock;
        private GameObject _goldBlock;
        private PlayerController _playerController;
        private RectTransform _objectCam;
        private RectTransform _objectCamMask;

        private void Start()
        {
            _objectCam = LevelManager.Instance.objectCam.GetComponent<RectTransform>();
            _objectCamMask = LevelManager.Instance.objectCamMask.GetComponent<RectTransform>();
            _bronzeBlock = GameObject.Find("BronzeSquare");
            _silverBlock = GameObject.Find("SilverSquare");
            _goldBlock = GameObject.Find("GoldSquare");
            _playerController = gameObject.GetComponent<PlayerController>();
            if (_bronzeBlock == null || _silverBlock == null || _goldBlock == null)
            {
                
            }
        }
        

        IEnumerator ObjectCamAnimation(GameObject object1)
        {
            while (_objectCamMask.rect.width < 500)
            {
                _objectCam.sizeDelta += new Vector2(15,11);
                _objectCamMask.sizeDelta += new Vector2(14,10);
                yield return new WaitForSeconds(0.01f);
            }
            object1.GetComponent<Animator>().SetBool("Broken", true);
            yield return new WaitForSeconds(3);
            while (_objectCamMask.rect.width > 0)
            {
                _objectCam.sizeDelta -= new Vector2(15,11);
                _objectCamMask.sizeDelta -= new Vector2(14,10); 
                yield return new WaitForSeconds(0.01f);
            }
            
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Object"))
            {
                _objectID = col.gameObject.GetComponent<ObjectID>().objectID;
                _isShadow = col.gameObject.GetComponent<ObjectID>().isShadow;
                switch (_objectID)
                {
                    case 1: //Bronze Key
                        if (_isShadow == _bronzeBlock.GetComponent<ObjectID>().isShadow)
                        {
                            StartCoroutine(gameObject.GetComponent<PlayerController>().PauseMovement(3));
                            LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", true);
                            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(_bronzeBlock);
                            _bronzeBlock.GetComponent<Animator>().SetBool("Broken", true);
                        }
                        else
                        {
                            StartCoroutine(ObjectCamAnimation(_bronzeBlock));
                            LevelManager.Instance.objectCamera.transform.position = _bronzeBlock.transform.position;
                        }
                        Destroy(col.gameObject);
                        break;
                    case 2: //Silver Key
                        if (_isShadow == _silverBlock.GetComponent<ObjectID>().isShadow)
                        {
                            StartCoroutine(gameObject.GetComponent<PlayerController>().PauseMovement(3));
                            LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", true);
                            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(_silverBlock);
                            _silverBlock.GetComponent<Animator>().SetBool("Broken", true);
                        }
                        else
                        {
                            StartCoroutine(ObjectCamAnimation(_silverBlock));
                            LevelManager.Instance.objectCamera.transform.position = _silverBlock.transform.position;
                        }
                        Destroy(col.gameObject);
                        break;
                    case 3: //Gold Key
                        if (_isShadow == _goldBlock.GetComponent<ObjectID>().isShadow)
                        {
                            StartCoroutine(gameObject.GetComponent<PlayerController>().PauseMovement(3));
                            LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", true);
                            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(_goldBlock);
                            _silverBlock.GetComponent<Animator>().SetBool("Broken", true);
                        }
                        else
                        {
                            StartCoroutine(ObjectCamAnimation(_goldBlock));
                            LevelManager.Instance.objectCamera.transform.position = _goldBlock.transform.position;
                        }
                        Destroy(col.gameObject);
                        break;
                    case 4: //Holy
                        col.gameObject.GetComponent<Animator>().SetTrigger("Unlocked");
                        _playerController.ModifyShiftCount(2);
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
                    case 9999: //Quit Game MTFKKKKKK
                        Application.Quit();
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
                    case 1001:
                        _playerController.colliderID = 0;
                        break;
                }
            }
        }
    }
}
