using UnityEngine;
using Cinemachine;

public class ObjectAim : MonoBehaviour
{ 
    GameObject _objectTarget;
    CinemachineVirtualCamera _cam;

    void Start()
    {
        _cam = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_objectTarget != null)
        {
            _cam.m_Follow = _objectTarget.transform;
        }
    }

    public void GameObjectToTarget(GameObject target)
    {
        _objectTarget = target;
        Debug.Log(_objectTarget.name);
        if (_objectTarget != null)
        {
            Debug.Log(_objectTarget.name);
            _cam.m_Follow = _objectTarget.transform;
        }
    }
    
}
