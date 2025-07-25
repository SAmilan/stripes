using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using DanielLochner.Assets.SimpleScrollSnap;

public class ViewLevelOptionsPanel : UIView
{
    #region Private_Variables
    private SinglePlayerInputs m_LevelOpionInputs;
    private InputAction m_LeftStickUp;
    private InputAction m_LeftStickDown;
    private InputAction m_LeftStickRight;
    private InputAction m_LeftStickLeft;
    private InputAction m_DpadLeft;
    private InputAction m_DpadRight;
    private InputAction m_DpadLeftUp;
    private InputAction m_DpadLeftDown;
    private InputAction m_DpadRightUp;
    private InputAction m_DpadRightDown;
    private InputAction m_SubmitButton;

    [SerializeField]
    private Button m_SaveButton;
    [SerializeField]
    private Button m_HomeButton;
    [SerializeField]
    private Animator m_MainAnimator;
    [SerializeField]
    private Slider m_StartingPieceTimer;
    private int m_StartingPieceTimerValue;
    [SerializeField]
    private Slider m_SinglePlayerColumn;
    private int m_SinglePlayerColumnValue;
    [SerializeField]
    private Slider m_SinglePlayerRows;
    private int m_SinglePlayerRowsValue;
    [SerializeField]
    private Slider m_MultiplayerColumns;
    private int m_MultiplayerColumnValue;
    [SerializeField]
    private Slider m_MultiPlayerRows;
    private int m_MultiplayerRowValue;
    [SerializeField]
    private Slider m_MultiplayerLoopCompleted;
    private int m_MultiplayerLoopCompletedValue;
    [SerializeField]
    private Slider m_RowColumnIncrease;
    private int m_RowColumnIncreaseValue;
    [SerializeField]
    private Slider m_ObstacleCoverage;
    private int m_ObstacleCoverageValue;
    [SerializeField]
    private Slider m_FlowSpeed;
    private float m_FlowSpeedValue;
    [SerializeField]
    private Slider m_LenghtPoints;
    private int m_LenghtPointsValue;
    [SerializeField]
    private Slider m_CrossPiecePoints;
    private int m_CrossPiecePointsValue;
    [SerializeField]
    private Slider m_ClosedLoopPoints;
    private int m_ClosedLoopPointsValue;
    [SerializeField]
    private Slider m_TileReplacementPenalty;
    private int m_TileReplacementPenaltyValue;
    [SerializeField]
    private Slider m_UnusedTilePenalty;
    [SerializeField]
    private Slider m_StartingPieceDeltaSlider;
    private int m_UnusedTilePenaltyValue;
    [SerializeField]
    private SimpleScrollSnap m_BombScroller;
    [SerializeField]
    private string m_BombProbability;
    [SerializeField]
    private SimpleScrollSnap m_DisplayHighScoreScroller;
    [SerializeField]
    private SimpleScrollSnap m_UpcomingTileStatusScroller;
    [SerializeField]
    private SimpleScrollSnap m_OptionMenuStatusScroller;
    //for gamepadcontrols
    [SerializeField]
    private int m_NextOptionIndex;
    [SerializeField]
    private int m_PreviosOptionIndex;
    [SerializeField]
    private int m_UPOptionIndex;
    [SerializeField]
    private int m_DownOptionIndex;
    private bool m_IsGamePadActive;
    private bool m_DPadLeftStatus;
    private bool m_DPadRightStatus;
    private int m_StartingPieceDeltaValue;
    private Coroutine m_LeftDpadMovement;
    private Coroutine m_RightDpadMovement;

    #endregion
    #region Public_Variables
    public static Action<int, Action<int, int, int, int>> s_HighlightOption;
    public static Action s_OnOptionPanelCloses;
    public static Action<int, int, int, int> s_OnStartingPiecePoolChanges;
    public static Action s_OnObstacleCoverageChanges;
    public static Action s_OnBombProbabiltyChanges;
    public static Action s_OnUpcomingStatusChanges;
    public static Action s_ResetOption;
    public static Action s_NextButtonTrigger;
    public static Action s_PreviosButtonTrigger;
    public static Action s_OnGamepadSubmit;

