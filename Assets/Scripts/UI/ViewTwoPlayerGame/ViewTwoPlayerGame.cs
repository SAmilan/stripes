using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using System.Linq;

public class ViewTwoPlayerGame : ViewGame
{
    #region Private_Variables
    
    
    [SerializeField]
    private RectTransform m_Player1BGImage;
    [SerializeField]
    private RectTransform m_Player2BGImage;
    [SerializeField]
    private GameObject m_Loader;
    [SerializeField]
    private GameObject m_PausePanel;
    [SerializeField]
    private GameObject m_Player1GridArea;
    [SerializeField]
    private GameObject m_Player2GridArea;
    [SerializeField]
    private GameObject m_StartingPieceArea_Player1;
    [SerializeField]
    private GameObject m_StartingPieceArea_Player2;
    [SerializeField]
    private GameObject m_UpcomingTileParentPlayer1;
    [SerializeField]
    private GameObject m_UpcomingTileParentPlayer2;
    [SerializeField]
    private GameObject m_BrokenPieceArea_Player1;
    [SerializeField]
    private GameObject m_TutorialPanel;
    [SerializeField]
    private GameObject m_BrokenPieceArea_Player2;
    [SerializeField]
    private GridTilePlayer1 m_GridTilePlayer1;
    [SerializeField]
    private GridTilePlayer2 m_GridTilePlayer2;
    [SerializeField]
    private float m_TileSize;
    [SerializeField]
    private SidePieceTye[] PieceTypesContainer;
    [SerializeField]
    private GameObject m_SideTileAreaPlayer1;
    [SerializeField]
    private GameObject m_SideTileAreaPlayer2;
    [SerializeField]
    private UpcomingTilePlayer1 m_UpcomingTilePlayer1;
    [SerializeField]
    private UpcomingTilePlayer2 m_UpcomingTilePlayer2;
    [SerializeField]
    private Button m_CloseButton;
    [SerializeField]
    private TextMeshProUGUI m_Player1NameText;
    [SerializeField]
    private TextMeshProUGUI m_Player2NameText;
    [SerializeField]
    private GameObject m_LeavePanel;
    [SerializeField]
    private Button m_LeavePanelOpenButton;
    [SerializeField]
    private Button m_PauseUIbutton;
    [SerializeField]
    private Button m_LeavePanelCloseButton;
    private SinglePlayerInputs m_TwoPlayerInputs;
    private SecondPlayerInputs m_SecondPlayerInputs;
    private InputAction m_MovePiecePlayer1;
    private InputAction m_MovePiecePlayer1Dpads;
    private InputAction m_PlacePiecePlayer1;
    private InputAction m_PlacePiecePlayer1Gamepads;
    private InputAction m_MovePiecePlayer2;
    private InputAction m_MovePiecePlayer2Dpads;
    private InputAction m_PlacePiecePlayer2;
    private InputAction m_PlacePiecePlayer2Gamepads;
    private InputAction m_QuitButton;
    private InputAction m_PauseButton;
    private InputAction m_StorePiece_Player1;
    //storechangesprivate InputAction m_StorePiece_Player1Gamepads;
    private InputAction m_StorePiece_Player2;
    //storchangesprivate InputAction m_StorePiece_Player2Gamepads;
    [SerializeField]
    private List<Vector2> m_ObstacleIgnoredArea_Player1 = new List<Vector2>();
    [SerializeField]
    private List<Vector2> m_ObstacleIgnoredArea_Player2 = new List<Vector2>();
    [SerializeField]
    private List<TilePiece> m_ObstacleSpawnArea_Player1 = new List<TilePiece>();
    [SerializeField]
    private List<TilePiece> m_ObstacleSpawnArea_Player2 = new List<TilePiece>();
    [SerializeField]
    private TextMeshProUGUI m_ScoreText_Player1;
    [SerializeField]
    private int m_CurrentScore_Player1;
    private int m_DetachPiecesCount_Player1;
    private bool m_IsGameOver_Player1 = false;
    [SerializeField]
    private TextMeshProUGUI m_ScoreText_Player2;
    [SerializeField]
    private int m_CurrentScore_Player2;
    private int m_DetachPiecesCount_Player2;
    [SerializeField]
    private int m_DiffficultyFlowTimerFlag_PLAYER1 = 0;
    [SerializeField]
    private int m_DiffficultyFlowTimerFlag_PLAYER2 = 0;
    private bool m_IsGameOver_Player2 = false;
    [SerializeField]
    private GameObject m_PieceList_Player1;
    [SerializeField]
    private Image m_SidePanelShowListBG_Player1;
    [SerializeField]
    private GameObject m_SidePanelHideListBG_Player1;
    [SerializeField]
    private GameObject m_PieceList_Player2;
    [SerializeField]
    private Image m_SidePanelShowListBG_Player2;
    [SerializeField]
    private GameObject m_SidePanelHideListBG_Player2;
    private Coroutine m_GameOverCoroutine;
    private Coroutine m_DetachPieceCoroutine_Player1;
    private Coroutine m_DetachPieceCoroutine_Player2;
    private Coroutine m_GameClockPlayer1;
    private Coroutine m_GameClockPlayer2;

    [SerializeField]
    private List<Vector2> m_StartingPieceSpawnSpots_Player1 = new List<Vector2>();
    [SerializeField]
    private List<Vector2> m_StartingPieceHorizontalSpawnSpots_Player1 = new List<Vector2>();
    [SerializeField]
    private List<Vector2> m_StartingPieceVerticalSpawnSpots_Player1 = new List<Vector2>();
    private int[] m_StartingPieceRemainingTime_Player1 = new int[Constants.INT_ONE];
    [SerializeField]
    private List<Vector2> m_StartingPieceSpawnSpots_Player2 = new List<Vector2>();
    [SerializeField]
    private List<Vector2> m_StartingPieceHorizontalSpawnSpots_Player2 = new List<Vector2>();
    [SerializeField]
    private List<Vector2> m_StartingPieceVerticalSpawnSpots_Player2 = new List<Vector2>();
    private int[] m_StartingPieceRemainingTime_Player2 = new int[Constants.INT_ONE];
    [SerializeField]
    private int m_TotalPenaltyValuePlayer1;
    [SerializeField]
    private int m_TotalPenaltyValuePlayer2;
    private float m_FlowAnimationTimer;
    private int m_MaxGameTimer;
    private int m_GameTimerPlayer1;
    private int m_GameTimerPlayer2;
    private int m_FirstPlayerLoopCount;
    private int m_SecondPlayerLoopCount;
    private bool m_FlowStatus_Player1;
    private bool m_FlowStatus_Player2;
    private bool m_LeftStickStatusPlayer1 = false;
    private bool m_LeftStickStatusPlayer2 = false;
    private float m_StartingPieceBaseTimer_Player1;
    private float m_StartingPieceBaseTimer_Player2;
    [SerializeField]
    private int m_CurrentAuxillaryTimer_Player1;
    private Coroutine m_AuxillaryTimerCoroutine_Player1 = null;
    [SerializeField]
    private int m_IncreaseTimeDifference_Player1;
    [SerializeField]
    private int m_CurrentAuxillaryTimer_Player2;
    private Coroutine m_AuxillaryTimerCoroutine_Player2 = null;
    [SerializeField]
    private int m_IncreaseTimeDifference_Player2;
    [SerializeField]
    private TextMeshProUGUI m_DifficultyTimer_Player1Text;
    [SerializeField]
    private TextMeshProUGUI m_DifficultyTimer_Player2Text;

    List<float> m_AngleList = new List<float>()
     {
         Constants.PIECE_ANGLE0,
         Constants.PIECE_ANGLE90,
         Constants.PIECE_ANGLE180,
         Constants.PIECE_ANGLE270,
     };
    

    #endregion

    #region Public_Variables

    public static Action s_OnGameEnds;
    public static Action<string, string, int, int> s_OnGameOverCallback;
    //Player1 Actions and callbacks
    public static Action<TilePiece, TilePiece, Action> s_SetGridTilePosition_Player1;
    public static Action<TilePiece, Action> s_SetUpcomingBombPositon_Player1;
    public static Action<TilePiece, Vector2> s_ActiveTilePiece_Player1;
    public static Action<Vector2, Action<int>> s_PlaceTilePiece_Player1;
    public static Action<Vector2> s_DeActiveTilePiece_Player1;
    public static Action<Vector2> s_ActiveBomb_Player1;
    public static Action<Vector2> s_PlaceBomb_Player1;
    public static Action<GridTilePlayer1, Vector2> s_CheckTileLeftVectorStatus_Player1;
    public static Action<GridTilePlayer1, Vector2> s_CheckTileRightVectorStatus_Player1;
    public static Action<GridTilePlayer1, Vector2> s_CheckTileUPVectorStatus_Player1;
    public static Action<GridTilePlayer1, Vector2> s_CheckTileDownVectorStatus_Player1;
    public static Action<Action<GridTilePlayer1>, Action<GridTilePlayer1>, Action<bool>, int[], Action<GridTilePlayer1>> s_OnStartingPieceCounterChanges_Player1;
    public static Action s_OnGameEnds_Player1;
    public static Action<Vector2, int, int> s_SetStartingPositon_Player1;
    public static Action<Vector2> s_OnRoundObstacleSpawn_Player1;
    public static Action<Vector2, Vector2> s_OnHorizontalObstacleSpawn_Player1;
    public static Action<Vector2, Vector2, Vector2> s_OnVerticalObstacleSpawn_Player1;
    public static Action<GridTilePlayer1, Vector2, Vector2, Action<GridTilePlayer1, Vector2, Vector2>> s_CheckStartingPieceLoopStatus_Player1;
    public static Action<GridTilePlayer1, Vector2, Vector2, Action<GridTilePlayer1, Vector2, Vector2>> s_MidCounterLoopStatus_Player1;
    public static Action<int> s_OnPlayer1ScoreUpdates;
    public static Action<Action<int>, bool> s_CalculateDetachPieces_Player1;
    public static Action<bool> s_OnGameOvers_Player1;
    public static Action<Vector2> s_StartingPieceCounted_Player1;
    public static Action<Action<Vector2, Action<bool>>, List<Vector2>> s_SpawnNextStartingPiece_Player1;
    public static Action<Action<Vector2, List<GridTilePlayer1>, Action<bool>>, List<GridTilePlayer1>, List<Vector2>> s_SpawnStartingPiecePlayer1_Horizontal;
    public static Action<Action<Vector2, List<GridTilePlayer1>, Action<bool>>, List<GridTilePlayer1>, List<Vector2>> s_SpawnStartingPiecePlayer1_Vertical;
    public static Action<Vector2, Action<bool>> s_CheckSafeAreaStatus_Player1;
    public static Action<Vector2, List<GridTilePlayer1>, Action<bool>> s_CheckStartingPieceHorizontalSafeAreaStatus_Player1;
    public static Action<Vector2, List<GridTilePlayer1>, Action<bool>> s_CheckStartingPieceVerticalSafeAreaStatus_Player1;
    public static Action<Vector2, int, bool, bool> s_SpawnStartingPiece_Player1;
    public static Action<Vector2, float, Action<Vector2>, bool> s_StartFlowAnimation_Player1;
    public static Action s_AutomaticTilePlaceCallback_Player1;
    public static Action<Vector2> s_SetCurrentPiece_Player1;
    public static Action<Vector2, Action<GridTilePlayer1>> s_NextTileValidPosition_Player1;
    public static Action<Vector2> s_LockLoopPiece_Player1;
    public static Action<Vector2> s_ResetStartingPiecePositions_Player1;
    public static Action<Vector2, float, Vector2> s_TriggerBrokenAnimation_Player1;
    public static Action<Action<bool, Vector2>> s_StorePieceTriggered_Player1;
    public static Action s_ActiveUpcomingInputs_Player1;
    public static Action<Vector2> s_ResetCurrentPosition_Player1;
    public static Action<Vector2, bool> s_FlashStartingPiece_Player1;
    public static Action<Vector2, Vector2, Action<Vector2, Vector2>, Action<GridTilePlayer1, float>, Action<int, bool>, GridTilePlayer1, Action<Vector2>, Action<GridTilePlayer1>, float, Action> s_HandlContinuousFlowAnimation_Player1;
    public static Action s_RefreshHoldTilePlacement_Player1;



