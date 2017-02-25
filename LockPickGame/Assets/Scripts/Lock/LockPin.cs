using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PinColour
{
    White = 0, 
    Red = 1, 
    Blue = 2
}

[RequireComponent (typeof(Animator))]
public class LockPin : MonoBehaviour {

    [SerializeField]
    private Material[] m_materials;
    [SerializeField]
    private GameObject[] m_particles;
    [SerializeField]
    private Vector3 m_particleOffset;
    [SerializeField]
    private string m_pickTrigger = "Pick";
    [SerializeField]
    private string m_downTrigger = "Down";
    [SerializeField]
    private string m_blueBool = "Blue";
    [SerializeField]
    private string m_bumpTrigger = "Bump";

    private PinColour m_currentColour = PinColour.White;
    private GameObject m_particleObject;
    private ParticleSystem m_currentParticles;
    private bool m_isPressed = false;

    [SerializeField]
    private MeshRenderer m_renderer;
    private Animator m_animator;
    private LockController m_controller;

    void Start()
    {
        m_controller = GetComponentInParent<LockController>();
        m_animator = GetComponent<Animator>();
    }

    public void Pick()
    {
        switch (m_currentColour)
        {
            case PinColour.White:
                m_isPressed = true;
                break;
            case PinColour.Red:

                break;
            case PinColour.Blue:
                m_animator.SetBool(m_blueBool, true);
                m_controller.SetRedToWhite();
                break;
        }
        m_animator.SetTrigger(m_pickTrigger);
    }

    public bool IsPressed()
    {
        return m_isPressed;
    }

    public PinColour GetPinColour()
    {
        return m_currentColour;
    }

    public void SetPinColour(PinColour newColour)
    {
        m_currentColour = newColour;

        if (m_particleObject != null)
        {
            Destroy(m_particleObject);
        }

        switch (m_currentColour)
        {
            case PinColour.White:
                m_renderer.material = m_materials[(int)PinColour.White];
                m_particleObject = Instantiate(m_particles[(int)PinColour.White], transform.position + m_particleOffset, Quaternion.identity) as GameObject;
                break;
            case PinColour.Red:
                m_renderer.material = m_materials[(int)PinColour.Red];
                m_particleObject = Instantiate(m_particles[(int)PinColour.Red], transform.position + m_particleOffset, Quaternion.identity) as GameObject;
                break;
            case PinColour.Blue:
                m_renderer.material = m_materials[(int)PinColour.Blue];
                m_particleObject = Instantiate(m_particles[(int)PinColour.Blue], transform.position + m_particleOffset, Quaternion.identity) as GameObject;
                break;
        }
        m_particleObject.transform.parent = transform;
    }

    public void Spark()
    {
        if (m_particleObject != null)
        {
            m_animator.SetTrigger(m_bumpTrigger);
            m_particleObject.GetComponent<ParticleSystem>().Play();
        }
    }

    public bool Win()
    {
        if(m_isPressed && m_currentColour == PinColour.White)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Lose()
    {
        if (m_isPressed && m_currentColour == PinColour.Red)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
