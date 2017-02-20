using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPick : MonoBehaviour {

    [SerializeField]
    private GameObject m_pick;
    [SerializeField]
    private Transform[] m_pinReferences;
    [SerializeField]
    private Transform[] m_pinPositions;
    [SerializeField]
    private float m_lockPickSpeed = 1.0f;
    [SerializeField]
    private float m_minDistance = 1.0f;
    [SerializeField]
    private string m_pickTrigger = "Pick";

    private int m_nextPin = 0;
    private bool m_isPicking = false;
    private bool m_pickAtNextPin = false;
    private bool m_isMovingForward = true;

    private Animator m_pickAnimator;

	void Start ()
    {
        m_pickAnimator = m_pick.GetComponentInChildren<Animator>();

        for(int i = 0; i < m_pinReferences.Length; i++)
        {
            Vector3 tempPos = m_pinReferences[i].position;

            tempPos.y = m_pick.transform.position.y;

            m_pinPositions[i].position = tempPos;
        }
	}

	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!m_isPicking)
            {
                m_pickAtNextPin = true;
            }
        }

        MoveLockPick();
    }

    private void MoveLockPick()
    {
        // Move/Pick
        if (!m_isPicking)
        {
            m_pick.transform.position = Vector3.Lerp(m_pick.transform.position, m_pinPositions[m_nextPin].position, m_lockPickSpeed * Time.deltaTime);

            if(Vector3.Distance(m_pick.transform.position, m_pinPositions[m_nextPin].position) < m_minDistance)
            {
                if(m_pickAtNextPin)
                {
                    m_isPicking = true;
                    m_pickAnimator.SetTrigger(m_pickTrigger);
                }

                if (m_isMovingForward)
                {
                    m_nextPin++;
                    if (m_nextPin >= m_pinPositions.Length)
                    {
                        m_isMovingForward = false;
                        m_nextPin--;
                    }
                }
                else
                {
                    m_nextPin--;
                    if (m_nextPin < 0)
                    {
                        m_isMovingForward = true;
                        m_nextPin++;
                    }
                }
            }
        }
    }

    public void SetLockPickingToFalse()
    {
        m_isPicking = false;
        m_pickAtNextPin = false;
    }
}