    //Player2 Actions and callbacks
    public static Action<TilePiece, TilePiece, Action> s_SetGridTilePosition_Player2;
    public static Action<TilePiece, Action> s_SetUpcomingBombPositon_Player2;
    public static Action<TilePiece, Vector2> s_ActiveTilePiece_Player2;
    public static Action<Vector2, Action<int>> s_PlaceTilePiece_Player2;
    public static Action<Vector2> s_DeActiveTilePiece_Player2;
    public static Action<Vector2> s_ActiveBomb_Player2;
    public static Action<Vector2> s_PlaceBomb_Player2;
    public static Action<GridTilePlayer2, Vector2> s_CheckTileLeftVectorStatus_Player2;
    public static Action<GridTilePlayer2, Vector2> s_CheckTileRightVectorStatus_Player2;
    public static Action<GridTilePlayer2, Vector2> s_CheckTileUPVectorStatus_Player2;
    public static Action<GridTilePlayer2, Vector2> s_CheckTileDownVectorStatus_Player2;
    public static Action<Action<GridTilePlayer2>, Action<GridTilePlayer2>, Action<bool>, int[], Action<GridTilePlayer2>> s_OnStartingPieceCounterChanges_Player2;
    public static Action s_OnGameEnds_Player2;
    public static Action<Vector2, int, int> s_SetStartingPositon_Player2;
    public static Action<Vector2> s_OnRoundObstacleSpawn_Player2;
    public static Action<Vector2, Vector2> s_OnHorizontalObstacleSpawn_Player2;
    public static Action<Vector2, Vector2, Vector2> s_OnVerticalObstacleSpawn_Player2;
    public static Action<GridTilePlayer2, Vector2, Vector2, Action<GridTilePlayer2, Vector2, Vector2>> s_CheckStartingPieceLoopStatus_Player2;
    public static Action<GridTilePlayer2, Vector2, Vector2, Action<GridTilePlayer2, Vector2, Vector2>> s_MidCounterLoopStatus_Player2;
    public static Action<int> s_OnPlayer2ScoreUpdates;
    public static Action<Action<int>, bool> s_CalculateDetachPieces_Player2;
    public static Action<bool> s_OnGameOvers_Player2;
    public static Action<Vector2> s_StartingPieceCounted_Player2;
    public static Action<Action<Vector2, Action<bool>>, List<Vector2>> s_SpawnNextStartingPiece_Player2;
    public static Action<Action<Vector2, List<GridTilePlayer2>, Action<bool>>, List<GridTilePlayer2>, List<Vector2>> s_SpawnStartingPiecePlayer2_Horizontal;
    public static Action<Action<Vector2, List<GridTilePlayer2>, Action<bool>>, List<GridTilePlayer2>, List<Vector2>> s_SpawnStartingPiecePlayer2_Vertical;
    public static Action<Vector2, Action<bool>> s_CheckSafeAreaStatus_Player2;
    public static Action<Vector2, List<GridTilePlayer2>, Action<bool>> s_CheckStartingPieceHorizontalSafeAreaStatus_Player2;
    public static Action<Vector2, List<GridTilePlayer2>, Action<bool>> s_CheckStartingPieceVerticalSafeAreaStatus_Player2;
    public static Action<Vector2, int, bool, bool> s_SpawnStartingPiece_Player2;
    public static Action<Vector2, float, Action<Vector2>, bool> s_StartFlowAnimation_Player2;
    public static Action s_AutomaticTilePlaceCallback_Player2;
    public static Action<Vector2> s_SetCurrentPiece_Player2;
    public static Action<Vector2, Action<GridTilePlayer2>> s_NextTileValidPosition_Player2;
    public static Action<Vector2> s_LockLoopPiece_Playe2;
    public static Action<Vector2> s_ResetStartingPiecePositions_Player2;
    public static Action<Vector2, float, Vector2> s_TriggerBrokenAnimation_Player2;
    public static Action<Action<bool, Vector2>> s_StorePieceTriggered_Player2;
    public static Action s_ActiveUpcomingInputs_Player2;
    public static Action<Vector2> s_ResetCurrentPosition_Player2;
    public static Action<Vector2, bool> s_FlashStartingPiece_Player2;
    public static Action<string> s_OnLoopCompleteWinnerTriggered;
    public static Action<bool> s_TwoPlayerGamePauseStatus;
    public static Action<Vector2, Vector2, Action<Vector2, Vector2>, Action<GridTilePlayer2, float>, Action<int, bool>, GridTilePlayer2, Action<Vector2>, Action<GridTilePlayer2>, float, Action> s_HandlContinuousFlowAnimation_Player2;
    public static Action s_RefreshHoldTilePlacement_Player2;
    public static Action<bool> s_OnFirstPlayerPaused;
    public static Action<bool> s_OnSecondPlayerPaused;



    #endregion

    #region Unity_Callbacks

    private void Awake()
    {
        m_TwoPlayerInputs = new SinglePlayerInputs();
        m_SecondPlayerInputs = new SecondPlayerInputs();
        m_LeavePanelOpenButton.onClick.AddListener(OnLeavePanelOpens);
        m_LeavePanelCloseButton.onClick.AddListener(OnLeavePaneCloses);
        m_CloseButton.onClick.AddListener(OnGameEndCalls);
        m_PauseUIbutton.onClick.AddListener(OnPauseButtonTap);
    }
    private void OnEnable()
    {
        EnableAllEvents();
    }
    private void OnDisable()
    {
        s_TwoPlayerGamePauseStatus?.Invoke(false);
        DisableAllEvents();
        m_MovePiecePlayer1.performed -= MoveTilePieces_Player1;
        m_MovePiecePlayer1Dpads.performed -= MoveTilePiecesusingGamepad_Player1;
        m_PlacePiecePlayer1.performed -= PlaceTilePiece_Player1;
        m_PlacePiecePlayer1Gamepads.performed -= PlaceTilePiece_Player1Gamepads;
        m_StorePiece_Player1.performed -= StoreTilePieces_Player1;
      //storechanges  m_StorePiece_Player1Gamepads.performed -= StoreTilePieces_Player1Gamepad;
        m_MovePiecePlayer2.performed -= MoveTilePieces_Player2;
        m_MovePiecePlayer2Dpads.performed -= MoveTilePieces_Player2Dpads;
        m_PlacePiecePlayer2.performed -= PlaceTilePiece_Player2;
        m_PlacePiecePlayer2Gamepads.performed -= PlaceTilePiece_Player2Gamepads;
        m_StorePiece_Player2.performed -= StoreTilePieces_Player2;
        //storechnagesm_StorePiece_Player2Gamepads.performed -= StoreTilePieces_Player2Gamepad;
        m_StartingPieceSpawnSpots_Player1.Clear();
        m_StartingPieceHorizontalSpawnSpots_Player1.Clear();
        m_StartingPieceVerticalSpawnSpots_Player1.Clear();
        m_StartingPieceSpawnSpots_Player2.Clear();
        m_StartingPieceHorizontalSpawnSpots_Player2.Clear();
        m_StartingPieceVerticalSpawnSpots_Player2.Clear();
    }

    private void Update()
    {
        HandlePlayer1LeftStickMovements();
        HandlePlayer2LeftStickMovements();
        StoreTilePieces_Player1Gamepad();
        StoreTilePieces_Player2Gamepad();
        CheckGameStatus();
    }


    private void CheckGameStatus()
    {
        if (m_IsGameOver_Player1 && m_IsGameOver_Player2)
            return;
        Action player1GameoverAction = () =>
        {
            m_IsGameOver_Player2 = (m_CurrentScore_Player2 > m_CurrentScore_Player1 && !m_IsGameOver_Player2) ? true : m_IsGameOver_Player2;
            StartCoroutine(GameOverCallback(Constants.INT_TWO));
        };

        Action player2GameoverAction = () =>
        {
            m_IsGameOver_Player1 = (m_CurrentScore_Player1 > m_CurrentScore_Player2 && !m_IsGameOver_Player1) ? true : m_IsGameOver_Player1;
            StartCoroutine(GameOverCallback(Constants.INT_TWO));
        };
        if (m_IsGameOver_Player1)
            player1GameoverAction.Invoke();
        if(m_IsGameOver_Player2)
            player2GameoverAction.Invoke();
    }
    private void HandlePlayer1LeftStickMovements()
    {
        if (GamepadManager.Instance.m_Gamepad1 != null && m_LeftStickStatusPlayer1)
        {
            if (GamepadManager.Instance.m_Gamepad1.leftStick.up.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.leftStick.down.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.leftStick.left.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.leftStick.right.wasPressedThisFrame)
            {
                Debug.Log("Gamepad triggerd");
                var leftStickInputValue = GamepadManager.Instance.m_Gamepad1.leftStick.ReadValue().normalized;
                Vector2 movementVector = ApplyApproximationonMovementVector(leftStickInputValue);
                TriggerTileMovement(m_Player1GridArea, s_ActiveBomb_Player1, s_DeActiveTilePiece_Player1, s_ActiveTilePiece_Player1, movementVector);
            }
        }
    }
    private void HandlePlayer2LeftStickMovements()
    {
        if (GamepadManager.Instance.m_Gamepad2 != null && m_LeftStickStatusPlayer2)
        {
            if (GamepadManager.Instance.m_Gamepad2.leftStick.up.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad2.leftStick.down.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad2.leftStick.left.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad2.leftStick.right.wasPressedThisFrame)
            {
                var leftStickInputValue = GamepadManager.Instance.m_Gamepad2.leftStick.ReadValue().normalized;
                Vector2 movementVector = ApplyApproximationonMovementVector(leftStickInputValue);
                TriggerTileMovement(m_Player2GridArea, s_ActiveBomb_Player2, s_DeActiveTilePiece_Player2, s_ActiveTilePiece_Player2, movementVector);
            }
        }
    }


    #endregion

    #region Public_Methods

    #endregion

    #region Private_Methods
    private void ResetHoldTile_Player1(Vector2 currentHoldPosition)
    {
        bool activePieceStaus = false;
        Vector2 activePosition = Vector2.zero;
        s_StorePieceTriggered_Player1?.Invoke((bool status, Vector2 currenPosition) =>
        {
            activePieceStaus = status;
            activePosition = currenPosition;
        });
        if (activePieceStaus)
        {
            DisablePlayer1Inputs();
            s_ResetCurrentPosition_Player1?.Invoke(currentHoldPosition);
            s_ActiveUpcomingInputs_Player1?.Invoke();
        }
    }

    private void OnLoopFlowStarts_Player1()
    {
        DisablePlayer1Inputs();
        s_OnFirstPlayerPaused?.Invoke(true);
    }

    private void OnLoopEnds_Player1()
    {
        s_OnFirstPlayerPaused?.Invoke(false);
        ActiveSinglePlayerInputs();
    }

    private void OnLoopFlowStarts_Player2()
    {
        DisablePlayer2Inputs();
        s_OnSecondPlayerPaused?.Invoke(true);
    }

    private void OnLoopEnds_Player2()
    {
        s_OnSecondPlayerPaused?.Invoke(false);
        ActiveDoublePlayerInputs();
    }

    private void StoreTilePieces_Player1(InputAction.CallbackContext context)
    {
       /* bool activePieceStaus = false;
        Vector2 activePosition = Vector2.zero;
        s_StorePieceTriggered_Player1?.Invoke((bool status, Vector2 currenPosition) =>
        {
            activePieceStaus = status;
            activePosition = currenPosition;
        });
        if (activePieceStaus)
        {
            DisablePlayer1Inputs();
            s_ResetCurrentPosition_Player1?.Invoke(activePosition);
            s_ActiveUpcomingInputs_Player1?.Invoke();
        }*/
    }
    private void StoreTilePieces_Player1Gamepad()
    {
       /* if (GamepadManager.Instance.m_Gamepad1 != null && m_LeftStickStatusPlayer1)
        {
            if (GamepadManager.Instance.m_Gamepad1.xButton.wasPressedThisFrame)
            {
                bool activePieceStaus = false;
                Vector2 activePosition = Vector2.zero;
                s_StorePieceTriggered_Player1?.Invoke((bool status, Vector2 currenPosition) =>
                {
                    activePieceStaus = status;
                    activePosition = currenPosition;
                });
                if (activePieceStaus)
                {
                    DisablePlayer1Inputs();
                    s_ResetCurrentPosition_Player1?.Invoke(activePosition);
                    s_ActiveUpcomingInputs_Player1?.Invoke();
                }
            }
        }*/
    }
    private void ResetTilePiece_Player2(Vector2 currentHoldPosition)
    {
        bool activePieceStaus = false;
        Vector2 activePosition = Vector2.zero;
        s_StorePieceTriggered_Player2?.Invoke((bool status, Vector2 currenPosition) =>
        {
            activePieceStaus = status;
            activePosition = currenPosition;
        });
        if (activePieceStaus)
        {
            DisablePlayer2Inputs();
            s_ResetCurrentPosition_Player2?.Invoke(activePosition);
            s_ActiveUpcomingInputs_Player2?.Invoke();
        }
    }
    private void StoreTilePieces_Player2(InputAction.CallbackContext context)
    {
       /* bool activePieceStaus = false;
        Vector2 activePosition = Vector2.zero;
        s_StorePieceTriggered_Player2?.Invoke((bool status, Vector2 currenPosition) =>
        {
            activePieceStaus = status;
            activePosition = currenPosition;
        });
        if (activePieceStaus)
        {
            DisablePlayer2Inputs();
            s_ResetCurrentPosition_Player2?.Invoke(activePosition);
            s_ActiveUpcomingInputs_Player2?.Invoke();
        }*/
    }

    private void StoreTilePieces_Player2Gamepad()
    {
        /*if (GamepadManager.Instance.m_Gamepad2 != null && m_LeftStickStatusPlayer2)
        {
            if (GamepadManager.Instance.m_Gamepad2.xButton.wasPressedThisFrame)
            {
                bool activePieceStaus = false;
                Vector2 activePosition = Vector2.zero;
                s_StorePieceTriggered_Player2?.Invoke((bool status, Vector2 currenPosition) =>
                {
                    activePieceStaus = status;
                    activePosition = currenPosition;
                });
                if (activePieceStaus)
                {
                    DisablePlayer2Inputs();
                    s_ResetCurrentPosition_Player2?.Invoke(activePosition);
                    s_ActiveUpcomingInputs_Player2?.Invoke();
                }
            }
        }*/
    }

    private void ClearBrokenPiecePieceArea_Player1()
    {
        for (int tileIndex = (int)Constants.ZERO; tileIndex < m_BrokenPieceArea_Player1.transform.childCount; tileIndex++)
        {
            Destroy(m_BrokenPieceArea_Player1.transform.GetChild(tileIndex).gameObject);
        }
    }
    private void ClearBrokenPiecePieceArea_Player2()
    {
        for (int tileIndex = (int)Constants.ZERO; tileIndex < m_BrokenPieceArea_Player2.transform.childCount; tileIndex++)
        {
            Destroy(m_BrokenPieceArea_Player2.transform.GetChild(tileIndex).gameObject);
        }
    }
    private void AssignBrokenPieceeGrid_Player1(GridTilePlayer1 tile, float time, Vector2 targetVector)
    {
        m_BrokenPieceArea_Player1.GetComponent<RectTransform>().sizeDelta = m_Player1GridArea.GetComponent<RectTransform>().sizeDelta;//new skchanges
        if (tile != null)
        {
            GridTilePlayer1 brokenPieceTile = Instantiate(tile, m_BrokenPieceArea_Player1.transform) as GridTilePlayer1;
            s_TriggerBrokenAnimation_Player1?.Invoke(tile.GridPosition, time, targetVector);
        }

    }
    private void AssignBrokenPieceeGrid_Player2(GridTilePlayer2 tile, float time, Vector2 targetVector)
    {
        m_BrokenPieceArea_Player2.GetComponent<RectTransform>().sizeDelta = m_Player2GridArea.GetComponent<RectTransform>().sizeDelta;//new skchanges
        if (tile != null)
        {
            GridTilePlayer2 brokenPieceTile = Instantiate(tile, m_BrokenPieceArea_Player2.transform) as GridTilePlayer2;
            s_TriggerBrokenAnimation_Player2?.Invoke(tile.GridPosition, time, targetVector);
        }

    }

