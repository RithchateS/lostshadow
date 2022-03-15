using UnityEngine;
using Cinemachine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    CinemachineStateDrivenCamera _cam;
    bool foundPlayer = false;
    bool followPlayer = false;
    void Start()
    {
        _cam = gameObject.GetComponent<CinemachineStateDrivenCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!foundPlayer) {
            playerPrefab = GameObject.FindWithTag("Player");
            if (playerPrefab != null)
            {
                foundPlayer = true;
                _cam.m_Follow = playerPrefab.transform;
            }
        }
        else {
            if (!followPlayer) {
                _cam.m_Follow = playerPrefab.transform;
                //_cam.m_AnimatedTarget = playerPrefab.GetComponent<Animator>();
            }
        }
    }
}
