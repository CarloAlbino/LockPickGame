using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour {

    [SerializeField]
    private Camera m_playerCam;

    private LockPickingCam m_lockPickCam;

    void Start()
    {
        m_lockPickCam = GetComponent<LockPickingCam>();
    }

    void FixedUpdate()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(m_playerCam.transform.position, m_playerCam.transform.forward, out hitInfo, 10.0f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hitInfo.collider.GetComponent<InGameLock>())
                {
                    hitInfo.collider.GetComponent<InGameLock>().GoToLock();
                }
            }
        }
    }

}