    private Vector2 GetLastPieceFlowVector_Player1(GridTilePlayer1 lastTile)
    {
        Vector2 brokenPiecePosition = Vector2.zero;
        Vector2 targetVector = Vector2.zero;
        if (lastTile.Type != Constants.L_TYPE1PIECE && lastTile.Type != Constants.L_TYPE2PIECE)
        {
            brokenPiecePosition = lastTile.GridPosition + lastTile.StartingPieceFlowVector;
            targetVector = lastTile.StartingPieceFlowVector;
        }
        else
        {
            for (int tileCodeIndex = Constants.INT_ZERO; tileCodeIndex < lastTile.TileCode.Count; tileCodeIndex++)
            {
                if (lastTile.TileCode[tileCodeIndex] != Constants.INT_ZERO)
                {
                    Vector2 tempPosition = lastTile.GridPosition + GameData.Instance.BrokenPieceVectors.SurroundingVectors[tileCodeIndex];
                    int attachedstatus = lastTile.AttachedPiece.FindIndex(element => element.GridPosition == tempPosition);
                    if (attachedstatus == Constants.NULLINDEX)
                    {
                        brokenPiecePosition = tempPosition;
                        targetVector = GameData.Instance.BrokenPieceVectors.SurroundingVectors[tileCodeIndex];
                    }

                }
            }
        }
        return targetVector;
    }

    private Vector2 GetLastPieceFlowVector_Player2(GridTilePlayer2 lastTile)
    {
        Vector2 brokenPiecePosition = Vector2.zero;
        Vector2 targetVector = Vector2.zero;
        if (lastTile.Type != Constants.L_TYPE1PIECE && lastTile.Type != Constants.L_TYPE2PIECE)
        {
            brokenPiecePosition = lastTile.GridPosition + lastTile.StartingPieceFlowVector;
            targetVector = lastTile.StartingPieceFlowVector;
        }
        else
        {
            for (int tileCodeIndex = Constants.INT_ZERO; tileCodeIndex < lastTile.TileCode.Count; tileCodeIndex++)
            {
                if (lastTile.TileCode[tileCodeIndex] != Constants.INT_ZERO)
                {
                    Vector2 tempPosition = lastTile.GridPosition + GameData.Instance.BrokenPieceVectors.SurroundingVectors[tileCodeIndex];
                    int attachedstatus = lastTile.AttachedPiece.FindIndex(element => element.GridPosition == tempPosition);
                    if (attachedstatus == Constants.NULLINDEX)
                    {
                        brokenPiecePosition = tempPosition;
                        targetVector = GameData.Instance.BrokenPieceVectors.SurroundingVectors[tileCodeIndex];
                    }

                }
            }
        }
        return targetVector;
    }

    private bool CheckBrokenPieceStatusPlayer1(List<GridTilePlayer1> loopPieces)
    {

        bool brokenPieceStatus = true;
        int gridIndex = loopPieces.FindIndex(element => element.IsStartingPiece);
        if (gridIndex != Constants.NULLINDEX)
        {
            brokenPieceStatus = false;
        }
        return brokenPieceStatus;
    }
    private bool CheckBrokenPieceStatusPlayer2(List<GridTilePlayer2> loopPieces)
    {

        bool brokenPieceStatus = true;
        int gridIndex = loopPieces.FindIndex(element => element.IsStartingPiece);
        if (gridIndex != Constants.NULLINDEX)
        {
            brokenPieceStatus = false;
        }
        return brokenPieceStatus;
    }
    private void FlowAnimationCallbackPlayer1(GridTilePlayer1 startingPiece)
    {
        StartCoroutine(CalculateScorePlayer1(startingPiece, false));
    }
    private void FlowAnimationCallbackPlayer2(GridTilePlayer2 startingPiece)
    {
        StartCoroutine(CalculateScorePlayer2(startingPiece, false));
    }
    private void UpdateFlowTimer(float totalTimer)
    {
        if (m_MaxGameTimer < totalTimer)
        {
            StopCoroutine(m_GameOverCoroutine);
            m_GameOverCoroutine = StartCoroutine(GameOver(totalTimer, true));
        }
    }

    private void StartPipeFlow_Player1(List<GridTilePlayer1> loopPieces)
    {
        if(loopPieces.Count > Constants.ZERO)
        {
            Vector2 targetVector = Vector2.zero;
            int gridIndex = loopPieces.FindIndex(element => element.IsStartingPiece);
            if (gridIndex != Constants.NULLINDEX)
            {
                GridTilePlayer1 tile = loopPieces[gridIndex];
                loopPieces.RemoveAt(gridIndex);
            }
            bool brokernPieceStatus = CheckBrokenPieceStatusPlayer1(loopPieces);
            if (brokernPieceStatus)
            {
                targetVector = GetLastPieceFlowVector_Player1(loopPieces[loopPieces.Count - 1]);
            }
            StartCoroutine(StartTileFlow_Player1(loopPieces, brokernPieceStatus, targetVector));
        }
        
    }
    private void StartPipeFlow_Player2(List<GridTilePlayer2> loopPieces)
    {
        if(loopPieces.Count > Constants.ZERO)
        {
            Vector2 targetVector = Vector2.zero;
            int gridIndex = loopPieces.FindIndex(element => element.IsStartingPiece);
            if (gridIndex != Constants.NULLINDEX)
            {
                GridTilePlayer2 tile = loopPieces[gridIndex];
                loopPieces.RemoveAt(gridIndex);
            }
            bool brokernPieceStatus = CheckBrokenPieceStatusPlayer2(loopPieces);
            if (brokernPieceStatus)
            {
                targetVector = GetLastPieceFlowVector_Player2(loopPieces[loopPieces.Count - 1]);
            }
            StartCoroutine(StartTileFlow_Player2(loopPieces, brokernPieceStatus, targetVector));
        }
       
    }


    private void SidePanelStatusChanges()
    {
        bool status = GameManager.instace.UpcomingTileStatus;
        m_PieceList_Player1.SetActive(status);
        m_SidePanelShowListBG_Player1.enabled = status;
        m_SidePanelHideListBG_Player1.SetActive(!status);
        m_PieceList_Player2.SetActive(status);
        m_SidePanelShowListBG_Player2.enabled = status;
        m_SidePanelHideListBG_Player2.SetActive(!status);

    }
    private void CalculateFinalScorePlayer1(GridTilePlayer1 startingPiece)
    {
        InvokeScoreManager_Player1(startingPiece.PieceLoop);
    }
    private void CalculateFinalScorePlayer2(GridTilePlayer2 startingPiece)
    {
        InvokeScoreManager_Player2(startingPiece.PieceLoop);
    }
    private void InvokeScoreManager_Player1(List<GridTilePlayer1> loopPieces)
    {
        int crosspieces = 0;
        int loopLength = 0;
        int startingPiecesCount = 0;
        Vector2 startingPiecePosition = Vector2.zero;
        List<GridTilePlayer1> startingPieceList = new List<GridTilePlayer1>();
        foreach (GridTilePlayer1 piece in loopPieces)
        {
            crosspieces = (piece.Type == Constants.CROSS_TYPEPIECE) ? crosspieces + 1 : crosspieces;
            loopLength = (!piece.IsStartingPiece) ? loopLength + 1 : loopLength;
        }
        startingPieceList = loopPieces.Where(piece => piece.IsStartingPiece).ToList();
        for (int pieceIndex1 = (int)Constants.ZERO; pieceIndex1 < startingPieceList.Count; pieceIndex1++)
        {
            for (int pieceIndex2 = pieceIndex1 + 1; pieceIndex2 < startingPieceList.Count; pieceIndex2++)
            {
                if (startingPieceList[pieceIndex1] == startingPieceList[pieceIndex2])
                {
                    startingPiecesCount++;
                }

            }
        }
        int finalScore = (loopLength * GameData.Instance.GameOptions.LengthPoints) + (crosspieces * GameData.Instance.GameOptions.CrossPoints) + (loopLength * startingPiecesCount * GameData.Instance.GameOptions.ClosedLoopPoints);
        StartPipeFlow_Player1(loopPieces);
        StartCoroutine(IncreaseScorePlayer1(finalScore));
    }
    private void InvokeScoreManager_Player2(List<GridTilePlayer2> loopPieces)
    {
        int crosspieces = 0;
        int loopLength = 0;
        int startingPiecesCount = 0;
        Vector2 startingPiecePosition = Vector2.zero;
        List<GridTilePlayer2> startingPieceList = new List<GridTilePlayer2>();
        foreach (GridTilePlayer2 piece in loopPieces)
        {
            crosspieces = (piece.Type == Constants.CROSS_TYPEPIECE) ? crosspieces + 1 : crosspieces;
            loopLength = (!piece.IsStartingPiece) ? loopLength + 1 : loopLength;
        }
        startingPieceList = loopPieces.Where(piece => piece.IsStartingPiece).ToList();
        for (int pieceIndex1 = (int)Constants.ZERO; pieceIndex1 < startingPieceList.Count; pieceIndex1++)
        {
            for (int pieceIndex2 = pieceIndex1 + 1; pieceIndex2 < startingPieceList.Count; pieceIndex2++)
            {
                if (startingPieceList[pieceIndex1] == startingPieceList[pieceIndex2])
                {
                    startingPiecesCount++;
                }
            }
        }
        int finalScore = (loopLength * GameData.Instance.GameOptions.LengthPoints) + (crosspieces * GameData.Instance.GameOptions.CrossPoints) + (loopLength * startingPiecesCount * GameData.Instance.GameOptions.ClosedLoopPoints);
        StartPipeFlow_Player2(loopPieces);
        StartCoroutine(IncreaseScorePlayer2(finalScore));
    }



    private void OnGameEndCalls()
    {
        Time.timeScale = 1f;
        m_PausePanel.gameObject.SetActive(true);
        OnGameEnds(s_OnGameEnds);
    }

    private void EnableAllEvents()
    {
        m_DifficultyTimer_Player1Text.text = m_DifficultyTimer_Player2Text.text = GameData.Instance.GameOptions.AuxillaryTimer.ToString();
        m_AuxillaryTimerCoroutine_Player1 = null;
        m_AuxillaryTimerCoroutine_Player2 = null;
        m_CurrentAuxillaryTimer_Player1 = Constants.INT_ZERO;
        m_CurrentAuxillaryTimer_Player2 = Constants.INT_ZERO;
        m_StartingPieceBaseTimer_Player1 = GameData.Instance.GameOptions.StartingPieceTimer;
        m_StartingPieceBaseTimer_Player2 = GameData.Instance.GameOptions.StartingPieceTimer;
        m_IncreaseTimeDifference_Player1 = Constants.INT_ZERO;
        m_IncreaseTimeDifference_Player2 = Constants.INT_ZERO;
        p_IsGamePaused = false;
        m_PausePanel.SetActive(false);
        SoundManager.Instance.PlayWindowTransitionSound();
        SoundManager.Instance.PlayThemeSound();
        m_DiffficultyFlowTimerFlag_PLAYER1 = Constants.INT_ZERO;
        m_DiffficultyFlowTimerFlag_PLAYER2 = Constants.INT_ZERO;
        m_FirstPlayerLoopCount = Constants.INT_ZERO;
        m_SecondPlayerLoopCount = Constants.INT_ZERO;
        m_FlowAnimationTimer = Constants.ZERO;
        m_TotalPenaltyValuePlayer1 = Constants.INT_ZERO;
        m_TotalPenaltyValuePlayer2 = Constants.INT_ZERO;
        SidePanelStatusChanges();
        m_Loader.SetActive(false);
        m_IsGameOver_Player1 = false;
        m_IsGameOver_Player2 = false;
        m_DetachPiecesCount_Player1 = (int)Constants.ZERO;
        m_CurrentScore_Player1 = (int)Constants.ZERO;
        m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
        m_DetachPiecesCount_Player2 = (int)Constants.ZERO;
        m_CurrentScore_Player2 = (int)Constants.ZERO;
        m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
        m_LeavePanel.SetActive(false);
        m_Player1NameText.text = NameManager.GetPlayer1Name();
        m_Player2NameText.text = NameManager.GetPlayer2Name();
        ClearStartingPieceArea(m_StartingPieceArea_Player1);
        ClearStartingPieceArea(m_StartingPieceArea_Player2);
        ClearBrokenPiecePieceArea_Player1();
        ClearBrokenPiecePieceArea_Player2();
        m_Player1GridArea.GetComponent<GridLayoutGroup>().enabled = true;
        m_Player2GridArea.GetComponent<GridLayoutGroup>().enabled = true;
        HandleTileGeneration();
        HandleSideTilePanelGeneration();
        TwoPlayerTutorialPanel.s_TutorialEnds += OnTutorialEnds;
        StartCoroutine(ShowTutorialPanel());
    }
    private void DisableAllEvents()
    {
        TwoPlayerTutorialPanel.s_TutorialEnds -= OnTutorialEnds;
        ClearData();
        DisablePlayer1Inputs();
        DisablePlayer2Inputs();
        m_QuitButton.performed -= QuitGame;
        m_PauseButton.performed -= PauseGame;
        m_QuitButton.Disable();
        m_PauseButton.Disable();
        UpcomingTilePlayer1.s_OnTileMove -= SetUpcomingTile_Player1;
        UpcomingTilePlayer2.s_OnTileMove -= SetUpcomingTile_Player2;
        HoldTilePlayer1.s_ResetOnHold -= ResetHoldTile_Player1;
        HoldTilePlayer2.s_ResetOnHold -= ResetTilePiece_Player2;
        PausePanelTwoPlayer.s_OnGameResumes -= GameResumes;
        SoundManager.Instance.StopThemeSound();
    }
    private void QuitGame(InputAction.CallbackContext context)
    {
        //OnLeavePanelOpens();
    }
    private void ClearData()
    {
        p_SidePiecePercentages.Clear();
        m_ObstacleIgnoredArea_Player1.Clear();
        m_ObstacleSpawnArea_Player1.Clear();
        m_ObstacleIgnoredArea_Player2.Clear();
        m_ObstacleSpawnArea_Player2.Clear();
    }

