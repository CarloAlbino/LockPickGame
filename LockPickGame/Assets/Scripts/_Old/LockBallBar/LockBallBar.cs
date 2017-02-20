using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBallBar : MonoBehaviour {

    [SerializeField]
    private float m_ballGravity = 10.0f;

    private float m_maxOutTime = 3.0f;
    private float m_barPercentage = 30.0f;
    private float m_barSpeed = 0.5f;
    private bool m_isInActiveArea = false;

    public float ballGravity { get { return m_ballGravity; } }

    public float maxOutTime { get { return m_maxOutTime; } set { m_maxOutTime = Mathf.Clamp(value, 0, 10); } }
    public float barSpeed { get { return m_barSpeed; } set { m_barSpeed = Mathf.Clamp(value, 0, 5); } }
    public float barPercentage {  get { return m_barPercentage;  } set { m_barPercentage = Mathf.Clamp(value, 0, 100); } }
    public bool isInActiveArea { get { return m_isInActiveArea; } set { m_isInActiveArea = value; } }

    public void SetNewBar(float newMaxOutTime, float newBarPercentage, float newBarSpeed)
    {
        maxOutTime = newMaxOutTime;
        barPercentage = newBarPercentage;
        barSpeed = newBarSpeed;
    }

    void Update()
    {
        Debug.Log(m_isInActiveArea);
    }

}
