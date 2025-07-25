using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[Serializable]
public class GamePadButtons
{
    public List<GameObject> highlightedButtons = new List<GameObject>();
}

public class ViewTitlePanel : UIView
{
    #region Private_Vars
    [SerializeField]
    private Button m_Exit;
    [SerializeField]
    private GameObject m_LeaderBoardEntryPrefab;
    [SerializeField]
    private GameObject m_LeaderBoardEntryParent;
    [SerializeField]
    private GameObject m_Loader;
    [SerializeField]
    private GameObject m_Leaderboard;
    [SerializeField]
    private GameObject m_LoadingPanel;
    [SerializeField]
    private Button m_Option;
    private SinglePlayerInputs m_TitleInputs;
    private InputAction m_OpenIOption;
    [SerializeField]
    private List<GamePadButtons> m_GamePadButtons = new List<GamePadButtons>();
    [SerializeField]
    private int m_CurrentIndexX = 0;
    [SerializeField]
    private int m_CurrentIndexY = 0;
    private InputAction m_UpActions;
    private InputAction m_DownActions;
    private InputAction m_LeftActions;
    private InputAction m_RightActions;
    private InputAction m_SelectButton;
    private bool m_GamePadSelectionSelectStatus;
    [SerializeField]
    private Button[] m_AllTitleButtons;

    #endregion
    #region Public_Vars

    public static Action<int, int> s_HighlightButtonCallback;
    public static Action s_ResetButtonCallback;
    public static Action<int, int> s_ClickButtonCallback;

    private void Awake()
    {

        m_TitleInputs = new SinglePlayerInputs();
        m_Exit.onClick.AddListener(OnGameExit);

    }
    private void OnEnable()
    {
        m_LoadingPanel.gameObject.SetActive(false);
        m_CurrentIndexX = Constants.INT_ZERO;
        m_CurrentIndexY = Constants.INT_ZERO;
        m_GamePadSelectionSelectStatus = false;
        SoundManager.Instance.PlayBGSound();
        m_Option.gameObject.SetActive(GameData.Instance.GameOptions.ShowOptionStatus);
        m_OpenIOption = m_TitleInputs.OptionPanel.OpenOptiion;
        m_UpActions = m_TitleInputs.GamepadControllers.UpControls;
        m_DownActions = m_TitleInputs.GamepadControllers.DownControls;
        m_LeftActions = m_TitleInputs.GamepadControllers.LeftControls;
        m_RightActions = m_TitleInputs.GamepadControllers.RightControls;
        m_SelectButton = m_TitleInputs.GamepadControllers.Selection;
        m_OpenIOption.Enable();
        m_OpenIOption.performed += OpenOptionButton;
        m_UpActions.performed += UpActions;
        m_DownActions.performed += DownActions;
        m_LeftActions.performed += LeftActions;
        m_RightActions.performed += RightActions;
        m_SelectButton.performed += ButtonSelectionActions;
        m_UpActions.Enable();
        m_DownActions.Enable();
        m_LeftActions.Enable();
        m_RightActions.Enable();
        m_SelectButton.Enable();
        m_Leaderboard.gameObject.SetActive(GameData.Instance.GameOptions.DisplayHighScore);
        StartCoroutine(LoadData());
        OnMouseExits();
    }
    private void OnDisable()
    {
        SoundManager.Instance.StopBGSound();
        m_CurrentIndexX = Constants.INT_ZERO;
        m_CurrentIndexY = Constants.INT_ZERO;
        m_GamePadSelectionSelectStatus = false;
        m_OpenIOption.Disable();
        m_OpenIOption.performed -= OpenOptionButton;
        m_UpActions.performed -= UpActions;
        m_DownActions.performed -= DownActions;
        m_LeftActions.performed -= LeftActions;
        m_RightActions.performed -= RightActions;
        m_SelectButton.performed -= ButtonSelectionActions;
        m_UpActions.Disable();
        m_DownActions.Disable();
        m_LeftActions.Disable();
        m_RightActions.Disable();
        m_SelectButton.Disable();
        RemoveAllUIEntries();
        OnMouseExits();
    }

    #endregion

    #region Unity_CallBacks

    #endregion

    #region Private_Methods
    private void OnGameExit()
    {
        Application.Quit();
    }


    private void OnMouseHover(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Debug.Log("Mouse Hover Test");
    }
    IEnumerator LoadData()
    {
        m_Loader.SetActive(true);
        yield return new WaitForSeconds(Constants.LEADERBOARD_DELAY);
        m_Loader.SetActive(false);
        OnLeaderboardUpdated(LeaderboardManager.instance.LoadFile());
    }

