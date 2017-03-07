using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitch : ActivatorObject{

    public bool activate = false;
    public bool deactivate = false;
    public bool changeColour = false;

    void Update()
    {
        if(activate)
        {
            base.Activate();
            activate = false;
        }

        if(deactivate)
        {
            base.Deactivate();
            deactivate = false;
        }

        if(changeColour)
        {
            foreach(Door d in m_activatableObjects)
            {
                Color randColour = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                d.Call("changeColour", randColour);
            }
            changeColour = false;
        }
    }

}
