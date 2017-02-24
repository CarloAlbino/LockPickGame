using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : ActivatorObject
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

    private LockPick m_lockPick;
    private Coroutine m_wTrCoroutine;

    void Start()
    {
        m_lockPick = GetComponentInChildren<LockPick>();

        for(int i = 0; i < m_pins.Length; i++)
        {
            m_pins[i].SetPinColour(m_pinColours[i]);
            if(m_isSmoke[i])
            {
                m_smokeObjects[i].Play();
            }
        }

        m_changedPins = new bool[m_pins.Length];
    }

    void Update()
    {
        CheckForCompletion();
    }

    private void CheckForCompletion()
    {
        foreach(LockPin p in m_pins)
        {
            if(p.IsPressed())
            {
                base.Activate();
                m_isComplete = true;
                break;
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
}
