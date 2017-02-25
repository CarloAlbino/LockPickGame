using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class Door : ActivatableObject {

    [SerializeField]
    private string m_animatorOpen = "";

    private Animator m_animator;
    private MeshRenderer m_renderer;

	void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponentInChildren<MeshRenderer>();
	}

    public override void ActivateObject()
    {
        base.ActivateObject();

        Open();
    }

    public override void DeactivateObject()
    {
        base.DeactivateObject();

        Close();
    }

    public override void Call(string method, params object[] list)
    {
        switch(method.ToLower())
        {
            case "open":
                Open();
                break;
            case "close":
                Close();
                break;
            case "changecolour":
                ChangeColour((Color)list[0]);
                break;
            default:
                Debug.LogWarning("Unknown method call.");
                break;
        }
    }

    private void Open()
    {
        m_animator.SetBool(m_animatorOpen, true);
    }

    private void Close()
    {
        m_animator.SetBool(m_animatorOpen, false);
    }

    private void ChangeColour(Color colour)
    {
        m_renderer.material.color = colour;
    }
}
