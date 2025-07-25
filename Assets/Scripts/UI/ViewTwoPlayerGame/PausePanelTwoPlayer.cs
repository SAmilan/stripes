using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Runtime.InteropServices;

public class PausePanelTwoPlayer : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [SerializeField]
    private Button[] m_PausePanelButtons;
    [SerializeField]
    private GameObject[] HighlightedButtons;
    [SerializeField]
    private GameObject[] UnHighlightedButtons;
    public static Action s_OnGameResumes;
    private SinglePlayerInputs m_PauseMenuInputs;
    private InputAction m_UpAction;
    private InputAction m_DownAction;
    private InputAction m_SelectionAction;
    private bool m_IsGamePadActive = false;
    [SerializeField]
    private int m_CurrentIndex;
    // Start is called before the first frame update

    private void Awake()
    {
        m_PauseMenuInputs = new SinglePlayerInputs();
        m_UpAction = m_PauseMenuInputs.GamepadControllers.UpControls;
        m_DownAction = m_PauseMenuInputs.GamepadControllers.DownControls;
        m_SelectionAction = m_PauseMenuInputs.GamepadControllers.Selection;
    }

    private void OnEnable()
    {
        SetCursorPos(Constants.INT_ZERO, Screen.height);
        SoundManager.Instance.PlayPausePanelSound();
        m_CurrentIndex = 0;
        m_UpAction.performed += UpActions;
        m_DownAction.performed += DownActions;
        m_SelectionAction.performed += SelectionActions;
        m_UpAction.Enable();
        m_DownAction.Enable();
        m_SelectionAction.Enable();
        ResetButtons();
        StartCoroutine(ActiveGamepad());
    }

    private void OnDisable()
    {
        m_IsGamePadActive = false;
        Time.timeScale = 1f;
        SoundManager.Instance.PlayCloseWindowSound();
        m_UpAction.performed -= UpActions;
        m_DownAction.performed -= DownActions;
        m_SelectionAction.performed -= SelectionActions;
        m_UpAction.Disable();
        m_DownAction.Disable();
        m_SelectionAction.Disable();
    }
    public void OnButtonHighlight(int currentIndex)
    {
        m_IsGamePadActive = false;
        SoundManager.Instance.PlayMouseHoverSound();
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < HighlightedButtons.Length; buttonIndex++)
        {
            bool status = false;

            if (buttonIndex == currentIndex)
            {
                status = true;
                SoundManager.Instance.PlayMouseHoverSound();
            }
            HighlightedButtons[buttonIndex].SetActive(status);
            UnHighlightedButtons[buttonIndex].SetActive(!status);
        }
        // currentObject.SetActive(true);
    }

    public void OnButtonExits(GameObject currentObject)
    {
        currentObject.SetActive(false);
    }

    public void OnGameExits()
    {
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        s_OnGameResumes?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void UpActions(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        m_CurrentIndex--;
        if (m_CurrentIndex < Constants.INT_ZERO)
        {
            m_CurrentIndex = 0;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < HighlightedButtons.Length; buttonIndex++)
        {
            bool status = false;

            if (buttonIndex == m_CurrentIndex)
            {
                status = true;
                SoundManager.Instance.PlayMouseHoverSound();
            }
            HighlightedButtons[buttonIndex].SetActive(status);
            UnHighlightedButtons[buttonIndex].SetActive(!status);
        }
    }
    private void DownActions(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        m_CurrentIndex++;
        if (m_CurrentIndex >= HighlightedButtons.Length)
        {
            m_CurrentIndex = HighlightedButtons.Length - Constants.INT_ONE;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < HighlightedButtons.Length; buttonIndex++)
        {
            bool status = false;

            if (buttonIndex == m_CurrentIndex)
            {
                status = true;
                SoundManager.Instance.PlayMouseHoverSound();

            }
            HighlightedButtons[buttonIndex].SetActive(status);
            UnHighlightedButtons[buttonIndex].SetActive(!status);
        }

    }

    public void OnMouseMovement()
    {
        m_IsGamePadActive = false;
        //ResetButtons();
    }
    private void SelectionActions(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_PausePanelButtons[m_CurrentIndex].onClick.Invoke();
        }
    }

    public void ResetButtons()
    {
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < HighlightedButtons.Length; buttonIndex++)
        {
            bool highlightStatus = false;
            highlightStatus = (buttonIndex == Constants.INT_ZERO) ? true : highlightStatus;
            HighlightedButtons[buttonIndex].SetActive(highlightStatus);
            UnHighlightedButtons[buttonIndex].SetActive(!highlightStatus);
        }
    }

    private IEnumerator ActiveGamepad()
    {
        yield return new WaitForSeconds(0.3f);
        m_IsGamePadActive = true;
    }
}