    private void DisablePlayer1Inputs()
    {
        m_MovePiecePlayer1.Disable();
        m_MovePiecePlayer1Dpads.Disable();
        m_PlacePiecePlayer1.Disable();
        m_PlacePiecePlayer1Gamepads.Disable();
        m_StorePiece_Player1.Disable();
       //storechanges m_StorePiece_Player1Gamepads.Disable();
        m_LeftStickStatusPlayer1 = false;
    }
    private void DisablePlayer2Inputs()
    {
        m_MovePiecePlayer2.Disable();
        m_MovePiecePlayer2Dpads.Disable();
        m_PlacePiecePlayer2.Disable();
        m_PlacePiecePlayer2Gamepads.Disable();
        m_StorePiece_Player2.Disable();
        //storechanges m_StorePiece_Player2Gamepads.Disable();
        m_LeftStickStatusPlayer2 = false;

    }
    private void OnTutorialEnds()
    {
        StartCoroutine(GenerateStartingPieces());
        m_MovePiecePlayer1 = m_TwoPlayerInputs.TwoPlayers.PieceMovementPlayer1;
        m_MovePiecePlayer1Dpads = m_TwoPlayerInputs.TwoPlayers.PieceMovementPlayer1Dpads;
        m_PlacePiecePlayer1 = m_TwoPlayerInputs.TwoPlayers.PlacePiecePlayer1;
        m_PlacePiecePlayer1Gamepads = m_TwoPlayerInputs.TwoPlayers.PlacePiecePlayer1Gamepads;
        m_StorePiece_Player1 = m_TwoPlayerInputs.TwoPlayers.StorePiecePlayer1;
     //storechanges   m_StorePiece_Player1Gamepads = m_TwoPlayerInputs.TwoPlayers.StorePieceGamepad;
        m_MovePiecePlayer2 = m_TwoPlayerInputs.TwoPlayers.PieceMovementPlayer2;
        m_MovePiecePlayer2Dpads = m_TwoPlayerInputs.TwoPlayers.PieceMovementPlayer2Gamepad;
        m_PlacePiecePlayer2 = m_TwoPlayerInputs.TwoPlayers.PlacePiecePlayer2;
        m_PlacePiecePlayer2Gamepads = m_TwoPlayerInputs.TwoPlayers.PlacePiecePlayer2Gamepads;
        m_StorePiece_Player2 = m_TwoPlayerInputs.TwoPlayers.StorePiecePlayer2;
        //storechnages m_StorePiece_Player2Gamepads = m_TwoPlayerInputs.TwoPlayers.StorePieceGamepad;
        m_QuitButton = m_TwoPlayerInputs.TwoPlayers.QuitButton;
        m_PauseButton = m_TwoPlayerInputs.TwoPlayers.PauseButton;
        m_PauseButton.performed += PauseGame;
        m_QuitButton.performed += QuitGame;
        m_QuitButton.Enable();
        m_PauseButton.Enable();
        UpcomingTilePlayer1.s_OnTileMove += SetUpcomingTile_Player1;
        UpcomingTilePlayer2.s_OnTileMove += SetUpcomingTile_Player2;
        PausePanelTwoPlayer.s_OnGameResumes += GameResumes;
        HoldTilePlayer1.s_ResetOnHold += ResetHoldTile_Player1;
        HoldTilePlayer2.s_ResetOnHold += ResetTilePiece_Player2;

        //StartCoroutine(GenerateStartingPieces());
        m_MovePiecePlayer1.performed += MoveTilePieces_Player1;
        m_MovePiecePlayer1Dpads.performed += MoveTilePiecesusingGamepad_Player1;
        m_PlacePiecePlayer1.performed += PlaceTilePiece_Player1;
        m_PlacePiecePlayer1Gamepads.performed += PlaceTilePiece_Player1Gamepads;
        m_MovePiecePlayer2.performed += MoveTilePieces_Player2;
        m_MovePiecePlayer2Dpads.performed += MoveTilePieces_Player2Dpads;
        m_PlacePiecePlayer2.performed += PlaceTilePiece_Player2;
        m_PlacePiecePlayer2Gamepads.performed += PlaceTilePiece_Player2Gamepads;
        m_StorePiece_Player1.performed += StoreTilePieces_Player1;
       //storechanges m_StorePiece_Player1Gamepads.performed += StoreTilePieces_Player1Gamepad;
        m_StorePiece_Player2.performed += StoreTilePieces_Player2;
        //storechnagesm_StorePiece_Player2Gamepads.performed += StoreTilePieces_Player2Gamepad;
        m_GameOverCoroutine = StartCoroutine(GameOver());
        StartCoroutine(StartFlashingStartingPiece());
        Invoke(nameof(StartInitialAuxillaryTimer_Player1), Constants.INT_ONE);
        Invoke(nameof(StartInitialAuxillaryTimer_Player2), Constants.INT_ONE);
    }

    private void StartInitialAuxillaryTimer_Player1()
    {
        m_DifficultyTimer_Player1Text.color = Color.white;
        m_AuxillaryTimerCoroutine_Player1 = StartCoroutine(StartAuxillaryTimer_Player1());
    }
    private void StartInitialAuxillaryTimer_Player2()
    {
        m_DifficultyTimer_Player2Text.color = Color.white;
        m_AuxillaryTimerCoroutine_Player2 = StartCoroutine(StartAuxillaryTimer_Player2());
    }

    private void GameResumes()
    {
       
        StartCoroutine(ActiveInputsAfterPause());
        /*StartCoroutine(ActiveInputsAfterdelay_Player1());
        StartCoroutine(ActiveInputsAfterdelay_Player2());*/
    }

    private void OnLeavePanelOpens()
    {
        s_TwoPlayerGamePauseStatus?.Invoke(true);
        m_LeavePanel.SetActive(true);
        DisablePlayer1Inputs();
        DisablePlayer2Inputs();
    }

    private void OnLeavePaneCloses()
    {
        m_LeavePanel.SetActive(false);

        /* s_TwoPlayerGamePauseStatus?.Invoke(false);
         m_LeavePanel.SetActive(false);
         StartCoroutine(ActiveInputsAfterdelay_Player1());
         StartCoroutine(ActiveInputsAfterdelay_Player2());*/
    }

    private void AssignStartingPieceGrid_Player1()
    {
        m_StartingPieceArea_Player1.GetComponent<RectTransform>().sizeDelta = m_Player1GridArea.GetComponent<RectTransform>().sizeDelta;
        for (int tileIndex = (int)Constants.ZERO; tileIndex < m_Player1GridArea.transform.childCount; tileIndex++)
        {
            GridTilePlayer1 gridTile = m_Player1GridArea.transform.GetChild(tileIndex).GetComponent<GridTilePlayer1>();
            GridTilePlayer1 startingTile = (gridTile.IsStartingPiece && !gridTile.IsStartingPieceCounted) ? Instantiate(gridTile, m_StartingPieceArea_Player1.transform) as GridTilePlayer1 : gridTile;
        }
    }
    private void AssignStartingPieceGrid_Player2()
    {
        m_StartingPieceArea_Player2.GetComponent<RectTransform>().sizeDelta = m_Player2GridArea.GetComponent<RectTransform>().sizeDelta;
        for (int tileIndex = (int)Constants.ZERO; tileIndex < m_Player2GridArea.transform.childCount; tileIndex++)
        {
            GridTilePlayer2 gridTile = m_Player2GridArea.transform.GetChild(tileIndex).GetComponent<GridTilePlayer2>();
            GridTilePlayer2 startingTile = (gridTile.IsStartingPiece && !gridTile.IsStartingPieceCounted) ? Instantiate(gridTile, m_StartingPieceArea_Player2.transform) as GridTilePlayer2 : gridTile;
        }
    }
    private void ClearStartingPieceArea(GameObject grid)
    {
        for (int tileIndex = (int)Constants.ZERO; tileIndex < grid.transform.childCount; tileIndex++)
        {
            Destroy(grid.transform.GetChild(tileIndex).gameObject);
        }
    }
    // Player 1 Movements, Placing and Others logic
    private void ActiveSinglePlayerInputs()
    {
        m_PlacePiecePlayer1.Enable();
        m_PlacePiecePlayer1Gamepads.Enable();
        m_MovePiecePlayer1.Enable();
        m_MovePiecePlayer1Dpads.Enable();
        m_StorePiece_Player1.Enable();
        //storechangesm_StorePiece_Player1Gamepads.Enable();
        m_LeftStickStatusPlayer1 = true;

    }
    private void PlaceTilePiece_Player1(InputAction.CallbackContext context)
    {
        m_MovePiecePlayer1.Disable();
        m_PlacePiecePlayer1.Disable();
        m_PlacePiecePlayer1Gamepads.Disable();
        m_MovePiecePlayer1Dpads.Disable();
        m_StorePiece_Player1.Disable();
        //storechanges m_StorePiece_Player1Gamepads.Disable();
        m_LeftStickStatusPlayer1 = false;
        PlaceTilePiecesinGameForFirstPlayer(m_Player1GridArea, s_PlaceBomb_Player1, s_PlaceTilePiece_Player1, s_CheckTileLeftVectorStatus_Player1, s_CheckTileRightVectorStatus_Player1, s_CheckTileUPVectorStatus_Player1, s_CheckTileDownVectorStatus_Player1, s_AutomaticTilePlaceCallback_Player1, s_SetCurrentPiece_Player1, ActiveSinglePlayerInputs, s_RefreshHoldTilePlacement_Player1, DecrementScoreCallback_Player1);
       
    }
    private void PlaceTilePiece_Player1Gamepads(InputAction.CallbackContext context)
    {
        Action placeTile = () =>
        {
            m_MovePiecePlayer1.Disable();
            m_PlacePiecePlayer1.Disable();
            m_PlacePiecePlayer1Gamepads.Disable();
            m_MovePiecePlayer1Dpads.Disable();
            m_StorePiece_Player1.Disable();
            //storechangesm_StorePiece_Player1Gamepads.Disable();
            m_LeftStickStatusPlayer1 = false;
            PlaceTilePiecesinGameForFirstPlayer(m_Player1GridArea, s_PlaceBomb_Player1, s_PlaceTilePiece_Player1, s_CheckTileLeftVectorStatus_Player1, s_CheckTileRightVectorStatus_Player1, s_CheckTileUPVectorStatus_Player1, s_CheckTileDownVectorStatus_Player1, s_AutomaticTilePlaceCallback_Player1, s_SetCurrentPiece_Player1, ActiveSinglePlayerInputs, s_RefreshHoldTilePlacement_Player1, DecrementScoreCallback_Player1);
        };
        
        if(GamepadManager.Instance.m_Gamepad1 != null)
        {
            if (GamepadManager.Instance.m_Gamepad1.aButton.IsActuated(0.1f))
            {
                placeTile?.Invoke();
            }
        }
    }
    private void MoveTilePieces_Player1(InputAction.CallbackContext context)
    {
        Vector2 movementVector = context.ReadValue<Vector2>();
        TriggerTileMovement(m_Player1GridArea, s_ActiveBomb_Player1, s_DeActiveTilePiece_Player1, s_ActiveTilePiece_Player1, movementVector);
    }
    private void MoveTilePiecesusingGamepad_Player1(InputAction.CallbackContext context)
    {
        if(GamepadManager.Instance.m_Gamepad1 != null)
        {
            if ((GamepadManager.Instance.m_Gamepad1.dpad.down.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.dpad.up.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.dpad.left.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.dpad.right.wasPressedThisFrame) /*|| (GamepadManager.Instance.m_Gamepad2.dpad.down.wasReleasedThisFrame || GamepadManager.Instance.m_Gamepad2.dpad.up.wasReleasedThisFrame || GamepadManager.Instance.m_Gamepad2.dpad.left.wasReleasedThisFrame || GamepadManager.Instance.m_Gamepad2.dpad.right.wasReleasedThisFrame)*/)
            {
                Vector2 movementVector = GamepadManager.Instance.m_Gamepad1.dpad.ReadValue();
                TriggerTileMovement(m_Player1GridArea, s_ActiveBomb_Player1, s_DeActiveTilePiece_Player1, s_ActiveTilePiece_Player1, movementVector);
            }
        }
    }
    private void SetUpcomingTile_Player1(Action<TilePiece> tileAction, Vector2 nextPosition)
    {
        if (!m_IsGameOver_Player1)
            PlaceUpcomingTile(m_Player1GridArea, m_SideTileAreaPlayer1, m_UpcomingTileParentPlayer1, tileAction, PieceTypesContainer, s_SetUpcomingBombPositon_Player1, s_SetGridTilePosition_Player1, DestroyUpcomingPiece_Player1, nextPosition, s_NextTileValidPosition_Player1);
    }
    private void DestroyUpcomingPiece_Player1()
    {
        Destroy(m_SideTileAreaPlayer1.transform.GetChild(0).GetComponent<TilePiece>().gameObject);
        StartCoroutine(ActiveInputsAfterdelay_Player1());
    }

    // Player 2 Movements, Placing and Others logic
    private void ActiveDoublePlayerInputs()
    {
        m_PlacePiecePlayer2.Enable();
        m_PlacePiecePlayer2Gamepads.Enable();
        m_MovePiecePlayer2Dpads.Enable();
        m_MovePiecePlayer2.Enable();
        m_StorePiece_Player2.Enable();
        //storechanges m_StorePiece_Player2Gamepads.Enable();
        m_LeftStickStatusPlayer2 = true;

    }
    private void PlaceTilePiece_Player2(InputAction.CallbackContext context)
    {
        m_MovePiecePlayer2.Disable();
        m_PlacePiecePlayer2.Disable();
        m_PlacePiecePlayer2Gamepads.Disable();
        m_MovePiecePlayer2Dpads.Disable();
        m_StorePiece_Player2.Disable();
        //storechnagesm_StorePiece_Player2Gamepads.Disable();
        m_LeftStickStatusPlayer2 = false;
        PlaceTilePiecesinGameForSecondPlayer(m_Player2GridArea, s_PlaceBomb_Player2, s_PlaceTilePiece_Player2, s_CheckTileLeftVectorStatus_Player2, s_CheckTileRightVectorStatus_Player2, s_CheckTileUPVectorStatus_Player2, s_CheckTileDownVectorStatus_Player2, s_AutomaticTilePlaceCallback_Player2, s_SetCurrentPiece_Player2, ActiveDoublePlayerInputs, s_RefreshHoldTilePlacement_Player2, DecrementScoreCallback_Player2);
       
    }

