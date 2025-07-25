using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ViewTwoPlayerLoopComplete : UIView
{
    [SerializeField]
    private TextMeshProUGUI m_HeaderText;
    [SerializeField]
    private TextMeshProUGUI m_PanelText;
    [SerializeField]
    private TextMeshProUGUI m_LoopCount;
    private SinglePlayerInputs m_LoopCompletePanel;
    private InputAction m_LeftAction;
    private InputAction m_RightAction;
    private InputAction m_SelectionAction;
    [SerializeField]
    private int m_CurrentIndex;
    private bool m_IsGamePadActive;
    [SerializeField]
    private List<GameObject> m_PanelButtons = new List<GameObject>();
    private string m_GamePad1 = "";
    private string m_GamePad2 = "";


    private void Awake()
    {
        m_LoopCompletePanel = new SinglePlayerInputs();
        m_LeftAction = m_LoopCompletePanel.GamepadControllers.LeftControls;
        m_RightAction = m_LoopCompletePanel.GamepadControllers.RightControls;
        m_SelectionAction = m_LoopCompletePanel.GamepadControllers.Selection;
    }
    private void OnEnable()
    {
        SoundManager.Instance.PlayGameOverSound();
        string headerText = GameManager.instace.TwoPlayerModeLoopCompletedWinner.ToString();
        m_HeaderText.SetText(headerText);
        m_PanelText.text = GameManager.instace.TwoPlayerModeLoopCompletedWinner;
        m_LoopCount.text = GameData.Instance.GameOptions.MultiplayerLoopCompleted.ToString() + " " + "Loop";
        m_CurrentIndex = 0;
        m_LeftAction.performed += LeftActions;
        m_RightAction.performed += RightActions;
        m_SelectionAction.performed += SelectionActions;
        m_LeftAction.Enable();
        m_RightAction.Enable();
        m_SelectionAction.Enable();
        ResetButtons();
    }

    private void Update()
    {
        if (Input.GetButton("A_button1"))
        {
            Debug.Log("Selection 1 button is pressed");
        }
        if (Input.GetButton("A_button2"))
        {
            Debug.Log("Selection 2 button is pressed");
        }
        float lt1 = Input.GetAxis("LT1");
        float lt2 = Input.GetAxis("LT2");
       
        if (lt1 > 0f || lt2 > 0f)
        {
            Debug.Log("GamePad LT1: " + lt1);
            Debug.Log("GamePad LT2: " + lt2);
        }

    }
    private void OnDisable()
    {
        m_LeftAction.performed -= LeftActions;
        m_RightAction.performed -= RightActions;
        m_SelectionAction.performed -= SelectionActions;
        m_LeftAction.Disable();
        m_RightAction.Disable();
        m_SelectionAction.Disable();
    }
    private void SelectionActions(InputAction.CallbackContext context)
    {
       
        if (m_IsGamePadActive)
        {
            m_PanelButtons[m_CurrentIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    private void LeftActions(InputAction.CallbackContext context)
    {
        float lt1 = Input.GetAxis("LT1");
        float lt2 = Input.GetAxis("LT2");
       /* int gamepadCount = 0;
        for (int gamepadIndex = Constants.INT_ZERO; gamepadIndex < gamepad.Length; gamepadIndex++)
        {
            if (gamepad[gamepadIndex] != null)
            {

            }
            Debug.Log("GamePad name is: " + gamepad[gamepadIndex]);
        }*/
        

        m_IsGamePadActive = true;
        m_CurrentIndex--;
        SoundManager.Instance.PlayMouseHoverSound();
        if (m_CurrentIndex < Constants.INT_ZERO)
        {
            m_CurrentIndex = 0;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Count; buttonIndex++)
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
        if (m_CurrentIndex >= m_PanelButtons.Count)
        {
            m_CurrentIndex = m_PanelButtons.Count - Constants.INT_ONE;
        }
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Count; buttonIndex++)
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

    public void HighlightButtons(int currentIndex)
    {
        m_IsGamePadActive = false;
        SoundManager.Instance.PlayMouseHoverSound();
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Count; buttonIndex++)
        {
            string currentAnimation;
            if (buttonIndex == currentIndex)
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

    public void ResetButtons()
    {
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Count; buttonIndex++)
        {
            m_PanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger("Normal");
        }
    }


}