    #endregion

    #region Private_Methods

    private void Awake()
    {
        m_LevelOpionInputs = new SinglePlayerInputs();
       
        m_LeftStickUp = m_LevelOpionInputs.OptionController.UpControls;
        m_LeftStickDown = m_LevelOpionInputs.OptionController.DownControls;
        m_LeftStickLeft = m_LevelOpionInputs.OptionController.LeftControls;
        m_LeftStickRight = m_LevelOpionInputs.OptionController.RightControls;
        m_DpadLeft = m_LevelOpionInputs.OptionController.LeftControlsDPad;
        m_DpadRight = m_LevelOpionInputs.OptionController.RightControlsDPad;
        m_DpadLeftUp = m_LevelOpionInputs.OptionController.LeftControlsDPadUP;
        m_DpadLeftDown = m_LevelOpionInputs.OptionController.LeftControlsDPadDown;
        m_DpadRightUp = m_LevelOpionInputs.OptionController.RightControlsDPadUP;
        m_DpadRightDown = m_LevelOpionInputs.OptionController.RightControlsDPadDown;
        m_SubmitButton = m_LevelOpionInputs.OptionController.Selection;
        m_IsGamePadActive = false;
        m_HomeButton.onClick.AddListener(OnHomeButtonTap);
        m_SaveButton.onClick.AddListener(OnSaveButtonTap);
        m_StartingPieceTimer.onValueChanged.AddListener(OnStartingPiecTimerChanges);
        m_SinglePlayerColumn.onValueChanged.AddListener(OnSinglePlayerColumnChanges);
        m_SinglePlayerRows.onValueChanged.AddListener(OnSinglePlayerRowChanges);
        m_MultiplayerColumns.onValueChanged.AddListener(OnMultiPlayerColumnChanges);
        m_MultiPlayerRows.onValueChanged.AddListener(OnMultiPlayerRowChanges);
        m_MultiplayerLoopCompleted.onValueChanged.AddListener(OnMultiPlayerLoopCompleteChanges);
        m_RowColumnIncrease.onValueChanged.AddListener(OnRowColumnIncreaseRateChanges);
        m_StartingPieceDeltaSlider.onValueChanged.AddListener(OnStartingPieceDeltaChanges);
        m_ObstacleCoverage.onValueChanged.AddListener(OnObstaceleCoverageAreaChanges);
        m_FlowSpeed.onValueChanged.AddListener(OnFlowSpeedChanges);
        m_LenghtPoints.onValueChanged.AddListener(OnLengthPointValueChanges);
        m_CrossPiecePoints.onValueChanged.AddListener(OnCrossPointsValueChanges);
        m_ClosedLoopPoints.onValueChanged.AddListener(OnClosedLoopPointsValueChanges);
        m_TileReplacementPenalty.onValueChanged.AddListener(OnTileReplacementValueChanges);
        m_UnusedTilePenalty.onValueChanged.AddListener(OnUnUsedTilePenaltyValueChanges);
        m_BombScroller.OnPanelCentered.AddListener(OnBombProbabilityChanges);
        m_DisplayHighScoreScroller.OnPanelCentered.AddListener(OnDisplayHighScoreStatusChanges);
        m_UpcomingTileStatusScroller.OnPanelCentered.AddListener(OnDisplayUpcomingTileStatusChanges);
        m_OptionMenuStatusScroller.OnPanelCentered.AddListener(OnOptionPanelStatusChanges);
    }
    private void OnEnable()
    {
        m_NextOptionIndex = m_PreviosOptionIndex = m_UPOptionIndex = m_DownOptionIndex = Constants.INT_ZERO;
        m_DPadLeftStatus = false;
        m_DPadRightStatus = false;
        m_LeftStickUp.performed += LeftStickUpButton;
        m_LeftStickDown.performed += LeftStickDownButton;
        m_LeftStickLeft.performed += LeftStickLeftButton;
        m_LeftStickRight.performed += LeftStickRightButton;
        m_DpadLeft.performed += DpadLeftButton;
        m_DpadRight.performed += DpadRightButton;
        m_DpadLeftUp.performed += DpadLeftUP;
        m_DpadLeftDown.performed += DpadLeftDown;
        m_DpadRightUp.performed += DpadRightUP;
        m_DpadRightDown.performed += DpadRighttDown;
        m_SubmitButton.performed += OnGamePadSubmit;

        m_LeftStickUp.Enable();
        m_LeftStickDown.Enable();
        m_LeftStickLeft.Enable();
        m_LeftStickRight.Enable();
        m_DpadLeft.Enable();
        m_DpadRight.Enable();
        m_DpadLeftUp.Enable();
        m_DpadLeftDown.Enable();
        m_DpadRightUp.Enable();
        m_DpadRightDown.Enable();
        m_SubmitButton.Enable();
        GameData.Instance.GameOptions = OptionsReader.instance.LoadFile();
        m_SinglePlayerColumn.value = GameData.Instance.GameOptions.SinglePlayerGameColumns;
        m_SinglePlayerColumn.GetComponent<ValueChangingSlider>().ValueChange(m_SinglePlayerColumn.value);
        m_SinglePlayerColumnValue = (int)m_SinglePlayerColumn.value;

        m_SinglePlayerRows.value = GameData.Instance.GameOptions.SinglePlayerGameRows;
        m_SinglePlayerRows.GetComponent<ValueChangingSlider>().ValueChange(m_SinglePlayerRows.value);
        m_SinglePlayerRowsValue = (int)m_SinglePlayerRows.value;

        m_MultiPlayerRows.value = GameData.Instance.GameOptions.TwoPlayerGameRows;
        m_MultiPlayerRows.GetComponent<ValueChangingSlider>().ValueChange(m_MultiPlayerRows.value);
        m_MultiplayerRowValue = (int)m_MultiPlayerRows.value;

        m_MultiplayerColumns.value = GameData.Instance.GameOptions.TwoPlayerGameColumn;
        m_MultiplayerColumns.GetComponent<ValueChangingSlider>().ValueChange(m_MultiplayerColumns.value);
        m_MultiplayerColumnValue = (int)m_MultiplayerColumns.value;

        m_MultiplayerLoopCompleted.value = GameData.Instance.GameOptions.MultiplayerLoopCompleted;
        m_MultiplayerLoopCompleted.GetComponent<ValueChangingSlider>().ValueChange(m_MultiplayerLoopCompleted.value);
        m_MultiplayerLoopCompletedValue = (int)m_MultiplayerLoopCompleted.value;

        m_StartingPieceTimer.value = GameData.Instance.GameOptions.StartingPieceTimer;
        m_StartingPieceTimer.GetComponent<ValueChangingSlider>().ValueChange(m_StartingPieceTimer.value);
        m_StartingPieceTimerValue = (int)m_StartingPieceTimer.value;

        m_RowColumnIncrease.value = GameData.Instance.GameOptions.RowColumnIncreaseRate;
        m_RowColumnIncrease.GetComponent<ValueChangingSlider>().ValueChange(m_RowColumnIncrease.value);
        m_RowColumnIncreaseValue = (int)m_RowColumnIncrease.value;

        m_StartingPieceDeltaSlider.value = GameData.Instance.GameOptions.StartingPieceTimeDifference;
        m_StartingPieceDeltaSlider.GetComponent<ValueChangingSlider>().ValueChange(m_StartingPieceDeltaSlider.value);
        m_StartingPieceDeltaValue = (int)m_StartingPieceDeltaSlider.value;

        m_ObstacleCoverage.value = GameData.Instance.GameOptions.ObstacleCoverage;
        m_ObstacleCoverage.GetComponent<ValueChangingSlider>().ValueChange(m_ObstacleCoverage.value);
        m_ObstacleCoverageValue = (int)m_ObstacleCoverage.value;

        m_FlowSpeed.value = GameData.Instance.GameOptions.FlowSpeed;
        m_FlowSpeed.GetComponent<ValueChangingSlider>().ValueChange(m_FlowSpeed.value);
        m_FlowSpeedValue = m_FlowSpeed.value;

        m_LenghtPoints.value = GameData.Instance.GameOptions.LengthPoints;
        m_LenghtPoints.GetComponent<ValueChangingSlider>().ValueChange(m_LenghtPoints.value);
        m_LenghtPointsValue = (int)m_LenghtPoints.value;

        m_CrossPiecePoints.value = GameData.Instance.GameOptions.CrossPoints;
        m_CrossPiecePoints.GetComponent<ValueChangingSlider>().ValueChange(m_CrossPiecePoints.value);
        m_CrossPiecePointsValue = (int)m_CrossPiecePoints.value;

        m_ClosedLoopPoints.value = GameData.Instance.GameOptions.ClosedLoopPoints;
        m_ClosedLoopPoints.GetComponent<ValueChangingSlider>().ValueChange(m_ClosedLoopPoints.value);
        m_ClosedLoopPointsValue = (int)m_ClosedLoopPoints.value;

        m_TileReplacementPenalty.value = GameData.Instance.GameOptions.TileReplacementPenalty;
        m_TileReplacementPenalty.GetComponent<ValueChangingSlider>().ValueChange(m_TileReplacementPenalty.value);
        m_TileReplacementPenaltyValue = (int)m_TileReplacementPenalty.value;

        m_UnusedTilePenalty.value = GameData.Instance.GameOptions.UnusedTilePenalty;
        m_UnusedTilePenalty.GetComponent<ValueChangingSlider>().ValueChange(m_UnusedTilePenalty.value);
        m_UnusedTilePenaltyValue = (int)m_UnusedTilePenalty.value;

        m_BombProbability = GameData.Instance.GameOptions.BombProbability.ToLower();
        m_BombScroller.StartingPanel = (m_BombProbability == Constants.BOMB_NORMAL) ? Constants.OPTIONINDEX_0 : (m_BombProbability == Constants.BOMB_FEW) ? Constants.OPTIONINDEX_1 : (m_BombProbability == Constants.BOMB_MANY) ? Constants.OPTIONINDEX_2 : (m_BombProbability == Constants.NONEBOMB) ? Constants.OPTIONINDEX_3 : Constants.OPTIONINDEX_0;
        Canvas.ForceUpdateCanvases();

        m_DisplayHighScoreScroller.StartingPanel = (GameData.Instance.GameOptions.DisplayHighScore) ? Constants.OPTIONINDEX_0 : Constants.OPTIONINDEX_1;

        m_OptionMenuStatusScroller.StartingPanel = (GameData.Instance.GameOptions.ShowOptionStatus) ? Constants.OPTIONINDEX_0 : Constants.OPTIONINDEX_1;

        m_UpcomingTileStatusScroller.StartingPanel = (GameData.Instance.GameOptions.UpcomingTileStatus) ? Constants.OPTIONINDEX_0 : Constants.OPTIONINDEX_1;
        //m_UpcomingTileStatusScroller.SnapTarget = (SnapTarget)((GameData.Instance.GameOptions.UpcomingTileStatus) ? Constants.OPTIONINDEX_0 : Constants.OPTIONINDEX_1);
        Canvas.ForceUpdateCanvases();

    }

