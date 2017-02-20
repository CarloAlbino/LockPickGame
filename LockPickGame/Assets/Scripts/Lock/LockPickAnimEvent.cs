using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickAnimEvent : MonoBehaviour {

    [SerializeField]
    private LockPick m_lockPick;

    public void SetPickToFalse()
    {
        m_lockPick.SetLockPickingToFalse();
    }

}
