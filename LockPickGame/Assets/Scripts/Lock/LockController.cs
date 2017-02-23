using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour {

    [SerializeField]
    private LockPin[] m_pins;
    [SerializeField]
    private LockPick m_lockPick;

    public void Pick()
    {
        m_pins[m_lockPick.GetCurrentPin()].Pick();
    }
}
