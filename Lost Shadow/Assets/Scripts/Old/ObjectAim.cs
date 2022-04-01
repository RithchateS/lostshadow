using UnityEngine;
using Cinemachine;

public class ObjectAim : MonoBehaviour
{ 
    GameObject objectTarget;
    CinemachineVirtualCamera _cam;

    void Start()
    {
        _cam = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectTarget != null)
        {
            _cam.m_Follow = objectTarget.transform;
        }
        
    }

    public void GameObjectToTarget(GameObject target)
    {
        objectTarget = target;
        if (objectTarget != null)
        {
            _cam.m_Follow = objectTarget.transform;
        }
    }
    
}
