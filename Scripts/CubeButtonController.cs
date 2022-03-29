using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class CubeButtonController : MonoBehaviour
{

    private Renderer renderer;

    private void Awake()
    {
        renderer = this.GetComponent<Renderer>();
    }

    public void ColorChanged(InteractableStateArgs obj)
    {
        switch (obj.NewInteractableState)
        {
            case InteractableState.ProximityState:
                renderer.material.color = Color.yellow;
                break;
            case InteractableState.ContactState:
                renderer.material.color = Color.red;
                break;
            case InteractableState.ActionState:
                renderer.material.color = Color.green;
                break;
            default:
                renderer.material.color = Color.blue;
                break;
        }
    }
}
