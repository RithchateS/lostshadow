using System.Collections;
using Cinemachine;
using Identifier;
using UnityEngine;

namespace Trigger
{
    public class PlayerCameraTrigger : MonoBehaviour
    {
        CinemachineVirtualCamera _mainCineCamera;
        private bool _cameraSizeIsChangable;
        private float _orthoSize;
        
        //TODO: Remove this script
        private void Start()
        {
            _orthoSize = 3;
            _cameraSizeIsChangable = true;
            _mainCineCamera = GameObject.Find("MainCineCamera").GetComponent<CinemachineVirtualCamera>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Trigger"))
            {
                _orthoSize = col.GetComponent<TriggerID>().orthoSize;
            }
        
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Trigger") && _cameraSizeIsChangable)
            {
                StartCoroutine(CameraAnimation());
            }
        
        }
        IEnumerator CameraAnimation()
        {
            if (_mainCineCamera.m_Lens.OrthographicSize < _orthoSize)
            {
                _cameraSizeIsChangable = false;
                while (_mainCineCamera.m_Lens.OrthographicSize < _orthoSize)
                {
                    _mainCineCamera.m_Lens.OrthographicSize += 0.1f;
                    yield return new WaitForSeconds(0.01f);
                }

                _mainCineCamera.m_Lens.OrthographicSize = Mathf.Round(_mainCineCamera.m_Lens.OrthographicSize);
                _cameraSizeIsChangable = true;
            }
            else if (_mainCineCamera.m_Lens.OrthographicSize > _orthoSize)
            {
                _cameraSizeIsChangable = false;
                while (_mainCineCamera.m_Lens.OrthographicSize > _orthoSize)
                {
                    _mainCineCamera.m_Lens.OrthographicSize -= 0.1f;
                    yield return new WaitForSeconds(0.01f);
                }

                _mainCineCamera.m_Lens.OrthographicSize = Mathf.Round(_mainCineCamera.m_Lens.OrthographicSize);
                _cameraSizeIsChangable = true;
            }
        }

    }
}
