using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;



public class ViewSinglePlayerNamePanel : UIView
{
    [SerializeField]
    private string m_PlayerName;
    public string PlayerName => m_PlayerName;
    [SerializeField]
    private TMP_InputField m_PlayerNameInputField;
    public static ViewSinglePlayerNamePanel instance;
    [SerializeField]
    private Button m_SubmitData;
    [SerializeField]
    private GameObject m_WarningText;
    public static Action<string> s_OnSinglePlayerNameChanges;
    private bool m_IsGamePadActive;
    private SinglePlayerInputs m_NamePanelInputs;
    private InputAction m_LeftAction;
    private InputAction m_RightAction;
    private InputAction m_UpAction;
    private InputAction m_DownAction;
    private InputAction m_SelectionAction;
    private void Awake()
    {
        instance = this;
        m_PlayerNameInputField.onEndEdit.AddListener(Player1NameInputs);
        m_PlayerNameInputField.onValueChanged.AddListener(Player1NameInputs);
        m_SubmitData.onClick.AddListener(OnSubmitButtonTap);
        m_NamePanelInputs = new SinglePlayerInputs();
        m_SelectionAction = m_NamePanelInputs.GamepadControllers.Selection;
        m_LeftAction = m_NamePanelInputs.GamepadControllers.LeftControls;
        m_RightAction = m_NamePanelInputs.GamepadControllers.RightControls;
        m_UpAction  = m_NamePanelInputs.GamepadControllers.UpControls;
        m_DownAction = m_NamePanelInputs.GamepadControllers.DownControls;
    }
    private void OnEnable()
    {
        m_IsGamePadActive = false;
        ResetTextAreas();
        m_LeftAction.performed += OnGamePadActions;
        m_RightAction.performed += OnGamePadActions;
        m_UpAction.performed += OnGamePadActions;
        m_DownAction.performed += OnGamePadActions;
        m_SelectionAction.performed += SelectionActions;
        m_SelectionAction.Enable();
        m_UpAction.Enable();
        m_DownAction.Enable();
        m_LeftAction.Enable();
        m_DownAction.Enable();
    }

    private void OnDisable()
    {
        m_SelectionAction.performed -= SelectionActions;
        m_LeftAction.performed -= OnGamePadActions;
        m_RightAction.performed -= OnGamePadActions;
        m_UpAction.performed -= OnGamePadActions;
        m_DownAction.performed -= OnGamePadActions;
        m_SelectionAction.Disable();
        m_UpAction.Disable();
        m_DownAction.Disable();
        m_LeftAction.Disable();
        m_DownAction.Disable();
    }
    private void Player1NameInputs(string inputText)
    {
        Action<string> dataAction = (string name) =>
        {
            m_WarningText.SetActive(false);
            m_IsGamePadActive = true;
            m_SubmitData.gameObject.SetActive(true);
          
        };
        Action validatioAction = () =>
        {
            m_SubmitData.gameObject.SetActive(false);
            m_IsGamePadActive = false;
            m_WarningText.SetActive(true);
            m_WarningText.GetComponent<TextMeshProUGUI>().text = Constants.VALIDATION_WARNING;
        };
        m_PlayerName = inputText;
        m_PlayerName = Utillities.RemoveWhiteSpaces(m_PlayerName);
        bool status = Utillities.ApplyValidation(m_PlayerName);
        if (status)
            dataAction?.Invoke(m_PlayerName);
        else
            validatioAction?.Invoke();


    }
    private void OnSubmitButtonTap()
    {
        s_OnSinglePlayerNameChanges?.Invoke(m_PlayerName);
    }
    public void OnHighlightButton()
    {
        m_SubmitData.gameObject.GetComponent<Animator>().SetTrigger("Highlighted");
        SoundManager.Instance.PlayMouseHoverSound();
    }
    

    public void OnResetButton()
    {
        m_SubmitData.gameObject.GetComponent<Animator>().SetTrigger("Normal");
    }

    private void OnGamePadActions(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            OnHighlightButton();
        }
    }

    private void ResetTextAreas()
    {
        m_SubmitData.gameObject.SetActive(false);
        m_WarningText.SetActive(false);
        m_PlayerName = "";
        m_PlayerNameInputField.text = "";
    }
    private void SelectionActions(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            OnSubmitButtonTap();
        }
    }
}