    private void PlaceTilePiece_Player2Gamepads(InputAction.CallbackContext context)
    {
        Action placeTile = () =>
        {
            m_MovePiecePlayer2.Disable();
            m_PlacePiecePlayer2.Disable();
            m_PlacePiecePlayer2Gamepads.Disable();
            m_MovePiecePlayer2Dpads.Disable();
            m_StorePiece_Player2.Disable();
            //storechanges m_StorePiece_Player2Gamepads.Disable();
            m_LeftStickStatusPlayer2 = false;
            PlaceTilePiecesinGameForSecondPlayer(m_Player2GridArea, s_PlaceBomb_Player2, s_PlaceTilePiece_Player2, s_CheckTileLeftVectorStatus_Player2, s_CheckTileRightVectorStatus_Player2, s_CheckTileUPVectorStatus_Player2, s_CheckTileDownVectorStatus_Player2, s_AutomaticTilePlaceCallback_Player2, s_SetCurrentPiece_Player2, ActiveDoublePlayerInputs, s_RefreshHoldTilePlacement_Player2, DecrementScoreCallback_Player2);
            
        };
        if(GamepadManager.Instance.m_Gamepad2 != null)
        {
            if(GamepadManager.Instance.m_Gamepad2.aButton.IsActuated(0.1f))
            {
                placeTile?.Invoke();
            }
        }
    }
    private void MoveTilePieces_Player2(InputAction.CallbackContext context)
    {
        Vector2 movementVector = context.ReadValue<Vector2>();
        TriggerTileMovement(m_Player2GridArea, s_ActiveBomb_Player2, s_DeActiveTilePiece_Player2, s_ActiveTilePiece_Player2, movementVector);
    }
    private void MoveTilePieces_Player2Dpads(InputAction.CallbackContext context)
    {
        if(GamepadManager.Instance.m_Gamepad2 != null)
        {
            if((GamepadManager.Instance.m_Gamepad2.dpad.down.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad2.dpad.up.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad2.dpad.left.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad2.dpad.right.wasPressedThisFrame) /*|| (GamepadManager.Instance.m_Gamepad1.dpad.down.wasReleasedThisFrame || GamepadManager.Instance.m_Gamepad1.dpad.up.wasReleasedThisFrame || GamepadManager.Instance.m_Gamepad1.dpad.left.wasReleasedThisFrame || GamepadManager.Instance.m_Gamepad1.dpad.right.wasReleasedThisFrame)*/)
            {
                Vector2 movementVector = GamepadManager.Instance.m_Gamepad2.dpad.ReadValue();
                TriggerTileMovement(m_Player2GridArea, s_ActiveBomb_Player2, s_DeActiveTilePiece_Player2, s_ActiveTilePiece_Player2, movementVector);
            }
        }
    }
    private void SetUpcomingTile_Player2(Action<TilePiece> tileAction, Vector2 nextPosition)
    {
        if (!m_IsGameOver_Player2)
            PlaceUpcomingTile(m_Player2GridArea, m_SideTileAreaPlayer2, m_UpcomingTileParentPlayer2, tileAction, PieceTypesContainer, s_SetUpcomingBombPositon_Player2, s_SetGridTilePosition_Player2, DestroyUpcomingPiece_Player2, nextPosition, s_NextTileValidPosition_Player2);
    }
    private void DestroyUpcomingPiece_Player2()
    {
        Destroy(m_SideTileAreaPlayer2.transform.GetChild(0).GetComponent<TilePiece>().gameObject);
        StartCoroutine(ActiveInputsAfterdelay_Player2());
    }
    private void HandleSideTilePanelGeneration()
    {
        PieceTypesContainer = new SidePieceTye[Constants.TOTAL_SIDEPANELPIECES];
        PiecePercentages();
        AllocateSidePieceContainer(PieceTypesContainer);
        ShuffleSidePieceContainer(PieceTypesContainer);
        RemoveConsecutivePieceFromContainer(PieceTypesContainer);
        FillSidePieceContainer(m_SideTileAreaPlayer1, PieceTypesContainer);
        FillSidePieceContainer(m_SideTileAreaPlayer2, PieceTypesContainer);
        StartCoroutine(SetDelayinInitialUpcomigTilePlayer1());
        StartCoroutine(SetDelayinInitialUpcomigTilePlayer2());
    }

    private void HandleTileGeneration()
    {
        float tileSizeX = CalculateTileSizeX(Constants.TWOPLAYERGRIDSIZE, GameData.Instance.GameOptions.TwoPlayerGameRows);
        float tileSizeY = CalculateTileSizeY(Constants.TWOPLAYERGRIDSIZE, GameData.Instance.GameOptions.TwoPlayerGameColumn);
        float minSize = (tileSizeX < tileSizeY) ? tileSizeX : tileSizeY;
        AllocateSizeToGridTwoPlayerMode(m_Player1GridArea, minSize, m_Player1BGImage);
        DestroyTileArea(m_Player1GridArea);
        GenerateTileAreaPlayer1(m_Player1GridArea, m_GridTilePlayer1);
        AllocateSizeToGridTwoPlayerMode(m_Player2GridArea, minSize, m_Player2BGImage);
        DestroyTileArea(m_Player2GridArea);
        GenerateTileAreaPlayer2(m_Player2GridArea, m_GridTilePlayer2);
    }

