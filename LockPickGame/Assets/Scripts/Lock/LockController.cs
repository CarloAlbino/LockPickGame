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
    private int m_timeRemaining = 0;

    private bool m_hasChangedPins = false;
    private bool[] m_changedPins;
    private bool m_canPick = false;
    private int m_lastBump = -1;
    private float m_pickSpeed;

    private LockPick m_lockPick;
    private GameObject m_lockCamera;
    private InGameLock m_lastInGameLock;
    private Coroutine m_wTrCoroutine;
    private AudioSource m_audio;
    private Coroutine m_timeCoroutine;

    // Opening cutscene
    [SerializeField]
    private Transform m_startPos;
    [SerializeField]
    private Transform m_endPos;
    [SerializeField]
    private float m_cameraSpeed;
    private bool m_cameraCutscene = false;
    private bool m_openingCutscene = true;

    void Start()
    {
        m_lockPick = GetComponentInChildren<LockPick>();
        m_lockCamera = GetComponentInChildren<Camera>().gameObject;
        m_lockCamera.SetActive(false);
        m_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckForCompletion();

        if(m_openingCutscene)
        {
            LockCutScene(m_lockCamera.transform, m_endPos);
        }
        else
        {
            LockCutScene(m_lockCamera.transform, m_startPos);
        }

        if(m_timeRemaining <= 0)
        {
            // Time out
            if(m_lastInGameLock != null)
                m_lastInGameLock.FailedLock();
            ResetLock(false);
            if (m_timeCoroutine != null)
            {
                StartCoroutine(StartCutscene());
                m_canPick = false;     // Makes lock pick leave lock
                StopCoroutine(m_timeCoroutine);
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
                    m_audio.Play();
                    // Unlock lock
                    m_canPick = false;     // Makes lock pick leave lock
                    m_lastInGameLock.UnlockedLock();
                    ResetLock(true);
                    StartCoroutine(StartCutscene());
                    StopCoroutine(m_timeCoroutine);
                }

                if (p.Lose())
                {
                    // Did not unlock lock
                    m_canPick = false;    // Makes lock pick leave lock
                    m_lastInGameLock.FailedLock();
                    ResetLock(false);
                    StartCoroutine(StartCutscene());
                    StopCoroutine(m_timeCoroutine);
                }
            }
        }
    }

    public bool CanPick()
    {
        return m_canPick;
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
        if (m_canPick)
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

    public void StartLockPicking(PinColour[] pinColours, bool[] isSmoke, float speed, bool randomized, int timeRemaining, InGameLock lastLock)
    {
        m_lastInGameLock = lastLock;
        m_timeRemaining = timeRemaining;

        m_pinColours = pinColours;
        m_isSmoke = isSmoke;
        m_pickSpeed = speed;

        m_lockPick.SetPickSpeed(m_pickSpeed);
        m_lockPick.SetRandomizedDirections(randomized);

        for (int i = 0; i < m_pins.Length; i++)
        {
            m_pins[i].SetPinColour(m_pinColours[i]);
            if (m_isSmoke[i])
            {
                m_smokeObjects[i].Play();
            }
            else
            {
                m_smokeObjects[i].Stop();
            }
        }

        m_changedPins = new bool[m_pins.Length];

        // Show lock
        m_lockCamera.transform.position = m_startPos.position;
        m_lockCamera.transform.rotation = m_startPos.rotation;
        m_cameraCutscene = true;
        m_canPick = true;
        m_openingCutscene = true;

        if(m_timeCoroutine != null)
        {
            StopCoroutine(m_timeCoroutine);
        }
        m_timeCoroutine = StartCoroutine(Timer());
    }

    private void ResetLock(bool isWin)
    {
        for(int i = 0; i < m_pins.Length; i++)
        {
            m_pins[i].ResetPin(isWin);
            m_smokeObjects[i].Stop();
        }
    }

    public int GetTime()
    {
        return m_timeRemaining;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        m_timeRemaining--;

        if(m_timeRemaining > 0)
        {
            if (m_timeCoroutine != null)
            {
                StopCoroutine(m_timeCoroutine);
            }
            m_timeCoroutine = StartCoroutine(Timer());
        }
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(2);
        m_cameraCutscene = true;
        m_openingCutscene = false;
    }

    private void LockCutScene(Transform start, Transform end)
    {
        if (m_cameraCutscene)
        {
            if (m_openingCutscene)
            {
                m_lockCamera.SetActive(true);
            }
            Vector3 newPos = Vector3.Lerp(start.position, end.position, m_cameraSpeed * Time.deltaTime);
            Quaternion newRot = Quaternion.Lerp(start.rotation, end.rotation, m_cameraSpeed * Time.deltaTime);
            start.transform.position = newPos;
            start.transform.rotation = newRot;

            if (Vector3.Distance(start.position, end.position) < 2)
            {
                Debug.Log("End");
                m_cameraCutscene = false;
                if(!m_openingCutscene)
                {
                    m_lockCamera.SetActive(false);
                }
            }
        }
    }
}
