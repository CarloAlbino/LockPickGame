using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class PlayerRaycast : MonoBehaviour {

    [SerializeField]
    private Camera m_playerCam;

    private FirstPersonController m_fpsController;
    private Bloom m_bloom;
    private Blur m_blur;

    void Start()
    {
        m_fpsController = GetComponent<FirstPersonController>();
        m_bloom = GetComponentInChildren<Bloom>();
        m_blur = GetComponentInChildren<Blur>();

        UnfreezePlayer();
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
                    hitInfo.collider.GetComponent<InGameLock>().GoToLock(this);
                }
            }
        }
    }

    public void FreezePlayer()
    {
        m_fpsController.enabled = false;
        m_bloom.enabled = true;
        m_blur.enabled = true;
    }

    public void UnfreezePlayer()
    {
        m_fpsController.enabled = true;
        m_bloom.enabled = false;
        m_blur.enabled = false;
    }

}
