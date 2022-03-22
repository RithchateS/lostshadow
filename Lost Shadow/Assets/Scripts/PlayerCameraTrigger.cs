using System.Collections;
using Cinemachine;
using Identifier;
using UnityEngine;

public class PlayerCameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private bool _cameraSizeIsChangable;
    private int _orthoSize;

    private void Start()
    {
        _cameraSizeIsChangable = true;
        virtualCamera = GameObject.Find("MainCineCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Trigger"))
        {
            _orthoSize = col.GetComponent<OrthoSizeTrigger>().orthoSize;
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
        if (virtualCamera.m_Lens.OrthographicSize < _orthoSize)
        {
            _cameraSizeIsChangable = false;
            while (virtualCamera.m_Lens.OrthographicSize < _orthoSize)
            {
                virtualCamera.m_Lens.OrthographicSize += 0.1f;
                yield return new WaitForSeconds(0.01f);
            }

            virtualCamera.m_Lens.OrthographicSize = Mathf.Round(virtualCamera.m_Lens.OrthographicSize);
            _cameraSizeIsChangable = true;
        }
        else if (virtualCamera.m_Lens.OrthographicSize > _orthoSize)
        {
            _cameraSizeIsChangable = false;
            while (virtualCamera.m_Lens.OrthographicSize > _orthoSize)
            {
                virtualCamera.m_Lens.OrthographicSize -= 0.1f;
                yield return new WaitForSeconds(0.01f);
            }

            virtualCamera.m_Lens.OrthographicSize = Mathf.Round(virtualCamera.m_Lens.OrthographicSize);
            _cameraSizeIsChangable = true;
        }
    }

}
