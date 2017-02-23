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
    private Color[] m_colours = { Color.white, Color.red, Color.blue };
    [SerializeField]
    private string m_pickTrigger = "Pick";
    [SerializeField]
    private string m_downTrigger = "Down";
    [SerializeField]
    private string m_blueBool = "Blue";
    [SerializeField]
    private MeshRenderer m_renderer;

    private PinColour m_currentColour = PinColour.White;
    private Material m_material;
    [SerializeField]
    private Animator m_animator;

    void Start()
    {
        if (m_renderer != null)
        {
            m_material = m_renderer.GetComponent<Material>();
        }
        m_animator = GetComponent<Animator>();
    }

    public PinColour GetPinColour()
    {
        return m_currentColour;
    }

    public void SetPinColour(PinColour newColour)
    {
        m_currentColour = newColour;
        m_material.color = m_colours[(int)m_currentColour];
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
                break;
        }
        m_animator.SetTrigger(m_pickTrigger);
    }

}
