using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(LockBallBar))]
public class LockBallBarVisuals : MonoBehaviour {

    [SerializeField]
    private Transform m_ball;
    [SerializeField]
    private Transform m_activeArea;
    [SerializeField]
    private Transform m_topLimit, m_bottomLimit;

    private float m_barSize = 0;
    private float m_barPercentage = 0;
    private bool m_movingUp = true;
    private bool m_isIn = false;

    private Coroutine m_outCount;
    private LockBallBar m_data;

	void Start ()
    {
        m_data = GetComponent<LockBallBar>();	
	}
	
	void Update ()
    {
        SetActiveAreaPercentage();
        MoveActiveArea();
        IsBallInArea();
        m_data.isInActiveArea = m_isIn;
    }

    public float GetTopY()
    {
        return m_topLimit.position.y;
    }

    public float GetBottomY()
    {
        return m_bottomLimit.position.y;
    }

    private void SetActiveAreaPercentage()
    {
        // Resize the active area depending on the percentage that is set
        m_barSize = Vector3.Distance(m_topLimit.position, m_bottomLimit.position);
        m_barPercentage = m_barSize * m_data.barPercentage * 0.01f;
    }

    private void MoveActiveArea()
    {
        // Check the direction the area is moving
        if (m_activeArea.position.y + m_barPercentage * 0.5f > m_topLimit.position.y)
            m_movingUp = false;
        else if (m_activeArea.position.y - m_barPercentage * 0.5f < m_bottomLimit.position.y)
            m_movingUp = true;

        // Move the area
        if (m_movingUp)
            m_activeArea.Translate(Vector3.up * m_data.barSpeed * Time.deltaTime);
        else
            m_activeArea.Translate(Vector3.down * m_data.barSpeed * Time.deltaTime);
    }

    // Return is the ball is in the active area
    private void IsBallInArea()
    {
        if(m_ball.position.y > m_activeArea.position.y - m_barPercentage * 0.5f
            && m_ball.position.y < m_activeArea.position.y + m_barPercentage * 0.5f)
        {
            StartCoroutine(BallOutCount(m_data.maxOutTime));
        }
        else
        {
            m_isIn = true;
            if(m_outCount != null)
            {
                StopCoroutine(m_outCount);
            }
        }
    }

    // Set the flag to out after a count
    private IEnumerator BallOutCount(float outTime)
    {
        yield return new WaitForSeconds(outTime);

        m_isIn = false;
    }
}
