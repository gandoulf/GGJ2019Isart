using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class TestControl : MonoBehaviour
{
    [SerializeField] private Controls controls;

    private void OnEnable()
    {
        controls.Controler.Fire.performed += HandleFire;
        controls.Controler.Fire.Enable();
    }

    private void OnDisable()
    {
        controls.Controler.Fire.performed -= HandleFire;
        controls.Controler.Fire.Disable();
    }

    private void HandleFire(InputAction.CallbackContext obj)
    {
        Debug.Log("hello");
    }
}
