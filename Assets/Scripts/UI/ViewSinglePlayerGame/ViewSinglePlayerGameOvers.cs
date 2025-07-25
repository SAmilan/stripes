using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class ViewSinglePlayerGameOvers : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_PanelButtons = new List<GameObject>();
    [SerializeField]
    private TextMeshProUGUI m_PlayerScoreText;
    [SerializeField]
    private TextMeshProUGUI m_PlayerName;
    [SerializeField]
    private Button m_SubmitScore;
    public static Action s_OnGameEnds;
    [SerializeField]
    private GameObject m_SubmitButton;
    public static Action s_OnSinglePlayerScoreSubmitted;
    string m_SubitButtonName = "Button - SubmitScore";
    private SinglePlayerInputs m_GameOverPanel;
    private InputAction m_LeftAction;
    private InputAction m_RightAction;
    private InputAction m_SelectionAction;
    [SerializeField]
    private int m_CurrentIndex;
    private bool m_IsGamePadActive;

    private void Awake()
    {
        m_SubmitScore.onClick.AddListener(OnSubmitScoreTap);
        m_GameOverPanel = new SinglePlayerInputs();
        m_LeftAction = m_GameOverPanel.GamepadControllers.LeftControls;
        m_RightAction = m_GameOverPanel.GamepadControllers.RightControls;
        m_SelectionAction = m_GameOverPanel.GamepadControllers.Selection;

    }

    private void OnEnable()
    {
        RemoveSubmitButton();
        SoundManager.Instance.PlayGameOverSound();
        CheckLeaderBoardStatus();
        m_PlayerScoreText.text = GameManager.instace.SinglePlayerModePlayerScore.ToString();
        m_PlayerName.text = GameManager.instace.SinglePlayerModePlayerName;
        m_CurrentIndex = 0;
        m_LeftAction.performed += LeftActions;
        m_RightAction.performed += RightActions;
        m_SelectionAction.performed += SelectionActions;
        m_LeftAction.Enable();
        m_RightAction.Enable();
        m_SelectionAction.Enable();
        ResetButtons();
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

    private void RemoveSubmitButton()
    {
        int buttonIndex = m_PanelButtons.FindIndex(element => element.transform.name == m_SubitButtonName);
        if (buttonIndex != Constants.NULLINDEX)
            m_PanelButtons.RemoveAt(buttonIndex);
    }

    private void CheckLeaderBoardStatus()
    {
        List<LeaderBoardData> leaderBoardData = LeaderboardManager.instance.LoadFile();
        List<LeaderBoardData> TopTenPlayers = new List<LeaderBoardData>();
        int leaderBoardIndex = leaderBoardData.FindIndex(leaderBoardelement => (leaderBoardelement.playerScore < GameManager.instace.SinglePlayerModePlayerScore) && (leaderBoardelement.playerRank <= Constants.MAX_LEADERBOARDENTRIES));
        bool leaderBoardStatus = (leaderBoardData.Count < Constants.MAX_LEADERBOARDENTRIES && GameManager.instace.SinglePlayerModePlayerScore > Constants.INT_ZERO) ? true : (leaderBoardIndex != Constants.NULLINDEX && GameManager.instace.SinglePlayerModePlayerScore > Constants.INT_ZERO) ? true : false;
        m_SubmitButton.gameObject.SetActive(leaderBoardStatus);
        if (leaderBoardStatus)
        {
            m_PanelButtons.Add(m_SubmitButton);
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
    private void OnSubmitScoreTap()
    {
        s_OnSinglePlayerScoreSubmitted?.Invoke();
    }
    public void ResetButtons()
    {
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Count; buttonIndex++)
        {
            m_PanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger("Normal");
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

    private void SelectionActions(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_PanelButtons[m_CurrentIndex].GetComponent<Button>().onClick.Invoke();
        }
    }
}
