using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LockSceneController : Singleton<LockSceneController>{

    [SerializeField]
    private int m_lockLevelIndex = 1;
    private int m_lastGameSceneIndex = 0;

    private PinColour[] m_pinColours;
    private bool[] m_isSmoke;
    private float m_speed;
    private LockController m_lock;

    private int m_lastLockID;
    private Transform m_lastPlayerPos;

    void OnLevelWasLoaded()
    {
        Debug.Log("Level was loaded");

        m_lock = FindObjectOfType<LockController>();

        if(m_lock != null)
        {
            // Transfer lock info to lock
            m_lock.SetLockColours(m_pinColours, m_isSmoke, m_speed);
        }
        else
        {
            // Find player and move them to the last position
            FindObjectOfType<PlayerRaycast>().transform.position = m_lastPlayerPos.position;
        }
    }

    public void SetLock(PinColour[] pinColours, bool[] isSmoke, float speed)
    {
        m_pinColours = pinColours;
        m_isSmoke = isSmoke;
        m_speed = speed;
    }

    public void GoToLock(int lastLockID, Transform lastPlayerPos)
    {
        m_lastLockID = lastLockID;
        m_lastPlayerPos = lastPlayerPos;
        m_lastGameSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(m_lockLevelIndex);
    }

    public void Win()
    {
        InGameLockController.Instance.SaveLockState(m_lastLockID, true);
        SceneManager.LoadScene(m_lastGameSceneIndex);
    }

    public void Lose()
    {
        InGameLockController.Instance.SaveLockState(m_lastLockID, false);
        SceneManager.LoadScene(m_lastGameSceneIndex);
    }

}
