using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour {

    [SerializeField]
    private LockPin[] m_pins;

    [SerializeField]
    private PinColour[] m_pinColours;

    [SerializeField]
    private LockPick m_lockPick;

    private bool m_hasChangedPins = false;
    private bool[] m_changedPins;

    private Coroutine m_wTrCoroutine;

    void Start()
    {
        for(int i = 0; i < m_pins.Length; i++)
        {
            m_pins[i].SetPinColour(m_pinColours[i]);
        }

        m_changedPins = new bool[m_pins.Length];
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

        for(int i = 0; i < m_changedPins.Length; i++)
        {
            if(m_changedPins[i] == true)
            {
                m_pins[i].SetPinColour(PinColour.Red);
            }
        }
    }
}
