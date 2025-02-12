using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;


public class ButtonInteractable : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] public ActionBasedController left;
    [SerializeField] public ActionBasedController right;

    private void Awake()
    {
        //left = ControllerManager.instance.leftController;
        //right = ControllerManager.instance.rightController;
    }

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("NoHit");
        if (other.gameObject.GetComponent<ActionBasedController>() != null)
        {
            Debug.Log(other.gameObject.GetComponent<ActionBasedController>());
            Debug.Log("Hit");
            button.onClick.Invoke();
        }
    }
}