    private void OnDisable()
    {
        m_NextOptionIndex = m_PreviosOptionIndex = m_UPOptionIndex = m_DownOptionIndex = Constants.INT_ZERO;
        m_LeftStickUp.performed -= LeftStickUpButton;
        m_LeftStickDown.performed -= LeftStickDownButton;
        m_LeftStickLeft.performed -= LeftStickLeftButton;
        m_LeftStickRight.performed -= LeftStickRightButton;
        m_DpadLeft.performed -= DpadLeftButton;
        m_DpadRight.performed -= DpadRightButton;
        m_DpadLeftUp.performed -= DpadLeftUP;
        m_DpadLeftDown.performed -= DpadLeftDown;
        m_DpadRightUp.performed -= DpadRightUP;
        m_DpadRightDown.performed -= DpadRighttDown;
        m_SubmitButton.performed -= OnGamePadSubmit;
        m_LeftStickUp.Disable();
        m_LeftStickDown.Disable();
        m_LeftStickLeft.Disable();
        m_LeftStickRight.Disable();
        m_DpadLeftUp.Disable();
        m_DpadLeftDown.Disable();
        m_DpadRightUp.Disable();
        m_DpadRightDown.Disable();
        m_DpadLeft.Disable();
        m_DpadRight.Disable();
        m_SubmitButton.Disable();
        GameData.Instance.GameOptions = OptionsReader.instance.LoadFile();
    }

