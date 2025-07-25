using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class ViewOptionPanel : UIView
{
    #region Private_Variables
    [SerializeField]
    private int m_Sidelength;
    [SerializeField]
    private ScrollRect m_SidelengthScrollRect;
    [SerializeField]
    private Button m_PlayingAreaNextButton;
    [SerializeField]
    private Button m_PlayingAreaPreviosButton;
    [SerializeField]
    private ScrollRect m_BombScrollRect;
    [SerializeField]
    private Button m_BombNextButton;
    [SerializeField]
    private Button m_BombPreviosButton;
    [SerializeField]
    private Button m_StartingPieceNextButton;
    [SerializeField]
    private Button m_StartingPiecePreviousButton;
    [SerializeField]
    private ScrollRect m_StartingPieceScrollRect;
    [SerializeField]
    private Button m_ObstacleNextButton;
    [SerializeField]
    private Button m_ObstaclePreviousButton;
    [SerializeField]
    private ScrollRect m_ObstacleScrollRect;
    [SerializeField]
    private Button m_UpcomingTileNextButton;
    [SerializeField]
    private Button m_UpcomingTilePreviousButton;
    [SerializeField]
    private ScrollRect m_UpcomingTileScrollRect;
    private SinglePlayerInputs m_PlayerInputs;
    private InputAction m_CloseAction;
    [SerializeField]
    private Button m_HomeButton;

    #endregion

    #region Static_Variables
    public static Action s_OnOptionPanelCloses;
    public static Action<int> s_SideLength;
    public static Action<int> s_StartingPieceStatusChanges;
    public static Action<float> s_OnBombProbabityChanges;
    public static Action<int> s_ObstacleStatusChanges;
    public static Action<bool> s_UpcomingTileStatusChanges;

    #endregion

    #region Public_Variables


    #endregion

    #region Unity_Callbacks
     private void Awake()
    {
        m_PlayerInputs = new SinglePlayerInputs();
        m_HomeButton.onClick.AddListener(OnOptionPanelClose);
        m_PlayingAreaNextButton.onClick.AddListener(OnPlayingAreaNextButtonTap);
        m_PlayingAreaPreviosButton.onClick.AddListener(OnPlayingAreaPreviousButtonTap);
        m_BombNextButton.onClick.AddListener(OnBombsNextButtonTap);
        m_BombPreviosButton.onClick.AddListener(OnBombsPreviousButtonTap);
        m_StartingPieceNextButton.onClick.AddListener(OnStartingPieceNextButtonTap);
        m_StartingPiecePreviousButton.onClick.AddListener(OnStartingPiecePreviousButtonTap);
        m_ObstacleNextButton.onClick.AddListener(OnObstacleNextButtonTap);
        m_ObstaclePreviousButton.onClick.AddListener(OnObstaclePreviousButtonTap);
        m_UpcomingTileNextButton.onClick.AddListener(OnUpcomingTileNextButtonTap);
        m_UpcomingTilePreviousButton.onClick.AddListener(OnUpcomingTilePreviousButtonTap);
    }

    #endregion

    #region Public_Methods
    #endregion

    #region Private_Methods
    private void OnUpcomingTileNextButtonTap()
    {
        s_UpcomingTileStatusChanges?.Invoke(false);
    }

    private void OnUpcomingTilePreviousButtonTap()
    {
        s_UpcomingTileStatusChanges?.Invoke(true);
    }

    private void OnObstacleNextButtonTap()
    {
        int currentIndex = GetPositiveScrollerIndex(m_ObstacleScrollRect);
        currentIndex = (currentIndex > (m_ObstacleScrollRect.content.childCount - 1)) ? (m_ObstacleScrollRect.content.childCount - 1) : currentIndex;
        s_ObstacleStatusChanges?.Invoke(currentIndex);
    }
    
    private void OnObstaclePreviousButtonTap()
    {
        int currentIndex = GetNegativeScrollerIndex(m_ObstacleScrollRect);
        currentIndex = (currentIndex < Constants.ZERO) ? (int)Constants.ZERO : currentIndex;
        s_ObstacleStatusChanges?.Invoke(currentIndex);
    }
    private void OnStartingPieceNextButtonTap()
    {
        int currentIndex = GetPositiveScrollerIndex(m_StartingPieceScrollRect);
        currentIndex = (currentIndex > (m_StartingPieceScrollRect.content.childCount - 1)) ? (m_StartingPieceScrollRect.content.childCount - 1) : currentIndex;
        s_StartingPieceStatusChanges?.Invoke(currentIndex);
    }
    private void OnStartingPiecePreviousButtonTap()
    {
        int currentIndex = GetNegativeScrollerIndex(m_StartingPieceScrollRect);
        currentIndex = (currentIndex < Constants.ZERO) ? (int)Constants.ZERO : currentIndex;
        s_StartingPieceStatusChanges?.Invoke(currentIndex);
    }

    private void OnBombsNextButtonTap()
    {
        int currentIndex = GetPositiveScrollerIndex(m_BombScrollRect);
        currentIndex = (currentIndex > (m_BombScrollRect.content.childCount - 1)) ? (m_SidelengthScrollRect.content.childCount - 1) : currentIndex;
        AssignBombProbability(currentIndex);
    }

    private void OnBombsPreviousButtonTap()
    {
        int currentIndex = GetNegativeScrollerIndex(m_BombScrollRect);
        currentIndex = (currentIndex < Constants.ZERO) ? (int)Constants.ZERO : currentIndex;
        AssignBombProbability(currentIndex);
    }
    private void OnPlayingAreaNextButtonTap()
    {
        int currentIndex = GetPositiveScrollerIndex(m_SidelengthScrollRect);
        currentIndex = (currentIndex > (m_SidelengthScrollRect.content.childCount - 1)) ? (m_SidelengthScrollRect.content.childCount - 1) : currentIndex;
        AssignSideLength(currentIndex);
    }

    private void OnPlayingAreaPreviousButtonTap()
    {
        int currentIndex = GetNegativeScrollerIndex(m_SidelengthScrollRect);
        currentIndex = (currentIndex < (int)Constants.ZERO) ? (int)Constants.ZERO : currentIndex;
        AssignSideLength(currentIndex);
    }
    private static void AssignSideLength(int currentIndex)
    {
        int sideLength = (currentIndex == Constants.OPTIONINDEX_0) ? Constants.SIDELENGTH_4 : (currentIndex == Constants.OPTIONINDEX_1) ? Constants.SIDELENGTH_5 : (currentIndex == Constants.OPTIONINDEX_2) ? Constants.SIDELENGTH_6 : (currentIndex == Constants.OPTIONINDEX_3) ? Constants.SIDELENGTH_7 : (currentIndex == Constants.OPTIONINDEX_4) ? Constants.SIDELENGTH_8 : (currentIndex == Constants.OPTIONINDEX_5) ? Constants.SIDELENGTH_9 : (currentIndex == Constants.OPTIONINDEX_6) ? Constants.SIDELENGTH_10 : (currentIndex == Constants.OPTIONINDEX_7) ? Constants.SIDELENGTH_11 : (currentIndex == Constants.OPTIONINDEX_8) ? Constants.SIDELENGTH_12 : Constants.SIDELENGTH_4;
        s_SideLength?.Invoke(sideLength);
    }

    private static void AssignBombProbability(int currentIndex)
    {
        float bombProbabilty = (currentIndex == Constants.OPTIONINDEX_0) ? Constants.BOMB_NORMALPROBABILITY : (currentIndex == Constants.OPTIONINDEX_1) ? Constants.BOMB_FEWPROBABILITY : (currentIndex == Constants.OPTIONINDEX_2) ? Constants.BOMB_MANYPROBABILITY : (currentIndex == Constants.OPTIONINDEX_3) ? Constants.NOBOMB : Constants.BOMB_NORMALPROBABILITY;
        s_OnBombProbabityChanges?.Invoke(bombProbabilty);
    }

    private static int GetPositiveScrollerIndex(ScrollRect currentScrollRect)
    {
        int currentIndex = (int)Mathf.Ceil(Mathf.Abs(currentScrollRect.content.GetComponent<RectTransform>().anchoredPosition.x)) / Constants.OPTIONPANEL_SCROLLBUTTON;
        currentIndex++;
        return currentIndex;
    }
    private static int GetNegativeScrollerIndex(ScrollRect currentScrollRect)
    {
        int currentIndex = (int)Mathf.Ceil(Mathf.Abs(currentScrollRect.content.GetComponent<RectTransform>().anchoredPosition.x)) / Constants.OPTIONPANEL_SCROLLBUTTON;
        currentIndex--;
        return currentIndex;
    }
    private void OnOptionPanelClose()
    {
        s_OnOptionPanelCloses?.Invoke();
    }
    #endregion

    #region Coroutines


    #endregion
}