    public void HighlightButton(int buttonIndex)
    {
        m_GamePadSelectionSelectStatus = false;
        for (int buttons = Constants.INT_ZERO; buttons < m_AllTitleButtons.Length; buttons++)
        {
            string currentAnimation;
            if (buttons == buttonIndex)
            {
                currentAnimation = "Highlighted";
            }
            else
            {
                currentAnimation = "Normal";
            }
            m_AllTitleButtons[buttons].GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }
    public void OnMouseExits()
    {
        for (int buttonIndexX = Constants.INT_ZERO; buttonIndexX < m_GamePadButtons.Count; buttonIndexX++)
        {
            string currentAnimation;
            for (int buttonIndexY = Constants.INT_ZERO; buttonIndexY < m_GamePadButtons[buttonIndexX].highlightedButtons.Count; buttonIndexY++)
            {
                currentAnimation = "Normal";
                m_GamePadButtons[buttonIndexX].highlightedButtons[buttonIndexY].GetComponent<Animator>().SetTrigger(currentAnimation);
            }
        }
    }

    public void OnTurnOffButton()
    {
        s_ResetButtonCallback?.Invoke();
    }



    private void OpenOptionButton(InputAction.CallbackContext context)
    {
        m_Option.gameObject.SetActive(true);
    }

    private void UpActions(InputAction.CallbackContext context)
    {
        m_GamePadSelectionSelectStatus = true;
        m_CurrentIndexY--;
        if (m_CurrentIndexY < Constants.ZERO)
        {
            m_CurrentIndexY = Constants.INT_ZERO;
        }
        s_HighlightButtonCallback?.Invoke(m_CurrentIndexX, m_CurrentIndexY);
       
        //m_CurrentIndexY--;
        /* for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_GamePadButtons[m_CurrentIndexX].highlightedButtons.Count; buttonIndex++)
         {
             string currentAnimation;
             if (buttonIndex != m_CurrentIndexY)
             {
                 currentAnimation = "Normal";
             }
             else
             {
                 currentAnimation = "Highlighted";
             }
             m_GamePadButtons[m_CurrentIndexX].highlightedButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
         }*/
        SoundManager.Instance.PlayMouseHoverSound();

    }

    private void DownActions(InputAction.CallbackContext context)
    {
        m_GamePadSelectionSelectStatus = true;
        m_CurrentIndexY++;
        if (m_CurrentIndexY >= m_GamePadButtons[m_CurrentIndexX].highlightedButtons.Count)
        {
            m_CurrentIndexY = m_GamePadButtons[m_CurrentIndexX].highlightedButtons.Count - 1;
        }
        s_HighlightButtonCallback?.Invoke(m_CurrentIndexX, m_CurrentIndexY);
       
        //m_CurrentIndexY++;

        /* for (int buttonIndex = Constants.INT_ZERO; buttonIndex < m_GamePadButtons[m_CurrentIndexX].highlightedButtons.Count; buttonIndex++)
         {
             string currentAnimation;
             if (buttonIndex != m_CurrentIndexY)
             {
                 currentAnimation = "Normal";
             }
             else
             {
                 currentAnimation = "Highlighted";
             }
             m_GamePadButtons[m_CurrentIndexX].highlightedButtons[buttonIndex].GetComponent<Animator>().SetTrigger(currentAnimation);
         }*/
        SoundManager.Instance.PlayMouseHoverSound();
    }
    private void LeftActions(InputAction.CallbackContext context)
    {
        m_GamePadSelectionSelectStatus = true;
        m_CurrentIndexY = Constants.INT_ZERO;
        //m_CurrentIndexX--;
        m_CurrentIndexX--;
        if (m_CurrentIndexX < Constants.ZERO)
        {
            m_CurrentIndexX = Constants.INT_ZERO;
        }
        s_HighlightButtonCallback?.Invoke(m_CurrentIndexX, m_CurrentIndexY);
       
        SoundManager.Instance.PlayMouseHoverSound();

        /*for (int buttonIndexX = Constants.INT_ZERO; buttonIndexX < m_GamePadButtons.Count; buttonIndexX++)
        {
            string currentAnimation;
            for (int buttonIndexY = Constants.INT_ZERO; buttonIndexY < m_GamePadButtons[buttonIndexX].highlightedButtons.Count; buttonIndexY++)
            {
                if (buttonIndexX == m_CurrentIndexX && buttonIndexY == m_CurrentIndexY)
                {
                    currentAnimation = "Highlighted";
                }
                else
                {
                    currentAnimation = "Normal";
                }
                m_GamePadButtons[buttonIndexX].highlightedButtons[buttonIndexY].GetComponent<Animator>().SetTrigger(currentAnimation);
            }
        }*/
    }
    private void RightActions(InputAction.CallbackContext context)
    {
        m_GamePadSelectionSelectStatus = true;
        m_CurrentIndexY = Constants.INT_ZERO;
        // m_CurrentIndexX++;
        m_CurrentIndexX++;
        if (m_CurrentIndexX >= m_GamePadButtons.Count)
        {
            m_CurrentIndexX = m_GamePadButtons.Count - 1;
        }
        s_HighlightButtonCallback?.Invoke(m_CurrentIndexX, m_CurrentIndexY);
        
        SoundManager.Instance.PlayMouseHoverSound();

        /*for (int buttonIndexX = Constants.INT_ZERO; buttonIndexX < m_GamePadButtons.Count; buttonIndexX++)
        {
            string currentAnimation;
            for (int buttonIndexY = Constants.INT_ZERO; buttonIndexY < m_GamePadButtons[buttonIndexX].highlightedButtons.Count; buttonIndexY++)
            {
                if (buttonIndexX == m_CurrentIndexX && buttonIndexY == m_CurrentIndexY)
                {
                    currentAnimation = "Highlighted";
                }
                else
                {
                    currentAnimation = "Normal";
                }
                m_GamePadButtons[buttonIndexX].highlightedButtons[buttonIndexY].GetComponent<Animator>().SetTrigger(currentAnimation);
            }
        }*/
        Debug.Log("Right - Actions");
    }

    private void ButtonSelectionActions(InputAction.CallbackContext context)
    {

        Action buttonSelectionAction = () =>
        {
            for (int buttonIndexX = Constants.INT_ZERO; buttonIndexX < m_GamePadButtons.Count; buttonIndexX++)
            {
                for (int buttonIndexY = Constants.INT_ZERO; buttonIndexY < m_GamePadButtons[buttonIndexX].highlightedButtons.Count; buttonIndexY++)
                {
                    if (buttonIndexX == m_CurrentIndexX && buttonIndexY == m_CurrentIndexY)
                    {
                        Debug.Log("Button Triggered Successfully");
                        m_GamePadButtons[buttonIndexX].highlightedButtons[buttonIndexY].GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
        };
        if (m_GamePadSelectionSelectStatus)
            buttonSelectionAction?.Invoke();
    }

    private void RemoveAllUIEntries()
    {
        foreach (Transform child in m_LeaderBoardEntryParent.transform)
        {
            if (child.gameObject.tag == "LeaderBoardRowEntry")
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void OnLeaderboardUpdated(List<LeaderBoardData> entries)
    {
        if (entries != null && entries.Count > Constants.INT_ZERO)
        {
            RemoveAllUIEntries();
            SortEnteries(entries);
            AssignRanks(entries);
            int rank = Constants.INT_ONE;
            int enteriesCount = 0;
            foreach (LeaderBoardData entry in entries)
            {
                enteriesCount++;
                GameObject entryRow = Instantiate(m_LeaderBoardEntryPrefab, m_LeaderBoardEntryParent.transform);
                LeaderboardDetails rowEntry = entryRow.GetComponent<LeaderboardDetails>();
                rowEntry.SetData(entry);
                if (enteriesCount >= Constants.MAX_LEADERBOARDENTRIES)
                {
                    break;
                }
            }
        }
        else if (entries == null)
        {
            Debug.Log("ups something went wrong");
        }
    }

    private void SortEnteries(List<LeaderBoardData> entries)
    {
        for (int enteryIndex1 = Constants.INT_ZERO; enteryIndex1 < entries.Count; enteryIndex1++)
        {
            for (int enteryIndex2 = enteryIndex1 + Constants.INT_ONE; enteryIndex2 < entries.Count; enteryIndex2++)
            {
                LeaderBoardData maxData = entries[enteryIndex1];
                if (maxData.playerScore < entries[enteryIndex2].playerScore)
                {
                    entries[enteryIndex1] = entries[enteryIndex2];
                    entries[enteryIndex2] = maxData;
                }
            }
        }
    }

    private void AssignRanks(List<LeaderBoardData> entries)
    {
        int rank = Constants.INT_ONE;
        for (int entryIndex = Constants.INT_ZERO; entryIndex < entries.Count; entryIndex++)
        {
            entries[entryIndex].playerRank = rank++;
        }
    }


    #endregion

    #region Public_Methods
    public void OnSinglePlayerTap()
    {
        DiableInnPuts();
        GameManager.instace.CurrentGame = Constants.INT_ZERO;
    }
    public void OnTwoPlayerTap()
    {
        DiableInnPuts();
        GameManager.instace.CurrentGame = Constants.INT_ONE;
    }

    private void DiableInnPuts()
    {
        m_UpActions.Disable();
        m_DownActions.Disable();
        m_LeftActions.Disable();
        m_RightActions.Disable();
        m_SelectButton.Disable();
    }
    #endregion
}
