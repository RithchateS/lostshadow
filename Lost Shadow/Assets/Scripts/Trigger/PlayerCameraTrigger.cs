using System.Collections;
using Cinemachine;
using Identifier;
using UnityEngine;

namespace Trigger
{
    public class PlayerCameraTrigger : MonoBehaviour
    {
        CinemachineVirtualCamera _virtualCamera;
        private bool _cameraSizeIsChangable;
        private int _orthoSize;

        private void Start()
        {
            _cameraSizeIsChangable = true;
            _virtualCamera = GameObject.Find("MainCineCamera").GetComponent<CinemachineVirtualCamera>();
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
            if (_virtualCamera.m_Lens.OrthographicSize < _orthoSize)
            {
                _cameraSizeIsChangable = false;
                while (_virtualCamera.m_Lens.OrthographicSize < _orthoSize)
                {
                    _virtualCamera.m_Lens.OrthographicSize += 0.1f;
                    yield return new WaitForSeconds(0.01f);
                }

                _virtualCamera.m_Lens.OrthographicSize = Mathf.Round(_virtualCamera.m_Lens.OrthographicSize);
                _cameraSizeIsChangable = true;
            }
            else if (_virtualCamera.m_Lens.OrthographicSize > _orthoSize)
            {
                _cameraSizeIsChangable = false;
                while (_virtualCamera.m_Lens.OrthographicSize > _orthoSize)
                {
                    _virtualCamera.m_Lens.OrthographicSize -= 0.1f;
                    yield return new WaitForSeconds(0.01f);
                }

                _virtualCamera.m_Lens.OrthographicSize = Mathf.Round(_virtualCamera.m_Lens.OrthographicSize);
                _cameraSizeIsChangable = true;
            }
        }

    }
}
