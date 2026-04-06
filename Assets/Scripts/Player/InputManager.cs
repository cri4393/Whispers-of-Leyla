using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public bool interactPressed = false;
    public bool submitPressed = false;
    public bool closeMenu = false;
    public bool canSave = false;
    public bool havePressAnyButton = false;
    public bool canSitUp;
    public bool canOpenMap = false;
    public bool openAmulets = false;
    public bool menuRightPressed = false;
    public bool menuLeftPressed = false;

    [Header("Bank")]
    private bool increment = false;
    private bool decrease = false;
    public bool confirmBank = false;

    public static InputManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this; 
    }

    public static InputManager GetInstance()
    {
        return instance;
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }
    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }
    public void SitDownSitUp(InputAction.CallbackContext context)
    {
        //if(PauseManager.paused) return;
        if(context.performed)
        {
            canSave = true;
            Debug.Log("SitDown");
        }
        else if(context.canceled)
        {
            canSave = false;
        }
    }
    public void SitUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            canSitUp = true;
        }
        else if (context.canceled)
        {
            canSitUp = false;
        }
    }
    public void Increment(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            increment = true;
        }
        else if(context.performed)
        {
            increment = false;
        }
    }
    public void OpenMenuAmulets(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            openAmulets = true;
        }
        else if (context.canceled)
        {
            openAmulets = false;
        }
    }
    public void MenuRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menuRightPressed = true;
        }
        else if (context.canceled)
        {
            menuRightPressed = false;
        }
    }
    public void MenuLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menuLeftPressed = true;
        }
        else if (context.canceled)
        {
            menuLeftPressed = false;
        }
    }
    public void OpenMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            canOpenMap = true;
        }
        else if (context.canceled)
        {
            canOpenMap = false;
        }
    }
    public void Decrease(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            decrease = true;
        }
        else if(context.canceled)
        {
            decrease = false;
        }
    }
    public void ConfirmBank(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            confirmBank = true;
            Debug.Log("conferma");
        }
        else if(context.canceled)
        {
            confirmBank = false;
        }
    }
    public void CloseMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            closeMenu = true;
        }
        else if (context.canceled)
        {
            closeMenu = false;
        }
    }
    public void PressAnyButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            havePressAnyButton = true;
        }
        else if (context.canceled)
        {
            havePressAnyButton = false;
        }
    }
    public bool PressedAnyButton()
    {
        bool result = havePressAnyButton;
        havePressAnyButton = false;
        return result;
    }
    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }
    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }
    public bool GetSitDownUp()
    {
        bool result = canSave;
        canSave = false;
        Debug.Log("PRESS E");
        return result;
    }
    public bool GetIncrement()
    {
        bool result = increment;
        increment = false;
        return result;
    }
    public bool GetDecrease()
    {
        bool result = decrease;
        decrease = false;
        return result;
    }
    public bool GetConfirmBank()
    {
        bool result = confirmBank;
        confirmBank = false;
        return result;
    }
    public bool GetCloseMenu()
    {
        bool result = closeMenu;
        closeMenu = false;
        return result;
    }
    public void RegisterSubmitPressed()
    {
        submitPressed = false;
        Debug.Log("RegisterSubmitPressed");
    }

}
