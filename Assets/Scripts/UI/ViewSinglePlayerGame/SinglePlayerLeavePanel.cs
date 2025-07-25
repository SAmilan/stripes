using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;

public class SinglePlayerLeavePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_LeavePanelButtons;
    private bool m_IsGamePadActive = false;
    private SinglePlayerInputs m_LeavePanelInputs;
    private InputAction m_LeftAction;
    private InputAction m_RightAction;
    private InputAction m_SelectionAction;
    [SerializeField]
    private int m_CurrentIndex;

    private void Awake()
    {
        m_LeavePanelInputs = new SinglePlayerInputs();
        m_LeftAction = m_LeavePanelInputs.GamepadControllers.LeftControls;
        m_RightAction = m_LeavePanelInputs.GamepadControllers.RightControls;
        m_SelectionAction = m_LeavePanelInputs.GamepadControllers.Selection;
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlayWindowTransitionSound();
        m_CurrentIndex = 0;
        m_IsGamePadActive = false;
        m_LeftAction.performed += UpActions;
        m_RightAction.performed += DownActions;
        m_SelectionAction.performed += SelectionActions;
        m_LeftAction.Enable();
        m_RightAction.Enable();
        m_SelectionAction.Enable();
        ResetButtons();
    }

    private void OnDisable()
    {
        m_CurrentIndex = 0;
        m_IsGamePadActive = false;
        m_LeftAction.performed -= UpActions;
        m_RightAction.performed -= DownActions;
        m_SelectionAction.performed -= SelectionActions;
        m_LeftAction.Disable();
        m_RightAction.Disable();
        m_SelectionAction.Disable();
        SoundManager.Instance.PlayCloseWindowSound();
    }


    public void ResetButtons()
    {
        for(int buttonIndex = Constants.INT_ZERO; buttonIndex < m_LeavePanelButtons.Length; buttonIndex++)
        {
            m_LeavePanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger("Normal");
        }
    }

    public void HighlightButton(int currentIndex)
    {
        m_IsGamePadActive = false;
        SoundManager.Instance.PlayMouseHoverSound();
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_LeavePanelButtons.Length; buttonIndex++)
        {
            string currentAnimation;
            if(currentIndex == buttonIndex)
            {
                currentAnimation = "Highlighted";
            }
            else
            {
                currentAnimation = "Normal";
            }
            m_LeavePanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }


    private void UpActions(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        m_CurrentIndex--;
        SoundManager.Instance.PlayMouseHoverSound();
        if (m_CurrentIndex < Constants.INT_ZERO)
        {
            m_CurrentIndex = 0;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_LeavePanelButtons.Length; buttonIndex++)
        {
            string currentAnimation; ;

            if (buttonIndex == m_CurrentIndex)
            {
                currentAnimation = "Highlighted";
            }
            else
            {
                currentAnimation = "Normal";
            }
            m_LeavePanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }
    private void DownActions(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        m_CurrentIndex++;
        SoundManager.Instance.PlayMouseHoverSound();
        if (m_CurrentIndex >= m_LeavePanelButtons.Length)
        {
            m_CurrentIndex = m_LeavePanelButtons.Length - Constants.INT_ONE;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_LeavePanelButtons.Length; buttonIndex++)
        {
            string currentAnimation; ;

            if (buttonIndex == m_CurrentIndex)
            {
                currentAnimation = "Highlighted";
            }
            else
            {
                currentAnimation = "Normal";
            }
            m_LeavePanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }

    }
   
    private void SelectionActions(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_LeavePanelButtons[m_CurrentIndex].GetComponent<Button>().onClick.Invoke();
        }
    }
}
