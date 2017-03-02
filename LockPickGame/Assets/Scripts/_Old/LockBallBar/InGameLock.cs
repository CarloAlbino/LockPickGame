using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLock : ActivatorObject {

    [SerializeField]
    private PinColour[] m_pinColours;
    [SerializeField]
    private bool[] m_isSmoke;
    [SerializeField]
    private float m_pickSpeed = 1.0f;

    private bool m_isLocked = false;

    private LockController m_lock;
    private PlayerRaycast m_player;

    void Start()
    {
        m_lock = FindObjectOfType<LockController>();
    }

    public void GoToLock(PlayerRaycast player)
    {
        Debug.Log("Called");
        if (!m_isLocked)
        {
            m_player = player;
            player.FreezePlayer();
            m_lock.StartLockPicking(m_pinColours, m_isSmoke, m_pickSpeed, this);
        }
    }

    public void FailedLock()
    {
        StartCoroutine(WaitForFail());
    }

    private IEnumerator WaitForFail()
    {
        yield return new WaitForSeconds(2);
        m_player.UnfreezePlayer();
        m_isLocked = true;
    }

    public void UnlockedLock()
    {
        StartCoroutine(WaitForUnlock());
    }

    private IEnumerator WaitForUnlock()
    {
        yield return new WaitForSeconds(2);
        m_player.UnfreezePlayer();
        Activate();
        m_isLocked = false;
    }

}
