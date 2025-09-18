using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssestInputExample : MonoBehaviour
{
    public bool printStuff = true;
    public InputActionReference testReference = null;

    // Start is called before the first frame update
    void Start()
    {
        testReference.action.started += DoPressedThing;
        testReference.action.performed += DoChangeThing;
        testReference.action.canceled += DoReleasedThing;
    }

    void OnEnable()
    {
        testReference.asset.Enable();
    }

    void OnDisable()
    {
        testReference.asset.Disable();
    }

    void OnDestroy()
    {
        testReference.action.started -= DoPressedThing;
        testReference.action.performed -= DoChangeThing;
        testReference.action.canceled -= DoReleasedThing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print("Pressed");
    }

    private void DoChangeThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print(context.ReadValue<float>());
    }

    private void DoReleasedThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print("Released");
    }
}
