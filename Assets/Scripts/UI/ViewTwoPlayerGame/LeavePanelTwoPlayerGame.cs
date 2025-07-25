using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LeavePanelTwoPlayerGame : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_PanelButtons;
    private SinglePlayerInputs m_LeavePanelInputs;
    private InputAction m_LeftAction;
    private InputAction m_RightAction;
    private InputAction m_SelectionAction;
    private int m_CurrentIndex;
    private bool m_IsGamePadActive;

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
        m_LeftAction.performed += LeftActions;
        m_RightAction.performed += RightActions;
        m_SelectionAction.performed += SelectionActions;
        m_LeftAction.Enable();
        m_RightAction.Enable();
        m_SelectionAction.Enable();
        ResetButton();
    }

    private void OnDisable()
    {
        m_LeftAction.performed -= LeftActions;
        m_RightAction.performed -= RightActions;
        m_SelectionAction.performed -= SelectionActions;
        m_LeftAction.Disable();
        m_RightAction.Disable();
        m_SelectionAction.Disable();
        SoundManager.Instance.PlayCloseWindowSound();
    }

    public void HighlightButton(int currentIndex)
    {
        SoundManager.Instance.PlayMouseHoverSound();
        for(int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Length; buttonIndex++)
        {
            string currentAnimation;
            if(buttonIndex == currentIndex)
            {
                currentAnimation = "Highlighted";
            }
            else
            {
                currentAnimation = "Normal";
            }
            m_PanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }

    public void ResetButton()
    {
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Length; buttonIndex++)
        {
            string currentAnimation;
            currentAnimation = "Normal";
            m_PanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }
    private void LeftActions(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        m_CurrentIndex--;
        SoundManager.Instance.PlayMouseHoverSound();
        if (m_CurrentIndex < Constants.INT_ZERO)
        {
            m_CurrentIndex = 0;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Length; buttonIndex++)
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
            m_PanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }
    private void RightActions(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        m_CurrentIndex++;
        SoundManager.Instance.PlayMouseHoverSound();
        if (m_CurrentIndex >= m_PanelButtons.Length)
        {
            m_CurrentIndex = m_PanelButtons.Length - Constants.INT_ONE;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Length; buttonIndex++)
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
            m_PanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }

    }

    private void SelectionActions(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_PanelButtons[m_CurrentIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

}
