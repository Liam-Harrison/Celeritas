using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineSwitcher : MonoBehaviour
{
    [SerializeField]
    private InputAction input;

    public Animator animator;

    private void onEnable() {
        input.Enable();
    }

    private void OnDisable() {
        input.Disable();
    }

    void Start()
    {
        input.performed += _ => SwitchState();
    }

    private void SwitchState() {
        Debug.Log("Entering Play Mode");
        animator.Play("Play");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
