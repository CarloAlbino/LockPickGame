using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private GameObject m_lockPanel;
    [SerializeField]
    private GameObject m_playerPanel;
    [SerializeField]
    private GameObject m_regularCursor;
    [SerializeField]
    private GameObject m_lockCursor;
    [SerializeField]
    private GameObject m_lockCamera;
    [SerializeField]
    private Text m_timeText;

    private PlayerRaycast m_player;
    private LockController m_lock;

    // Use this for initialization
    void Start ()
    {
        m_player = GetComponent<PlayerRaycast>();
        m_lock = FindObjectOfType<LockController>();
	}

	void Update ()
    {
        m_timeText.text = m_lock.GetTime().ToString();

		if(m_player.IsLookingAtLock())
        {
            m_regularCursor.SetActive(false);
            m_lockCursor.SetActive(true);
        }
        else
        {
            m_regularCursor.SetActive(true);
            m_lockCursor.SetActive(false);
        }

        if(m_lockCamera.activeInHierarchy)
        {
            m_playerPanel.SetActive(false);
            m_lockPanel.SetActive(true);
        }
        else
        {
            m_playerPanel.SetActive(true);
            m_lockPanel.SetActive(false);

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Application.LoadLevel(0);
            }
        }
	}
}
