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
    private string m_pickTrigger = "Pick";
    [SerializeField]
    private string m_downTrigger = "Down";
    [SerializeField]
    private string m_blueBool = "Blue";
    [SerializeField]
    private MeshRenderer m_renderer;

    private PinColour m_currentColour = PinColour.White;
    [SerializeField]
    private Animator m_animator;

    private LockController m_controller;

    void Start()
    {
        m_controller = GetComponentInParent<LockController>();
        m_animator = GetComponent<Animator>();
    }

    public PinColour GetPinColour()
    {
        return m_currentColour;
    }

    public void SetPinColour(PinColour newColour)
    {
        m_currentColour = newColour;

        switch (m_currentColour)
        {
            case PinColour.White:
                m_renderer.material = m_materials[0];
                break;
            case PinColour.Red:
                m_renderer.material = m_materials[1];
                break;
            case PinColour.Blue:
                m_renderer.material = m_materials[2];
                break;
        }

    }

    public void Pick()
    {
        switch(m_currentColour)
        {
            case PinColour.White:

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

}
