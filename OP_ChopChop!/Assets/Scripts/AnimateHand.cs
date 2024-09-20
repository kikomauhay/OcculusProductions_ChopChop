using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHand : MonoBehaviour
{
    public InputActionProperty pinch;
    public InputActionProperty grip;

    public Animator handAnim;

    // Update is called once per frame
    void Update()
    {
        //Gets input value of trigger for pinch
        float triggerValue = pinch.action.ReadValue<float>();
        handAnim.SetFloat("Trigger",triggerValue);

        //Gets input value of side trigger for grip
        float gripValue = grip.action.ReadValue<float>();
        handAnim.SetFloat("Grip", gripValue);  
    }
}
