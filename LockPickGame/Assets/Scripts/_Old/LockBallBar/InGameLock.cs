using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLock : ActivatorObject {

    [SerializeField]
    private int m_lockID;
    [SerializeField]
    private Transform m_lastPlayerPos;
    [SerializeField]
    private PinColour[] m_pinColours;
    [SerializeField]
    private bool[] m_isSmoke;
    [SerializeField]
    private float m_pickSpeed = 1.0f;

    private bool m_isUnlocked = false;

    void Update()
    {
        if(!m_isUnlocked)
        {
            if(InGameLockController.Instance.GetLockState(m_lockID))
            {
                m_isUnlocked = true;
                GoToLock();
            }
        }
    }

    public void GoToLock()
    {
        if (!m_isUnlocked)
        {
            LockSceneController.Instance.SetLock(m_pinColours, m_isSmoke, m_pickSpeed);
            LockSceneController.Instance.GoToLock(m_lockID, m_lastPlayerPos);
        }
    }

    public int GetID()
    {
        return m_lockID;
    }
}
