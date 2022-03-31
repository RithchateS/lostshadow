using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using Controller;
using Identifier;
using Manager;
using Old.Manager;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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
        

        IEnumerator ObjectCamAnimation()
        {
            while (_objectCamMask.sizeDelta.x < 500)
            {
                _objectCam.sizeDelta += new Vector2(10,10);
                _objectCamMask.sizeDelta += new Vector2(10,10);
                //yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(3);
            while (_objectCamMask.sizeDelta.x > 0)
            {
                _objectCam.sizeDelta -= new Vector2(10,10);
                _objectCamMask.sizeDelta -= new Vector2(10,10); 
                //yield return new WaitForSeconds(0.01f);
            }
            
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Object"))
            {
                _objectID = col.gameObject.GetComponent<ObjectID>().objectID;
                switch (_objectID)
                {
                    case 1: //Bronze Key
                        StartCoroutine(ObjectCamAnimation());
                        LevelManager.Instance.objectCamera.transform.position = _bronzeBlock.transform.position;
                        _bronzeBlock.GetComponent<Animator>().SetBool("Broken", true);
                        Destroy(col.gameObject);
                        break;
                    case 2: //Silver Key
                        StartCoroutine(ObjectCamAnimation());
                        LevelManager.Instance.objectCamera.transform.position = _silverBlock.transform.position;
                        _silverBlock.GetComponent<Animator>().SetBool("Broken", true);
                        Destroy(col.gameObject);
                        break;
                    case 3: //Gold Key
                        StartCoroutine(ObjectCamAnimation());
                        LevelManager.Instance.objectCamera.transform.position = _goldBlock.transform.position;
                        _goldBlock.GetComponent<Animator>().SetBool("Broken", true);
                        Destroy(col.gameObject);
                        break;
                    case 4: //Holy
                        StartCoroutine(ObjectCamAnimation());
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
                    case 1000:
                        _playerController.colliderID = 0;
                        break;
                }
            }
        }
    }
}
