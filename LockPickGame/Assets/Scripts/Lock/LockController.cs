using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField]
    private PinColour[] m_pinColours;
    [SerializeField]
    private bool[] m_isSmoke;
    [SerializeField]
    private ParticleSystem[] m_smokeObjects;
    [SerializeField]
    private LockPin[] m_pins;

    private bool m_hasChangedPins = false;
    private bool[] m_changedPins;
    private bool m_isComplete = false;
    private int m_lastBump = -1;
    private float m_pickSpeed;

    private LockPick m_lockPick;
    private GameObject m_lockCamera;
    private InGameLock m_lastInGameLock;
    private Coroutine m_wTrCoroutine;

    // Opening cutscene
    [SerializeField]
    private Transform m_startPos;
    [SerializeField]
    private Transform m_endPos;
    [SerializeField]
    private float m_cameraSpeed;
    private bool m_canMove = false;

    void Start()
    {
        m_lockPick = GetComponentInChildren<LockPick>();
        m_lockCamera = GetComponentInChildren<Camera>().gameObject;
        m_lockCamera.SetActive(false);
    }

    void Update()
    {
        CheckForCompletion();

        if(m_canMove)
        {
            Vector3 newPos = Vector3.Lerp(m_lockCamera.transform.position, m_endPos.position, m_cameraSpeed * Time.deltaTime);
            Quaternion newRot = Quaternion.Lerp(m_lockCamera.transform.rotation, m_endPos.rotation, m_cameraSpeed * Time.deltaTime);
            m_lockCamera.transform.position = newPos;
            m_lockCamera.transform.rotation = newRot;

            if(Vector3.Distance(m_lockCamera.transform.position, m_endPos.position) < Mathf.Epsilon)
            {
                m_canMove = false;
            }
        }
    }

    private void CheckForCompletion()
    {
        if (m_lastInGameLock != null)
        {
            foreach (LockPin p in m_pins)
            {
                if (p.Win())
                {
                    // Unlock lock
                    m_lastInGameLock.UnlockedLock();
                    EndLockPickMode();
                }

                if (p.Lose())
                {
                    // Did not unlock lock
                    m_lastInGameLock.FailedLock();
                    EndLockPickMode();
                }
            }
        }
    }

    public bool IsComplete()
    {
        return m_isComplete;
    }

    public void Pick()
    {
        m_pins[m_lockPick.GetCurrentPin()].Pick();
    }

    public void SetRedToWhite()
    {
        for (int i = 0; i < m_pins.Length; i++)
        {
            if (m_pins[i].GetPinColour() == PinColour.Red)
            {
                m_pins[i].SetPinColour(PinColour.White);
                m_hasChangedPins = true;
                m_changedPins[i] = true;
            }
        }
        if(m_hasChangedPins)
        {
            if(m_wTrCoroutine != null)
            {
                StopCoroutine(m_wTrCoroutine);
            }
            m_wTrCoroutine = StartCoroutine(WhiteToRed());
        }
    }

    private IEnumerator WhiteToRed()
    {
        yield return new WaitForSeconds(3.0f);
        if (!m_isComplete)
        {
            for (int i = 0; i < m_changedPins.Length; i++)
            {
                if (m_changedPins[i] == true)
                {
                    m_pins[i].SetPinColour(PinColour.Red);
                }
            }
        }
    }

    public void Bump(int pinIndex)
    {
        if(m_lastBump != pinIndex)
            m_pins[pinIndex].Spark();

        m_lastBump = pinIndex;
    }

    public void StartLockPicking(PinColour[] pinColours, bool[] isSmoke, float speed, InGameLock lastLock)
    {
        m_lastInGameLock = lastLock;

        m_pinColours = pinColours;
        m_isSmoke = isSmoke;
        m_pickSpeed = speed;

        m_lockPick.SetPickSpeed(m_pickSpeed);

        for (int i = 0; i < m_pins.Length; i++)
        {
            m_pins[i].SetPinColour(m_pinColours[i]);
            if (m_isSmoke[i])
            {
                m_smokeObjects[i].Play();
            }
        }

        m_changedPins = new bool[m_pins.Length];

        // Show lock
        m_lockCamera.transform.position = m_startPos.position;
        m_lockCamera.transform.rotation = m_startPos.rotation;
        m_lockCamera.SetActive(true);
        m_canMove = true;
    }

    private void EndLockPickMode()
    {
        m_lockCamera.SetActive(false);
        m_isComplete = true;
        foreach (LockPin pin in m_pins)
        {
            pin.ResetPin();
        }
    }
}