    private void HandleObstacleSpawn()
    {
        CreateObstacleIgnoredArea(m_Player1GridArea, m_ObstacleIgnoredArea_Player1);
        CreateObstacleIgnoredArea(m_Player2GridArea, m_ObstacleIgnoredArea_Player2);
        CreateObstacleSpawnArea(m_Player1GridArea, m_ObstacleIgnoredArea_Player1, m_ObstacleSpawnArea_Player1);
        CreateObstacleSpawnArea(m_Player2GridArea, m_ObstacleIgnoredArea_Player2, m_ObstacleSpawnArea_Player2);
        ShuffleTwoPlayerObstacleArea(m_ObstacleSpawnArea_Player1, m_ObstacleSpawnArea_Player2);
        SpawnRoundObstacles(m_ObstacleSpawnArea_Player1, s_OnRoundObstacleSpawn_Player1, GameManager.instace.RoundObstacleCount_TwoPlayer);
        SpawnRoundObstacles(m_ObstacleSpawnArea_Player2, s_OnRoundObstacleSpawn_Player2, GameManager.instace.RoundObstacleCount_TwoPlayer);
        SpawnHorizonatalObstacles(m_ObstacleSpawnArea_Player1, s_OnHorizontalObstacleSpawn_Player1);
        SpawnHorizonatalObstacles(m_ObstacleSpawnArea_Player2, s_OnHorizontalObstacleSpawn_Player2);
        SpawnVerticalObstacles(m_ObstacleSpawnArea_Player1, s_OnVerticalObstacleSpawn_Player1);
        SpawnVerticalObstacles(m_ObstacleSpawnArea_Player2, s_OnVerticalObstacleSpawn_Player2);
    }
    private void StartingPieceTimerCallback_Player1(GridTilePlayer1 startingPiecePosition)
    {
        Action gameOverAction = () =>
        {
            LoopLastPieceCallback_Player1(startingPiecePosition, Constants.INT_ONE);
        };
        Action<GridTilePlayer1, Vector2, Vector2> startingPieceStatusCheck = null;
        startingPieceStatusCheck = (GridTilePlayer1 startingPiece, Vector2 nextPiecePosition, Vector2 flowVector) =>
        {
            s_CheckStartingPieceLoopStatus_Player1?.Invoke(startingPiece, nextPiecePosition, flowVector, startingPieceStatusCheck);
        };
        s_CheckStartingPieceLoopStatus_Player1?.Invoke(startingPiecePosition, startingPiecePosition.GridPosition + startingPiecePosition.StartingPieceFlowVector, startingPiecePosition.StartingPieceFlowVector, startingPieceStatusCheck);
        Action continuousFlowAction = () =>
        {
            float flowSpeed = GameData.Instance.GameOptions.FlowSpeed + (m_DiffficultyFlowTimerFlag_PLAYER1 * GameData.Instance.GameOptions.DifficultFlowSpeed);
           
            Debug.Log("Continuos Flow Check");
            Action<Vector2, Vector2> nextAnimatedTile = null;
            nextAnimatedTile = (Vector2 previousTilePosition, Vector2 nextTilePosition) =>
            {
                s_HandlContinuousFlowAnimation_Player1?.Invoke(previousTilePosition, nextTilePosition, nextAnimatedTile, LoopLastPieceCallback_Player1, IncrementScoreCallback_Player1, startingPiecePosition, s_ResetStartingPiecePositions_Player1, CheckLoopStatus_Player1, flowSpeed, OnLoopFlowStarts_Player1);
            };
            s_HandlContinuousFlowAnimation_Player1?.Invoke(startingPiecePosition.GridPosition, startingPiecePosition.StartingPieceLoopPool[Constants.INT_ZERO].GridPosition, nextAnimatedTile, LoopLastPieceCallback_Player1, IncrementScoreCallback_Player1, startingPiecePosition, s_ResetStartingPiecePositions_Player1, CheckLoopStatus_Player1, flowSpeed, OnLoopFlowStarts_Player1);
        };
        if (startingPiecePosition.StartingPieceLoopPool.Count > Constants.ZERO && startingPiecePosition.transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
            continuousFlowAction?.Invoke();
        else
            gameOverAction?.Invoke();
        //21-06-22StartCoroutine(CalculateScorePlayer1(startingPiecePosition));
    }

    private void OnPlayer1GameOver()
    {
        s_OnGameOvers_Player1?.Invoke(true);
        DisablePlayer1Inputs();
        m_IsGameOver_Player1 = true;
        m_IsGameOver_Player2 = (m_CurrentScore_Player2 > m_CurrentScore_Player1 && !m_IsGameOver_Player2) ? true : m_IsGameOver_Player2;
        StartCoroutine(GameOverCallback());
    }
    private void OnPlayer2GameOver()
    {
        s_OnGameOvers_Player2?.Invoke(true);
        DisablePlayer2Inputs();
        m_IsGameOver_Player2 = true;
        m_IsGameOver_Player1 = (m_CurrentScore_Player1 > m_CurrentScore_Player2 && !m_IsGameOver_Player1) ? true : m_IsGameOver_Player1;
        StartCoroutine(GameOverCallback());
    }

    private void LoopLastPieceCallback_Player1(GridTilePlayer1 lastTile, float time)
    {
        DisablePlayer1Inputs();
        Vector2 targetVector = GetLastPieceFlowVector_Player1(lastTile);
        AssignBrokenPieceeGrid_Player1(lastTile, 0f, targetVector);
        Debug.Log("Last Piece Callback");
        OnPlayer1GameOver();
    }

    private void IncrementScoreCallback_Player1(int score, bool isStartingPiece)
    {
        Debug.Log("Starting piece callback successfull: " + isStartingPiece);
        StartCoroutine(IncreaseScoreDuringFlow_Player1(score));
        if (isStartingPiece)
        {
            Debug.Log("Starting piece callback successfull");
            StartCoroutine(CalculateDetachedPieces_Player1(false));
            StartCoroutine(DecreaseScoreDuringFlow_Player1(m_DetachPiecesCount_Player1 * GameData.Instance.GameOptions.UnusedTilePenalty + (int)m_TotalPenaltyValuePlayer1));
            SpawnNewStartingPiece_Player1(true);
        }
    }
    private void DecrementScoreCallback_Player1(int score)
    {
        StartCoroutine(DecreaseScoreDuringFlow_Player1(score));
    }

    private void StartingPieceTimerCallback_Player2(GridTilePlayer2 startingPiecePosition)
    {
        Action gameOverAction = () =>
        {
            LoopLastPieceCallback_Player2(startingPiecePosition, Constants.INT_ONE);
        };
        Action<GridTilePlayer2, Vector2, Vector2> startingPieceStatusCheck = null;
        startingPieceStatusCheck = (GridTilePlayer2 startingPiece, Vector2 nextPiecePosition, Vector2 flowVector) =>
        {
            s_CheckStartingPieceLoopStatus_Player2?.Invoke(startingPiece, nextPiecePosition, flowVector, startingPieceStatusCheck);
        };
        s_CheckStartingPieceLoopStatus_Player2?.Invoke(startingPiecePosition, startingPiecePosition.GridPosition + startingPiecePosition.StartingPieceFlowVector, startingPiecePosition.StartingPieceFlowVector, startingPieceStatusCheck);
        Action continuousFlowAction = () =>
        {
            float flowSpeed = GameData.Instance.GameOptions.FlowSpeed + (m_DiffficultyFlowTimerFlag_PLAYER2 * GameData.Instance.GameOptions.DifficultFlowSpeed);
            Debug.Log("Continuos Flow Check");
            Action<Vector2, Vector2> nextAnimatedTile = null;
            nextAnimatedTile = (Vector2 previousTilePosition, Vector2 nextTilePosition) =>
            {
                s_HandlContinuousFlowAnimation_Player2?.Invoke(previousTilePosition, nextTilePosition, nextAnimatedTile, LoopLastPieceCallback_Player2, IncrementScoreCallback_Player2, startingPiecePosition, s_ResetStartingPiecePositions_Player2, CheckLoopStatus_Player2, flowSpeed, OnLoopFlowStarts_Player2);
            };
            s_HandlContinuousFlowAnimation_Player2?.Invoke(startingPiecePosition.GridPosition, startingPiecePosition.StartingPieceLoopPool[Constants.INT_ZERO].GridPosition, nextAnimatedTile, LoopLastPieceCallback_Player2, IncrementScoreCallback_Player2, startingPiecePosition, s_ResetStartingPiecePositions_Player2, CheckLoopStatus_Player2, flowSpeed, OnLoopFlowStarts_Player2);
        };
        if (startingPiecePosition.StartingPieceLoopPool.Count > Constants.ZERO && startingPiecePosition.transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
            continuousFlowAction?.Invoke();
        else
            gameOverAction?.Invoke();
        //21-06-22  StartCoroutine(CalculateScorePlayer2(startingPiecePosition));
    }
    private void LoopLastPieceCallback_Player2(GridTilePlayer2 lastTile, float time)
    {
        DisablePlayer2Inputs();
        Vector2 targetVector = GetLastPieceFlowVector_Player2(lastTile);
        AssignBrokenPieceeGrid_Player2(lastTile, 0f, targetVector);
        Debug.Log("Last Piece Callback");
        OnPlayer2GameOver();
    }

    private void IncrementScoreCallback_Player2(int score, bool isStartingPiece)
    {
        Debug.Log("Starting piece callback successfull: " + isStartingPiece);
        StartCoroutine(IncreaseScoreDuringFlow_Player2(score));
        if (isStartingPiece)
        {
            Debug.Log("Starting piece callback successfull");
            StartCoroutine(CalculateDetachedPieces_Player2(false));
            StartCoroutine(DecreaseScoreDuringFlow_Player2(m_DetachPiecesCount_Player2 * GameData.Instance.GameOptions.UnusedTilePenalty + (int)m_TotalPenaltyValuePlayer2));
            SpawnNewStartingPiece_Player2(true);
        }
    }

    private void DecrementScoreCallback_Player2(int score)
    {
        StartCoroutine(DecreaseScoreDuringFlow_Player2(score));
    }


    private void HandleInputDuringFlow_Player1()
    {
        if (!m_FlowStatus_Player1)
        {
            m_MovePiecePlayer1.Enable();
            m_MovePiecePlayer1Dpads.Enable();
            m_PlacePiecePlayer1.Enable();
            m_PlacePiecePlayer1Gamepads.Enable();
            m_StorePiece_Player1.Enable();
           //storechanges m_StorePiece_Player1Gamepads.Enable();
            m_LeftStickStatusPlayer1 = true;
            m_FlowStatus_Player1 = true;
        }
    }
    private void HandleInputDuringFlow_Player2()
    {
        if (!m_FlowStatus_Player2)
        {
            m_MovePiecePlayer2.Enable();
            m_PlacePiecePlayer2.Enable();
            m_PlacePiecePlayer2Gamepads.Enable();
            m_MovePiecePlayer2Dpads.Enable();
            m_StorePiece_Player2.Enable();
            //storechnages m_StorePiece_Player2Gamepads.Enable();
            m_FlowStatus_Player2 = true;
            m_LeftStickStatusPlayer2 = true;
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator StartTileFlow_Player1(List<GridTilePlayer1> loopPieces, bool brokenLoop, Vector2 targetVector)
    {
        m_FlowStatus_Player1 = false;
       //skchnages float speed = GameData.Instance.GameOptions.FlowSpeed;
        float speed = Constants.MAX_FLOWSPEED;
        float time = 1f / speed;
        float totalTimer = time * loopPieces.Count;
        if (brokenLoop && loopPieces.Count > Constants.ZERO)
        {
            AssignBrokenPieceeGrid_Player1(loopPieces[loopPieces.Count - Constants.INT_ONE], totalTimer, targetVector);
        }
        UpdateFlowTimer(totalTimer);
        
        for (int loopPieceIndex = Constants.INT_ZERO; loopPieceIndex < loopPieces.Count; loopPieceIndex++)
        {
            s_StartFlowAnimation_Player1?.Invoke(loopPieces[loopPieceIndex].GridPosition, time, s_ResetStartingPiecePositions_Player1, brokenLoop);
            yield return new WaitForSeconds(time);
        }

    }

    private IEnumerator StartTileFlow_Player2(List<GridTilePlayer2> loopPieces, bool brokenLoop, Vector2 targetVector)
    {
        m_FlowStatus_Player2 = false;
        //skchnagesfloat speed = GameData.Instance.GameOptions.FlowSpeed;
        float speed = Constants.MAX_FLOWSPEED;
        float time = 1f / speed;
        float totalTimer = time * loopPieces.Count;
        if (brokenLoop && loopPieces.Count > Constants.ZERO)
        {
            AssignBrokenPieceeGrid_Player2(loopPieces[loopPieces.Count - Constants.INT_ONE], totalTimer, targetVector);
        }
        UpdateFlowTimer(totalTimer);
        for (int loopPieceIndex = Constants.INT_ZERO; loopPieceIndex < loopPieces.Count; loopPieceIndex++)
        {
            s_StartFlowAnimation_Player2?.Invoke(loopPieces[loopPieceIndex].GridPosition, time, s_ResetStartingPiecePositions_Player2, brokenLoop);
            yield return new WaitForSeconds(time);
        }

    }
    private IEnumerator CalculateDetachedPieces_Player1(bool isLoopBreaked)
    {
        m_TotalPenaltyValuePlayer1 = 0;
        yield return new WaitForSeconds(Constants.SCOREDELAY);
       
       /*skchnages foreach (Transform staringPiece in m_Player1GridArea.transform)
        {
            GridTilePlayer1 tile = staringPiece.GetComponent<GridTilePlayer1>();
            m_TotalPenaltyValuePlayer1 += (int)tile.PenaltyValue;
        }*/
        Action<int> detachPieceCallback = (int detachPieces) =>
        {
            m_DetachPiecesCount_Player1++;
        };
        s_CalculateDetachPieces_Player1?.Invoke(detachPieceCallback, isLoopBreaked);
    }
    
    private IEnumerator CalculateDetachedPieces_Player2(bool isLoopBreaked)
    {
        m_TotalPenaltyValuePlayer2 = 0;
        yield return new WaitForSeconds(Constants.SCOREDELAY);
       /* skchnagesforeach (Transform staringPiece in m_Player2GridArea.transform)
        {
            GridTilePlayer2 tile = staringPiece.GetComponent<GridTilePlayer2>();
            m_TotalPenaltyValuePlayer2 += (int)tile.PenaltyValue;
        }*/
        Action<int> detachPieceCallback = (int detachPieces) =>
        {
            m_DetachPiecesCount_Player2++;
        };
        s_CalculateDetachPieces_Player2?.Invoke(detachPieceCallback, isLoopBreaked);
    }

    private IEnumerator GenerateStartingPieces()
    {
        s_SpawnNextStartingPiece_Player1?.Invoke(s_CheckSafeAreaStatus_Player1, m_StartingPieceSpawnSpots_Player1);
        yield return new WaitForSeconds(0.01f);
        int angleList = (int)Random.Range(Constants.ZERO, Constants.THREE);
        int randomeAngle = (int)m_AngleList[angleList];
       // ShuffleStartingPiecePositions(GameManager.instace.StartingPiecePoolTwoPlayer);
        ShuffleStartingPiecePositions(m_StartingPieceSpawnSpots_Player1);
        //PlaceStarttingPiece(m_Player1GridArea, GameManager.instace.StartingPieceCount, GameManager.instace.StartingPiecePoolTwoPlayer, s_SetStartingPositon_Player1, randomeAngle);
        PlaceStarttingPiece(m_Player1GridArea, GameManager.instace.StartingPieceCount, m_StartingPieceSpawnSpots_Player1, s_SetStartingPositon_Player1, randomeAngle);
        AssignStartingPieceGrid_Player1();
       // PlaceStarttingPiece(m_Player2GridArea, GameManager.instace.StartingPieceCount, GameManager.instace.StartingPiecePoolTwoPlayer, s_SetStartingPositon_Player2, randomeAngle);
        PlaceStarttingPiece(m_Player2GridArea, GameManager.instace.StartingPieceCount, m_StartingPieceSpawnSpots_Player1, s_SetStartingPositon_Player2, randomeAngle);
        AssignStartingPieceGrid_Player2();
        yield return new WaitForSeconds(0.1f);
        m_StartingPieceSpawnSpots_Player1?.Clear();
        HandleObstacleSpawn();
        yield return StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1f);
        s_OnStartingPieceCounterChanges_Player1?.Invoke(StartingPieceTimerCallback_Player1, CheckLoopStatus_Player1, SpawnNewStartingPiece_Player1, m_StartingPieceRemainingTime_Player1, FlowAnimationCallbackPlayer1);
        s_OnStartingPieceCounterChanges_Player2?.Invoke(StartingPieceTimerCallback_Player2, CheckLoopStatus_Player2, SpawnNewStartingPiece_Player2, m_StartingPieceRemainingTime_Player2, FlowAnimationCallbackPlayer2);
    }

    private void CheckLoopStatus_Player1(GridTilePlayer1 startingPiecePosition)
    {
        HandleInputDuringFlow_Player1();
        Action<GridTilePlayer1, Vector2, Vector2> startingPieceStatusCheck = null;
        startingPieceStatusCheck = (GridTilePlayer1 startingPiece, Vector2 nextPiecePosition, Vector2 flowVector) =>
        {
            s_MidCounterLoopStatus_Player1?.Invoke(startingPiece, nextPiecePosition, flowVector, startingPieceStatusCheck);
        };
        s_MidCounterLoopStatus_Player1?.Invoke(startingPiecePosition, startingPiecePosition.GridPosition + startingPiecePosition.StartingPieceFlowVector, startingPiecePosition.StartingPieceFlowVector, startingPieceStatusCheck);
    }

    private void CheckLoopStatus_Player2(GridTilePlayer2 startingPiecePosition)
    {
        HandleInputDuringFlow_Player2();
        Action<GridTilePlayer2, Vector2, Vector2> startingPieceStatusCheck = null;
        startingPieceStatusCheck = (GridTilePlayer2 startingPiece, Vector2 nextPiecePosition, Vector2 flowVector) =>
        {
            s_MidCounterLoopStatus_Player2?.Invoke(startingPiece, nextPiecePosition, flowVector, startingPieceStatusCheck);
        };
        s_MidCounterLoopStatus_Player2?.Invoke(startingPiecePosition, startingPiecePosition.GridPosition + startingPiecePosition.StartingPieceFlowVector, startingPiecePosition.StartingPieceFlowVector, startingPieceStatusCheck);
    }

    private void SpawnNewStartingPiece_Player1(bool isContiniousFlow = false)
    {
        HandleInputDuringFlow_Player1();
        m_FirstPlayerLoopCount++;
        if (m_FirstPlayerLoopCount >= GameData.Instance.GameOptions.MultiplayerLoopCompleted)
        {
            StartCoroutine(OnLoopCompletedGameOver(m_Player1NameText.text));
        }
        List<GridTilePlayer1> loopPieces = new List<GridTilePlayer1>();
        bool horizontalStatus = false;
        bool verticalStatus = false;
        Action normalFlowActiion = () =>
        {
            OnLoopFlowStarts_Player1();
            for (int startingPieceIndex = Constants.INT_ZERO; startingPieceIndex < m_Player1GridArea.transform.childCount; startingPieceIndex++)
            {
                GridTilePlayer1 currenTile = m_Player1GridArea.transform.GetChild(startingPieceIndex).GetComponent<GridTilePlayer1>();
                if (currenTile.IsStartingPiece)
                    loopPieces.AddRange(currenTile.PieceLoop);
            }
            foreach (GridTilePlayer1 loopPiece in loopPieces)
            {
                s_LockLoopPiece_Player1?.Invoke(loopPiece.GridPosition);
            }
        };

        Action continuousFlowAction = () =>
        {
            for (int startingPieceIndex = Constants.INT_ZERO; startingPieceIndex < m_Player1GridArea.transform.childCount; startingPieceIndex++)
            {
                GridTilePlayer1 currenTile = m_Player1GridArea.transform.GetChild(startingPieceIndex).GetComponent<GridTilePlayer1>();
                if (currenTile.IsStartingPiece)
                    loopPieces.AddRange(currenTile.ContiniousFlowPieces);
            }
        };
        if (!isContiniousFlow)
            normalFlowActiion?.Invoke();
        else
            continuousFlowAction?.Invoke();
        m_ObstacleIgnoredArea_Player2.Clear();
        m_ObstacleSpawnArea_Player2.Clear();
        CreateObstacleIgnoredArea(m_Player2GridArea, m_ObstacleIgnoredArea_Player2);
        CreateObstacleSpawnArea(m_Player2GridArea, m_ObstacleIgnoredArea_Player2, m_ObstacleSpawnArea_Player2);
        ThrowObstacles(m_ObstacleSpawnArea_Player2, s_OnRoundObstacleSpawn_Player2, Constants.INT_ONE, loopPieces.Count);
        m_StartingPieceSpawnSpots_Player1.Clear();
        m_StartingPieceHorizontalSpawnSpots_Player1.Clear();
        m_StartingPieceVerticalSpawnSpots_Player1.Clear();
        float time = (Constants.INT_ONE / Constants.MAX_FLOWSPEED) * loopPieces.Count;
        if (isContiniousFlow)
            time = Constants.INT_ZERO;
        Invoke("SpawnNewStartingPiece_Player1withDelay", time + 1.5f);
        /*s_SpawnNextStartingPiece_Player1?.Invoke(s_CheckSafeAreaStatus_Player1, m_StartingPieceSpawnSpots_Player1);
        s_SpawnStartingPiecePlayer1_Horizontal?.Invoke(s_CheckStartingPieceHorizontalSafeAreaStatus_Player1, loopPieces, m_StartingPieceHorizontalSpawnSpots_Player1);
        s_SpawnStartingPiecePlayer1_Vertical?.Invoke(s_CheckStartingPieceVerticalSafeAreaStatus_Player1, loopPieces, m_StartingPieceVerticalSpawnSpots_Player1);

        Action<Vector2> SpawnStartingPiece = (Vector2 RandomPosition) =>
        {
            m_StartingPieceBaseTimer_Player1 -= GameData.Instance.GameOptions.StartingPieceTimeDifference;
            if (m_StartingPieceBaseTimer_Player1 <= Constants.ZERO)
            {
                OnPlayer1GameOver();
                return;
            }
           //21-06-22 s_SpawnStartingPiece_Player1?.Invoke(RandomPosition, m_StartingPieceRemainingTime_Player1[Constants.INT_ZERO] + 5, horizontalStatus, verticalStatus);
            s_SpawnStartingPiece_Player1?.Invoke(RandomPosition, (int)m_StartingPieceBaseTimer_Player1, horizontalStatus, verticalStatus);
            StartCoroutine(AssignStartingPieceToDummyGrid_Player1());
            StartCoroutine(StartTimer());
            StopCoroutine(m_GameOverCoroutine);
            m_GameOverCoroutine = StartCoroutine(GameOver());
        };
        if (m_StartingPieceSpawnSpots_Player1.Count > Constants.ZERO)
        {
            Vector2 RandomPosition = m_StartingPieceSpawnSpots_Player1[Random.Range(0, m_StartingPieceSpawnSpots_Player1.Count)];
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else if (m_StartingPieceHorizontalSpawnSpots_Player1.Count > Constants.ZERO)
        {
            Vector2 RandomPosition = m_StartingPieceHorizontalSpawnSpots_Player1[Random.Range(0, m_StartingPieceHorizontalSpawnSpots_Player1.Count)];
            horizontalStatus = true;
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else if (m_StartingPieceVerticalSpawnSpots_Player1.Count > Constants.ZERO)
        {
            Vector2 RandomPosition = m_StartingPieceVerticalSpawnSpots_Player1[Random.Range(0, m_StartingPieceSpawnSpots_Player1.Count)];
            verticalStatus = true;
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else
        {
            return;
        }*/
    }

    private void SpawnNewStartingPiece_Player1withDelay()
    {
        List<GridTilePlayer1> loopPieces = new List<GridTilePlayer1>();
        bool horizontalStatus = false;
        bool verticalStatus = false;
        s_SpawnNextStartingPiece_Player1?.Invoke(s_CheckSafeAreaStatus_Player1, m_StartingPieceSpawnSpots_Player1);
        s_SpawnStartingPiecePlayer1_Horizontal?.Invoke(s_CheckStartingPieceHorizontalSafeAreaStatus_Player1, loopPieces, m_StartingPieceHorizontalSpawnSpots_Player1);
        s_SpawnStartingPiecePlayer1_Vertical?.Invoke(s_CheckStartingPieceVerticalSafeAreaStatus_Player1, loopPieces, m_StartingPieceVerticalSpawnSpots_Player1);

        Action<Vector2> SpawnStartingPiece = (Vector2 RandomPosition) =>
        {
            m_StartingPieceBaseTimer_Player1 -= (GameData.Instance.GameOptions.StartingPieceTimeDifference + m_IncreaseTimeDifference_Player1);
            m_IncreaseTimeDifference_Player1 = Constants.INT_ZERO;
            m_StartingPieceBaseTimer_Player1 = (m_StartingPieceBaseTimer_Player1 <= Constants.ZERO) ? GameData.Instance.GameOptions.MinimumStartingTimer : m_StartingPieceBaseTimer_Player1;
            m_DiffficultyFlowTimerFlag_PLAYER1 = (m_StartingPieceBaseTimer_Player1 > GameData.Instance.GameOptions.MinimumStartingTimer) ? m_DiffficultyFlowTimerFlag_PLAYER1 + Constants.INT_ONE : m_DiffficultyFlowTimerFlag_PLAYER1;
            /*if (m_StartingPieceBaseTimer_Player1 <= Constants.ZERO)
            {
                OnPlayer1GameOver();
                return;
            }*/
            //21-06-22 s_SpawnStartingPiece_Player1?.Invoke(RandomPosition, m_StartingPieceRemainingTime_Player1[Constants.INT_ZERO] + 5, horizontalStatus, verticalStatus);
            s_SpawnStartingPiece_Player1?.Invoke(RandomPosition, (int)m_StartingPieceBaseTimer_Player1, horizontalStatus, verticalStatus);
            StartCoroutine(AssignStartingPieceToDummyGrid_Player1());
            StartCoroutine(StartTimer());
            StopCoroutine(m_GameOverCoroutine);
            m_GameOverCoroutine = StartCoroutine(GameOver());
        };
        if (m_StartingPieceSpawnSpots_Player1.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceSpawnSpots_Player1);
            Vector2 RandomPosition = m_StartingPieceSpawnSpots_Player1[Random.Range(0, m_StartingPieceSpawnSpots_Player1.Count)];
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else if (m_StartingPieceHorizontalSpawnSpots_Player1.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceHorizontalSpawnSpots_Player1);
            Vector2 RandomPosition = m_StartingPieceHorizontalSpawnSpots_Player1[Random.Range(0, m_StartingPieceHorizontalSpawnSpots_Player1.Count)];
            horizontalStatus = true;
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else if (m_StartingPieceVerticalSpawnSpots_Player1.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceVerticalSpawnSpots_Player1);
            Vector2 RandomPosition = m_StartingPieceVerticalSpawnSpots_Player1[Random.Range(0, m_StartingPieceSpawnSpots_Player1.Count)];
            verticalStatus = true;
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else
        {
            return;
        }
        OnLoopEnds_Player1();
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        OnPauseButtonTap();
    }

    private void OnPauseButtonTap()
    {
        if (!p_IsGamePaused)
        {
            s_TwoPlayerGamePauseStatus?.Invoke(true);
            DisablePlayer1Inputs();
            DisablePlayer2Inputs();
            StartCoroutine(OnPauseGame(m_PausePanel));
            p_IsGamePaused = true;
        }
    }
    private void SpawnNewStartingPiece_Player2(bool isContiniousFlow = false)
    {
        HandleInputDuringFlow_Player2();
        m_SecondPlayerLoopCount++;
        if (m_SecondPlayerLoopCount >= GameData.Instance.GameOptions.MultiplayerLoopCompleted)
        {
            StartCoroutine(OnLoopCompletedGameOver(m_Player2NameText.text));
        }
        List<GridTilePlayer2> loopPieces = new List<GridTilePlayer2>();
        bool horizontalStatus = false;
        bool verticalStatus = false;

        Action normalFlowAction = () =>
        {
            OnLoopFlowStarts_Player2();
            for (int startingPieceIndex = Constants.INT_ZERO; startingPieceIndex < m_Player2GridArea.transform.childCount; startingPieceIndex++)
            {
                GridTilePlayer2 currenTile = m_Player2GridArea.transform.GetChild(startingPieceIndex).GetComponent<GridTilePlayer2>();
                if (currenTile.IsStartingPiece)
                    loopPieces.AddRange(currenTile.PieceLoop);
            }
            foreach (GridTilePlayer2 loopPiece in loopPieces)
            {
                s_LockLoopPiece_Playe2?.Invoke(loopPiece.GridPosition);
            }
        };

        Action continuousFlowAction = () =>
        {
            for (int startingPieceIndex = Constants.INT_ZERO; startingPieceIndex < m_Player2GridArea.transform.childCount; startingPieceIndex++)
            {
                GridTilePlayer2 currenTile = m_Player2GridArea.transform.GetChild(startingPieceIndex).GetComponent<GridTilePlayer2>();
                if (currenTile.IsStartingPiece)
                    loopPieces.AddRange(currenTile.ContiniousFlowPieces);
            }
        };

        if (!isContiniousFlow)
            normalFlowAction?.Invoke();
        else
            continuousFlowAction?.Invoke();

        m_ObstacleIgnoredArea_Player1.Clear();
        m_ObstacleSpawnArea_Player1.Clear();
        CreateObstacleIgnoredArea(m_Player1GridArea, m_ObstacleIgnoredArea_Player1);
        CreateObstacleSpawnArea(m_Player1GridArea, m_ObstacleIgnoredArea_Player1, m_ObstacleSpawnArea_Player1);
        ThrowObstacles(m_ObstacleSpawnArea_Player1, s_OnRoundObstacleSpawn_Player1, Constants.INT_ONE, loopPieces.Count);
        m_StartingPieceSpawnSpots_Player2.Clear();
        m_StartingPieceHorizontalSpawnSpots_Player2.Clear();
        m_StartingPieceVerticalSpawnSpots_Player2.Clear();
        float time = (Constants.INT_ONE / Constants.MAX_FLOWSPEED) * loopPieces.Count;
        if (isContiniousFlow)
            time = Constants.INT_ZERO;
        Invoke("SpawnNewStartingPiece_Player2withDelay", time + 1.5f);
        /* s_SpawnNextStartingPiece_Player2?.Invoke(s_CheckSafeAreaStatus_Player2, m_StartingPieceSpawnSpots_Player2);
         s_SpawnStartingPiecePlayer2_Horizontal?.Invoke(s_CheckStartingPieceHorizontalSafeAreaStatus_Player2, loopPieces, m_StartingPieceHorizontalSpawnSpots_Player2);
         s_SpawnStartingPiecePlayer2_Vertical?.Invoke(s_CheckStartingPieceVerticalSafeAreaStatus_Player2, loopPieces, m_StartingPieceVerticalSpawnSpots_Player2);

         Action<Vector2> SpawnStartingPiece = (Vector2 RandomPosition) =>
         {
             m_StartingPieceBaseTimer_Player2 -= GameData.Instance.GameOptions.StartingPieceTimeDifference;
             if (m_StartingPieceBaseTimer_Player2 <= Constants.ZERO)
             {
                 OnPlayer2GameOver();
                 return;
             }
             s_SpawnStartingPiece_Player2?.Invoke(RandomPosition, (int)m_StartingPieceBaseTimer_Player2, horizontalStatus, verticalStatus);
             StartCoroutine(AssignStartingPieceToDummyGrid_Player2());
             StartCoroutine(StartTimer());
             StopCoroutine(m_GameOverCoroutine);
             m_GameOverCoroutine = StartCoroutine(GameOver());
         };
         if (m_StartingPieceSpawnSpots_Player2.Count > Constants.ZERO)
         {
             Vector2 RandomPosition = m_StartingPieceSpawnSpots_Player2[Random.Range(0, m_StartingPieceSpawnSpots_Player2.Count)];
             SpawnStartingPiece?.Invoke(RandomPosition);
         }
         else if (m_StartingPieceHorizontalSpawnSpots_Player2.Count > Constants.ZERO)
         {
             Vector2 RandomPosition = m_StartingPieceHorizontalSpawnSpots_Player2[Random.Range(0, m_StartingPieceHorizontalSpawnSpots_Player2.Count)];
             horizontalStatus = true;
             SpawnStartingPiece?.Invoke(RandomPosition);
         }
         else if (m_StartingPieceVerticalSpawnSpots_Player2.Count > Constants.ZERO)
         {
             Vector2 RandomPosition = m_StartingPieceVerticalSpawnSpots_Player2[Random.Range(0, m_StartingPieceVerticalSpawnSpots_Player2.Count)];
             verticalStatus = true;
             SpawnStartingPiece?.Invoke(RandomPosition);

         }
         else
         {
             return;
         }*/
    }

    private void SpawnNewStartingPiece_Player2withDelay()
    {
        List<GridTilePlayer2> loopPieces = new List<GridTilePlayer2>();
        bool horizontalStatus = false;
        bool verticalStatus = false;
        s_SpawnNextStartingPiece_Player2?.Invoke(s_CheckSafeAreaStatus_Player2, m_StartingPieceSpawnSpots_Player2);
        s_SpawnStartingPiecePlayer2_Horizontal?.Invoke(s_CheckStartingPieceHorizontalSafeAreaStatus_Player2, loopPieces, m_StartingPieceHorizontalSpawnSpots_Player2);
        s_SpawnStartingPiecePlayer2_Vertical?.Invoke(s_CheckStartingPieceVerticalSafeAreaStatus_Player2, loopPieces, m_StartingPieceVerticalSpawnSpots_Player2);

        Action<Vector2> SpawnStartingPiece = (Vector2 RandomPosition) =>
        {
            m_StartingPieceBaseTimer_Player2 -= (GameData.Instance.GameOptions.StartingPieceTimeDifference + m_IncreaseTimeDifference_Player2);
            m_IncreaseTimeDifference_Player2 = Constants.INT_ZERO;
            m_StartingPieceBaseTimer_Player2 = (m_StartingPieceBaseTimer_Player2 <= Constants.ZERO) ? GameData.Instance.GameOptions.MinimumStartingTimer : m_StartingPieceBaseTimer_Player2;
            m_DiffficultyFlowTimerFlag_PLAYER2 = (m_StartingPieceBaseTimer_Player2 > GameData.Instance.GameOptions.MinimumStartingTimer) ? m_DiffficultyFlowTimerFlag_PLAYER2 + Constants.INT_ONE : m_DiffficultyFlowTimerFlag_PLAYER2;
            /*if (m_StartingPieceBaseTimer_Player2 <= Constants.ZERO)
            {
                OnPlayer2GameOver();
                return;
            }*/
            s_SpawnStartingPiece_Player2?.Invoke(RandomPosition, (int)m_StartingPieceBaseTimer_Player2, horizontalStatus, verticalStatus);
            StartCoroutine(AssignStartingPieceToDummyGrid_Player2());
            StartCoroutine(StartTimer());
            StopCoroutine(m_GameOverCoroutine);
            m_GameOverCoroutine = StartCoroutine(GameOver());
        };
        if (m_StartingPieceSpawnSpots_Player2.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceSpawnSpots_Player2);
            Vector2 RandomPosition = m_StartingPieceSpawnSpots_Player2[Random.Range(0, m_StartingPieceSpawnSpots_Player2.Count)];
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else if (m_StartingPieceHorizontalSpawnSpots_Player2.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceHorizontalSpawnSpots_Player2);
            Vector2 RandomPosition = m_StartingPieceHorizontalSpawnSpots_Player2[Random.Range(0, m_StartingPieceHorizontalSpawnSpots_Player2.Count)];
            horizontalStatus = true;
            SpawnStartingPiece?.Invoke(RandomPosition);
        }
        else if (m_StartingPieceVerticalSpawnSpots_Player2.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceVerticalSpawnSpots_Player2);
            Vector2 RandomPosition = m_StartingPieceVerticalSpawnSpots_Player2[Random.Range(0, m_StartingPieceVerticalSpawnSpots_Player2.Count)];
            verticalStatus = true;
            SpawnStartingPiece?.Invoke(RandomPosition);

        }
        else
        {
            return;
        }
        OnLoopEnds_Player2();
    }

    private IEnumerator IncreaseScoreDuringFlow_Player1(int increasedScore)
    {
        Debug.Log("Score is increased");

        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player1;
        float finalScore = m_CurrentScore_Player1 + increasedScore;
        bool canDecrease = false;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player1 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player1 = (int)finalScore;
        m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
    }

    private IEnumerator DecreaseScoreDuringFlow_Player1(int decreasedScore)
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Score is decreased");
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player1;
        float finalScore = m_CurrentScore_Player1 + decreasedScore;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player1 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
            timeElapsed += Constants.DECREMENT_SCORESPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player1 = (int)finalScore;
        m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
        // DisableInputs();
    }

