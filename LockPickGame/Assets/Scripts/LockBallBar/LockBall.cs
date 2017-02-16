using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider), typeof(Rigidbody))]
public class LockBall : MonoBehaviour {

    [SerializeField]
    private LockBallBar m_data;
    [SerializeField]
    private LockBallBarVisuals m_visuals;
    [SerializeField]
    private Camera m_camera;

    private Collider m_collider;
    private Rigidbody m_rigidbody;
    private bool m_isMoving = false;

    void Start()
    {
        m_collider = GetComponent<Collider>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.useGravity = false;
        m_rigidbody.freezeRotation = true;
    }

    void Update()
    {
        MouseInput();

        if (!m_isMoving)
        {
            //transform.Translate(Vector3.down * m_data.ballGravity * Time.deltaTime);
            m_rigidbody.useGravity = true;
        }
        else
        {
            m_rigidbody.useGravity = false;
        }
    }

    private void MouseInput()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = transform.position.z - m_camera.transform.position.z;//Vector3.Distance(m_camera.transform.position, transform.position);
            mousePos = m_camera.ScreenToWorldPoint(mousePos);

            if(m_collider.bounds.Contains(mousePos))
            {
                Vector3 newPos = transform.position;
                newPos.y = mousePos.y;
                if(newPos.y > m_visuals.GetTopY())
                {
                    newPos.y = m_visuals.GetTopY();
                }
                else if(newPos.y < m_visuals.GetBottomY())
                {
                    newPos.y = m_visuals.GetBottomY();
                }

                transform.position = newPos;
                m_isMoving = true;
            }
            else
            {
                m_isMoving = false;
            }
        }
        else
        {
            m_isMoving = false;
        }
    }
}
