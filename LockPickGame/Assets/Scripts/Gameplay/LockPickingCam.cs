using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickingCam : MonoBehaviour {

    [SerializeField]
    private Camera m_lockPickCam;
    [SerializeField]
    private GameObject m_player;
    private Camera m_playerCam;
    private Transform m_lockPosition;

    private bool m_toCameraOne = false;
    private bool m_toCameraTwo = false;

	void Start ()
    {
        m_playerCam = m_player.GetComponentInChildren<Camera>();
	}
	
	void Update ()
    {
		if(m_toCameraOne)
        {
            Vector3 newPos = Vector3.Lerp(m_lockPickCam.gameObject.transform.position, m_playerCam.gameObject.transform.position, 2.0f * Time.deltaTime);
            m_lockPickCam.transform.position = newPos;
            Quaternion newRot = Quaternion.Lerp(m_lockPickCam.transform.rotation, m_playerCam.transform.rotation, Time.deltaTime);
            m_lockPickCam.transform.rotation = newRot;

            if (Vector3.Distance(m_lockPickCam.transform.position, m_playerCam.gameObject.transform.position) < 0.5f)
            {
                // Arrived at one
                m_lockPickCam.gameObject.SetActive(false);
                m_playerCam.gameObject.SetActive(true);
                m_toCameraOne = false;
            }
        }
        else if(m_toCameraTwo)
        {
            Vector3 newPos = Vector3.Lerp(m_lockPickCam.gameObject.transform.position, m_lockPosition.position, 2.0f * Time.deltaTime);
            m_lockPickCam.transform.position = newPos;
            m_lockPickCam.transform.LookAt(m_lockPickCam.transform.forward.normalized + newPos);

            if (Vector3.Distance(m_lockPickCam.transform.position, m_lockPosition.position) < 0.5f)
            {
                m_toCameraTwo = false;
            }
        }
	}

    public void CameraTwo(Transform lockPosition)
    {
        m_lockPosition = lockPosition;

        m_lockPickCam.gameObject.SetActive(true);
        m_playerCam.gameObject.SetActive(false);

        // Move camera 2 to wanted position   
        m_toCameraTwo = true;
    }

    public void CameraOne()
    {
        // Move camera 2 back to camera 1
        m_toCameraOne = true;
    }
}