    /*private void Update()
    {
        if (m_DPadLeftStatus && m_IsGamePadActive)
        {
            s_PreviosButtonTrigger?.Invoke();
        }
        if(m_DPadRightStatus && m_IsGamePadActive)
        {
            s_NextButtonTrigger?.Invoke();
        }
    }*/

    private void LeftStickUpButton(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        Action<int, int, int, int> AttachedIndexs = (int upIndex, int downIndex, int leftIndex, int righttIndex) =>
            {
                m_UPOptionIndex = upIndex;
                m_DownOptionIndex = downIndex;
                m_NextOptionIndex = righttIndex;
                m_PreviosOptionIndex = leftIndex;
            };
        if (m_UPOptionIndex != Constants.NULLINDEX)
            s_HighlightOption?.Invoke(m_UPOptionIndex, AttachedIndexs);
    }
    private void LeftStickDownButton(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        Action<int, int, int, int> AttachedIndexs = (int upIndex, int downIndex, int leftIndex, int righttIndex) =>
        {
            m_UPOptionIndex = upIndex;
            m_DownOptionIndex = downIndex;
            m_NextOptionIndex = righttIndex;
            m_PreviosOptionIndex = leftIndex;
        };
        if (m_DownOptionIndex != Constants.NULLINDEX)
            s_HighlightOption?.Invoke(m_DownOptionIndex, AttachedIndexs);
    }
    private void LeftStickLeftButton(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        Action<int, int, int, int> AttachedIndexs = (int upIndex, int downIndex, int leftIndex, int righttIndex) =>
        {
            m_UPOptionIndex = upIndex;
            m_DownOptionIndex = downIndex;
            m_NextOptionIndex = righttIndex;
            m_PreviosOptionIndex = leftIndex;
        };
        if (m_PreviosOptionIndex != Constants.NULLINDEX)
            s_HighlightOption?.Invoke(m_PreviosOptionIndex, AttachedIndexs);
    }
    private void LeftStickRightButton(InputAction.CallbackContext context)
    {
        m_IsGamePadActive = true;
        Action<int, int, int, int> AttachedIndexs = (int upIndex, int downIndex, int leftIndex, int righttIndex) =>
        {
            m_UPOptionIndex = upIndex;
            m_DownOptionIndex = downIndex;
            m_NextOptionIndex = righttIndex;
            m_PreviosOptionIndex = leftIndex;
        };
        if (m_NextOptionIndex != Constants.NULLINDEX)
            s_HighlightOption?.Invoke(m_NextOptionIndex, AttachedIndexs);
    }
    private void DpadLeftButton(InputAction.CallbackContext context)
    {
        /*if (m_IsGamePadActive)
            s_PreviosButtonTrigger?.Invoke();*/



    }
    private void DpadRightButton(InputAction.CallbackContext context)
    {
        /*if (m_IsGamePadActive)
            s_NextButtonTrigger?.Invoke();*/
    }
    private void OnGamePadSubmit(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
            s_OnGamepadSubmit?.Invoke();
    }
    private void DpadLeftUP(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_DPadLeftStatus = false;
            StopCoroutine(m_LeftDpadMovement);
        }
    }
    private void DpadLeftDown(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_DPadLeftStatus = true;
            m_LeftDpadMovement = StartCoroutine(LeftDpadMovement());
        }
    }


    private void DpadRightUP(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_DPadRightStatus = false;
            StopCoroutine(m_RightDpadMovement);
        }
    }
    private void DpadRighttDown(InputAction.CallbackContext context)
    {
        if (m_IsGamePadActive)
        {
            m_DPadRightStatus = true;
            m_RightDpadMovement = StartCoroutine(RightDpadMovement());
        }
    }


    public void OnMainPanelMouseMovement()
    {
        m_IsGamePadActive = false;
        s_ResetOption?.Invoke();
        m_UPOptionIndex = Constants.INT_ZERO;
        m_DownOptionIndex = Constants.INT_ZERO;
        m_NextOptionIndex = Constants.INT_ZERO;
        m_PreviosOptionIndex = Constants.INT_ZERO;
        m_DPadRightStatus = false;
        m_DPadLeftStatus = false;

    }


    private void OnStartingPieceTimerChanges()
    {
        m_StartingPieceTimerValue = (int)m_StartingPieceTimer.value;
    }
    private void OnBombProbabilityChanges(int activeIndex, int previosIndex)
    {
        string BombProbability = (activeIndex == Constants.OPTIONINDEX_0) ? Constants.BOMB_NORMAL : (activeIndex == Constants.OPTIONINDEX_1) ? Constants.BOMB_FEW : (activeIndex == Constants.OPTIONINDEX_2) ? Constants.BOMB_MANY : (activeIndex == Constants.OPTIONINDEX_3) ? Constants.NONEBOMB : Constants.BOMB_NORMAL;
        GameData.Instance.GameOptions.BombProbability = BombProbability;
    }

    private void OnDisplayHighScoreStatusChanges(int activeIndex, int previosIndex)
    {
        bool hightScoreStatus = (activeIndex == Constants.OPTIONINDEX_1) ? false : true;
        GameData.Instance.GameOptions.DisplayHighScore = hightScoreStatus;
    }

    private void OnDisplayUpcomingTileStatusChanges(int activeIndex, int previosIndex)
    {
        bool upcomingTileStatus = (activeIndex == Constants.OPTIONINDEX_1) ? false : true;
        GameData.Instance.GameOptions.UpcomingTileStatus = upcomingTileStatus;
    }

    private void OnOptionPanelStatusChanges(int activeIndex, int previosIndex)
    {
        bool optionPanelStatusChanges = (activeIndex == Constants.OPTIONINDEX_1) ? false : true;
        GameData.Instance.GameOptions.ShowOptionStatus = optionPanelStatusChanges;
    }


    private void OnHomeButtonTap()
    {
        m_MainAnimator.SetTrigger("Outro");
        StartCoroutine(CloseOptiions());
    }
    IEnumerator CloseOptiions()
    {
        yield return new WaitForSeconds(Constants.OPTIONPANEL_CLOSEDELAY);
        s_OnOptionPanelCloses?.Invoke();
    }

    private void OnStartingPiecTimerChanges(float value)
    {
        m_StartingPieceTimerValue = (int)value;
        m_StartingPieceTimer.GetComponent<ValueChangingSlider>().ValueChange(m_StartingPieceTimerValue);
        GameData.Instance.GameOptions.StartingPieceTimer = m_StartingPieceTimerValue;
    }

    private void OnSinglePlayerRowChanges(float value)
    {
        m_SinglePlayerRowsValue = (int)value;
        m_SinglePlayerRows.GetComponent<ValueChangingSlider>().ValueChange(m_SinglePlayerRowsValue);
        GameData.Instance.GameOptions.SinglePlayerGameRows = m_SinglePlayerRowsValue;
    }
    private void OnSinglePlayerColumnChanges(float value)
    {
        m_SinglePlayerColumnValue = (int)value;
        m_SinglePlayerColumn.GetComponent<ValueChangingSlider>().ValueChange(m_SinglePlayerColumnValue);
        GameData.Instance.GameOptions.SinglePlayerGameColumns = m_SinglePlayerColumnValue;
    }

    private void OnMultiPlayerRowChanges(float value)
    {
        m_MultiplayerRowValue = (int)value;
        m_MultiPlayerRows.GetComponent<ValueChangingSlider>().ValueChange(m_MultiplayerRowValue);
        GameData.Instance.GameOptions.TwoPlayerGameRows = m_MultiplayerRowValue;
    }
    private void OnMultiPlayerColumnChanges(float value)
    {
        m_MultiplayerColumnValue = (int)value;
        m_MultiplayerColumns.GetComponent<ValueChangingSlider>().ValueChange(m_MultiplayerColumnValue);
        GameData.Instance.GameOptions.TwoPlayerGameColumn = m_MultiplayerColumnValue;
    }

    private void OnMultiPlayerLoopCompleteChanges(float value)
    {
        m_MultiplayerLoopCompletedValue = (int)value;
        m_MultiplayerLoopCompleted.GetComponent<ValueChangingSlider>().ValueChange(m_MultiplayerLoopCompletedValue);
        GameData.Instance.GameOptions.MultiplayerLoopCompleted = m_MultiplayerLoopCompletedValue;
    }
    private void OnRowColumnIncreaseRateChanges(float value)
    {
        m_RowColumnIncreaseValue = (int)value;
        m_RowColumnIncrease.GetComponent<ValueChangingSlider>().ValueChange(m_RowColumnIncreaseValue);
        GameData.Instance.GameOptions.RowColumnIncreaseRate = m_RowColumnIncreaseValue;
    }
    private void OnStartingPieceDeltaChanges(float value)
    {
        m_StartingPieceDeltaValue = (int)value;
        m_StartingPieceDeltaSlider.GetComponent<ValueChangingSlider>().ValueChange(m_StartingPieceDeltaValue);
        GameData.Instance.GameOptions.StartingPieceTimeDifference = m_StartingPieceDeltaValue;
    }
    private void OnObstaceleCoverageAreaChanges(float value)
    {
        m_ObstacleCoverageValue = (int)value;
        m_ObstacleCoverage.GetComponent<ValueChangingSlider>().ValueChange(m_ObstacleCoverageValue);
        GameData.Instance.GameOptions.ObstacleCoverage = m_ObstacleCoverageValue;
    }
    private void OnFlowSpeedChanges(float value)
    {
        m_FlowSpeedValue = Mathf.Round(value * 100f) / 100f;
        Debug.Log("The fLOW sPEED IS: " + m_FlowSpeedValue);
        m_FlowSpeed.GetComponent<ValueChangingSlider>().ValueChange(m_FlowSpeedValue);
        GameData.Instance.GameOptions.FlowSpeed = m_FlowSpeedValue;
    }
    private void OnLengthPointValueChanges(float value)
    {
        m_LenghtPointsValue = (int)value;
        m_LenghtPoints.GetComponent<ValueChangingSlider>().ValueChange(m_LenghtPointsValue);
        GameData.Instance.GameOptions.LengthPoints = m_LenghtPointsValue;
    }

    private void OnCrossPointsValueChanges(float value)
    {
        m_CrossPiecePointsValue = (int)value;
        m_CrossPiecePoints.GetComponent<ValueChangingSlider>().ValueChange(m_CrossPiecePointsValue);
        GameData.Instance.GameOptions.CrossPoints = m_CrossPiecePointsValue;
    }
    private void OnClosedLoopPointsValueChanges(float value)
    {
        m_ClosedLoopPointsValue = (int)value;
        m_ClosedLoopPoints.GetComponent<ValueChangingSlider>().ValueChange(m_ClosedLoopPointsValue);
        GameData.Instance.GameOptions.ClosedLoopPoints = m_ClosedLoopPointsValue;
    }
    private void OnTileReplacementValueChanges(float value)
    {
        m_TileReplacementPenaltyValue = (int)value;
        m_TileReplacementPenalty.GetComponent<ValueChangingSlider>().ValueChange(m_TileReplacementPenaltyValue);
        GameData.Instance.GameOptions.TileReplacementPenalty = m_TileReplacementPenaltyValue;
    }
    private void OnUnUsedTilePenaltyValueChanges(float value)
    {
        m_UnusedTilePenaltyValue = (int)value;
        m_UnusedTilePenalty.GetComponent<ValueChangingSlider>().ValueChange(m_UnusedTilePenaltyValue);
        GameData.Instance.GameOptions.UnusedTilePenalty = m_UnusedTilePenaltyValue;
    }

    public void OnStartingPieceTimerButtonTap(int value)
    {
        m_StartingPieceTimer.value += value;
    }
    public void OnSinglePlayerColumnButtonTap(int value)
    {
        m_SinglePlayerColumn.value += value;
    }
    public void OnSinglePlayerRowButtonTap(int value)
    {
        m_SinglePlayerRows.value += value;
    }
    public void OnTwoPlayerColumnButtonTap(int value)
    {
        m_MultiplayerColumns.value += value;
    }
    public void OnTwoPlayerRowButtonTap(int value)
    {
        m_MultiPlayerRows.value += value;
    }
    public void OnLoopCoompletedButtonTap(int value)
    {
        m_MultiplayerLoopCompleted.value += value;
    }
    public void OnRowColumnRateButtonTap(int value)
    {
        m_RowColumnIncrease.value += value;
    }
    public void OnObstacleCoverageButtonTap(int value)
    {
        m_ObstacleCoverage.value += value;
    }
    public void OnFlowSpeedButtonTap(float value)
    {
        m_FlowSpeed.value += value;
    }
    public void OnLenghtPointButtonTap(int value)
    {
        m_LenghtPoints.value += value;
    }
    public void OnCrossPointButtonTap(int value)
    {
        m_CrossPiecePoints.value += value;
    }
    public void OnClosedLoopPointsButtonTap(int value)
    {
        m_ClosedLoopPoints.value += value;
    }
    public void OnTileReplacementButtonTap(int value)
    {
        m_TileReplacementPenalty.value += value;
    }
    public void OnUnusedTilePenaltyButtonTap(int value)
    {
        m_UnusedTilePenalty.value += value;
    }

    public void OnStartingPieceDeltaChanges(int value)
    {
        m_StartingPieceDeltaSlider.value += value;
    }


    private void OnSaveButtonTap()
    {
        OptionsReader.instance.SaveFile();
        s_OnObstacleCoverageChanges?.Invoke();
        s_OnBombProbabiltyChanges?.Invoke();
        s_OnUpcomingStatusChanges?.Invoke();
        s_OnStartingPiecePoolChanges?.Invoke(GameData.Instance.GameOptions.SinglePlayerGameRows, GameData.Instance.GameOptions.SinglePlayerGameColumns, GameData.Instance.GameOptions.TwoPlayerGameRows, GameData.Instance.GameOptions.TwoPlayerGameColumn);
        StartCoroutine(CloseOptiions());
    }



    #endregion


    #region Coroutines

    private IEnumerator LeftDpadMovement()
    {
        while (m_DPadLeftStatus)
        {
            s_PreviosButtonTrigger?.Invoke();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator RightDpadMovement()
    {
        while (m_DPadRightStatus)
        {
            s_NextButtonTrigger?.Invoke();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

}
