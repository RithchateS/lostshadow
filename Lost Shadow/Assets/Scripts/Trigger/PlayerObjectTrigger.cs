using System.Collections;
using Controller;
using Data;
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
            _playerController = transform.parent.GetComponent<PlayerController>();
            if (_bronzeBlock == null || _silverBlock == null || _goldBlock == null)
            {
                
            }
        }
        

        IEnumerator ObjectCamAnimation(GameObject object1, AudioClipData audioClipData)
        {
            while (_objectCamMask.sizeDelta.x < 500)
            {
                _objectCam.sizeDelta += new Vector2(15,11);
                _objectCamMask.sizeDelta += new Vector2(14,10);
                yield return new WaitForSeconds(0.01f);
            }
            object1.GetComponent<Animator>().SetBool("Broken", true);
            SoundManager.Instance.PlayEffect(audioClipData.GetAudioClip(1),0.3f);
            yield return new WaitForSeconds(3);
            while (_objectCamMask.sizeDelta.x > 0)
            {
                _objectCam.sizeDelta -= new Vector2(15,11);
                _objectCamMask.sizeDelta -= new Vector2(14,10); 
                yield return new WaitForSeconds(0.01f);
            }
            
        }

        IEnumerator BreakDelay(GameObject object1, AudioClipData audioClipData,int objectID)
        {
            switch (objectID)
            {
                case 1 or 2 or 3:
                    yield return new WaitForSeconds(audioClipData.GetAudioClip(1).length);
                    object1.GetComponent<Animator>().SetBool("Broken", true); 
                    SoundManager.Instance.PlayEffect(audioClipData.GetAudioClip(1),0.3f);
                    break;
                case 4:
                    yield return new WaitForSeconds(audioClipData.GetAudioClip(1).length);
                    object1.GetComponent<Animator>().SetTrigger("Unlocked");
                    SoundManager.Instance.PlayEffect(audioClipData.GetAudioClip(1),0.3f);
                    break;
                    
            }

            
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Object"))
            {
                _objectID = col.gameObject.GetComponent<ObjectID>().objectID;
                _isShadow = col.gameObject.GetComponent<ObjectID>().isShadow;
                AudioClipData audioClipData = col.GetComponent<AudioClipData>();
                switch (_objectID)
                {
                    case 1: //Bronze Key
                        SoundManager.Instance.PlayEffect(audioClipData.GetAudioClip(0),0.3f);
                        if (_isShadow == _bronzeBlock.GetComponent<ObjectID>().isShadow)
                        {
                            StartCoroutine(_playerController.PauseMovement(5));
                            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(_bronzeBlock);
                            LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", true);
                            StartCoroutine(BreakDelay(_bronzeBlock, audioClipData, _objectID));
                        }
                        else
                        {
                            StartCoroutine(ObjectCamAnimation(_bronzeBlock,audioClipData));
                            LevelManager.Instance.objectCamera.transform.position = _bronzeBlock.transform.position;
                        }
                        Destroy(col.gameObject);
                        break;
                    case 2: //Silver Key
                        SoundManager.Instance.PlayEffect(audioClipData.GetAudioClip(0),0.3f);
                        if (_isShadow == _silverBlock.GetComponent<ObjectID>().isShadow)
                        {
                            StartCoroutine(_playerController.PauseMovement(5));
                            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(_silverBlock);
                            LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", true);
                            StartCoroutine(BreakDelay(_silverBlock, audioClipData,_objectID));
                            
                        }
                        else
                        {
                            StartCoroutine(ObjectCamAnimation(_silverBlock,audioClipData));
                            LevelManager.Instance.objectCamera.transform.position = _silverBlock.transform.position;
                        }
                        Destroy(col.gameObject);
                        break;
                    case 3: //Gold Key
                        SoundManager.Instance.PlayEffect(audioClipData.GetAudioClip(0),0.3f);
                        if (_isShadow == _goldBlock.GetComponent<ObjectID>().isShadow)
                        {
                            StartCoroutine(_playerController.PauseMovement(5));
                            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(_goldBlock);
                            LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", true);
                            StartCoroutine(BreakDelay(_goldBlock, audioClipData,_objectID));
                        }
                        else
                        {
                            StartCoroutine(ObjectCamAnimation(_goldBlock,audioClipData));
                            LevelManager.Instance.objectCamera.transform.position = _goldBlock.transform.position;
                        }
                        Destroy(col.gameObject);
                        break;
                    case 4: //Holy
                        Destroy(col.GetComponent<BoxCollider2D>());
                        SoundManager.Instance.PlayEffect(audioClipData.GetAudioClip(0),0.3f);
                        _playerController.ModifyShiftCount(2);
                        StartCoroutine(BreakDelay(col.gameObject, audioClipData,_objectID));
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
                        StartCoroutine(GameSaveManager.Instance.GoNextScene(4));
                        break;
                    case 2002: //To Scenes01
                        StartCoroutine(GameSaveManager.Instance.GoNextScene(5));
                        break;
                    case 2003: //To Scenes02
                        StartCoroutine(GameSaveManager.Instance.GoNextScene(6));
                        break;
                    case 2004: //To Scenes03
                        StartCoroutine(GameSaveManager.Instance.GoNextScene(7));
                        break;
                    case 2005: //To Scenes04
                        StartCoroutine(GameSaveManager.Instance.GoNextScene(8));
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
