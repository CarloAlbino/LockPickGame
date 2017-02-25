using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLockController : Singleton<InGameLockController> {

    [SerializeField]
    private InGameLock[] m_locks;
    private Dictionary<int, bool> m_lockState = new Dictionary<int, bool>();

	void Start ()
    {
		for(int i = 0; i < m_locks.Length; i++)
        {
            m_lockState.Add(m_locks[i].GetID(), false);
        }
	}
	
    public void SaveLockState(int lockID, bool state)
    {
        m_lockState[lockID] = state;
    }

    public bool GetLockState(int lockID)
    {
        return m_lockState[lockID];
    }
}