    private IEnumerator IncreaseScoreDuringFlow_Player2(int increasedScore)
    {
        Debug.Log("Score is increased");

        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player2;
        float finalScore = m_CurrentScore_Player2 + increasedScore;
        bool canDecrease = false;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player2 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player2 = (int)finalScore;
        m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
    }

    private IEnumerator DecreaseScoreDuringFlow_Player2(int decreasedScore)
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Score is decreased");
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player2;
        float finalScore = m_CurrentScore_Player2 + decreasedScore;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player2 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
            timeElapsed += Constants.DECREMENT_SCORESPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player2 = (int)finalScore;
        m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
        // DisableInputs();
    }

    private IEnumerator AssignStartingPieceToDummyGrid_Player1()
    {
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        AssignStartingPieceGrid_Player1();
    }
    private IEnumerator AssignStartingPieceToDummyGrid_Player2()
    {
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        AssignStartingPieceGrid_Player2();
    }

    private IEnumerator ActiveInputsAfterdelay_Player2()
    {
        yield return new WaitForSeconds(Constants.INPUT_DELAY);
        ActiveDoublePlayerInputs();
    }
    private IEnumerator ActiveInputsAfterdelay_Player1()
    {
        yield return new WaitForSeconds(Constants.INPUT_DELAY);
        ActiveSinglePlayerInputs();
    }
    private IEnumerator CalculateScorePlayer1(GridTilePlayer1 startingPiece, bool isLoopBreaked = true)
    {
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        m_DetachPieceCoroutine_Player1 = StartCoroutine(CalculateDetachedPieces_Player1(isLoopBreaked));
        CalculateFinalScorePlayer1(startingPiece);
    }
    private IEnumerator CalculateScorePlayer2(GridTilePlayer2 startingPiece, bool isLoopBreaked = true)
    {
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        m_DetachPieceCoroutine_Player2 = StartCoroutine(CalculateDetachedPieces_Player2(isLoopBreaked));
        CalculateFinalScorePlayer2(startingPiece);
    }
    private IEnumerator IncreaseScorePlayer1(int increasedScore)
    {
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player1;
        float finalScore = m_CurrentScore_Player1 + increasedScore;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player1 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player1 = (int)finalScore;
        m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();

        yield return new WaitForSeconds(Constants.DETACHDPIECEDELAY);
        yield return StartCoroutine(DecreaseScorePlayer1(m_DetachPiecesCount_Player1 * GameData.Instance.GameOptions.UnusedTilePenalty));
    }

    private IEnumerator DecreaseScorePlayer1(int decreasedScore)
    {
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player1;
        float finalScore = m_CurrentScore_Player1 + decreasedScore + m_TotalPenaltyValuePlayer1;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player1 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player1 = (int)finalScore;
        m_ScoreText_Player1.text = m_CurrentScore_Player1.ToString();
    }
    private IEnumerator IncreaseScorePlayer2(int increasedScore)
    {
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player2;
        float finalScore = m_CurrentScore_Player2 + increasedScore;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player2 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player2 = (int)finalScore;
        m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();

        yield return new WaitForSeconds(Constants.DETACHDPIECEDELAY);
        yield return StartCoroutine(DecreaseScorePlayer2(m_DetachPiecesCount_Player2 * GameData.Instance.GameOptions.UnusedTilePenalty));
    }

    private IEnumerator DecreaseScorePlayer2(int decreasedScore)
    {
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore_Player2;
        float finalScore = m_CurrentScore_Player2 + decreasedScore + m_TotalPenaltyValuePlayer2;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore_Player2 = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore_Player2 = (int)finalScore;
        m_ScoreText_Player2.text = m_CurrentScore_Player2.ToString();
    }
    IEnumerator OnLoopCompletedGameOver(string playerName)
    {
        m_Loader.SetActive(true);
        yield return new WaitForSeconds(Constants.INT_ONE);
        m_Loader.SetActive(false);
        s_OnLoopCompleteWinnerTriggered?.Invoke(playerName);

    }
    IEnumerator GameOver(float flowTimer = 0f, bool loopFlowDelay = false)
    {
        m_MaxGameTimer = (int)flowTimer;
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        if (!loopFlowDelay)
        {
            for (int tile1Index = (int)Constants.ZERO; tile1Index < m_Player1GridArea.transform.childCount; tile1Index++)
            {
                Action<GridTilePlayer1> startingPieceAction = (GridTilePlayer1 tile) =>
                {
                    m_MaxGameTimer = (m_MaxGameTimer < tile.StartingPieceCounter) ? tile.StartingPieceCounter : m_MaxGameTimer;
                    m_GameTimerPlayer1 = tile.StartingPieceCounter;
                    /*21-06-22if(m_GameClockPlayer1 != null)
                    {
                        StopCoroutine(m_GameClockPlayer1);
                    }
                    m_GameClockPlayer1 = StartCoroutine(UpdatePlayer1Clock());*/
                    s_StartingPieceCounted_Player1?.Invoke(tile.GridPosition);
                };
                GridTilePlayer1 tile = m_Player1GridArea.transform.GetChild(tile1Index).GetComponent<GridTilePlayer1>();
                if (tile.IsStartingPiece && !tile.IsStartingPieceCounted)
                    startingPieceAction?.Invoke(tile);
            }
            for (int tile2Index = (int)Constants.ZERO; tile2Index < m_Player2GridArea.transform.childCount; tile2Index++)
            {
                Action<GridTilePlayer2> startingPieceAction = (GridTilePlayer2 tile) =>
                {
                    m_MaxGameTimer = (m_MaxGameTimer < tile.StartingPieceCounter) ? tile.StartingPieceCounter : m_MaxGameTimer;
                    m_GameTimerPlayer2 = tile.StartingPieceCounter;
                    /*21-06-22if(m_GameClockPlayer2 != null)
                    {
                        StopCoroutine(m_GameClockPlayer2);
                    }
                    m_GameClockPlayer2 = StartCoroutine(UpdatePlayer2Clock());*/
                    s_StartingPieceCounted_Player2?.Invoke(tile.GridPosition);
                };
                GridTilePlayer2 tile = m_Player2GridArea.transform.GetChild(tile2Index).GetComponent<GridTilePlayer2>();
                if (tile.IsStartingPiece && !tile.IsStartingPieceCounted)
                    startingPieceAction?.Invoke(tile);
            }
        }
        m_MaxGameTimer = m_MaxGameTimer + 4;
        while (m_MaxGameTimer > Constants.ZERO)
        {
            m_MaxGameTimer--;
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        /*21-06-22yield return new WaitForEndOfFrame();
        m_Loader.SetActive(true);
        yield return new WaitForSeconds(Constants.GAMEOVERSCOREDELAY);
        m_Loader.SetActive(false);
        s_OnGameOverCallback?.Invoke(m_Player1NameText.text, m_Player2NameText.text, m_CurrentScore_Player1, m_CurrentScore_Player2);*/
    }

    IEnumerator SetDelayinInitialUpcomigTilePlayer1()
    {
        yield return new WaitForSeconds(Constants.INPUT_DELAY);
        SetInitialUpcomingTilePlayer1(m_UpcomingTilePlayer1, m_UpcomingTileParentPlayer1, m_SideTileAreaPlayer1, PieceTypesContainer);
    }
    IEnumerator SetDelayinInitialUpcomigTilePlayer2()
    {
        yield return new WaitForSeconds(Constants.INPUT_DELAY);
        SetInitialUpcomingTilePlayer2(m_UpcomingTilePlayer2, m_UpcomingTileParentPlayer2, m_SideTileAreaPlayer2, PieceTypesContainer);
    }

    IEnumerator UpdatePlayer1Clock()
    {
        s_OnGameOvers_Player1?.Invoke(false);
        while (m_GameTimerPlayer1 > Constants.ZERO)
        {
            m_GameTimerPlayer1--;
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        yield return new WaitForEndOfFrame();
        s_OnGameOvers_Player1?.Invoke(true);
        DisablePlayer1Inputs();
        m_IsGameOver_Player1 = true;
    }

    IEnumerator UpdatePlayer2Clock()
    {
        s_OnGameOvers_Player2?.Invoke(false);
        while (m_GameTimerPlayer2 > Constants.ZERO)
        {
            m_GameTimerPlayer2--;
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        yield return new WaitForEndOfFrame();
        s_OnGameOvers_Player2?.Invoke(true);
        DisablePlayer2Inputs();
        m_IsGameOver_Player2 = true;
    }

    IEnumerator StartFlashingStartingPiece()
    {
        yield return new WaitForSeconds(0.1f);
        GridTilePlayer1 startingPieceTilePlayer1 = null;
        GridTilePlayer2 startingPieceTilePlayer2 = null;
        for (int tileIndex = Constants.INT_ZERO; tileIndex < m_Player1GridArea.transform.childCount; tileIndex++)
        {
            GridTilePlayer1 tile = m_Player1GridArea.transform.GetChild(tileIndex).GetComponent<GridTilePlayer1>();
            if (tile.IsStartingPiece)
            {
                startingPieceTilePlayer1 = tile;
            }
        }
        for (int tileIndex = Constants.INT_ZERO; tileIndex < m_Player2GridArea.transform.childCount; tileIndex++)
        {
            GridTilePlayer2 tile = m_Player2GridArea.transform.GetChild(tileIndex).GetComponent<GridTilePlayer2>();
            if (tile.IsStartingPiece)
            {
                startingPieceTilePlayer2 = tile;
            }
        }
        if (startingPieceTilePlayer1 != null)
        {
            s_FlashStartingPiece_Player1?.Invoke(startingPieceTilePlayer1.GridPosition, true);
        }
        if (startingPieceTilePlayer2 != null)
        {
            s_FlashStartingPiece_Player2?.Invoke(startingPieceTilePlayer2.GridPosition, true);
        }
        yield return new WaitForSeconds(Constants.FLASH_INTERVAL);
        s_FlashStartingPiece_Player1?.Invoke(startingPieceTilePlayer1.GridPosition, false);
        s_FlashStartingPiece_Player2?.Invoke(startingPieceTilePlayer2.GridPosition, false);

    }

    IEnumerator ShowTutorialPanel()
    {
        yield return new WaitForSeconds(0.8f);
        m_TutorialPanel.gameObject.SetActive(true);
    }

    private IEnumerator ActiveInputsAfterPause()
    {
        yield return new WaitForSeconds(0.4f);
        p_IsGamePaused = false;
        Time.timeScale = 1f;
        s_TwoPlayerGamePauseStatus?.Invoke(false);
        ActiveSinglePlayerInputs();
        ActiveDoublePlayerInputs();
    }

    IEnumerator GameOverCallback(float time = 0)
    {
        if(m_IsGameOver_Player1 && m_IsGameOver_Player2)
        {
            StopCoroutine(m_AuxillaryTimerCoroutine_Player1);
            StopCoroutine(m_AuxillaryTimerCoroutine_Player2);
            yield return new WaitForSeconds(time + Constants.INT_ONE);
            m_Loader.SetActive(true);
            yield return new WaitForSeconds(1f);
            m_Loader.SetActive(false);
            s_OnGameOverCallback?.Invoke(m_Player1NameText.text, m_Player2NameText.text, m_CurrentScore_Player1, m_CurrentScore_Player2);
        }
    }

    IEnumerator StartAuxillaryTimer_Player1()
    {
        Action colorAction = () =>
        {
            if (m_CurrentAuxillaryTimer_Player1 < Constants.AUXILLARYTIMER_COUNTDOWN)
                m_DifficultyTimer_Player1Text.color = Color.red;
            else
                m_DifficultyTimer_Player1Text.color = Color.white;
        };
        m_CurrentAuxillaryTimer_Player1 = GameData.Instance.GameOptions.AuxillaryTimer;
        m_DifficultyTimer_Player1Text.text = m_CurrentAuxillaryTimer_Player1.ToString();
        while (m_CurrentAuxillaryTimer_Player1 > Constants.INT_ZERO)
        {
            m_CurrentAuxillaryTimer_Player1--;
            colorAction?.Invoke();
            m_DifficultyTimer_Player1Text.text = m_CurrentAuxillaryTimer_Player1.ToString();
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        yield return new WaitForEndOfFrame();
        Debug.Log("Auxillary Time Ended");
        m_IncreaseTimeDifference_Player1 += (int)GameData.Instance.GameOptions.DifficultyStartingPieceTimeReduction;
        m_AuxillaryTimerCoroutine_Player1 = StartCoroutine(StartAuxillaryTimer_Player1());
    }

    IEnumerator StartAuxillaryTimer_Player2()
    {
        Action colorAction = () =>
        {
            if (m_CurrentAuxillaryTimer_Player2 < Constants.AUXILLARYTIMER_COUNTDOWN)
                m_DifficultyTimer_Player2Text.color = Color.red;
            else
                m_DifficultyTimer_Player2Text.color = Color.white;
        };
        m_CurrentAuxillaryTimer_Player2 = GameData.Instance.GameOptions.AuxillaryTimer;
        m_DifficultyTimer_Player2Text.text = m_CurrentAuxillaryTimer_Player2.ToString();
        while (m_CurrentAuxillaryTimer_Player2 > Constants.INT_ZERO)
        {
            m_CurrentAuxillaryTimer_Player2--;
            m_DifficultyTimer_Player2Text.text = m_CurrentAuxillaryTimer_Player2.ToString();
            colorAction?.Invoke();
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        yield return new WaitForEndOfFrame();
        Debug.Log("Auxillary Time Ended");
        m_IncreaseTimeDifference_Player2 += (int)GameData.Instance.GameOptions.DifficultyStartingPieceTimeReduction;
        m_AuxillaryTimerCoroutine_Player2 = StartCoroutine(StartAuxillaryTimer_Player2());
    }

    #endregion

}
