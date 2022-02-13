using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestComponent : MonoBehaviour
{
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
    
    // Start is called before the first frame update
    void Awake()
    {

        stateDrivenCamera.m_AnimatedTarget = new Animator();
        stateDrivenCamera.m_Follow = new RectTransform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
