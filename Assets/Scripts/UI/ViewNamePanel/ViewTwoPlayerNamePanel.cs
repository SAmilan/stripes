using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class ViewTwoPlayerNamePanel : UIView
{
    [SerializeField]
    private TMP_InputField m_Player1NameInputField;
    [SerializeField]
    private TMP_InputField m_Player2NameInputField;
    [SerializeField]
    private string m_Player1Name;
    [SerializeField]
    private string m_Player2Name;
    [SerializeField]
    private GameObject m_Player1PopUp;
    [SerializeField]
    private GameObject m_Player2PopUp;
    [SerializeField]
    private Button m_Player1SubmitButton;
    [SerializeField]
    private Button m_Player2SubmitButton;
    [SerializeField]
    private GameObject m_WarningText;
    [SerializeField]
    private UIView m_TwoPlayerGame;
    [SerializeField]
    private Button m_SubmitButton;
    [SerializeField]
    private GameObject[] m_SubmissionButtons;
    private SinglePlayerInputs m_PauseMenuInputs;
    private InputAction m_NextButtonLeftAction;
    private InputAction m_NextButtonRightAction;
    private InputAction m_NextButtonUpAction;
    private InputAction m_NextButtonDownAction;
    private InputAction m_NextButtonSelectionAction;
    private InputAction m_SubmitButtonLeftAction;
    private InputAction m_SubmitButtonRightAction;
    private InputAction m_SubmitButtonSelectionAction;
    [SerializeField]
    private int m_CurrentIndex = 0;

    public static ViewTwoPlayerNamePanel instance;
    public string Player1Name => m_Player1Name;
    public string Player2Name => m_Player2Name;
    public static Action<string, string> s_OnTwoPlayerNameChanges;
    private bool m_IsGamePadActive;

    private void Awake()
    {
        instance = this;
        m_Player1SubmitButton.onClick.AddListener(OpenSecondPlayerPopUp);
        m_Player1NameInputField.onEndEdit.AddListener(Player1NameInputs);
        m_Player1NameInputField.onValueChanged.AddListener(Player1NameInputs);
        m_Player2NameInputField.onValueChanged.AddListener(Player2NameInputs);
        m_Player2NameInputField.onEndEdit.AddListener(Player2NameInputs);
        m_SubmitButton.onClick.AddListener(OnSubmitButtonTap);
        m_PauseMenuInputs = new SinglePlayerInputs();
        m_NextButtonLeftAction = m_PauseMenuInputs.GamepadControllers.LeftControls;
        m_NextButtonRightAction = m_PauseMenuInputs.GamepadControllers.RightControls;
        m_NextButtonUpAction = m_PauseMenuInputs.GamepadControllers.UpControls;
        m_NextButtonDownAction = m_PauseMenuInputs.GamepadControllers.DownControls;
        m_NextButtonSelectionAction = m_PauseMenuInputs.GamepadControllers.Selection;
        m_SubmitButtonLeftAction = m_PauseMenuInputs.GamepadControllers.LeftControls2;
        m_SubmitButtonRightAction = m_PauseMenuInputs.GamepadControllers.RightControls2;
        m_SubmitButtonSelectionAction = m_PauseMenuInputs.GamepadControllers.Selection2;
    }
    private void OnEnable()
    {
        m_IsGamePadActive = false;
        m_WarningText.gameObject.SetActive(false);
        ResetTextAreas();
        ToggleGameObjectPopups(true);
        m_CurrentIndex = 0;
        m_NextButtonUpAction.performed += HighLightNextButtonGamepadHover;
        m_NextButtonDownAction.performed += HighLightNextButtonGamepadHover;
        m_NextButtonLeftAction.performed += HighLightNextButtonGamepadHover;
        m_NextButtonRightAction.performed += HighLightNextButtonGamepadHover;
        m_NextButtonSelectionAction.performed += NextButtonSelectionActions;
        m_SubmitButtonLeftAction.performed += HighLightSubmitButtonGamepadHover;
        m_SubmitButtonRightAction.performed += HighLightSubmitButtonGamepadHover;
        m_SubmitButtonSelectionAction.performed += SelectionSubmissionsActions;
        m_NextButtonUpAction.Enable();
        m_NextButtonDownAction.Enable();
        m_NextButtonLeftAction.Enable();
        m_NextButtonRightAction.Enable();
        m_NextButtonSelectionAction.Enable();
        m_SubmitButtonLeftAction.Disable();
        m_SubmitButtonRightAction.Disable();
        m_SubmitButtonSelectionAction.Disable();

    }
    private void OnDisable()
    {
        m_NextButtonUpAction.performed -= HighLightNextButtonGamepadHover;
        m_NextButtonDownAction.performed -= HighLightNextButtonGamepadHover;
        m_NextButtonLeftAction.performed -= HighLightNextButtonGamepadHover;
        m_NextButtonRightAction.performed -= HighLightNextButtonGamepadHover;
        m_NextButtonSelectionAction.performed -= NextButtonSelectionActions;
        m_SubmitButtonLeftAction.performed -= HighLightSubmitButtonGamepadHover;
        m_SubmitButtonRightAction.performed -= HighLightSubmitButtonGamepadHover;
        m_SubmitButtonSelectionAction.performed -= SelectionSubmissionsActions;
        m_NextButtonLeftAction.Disable();
        m_NextButtonRightAction.Disable();
        m_NextButtonSelectionAction.Disable();
        m_SubmitButtonLeftAction.Disable();
        m_SubmitButtonRightAction.Disable();
        m_SubmitButtonSelectionAction.Disable();
    }

    private void Player1NameInputs(string inputText)
    {
        m_Player1Name = inputText;
    }
    private void Player2NameInputs(string inputText)
    {
        m_Player2Name = inputText;
    }

    private void EnableSubmitButtonInputs()
    {
        m_SubmitButtonLeftAction.Enable();
        m_SubmitButtonRightAction.Enable();
        m_SubmitButtonSelectionAction.Enable();
    }
    private void DisableNextButtonInputs()
    {
        m_NextButtonUpAction.Disable();
        m_NextButtonDownAction.Disable();
        m_NextButtonSelectionAction.Disable();
    }

    private void OpenSecondPlayerPopUp()
    {
        Action player1Action = () =>
       {
           ToggleGameObjectPopups(false);
           EnableSubmitButtonInputs();
           DisableNextButtonInputs();
           m_WarningText.gameObject.SetActive(false);

       };
        Action validationAction = () =>
        {
            m_WarningText.GetComponent<TextMeshProUGUI>().text = Constants.VALIDATION_WARNING;
            m_WarningText.gameObject.SetActive(true);
        };
        m_Player1Name = Utillities.RemoveWhiteSpaces(m_Player1Name);
        bool validationStatus = Utillities.ApplyValidation(m_Player1Name);
        if (validationStatus)
            player1Action?.Invoke();
        else
            validationAction?.Invoke();
    }

    private void ToggleGameObjectPopups(bool status)
    {
        m_Player1PopUp.gameObject.SetActive(status);
        m_Player1SubmitButton.gameObject.SetActive(status);
        m_Player2PopUp.gameObject.SetActive(!status);
        m_Player2SubmitButton.gameObject.SetActive(!status);
    }
    private void ResetTextAreas()
    {
        m_Player1Name = "";
        m_Player1NameInputField.text = "";
        m_Player2Name = "";
        m_Player2NameInputField.text = "";
    }
    private void OnSubmitButtonTap()
    {
        Action submitAction = () =>
        {
            m_WarningText.gameObject.SetActive(false);
            s_OnTwoPlayerNameChanges?.Invoke(m_Player1Name, m_Player2Name);
        };
        Action sameNameAction = () =>
        {
            m_WarningText.GetComponent<TextMeshProUGUI>().text = Constants.SAMENAME_WARNING;
            m_WarningText.gameObject.SetActive(true);
        };
        Action validationAction = () =>
        {
            m_WarningText.GetComponent<TextMeshProUGUI>().text = Constants.VALIDATION_WARNING;
            m_WarningText.gameObject.SetActive(true);
        };

        m_Player2Name = Utillities.RemoveWhiteSpaces(m_Player2Name);
        bool validationStatus = Utillities.ApplyValidation(m_Player2Name);
        bool compareStatus = Utillities.CompareName(m_Player1Name, m_Player2Name);
        Debug.Log(m_Player1Name + " " + m_Player2Name);
        if(compareStatus)
            sameNameAction?.Invoke();
        else if (validationStatus)
            submitAction?.Invoke();
        else
            validationAction?.Invoke();
    }

    public void HighLightNextButtonOnMouseHover(int currentIndex)
    {
        string currentAnimation;
        if(currentIndex == Constants.INT_ZERO)
        {
            currentAnimation = "Highlighted";
        }
        else
        {
            currentAnimation = "Normal";
        }
        m_Player1SubmitButton.gameObject.GetComponent<Animator>().SetTrigger(currentAnimation);
    }

    public void HighLightSubmitButtonOnMouseHover(int currentIndex)
    {
        string currentAnimation;
        if (currentIndex == Constants.INT_ZERO)
        {
            currentAnimation = "Highlighted";
        }
        else
        {
            currentAnimation = "Normal";
        }
        m_Player2SubmitButton.gameObject.GetComponent<Animator>().SetTrigger(currentAnimation);
    }
    public void HighLightSubmitButtonGamepadHover(InputAction.CallbackContext context)
    {
        string currentAnimation = "Highlighted";
        m_IsGamePadActive = false;
        m_Player2SubmitButton.gameObject.GetComponent<Animator>().SetTrigger(currentAnimation);
    }

    public void HighLightNextButtonGamepadHover(InputAction.CallbackContext context)
    {
        string currentAnimation = "Highlighted";
        m_IsGamePadActive = false;
        m_Player1SubmitButton.gameObject.GetComponent<Animator>().SetTrigger(currentAnimation);
    }

   

   

    private void NextButtonSelectionActions(InputAction.CallbackContext context)
    {
        Debug.Log("Selection Action");
        m_Player1SubmitButton.onClick.Invoke();
    }

   

    private void SelectionSubmissionsActions(InputAction.CallbackContext context)
    {
        m_Player2SubmitButton.onClick.Invoke();
    }

}
