using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class ViewTwoPlayerGameOver : UIView
{
    [SerializeField]
    private List<GameObject> m_PanelButtons = new List<GameObject>();
    [SerializeField]
    private TextMeshProUGUI m_Player1ScoreText;
    [SerializeField]
    private TextMeshProUGUI m_Player2ScoreText;
    [SerializeField]
    private TextMeshProUGUI m_Player1Name;
    [SerializeField]
    private TextMeshProUGUI m_Player2Name;
    [SerializeField]
    private GameObject m_Player1Header;
    [SerializeField]
    private GameObject m_Player2Header;
    [SerializeField]
    private GameObject m_DrawHeader;
    [SerializeField]
    private GameObject m_SubmitButton;
    public static Action s_OnGameEnds;
    public static Action s_OnTwoPlayerGameSubmitted;
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
        m_SubmitButton.GetComponent<Button>().onClick.AddListener(OnSubmitButtonTap);
        m_GameOverPanel = new SinglePlayerInputs();
        m_LeftAction = m_GameOverPanel.GamepadControllers.LeftControls;
        m_RightAction = m_GameOverPanel.GamepadControllers.RightControls;
        m_SelectionAction = m_GameOverPanel.GamepadControllers.Selection;
    }


    private void OnEnable()
    {
        RemoveSubmitButton();
        CompareScore();
        SoundManager.Instance.PlayGameOverSound();
        CheckLeaderBoardStatus();
        m_Player1ScoreText.text = GameManager.instace.TwoPlayerModePlayer1_Score.ToString();
        m_Player2ScoreText.text = GameManager.instace.TwoPlayerModePlayer2_Score.ToString();
        m_Player1Name.text = GameManager.instace.TwoPlayerModePlayer1_Name.ToString();
        m_Player2Name.text = GameManager.instace.TwoPlayerModePlayer2_Name.ToString();
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

    private void RemoveSubmitButton()
    {
        int buttonIndex = m_PanelButtons.FindIndex(element => element.transform.name == m_SubitButtonName);
        if (buttonIndex != Constants.NULLINDEX)
            m_PanelButtons.RemoveAt(buttonIndex);
    }

    private void CompareScore()
    {

        if (GameManager.instace.TwoPlayerModePlayer1_Score == GameManager.instace.TwoPlayerModePlayer2_Score)
        {
            m_DrawHeader.gameObject.SetActive(true);
            m_Player1Header.gameObject.SetActive(false);
            m_Player2Header.gameObject.SetActive(false);
            return;
        }
        if (GameManager.instace.TwoPlayerModePlayer1_Score > GameManager.instace.TwoPlayerModePlayer2_Score)
        {
            m_Player1Header.gameObject.SetActive(true);
            m_Player2Header.gameObject.SetActive(false);
        }
        else
        {
            m_Player1Header.gameObject.SetActive(false);
            m_Player2Header.gameObject.SetActive(true);
        }
        m_DrawHeader.gameObject.SetActive(false);
    }

    public void IncreaseRowColumnRate()
    {
        //GameData.Instance.GameOptions.TwoPlayerGameRows += GameData.Instance.GameOptions.RowColumnIncreaseRate;
        GameData.Instance.GameOptions.TwoPlayerGameRows = ((GameData.Instance.GameOptions.TwoPlayerGameRows + GameData.Instance.GameOptions.RowColumnIncreaseRate) >= Constants.SIDELENGTH_12) ? Constants.SIDELENGTH_12 : (GameData.Instance.GameOptions.TwoPlayerGameRows + GameData.Instance.GameOptions.RowColumnIncreaseRate);
        GameData.Instance.GameOptions.TwoPlayerGameColumn = ((GameData.Instance.GameOptions.TwoPlayerGameColumn + GameData.Instance.GameOptions.RowColumnIncreaseRate) >= Constants.SIDELENGTH_12) ? Constants.SIDELENGTH_12 : (GameData.Instance.GameOptions.TwoPlayerGameColumn + GameData.Instance.GameOptions.RowColumnIncreaseRate);
        OptionsReader.instance.SaveFile();
    }

    

    private void CheckLeaderBoardStatus()
    {
        List<LeaderBoardData> leaderBoardData = LeaderboardManager.instance.LoadFile();
        List<LeaderBoardData> TopTenPlayers = new List<LeaderBoardData>();
        int leaderBoardIndex = leaderBoardData.FindIndex(leaderBoardelement => (leaderBoardelement.playerScore < GameManager.instace.TwoPlayerModePlayer1_Score || leaderBoardelement.playerScore < GameManager.instace.TwoPlayerModePlayer2_Score) && (leaderBoardelement.playerRank <= Constants.MAX_LEADERBOARDENTRIES));
        bool leaderBoardStatus = (leaderBoardData.Count < Constants.MAX_LEADERBOARDENTRIES && (GameManager.instace.TwoPlayerModePlayer1_Score > Constants.INT_ZERO || GameManager.instace.TwoPlayerModePlayer2_Score > Constants.INT_ZERO)) ? true : (leaderBoardIndex != Constants.NULLINDEX && (GameManager.instace.TwoPlayerModePlayer1_Score > Constants.INT_ZERO || GameManager.instace.TwoPlayerModePlayer2_Score > Constants.INT_ZERO)) ? true : false;
        m_SubmitButton.gameObject.SetActive(false);//5/07/22
        if (leaderBoardStatus)
        {
            m_PanelButtons.Add(m_SubmitButton);
        }
    }

    public void ResetButtons()
    {
        string currentAnimation = "Normal"; 
        for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_PanelButtons.Count; buttonIndex++)
        {
            m_PanelButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }

    private void OnSubmitButtonTap()
    {
        s_OnTwoPlayerGameSubmitted?.Invoke();
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
