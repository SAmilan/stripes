using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using System.Linq;

public class ViewSinglePlayerGame : ViewGame
{
    #region Private_Variables
    [SerializeField]
    private TextMeshProUGUI TestText;
    [SerializeField]
    private RectTransform m_BGImage;
    [SerializeField]
    private GameObject m_Loader;
    [SerializeField]
    private GameObject m_MainPlayGridArea;
    [SerializeField]
    private GameObject m_StartingPieceArea;
    [SerializeField]
    private GameObject m_BrokenPieceArea;
    [SerializeField]
    private GameObject m_SideTilePanelArea;
    [SerializeField]
    private GameObject m_UpcomingTileParent;
    [SerializeField]
    private GameObject m_TutorialPanel;
    [SerializeField]
    private GridTile m_Tile;
    [SerializeField]
    private float m_TileSize;
    [SerializeField]
    private SidePieceTye[] PieceTypesContainer;
    [SerializeField]
    private Button m_HomeButton;
    [SerializeField]
    UpcomingTile m_UpcomingTile;
    [SerializeField]
    private GameObject m_LeavePanel;
    [SerializeField]
    private Button m_LeavePanelOpenButton;
    [SerializeField]
    private Button m_LeavePanelCloseButton;
    private GridTile m_GridTileReference;
    private SinglePlayerInputs m_GridInputActions;
    private InputAction m_MovePiece;
    private InputAction m_PlacePiece;
    private InputAction m_PlacePiece_Gamepad;
    private InputAction m_LeftShoulderAction;
    private InputAction m_QuitButton;
    private InputAction m_PauseButton;
    private InputAction m_StorePiece;
    [SerializeField]
    private List<Vector2> m_ObstacleIgnoredArea = new List<Vector2>();
    [SerializeField]
    private List<TilePiece> m_ObstacleSpawnArea = new List<TilePiece>();
    [SerializeField]
    private TextMeshProUGUI m_ScoreText;
    [SerializeField]
    private TextMeshProUGUI m_PlayerNameText;
    [SerializeField]
    private int m_CurrentScore;
    [SerializeField]
    private int m_DetachPiecesCount;
    private bool m_IsGameOver = false;
    [SerializeField]
    private GameObject m_PieceList;
    [SerializeField]
    private Image m_SidePanelShowListBG;
    [SerializeField]
    private GameObject m_SidePanelHideListBG;
    [SerializeField]
    private GameObject m_PausePanel;
    [SerializeField]
    private string m_PlayerName;
    [SerializeField]
    private int m_MaxGameTimer;
    [SerializeField]
    private int m_GameClock;
    [SerializeField]
    private int m_DiffficultyFlowTimerFlag = 0;
    private float m_FlowAnimationTimer;
    [SerializeField]
    private List<Vector2> m_StartingPieceSpawnSpots = new List<Vector2>();
    [SerializeField]
    private List<Vector2> m_StartingPieceHorizontalSpawnSpots = new List<Vector2>();
    [SerializeField]
    private List<Vector2> m_StartingPieceVerticalSpawnSpots = new List<Vector2>();
    private int[] m_StartingPieceRemainingTime = new int[Constants.INT_ONE];
    private Coroutine m_GameOverCoroutine;
    private Coroutine m_DetachPieceCoroutine;
    private Coroutine m_FlowAnimationCoroutine;
    private Coroutine m_UpdateClockCoroutine;
    [SerializeField]
    private float m_TotalPenalty;
    private float m_StartingPieceBaseTimer;
    private bool m_FlowStatus;
    [SerializeField]
    private bool m_GameCheckingStatus = false;
    [SerializeField]
    private Button m_PauseUIbutton;
    private bool m_ShowTutorial = true;
    [SerializeField]
    private int m_CurrentAuxillaryTimer;
    private Coroutine m_AuxillaryTimerCoroutine = null;
    [SerializeField]
    private int m_IncreaseTimeDifference;
    [SerializeField]
    TextMeshProUGUI m_DifficultyTimerText;


    #endregion

    #region Public_Variables
    public static Action<TilePiece, TilePiece, Action> s_SetGridTilePosition;
    public static Action<TilePiece, Action> s_SetUpcomingBombPositon;
    public static Action<TilePiece, Vector2> s_ActiveTilePiece;
    public static Action<Vector2, Action<int>> s_PlaceTilePiece;
    public static Action<Vector2> s_DeActiveTilePiece;
    public static Action<Vector2> s_ActiveBomb;
    public static Action<Vector2> s_PlaceBomb;
    public static Action<GridTile, Vector2> s_CheckTileLeftVectorStatus;
    public static Action<GridTile, Vector2> s_CheckTileRightVectorStatus;
    public static Action<GridTile, Vector2> s_CheckTileUPVectorStatus;
    public static Action<GridTile, Vector2> s_CheckTileDownVectorStatus;
    public static Action<Action<GridTile>, Action<GridTile>, Action<bool>, int[], Action<GridTile>> s_OnStartingPieceCounterChanges;
    public static Action<Action<GridTile>, Action<GridTile>, Action, int[]> s_NewStartingPieceCounterChanges;
    public static Action<Vector2> s_OnRoundObstacleSpawn;
    public static Action<Vector2, Vector2> s_OnHorizontalObstacleSpawn;
    public static Action<Vector2, Vector2, Vector2> s_OnVerticalObstacleSpawn;
    public static Action<bool> s_OnGameOvers;
    public static Action s_OnGameEnds;
    public static Action<Vector2, int, int> s_SetStartingPositon;
    public static Action<GridTile, Vector2, Vector2, Action<GridTile, Vector2, Vector2>> s_CheckStartingPieceLoopStatus;
    public static Action<GridTile, Vector2, Vector2, Action<GridTile, Vector2, Vector2>> s_MidCounterLoopStatus;
    public static Action<int> s_OnScoreUpdates;
    public static Action<Action<int>, bool> s_CalculateDetachPieces;
    public static Action<string, int> s_OnGameManagerCallbacks;
    public static Action<Vector2> s_StartingPieceCounted;
    public static Action<Action<Vector2, Action<bool>>, List<Vector2>> s_SpawnNextStartingPiece;
    public static Action<Action<Vector2, List<GridTile>, Action<bool>>, List<GridTile>, List<Vector2>> s_SpawnStartingPiece_Horizontal;
    public static Action<Action<Vector2, List<GridTile>, Action<bool>>, List<GridTile>, List<Vector2>> s_SpawnStartingPiece_Vertical;
    public static Action<Vector2, Action<bool>> s_CheckSafeAreaStatus;
    public static Action<Vector2, List<GridTile>, Action<bool>> s_CheckStartingPieceHorizontalSafeAreaStatus;
    public static Action<Vector2, List<GridTile>, Action<bool>> s_CheckStartingPieceVerticalSafeAreaStatus;
    public static Action<Vector2, int, bool, bool> s_SpawnStartingPiece;
    public static Action<Vector2, float, Action<Vector2>, bool> s_StartFlowAnimation;
    public static Action s_AutomaticTilePlaceCallback;
    public static Action<Vector2> s_SetCurrentPiece;
    public static Action<Vector2> s_LockLoopPiece;
    public static Action<Vector2, Action<GridTile>> s_NextTileValidPosition;
    public static Action<Vector2> s_ResetStartingPiecePositions;
    public static Action<bool> s_SinglePlayerGamePauseStatus;
    public static Action<Vector2, float, Vector2> s_TriggerBrokenAnimation;
    public static Action<Action<bool, Vector2>> s_StorePieceTriggered;
    public static Action s_ActiveUpcomingInputs;
    public static Action<Vector2> s_ResetCurrentPosition;
    public static Action<Vector2, bool> s_FlashStartingPiece;
    public static Action<Vector2, Vector2, Action<Vector2, Vector2>, Action<GridTile, float>, Action<int, bool>, GridTile, Action<Vector2>, Action<GridTile>, float, Action> s_HandlContinuousFlowAnimation;
    public static Action s_RefreshHoldTilePlacement;

    #endregion

    #region Unity_Callbacks


    private void OnEnable()
    {
        m_AuxillaryTimerCoroutine = null;
        m_CurrentAuxillaryTimer = Constants.INT_ZERO;
        m_StartingPieceBaseTimer = GameData.Instance.GameOptions.StartingPieceTimer;
        m_GameCheckingStatus = false;
        SoundManager.Instance.PlayWindowTransitionSound();
        SoundManager.Instance.PlayThemeSound();
        m_PlayerName = ExtractName();
        m_PlayerNameText.text = m_PlayerName;
        p_IsGamePaused = false;
        m_PausePanel.SetActive(false);
        SidePanelStatusChanges();
        m_Loader.SetActive(false);
        ResetParams();
        HandleTileGeneration();
        HandleTilePanelGeneration();
        SinglePlayerTutorialPanel.s_TutorialEnds += OnTutorialEnds;
        StartCoroutine(ShowTutorialPanel());
    }
    private void OnDisable()
    {
        SinglePlayerTutorialPanel.s_TutorialEnds -= OnTutorialEnds;
        s_SinglePlayerGamePauseStatus?.Invoke(false);
        ClearData();
        DisableInputs();
        m_QuitButton.Disable();
        m_MovePiece.performed -= MoveTilePieces;
        m_PlacePiece.performed -= PlaceTilePiece;
        m_PlacePiece_Gamepad.performed -= PlaceTilePiece_Gamepads;
        m_LeftShoulderAction.performed -= OnLeftStickTriggered;
        m_PauseButton.performed -= PauseGame;
        m_StorePiece.performed -= StoreTilePieces;
        m_PauseButton.Disable();
        m_QuitButton.performed -= QuitGame;
        PausePanelSinglePlayer.s_OnGameResumes -= GameResumes;
        GamepadManager.Instance.StopVibration();
        UpcomingTile.s_OnTileMove -= SetUpcomingTile;
        UpcomingTile.s_OnTileMoveVirtually -= SetUpcomingTileVirtually;
        HoldTile.s_ResetOnHold -= ResetHoldTile;
        SoundManager.Instance.StopThemeSound();
    }

    private void Awake()
    {
        m_GridInputActions = new SinglePlayerInputs();
        m_GridTileReference = GetComponent<GridTile>();
        m_HomeButton.onClick.AddListener(OnGameEndCalls);
        m_LeavePanelCloseButton.onClick.AddListener(OnLeavePanelCloses);
        m_LeavePanelOpenButton.onClick.AddListener(OnLeavePanelOpens);
        m_PauseUIbutton.onClick.AddListener(OnPauseButtonTap);
    }



    #endregion
    #region Private_Methods

    private void OnTutorialEnds()
    {
        m_FlowAnimationCoroutine = StartCoroutine(Dummy());
        m_MovePiece = m_GridInputActions.SinglePlayer.PieceMovement;
        m_PlacePiece = m_GridInputActions.SinglePlayer.PlacePiece;
        m_PlacePiece_Gamepad = m_GridInputActions.SinglePlayer.PlacePieceGamepad;
        m_LeftShoulderAction = m_GridInputActions.SinglePlayer.LeftStickTriggered;
        m_QuitButton = m_GridInputActions.SinglePlayer.QuitButton;
        m_PauseButton = m_GridInputActions.SinglePlayer.PauseButton;
        m_StorePiece = m_GridInputActions.SinglePlayer.StorePiece;
        m_StorePiece.performed += StoreTilePieces;
        m_MovePiece.performed += MoveTilePieces;
        m_PlacePiece.performed += PlaceTilePiece;
        m_PlacePiece_Gamepad.performed += PlaceTilePiece_Gamepads;
        m_LeftShoulderAction.performed += OnLeftStickTriggered;
        m_QuitButton.performed += QuitGame;
        m_PauseButton.performed += PauseGame;
        m_QuitButton.Enable();
        m_PauseButton.Enable();
        PausePanelSinglePlayer.s_OnGameResumes += GameResumes;
        StartCoroutine(GenerateStartingPieces());
        m_GameOverCoroutine = StartCoroutine(GameOver());
        StartCoroutine(StartCheckinggameStatus());
        UpcomingTile.s_OnTileMove += SetUpcomingTile;
        UpcomingTile.s_OnTileMoveVirtually += SetUpcomingTileVirtually;
        StartCoroutine(StartFlashingStartingPiece());
        HoldTile.s_ResetOnHold += ResetHoldTile;
        Invoke(nameof(StartInitialAuxillaryTimer), Constants.INT_ONE);
       
    }

    private void StartInitialAuxillaryTimer()
    {
        m_DifficultyTimerText.color = Color.white;
        m_AuxillaryTimerCoroutine = StartCoroutine(StartAuxillaryTimer());
    }

    private void ResetHoldTile(Vector2 currentHoldPosition)
    {
        bool activePieceStaus = false;
        Vector2 activePosition = Vector2.zero;
        TilePiece holdedTile = null;
        s_StorePieceTriggered?.Invoke((bool status, Vector2 currenPosition) =>
        {
            activePieceStaus = status;
            activePosition = currenPosition;
        });
        if (activePieceStaus)
        {
            Debug.Log("Reset Hold tile triggered");
            DisableInputs();
            s_ResetCurrentPosition?.Invoke(currentHoldPosition);
            s_ActiveUpcomingInputs?.Invoke();
        }

    }
    private string ExtractName()
    {
        string name = NameManager.GetPlayer1Name();
        if (name == null)
        {
            name = Constants.GUEST_NAME;
        }
        return name;
    }
    private void SidePanelStatusChanges()
    {
        bool status = GameManager.instace.UpcomingTileStatus;
        m_PieceList.SetActive(status);
        m_SidePanelShowListBG.enabled = status;
        m_SidePanelHideListBG.SetActive(!status);
    }
    private void ClearData()
    {
        p_SidePiecePercentages.Clear();
        m_ObstacleIgnoredArea.Clear();
        m_ObstacleSpawnArea.Clear();
        m_StartingPieceSpawnSpots.Clear();
        m_StartingPieceHorizontalSpawnSpots.Clear();
        m_StartingPieceVerticalSpawnSpots.Clear();
        PieceTypesContainer = null;
    }
    private void ResetParams()
    {
        m_DifficultyTimerText.text = GameData.Instance.GameOptions.AuxillaryTimer.ToString();
        m_IncreaseTimeDifference = Constants.INT_ZERO;
        m_DiffficultyFlowTimerFlag = Constants.INT_ZERO;
        m_FlowAnimationTimer = Constants.ZERO;
        m_FlowAnimationCoroutine = null;
        m_IsGameOver = false;
        m_DetachPiecesCount = (int)Constants.ZERO;
        m_CurrentScore = (int)Constants.ZERO;
        m_ScoreText.text = m_CurrentScore.ToString();
        m_LeavePanel.SetActive(false);
        ClearStartingPieceArea();
        ClearBrokenPiecePieceArea();
        m_MainPlayGridArea.GetComponent<GridLayoutGroup>().enabled = true;
    }
    private void DisableInputs()
    {
        m_MovePiece.Disable();
        m_PlacePiece.Disable();
        m_PlacePiece_Gamepad.Disable();
        m_LeftShoulderAction.Disable();
        m_StorePiece.Disable();
    }

    private void OnLeavePanelOpens()
    {
        s_SinglePlayerGamePauseStatus?.Invoke(true);
        m_LeavePanel.gameObject.SetActive(true);
        DisableInputs();
    }
    private void OnLeavePanelCloses()
    {
        //m_PausePanel.gameObject.SetActive(false);
        m_LeavePanel.gameObject.SetActive(false);
        /* changes on 24-06-22
         * s_SinglePlayerGamePauseStatus?.Invoke(false);

         m_LeavePanel.gameObject.SetActive(false);
         StartCoroutine(ActiveInputsAfterdelay());*/
    }
    private void OnGameEndCalls()
    {
        Time.timeScale = 1f;
        m_PausePanel.gameObject.SetActive(true);
        OnGameEnds(s_OnGameEnds);
    }
    private IEnumerator ActiveInputsAfterdelay()
    {
        yield return new WaitForSeconds(Constants.INPUT_DELAY);
        ActiveSinglePlayerInputs();
    }
    private IEnumerator ActiveInputsAfterPause()
    {
        yield return new WaitForSeconds(0.4f);
        p_IsGamePaused = false;
        Time.timeScale = 1f;
        s_SinglePlayerGamePauseStatus?.Invoke(false);
        ActiveSinglePlayerInputs();
    }

    private void ActiveSinglePlayerInputs()
    {
        m_PlacePiece.Enable();
        m_PlacePiece_Gamepad.Enable();
        m_MovePiece.Enable();
        m_LeftShoulderAction.Enable();
        m_StorePiece.Enable();
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        OnPauseButtonTap();
    }

    private void OnPauseButtonTap()
    {
        if (!p_IsGamePaused)
        {
            DisableInputs();
            StartCoroutine(OnPauseGame(m_PausePanel));
            s_SinglePlayerGamePauseStatus?.Invoke(true);
            p_IsGamePaused = true;
        }
    }

    private void OnLoopFlowStarts()
    {
        DisableInputs();
        s_SinglePlayerGamePauseStatus?.Invoke(true);
        p_IsGamePaused = true;
    }

    private void OnLoopEnds()
    {
        p_IsGamePaused = false;
        s_SinglePlayerGamePauseStatus?.Invoke(false);
        ActiveSinglePlayerInputs();
    }

    private void GameResumes()
    {


        StartCoroutine(ActiveInputsAfterPause());

    }

    private void OnLeftStickTriggered(InputAction.CallbackContext context)
    {
        var leftStickInputValue = Gamepad.current.leftStick.ReadValue().normalized;
        Vector2 movementVector = ApplyApproximationonMovementVector(leftStickInputValue);
        TriggerTileMovement(m_MainPlayGridArea, s_ActiveBomb, s_DeActiveTilePiece, s_ActiveTilePiece, movementVector);
    }
    private void PlaceTilePiece(InputAction.CallbackContext context)
    {
        m_MovePiece.Disable();
        m_LeftShoulderAction.Disable();
        m_PlacePiece.Disable();
        m_PlacePiece_Gamepad.Disable();
        m_StorePiece.Disable();
        PlaceTilePiecesinGame(m_MainPlayGridArea, s_PlaceBomb, s_PlaceTilePiece, s_CheckTileLeftVectorStatus, s_CheckTileRightVectorStatus, s_CheckTileUPVectorStatus, s_CheckTileDownVectorStatus, s_AutomaticTilePlaceCallback, s_SetCurrentPiece, ActiveSinglePlayerInputs, s_RefreshHoldTilePlacement, DecrementScoreCallback);
    }

    private void PlaceTilePiece_Gamepads(InputAction.CallbackContext context)
    {
        Action placeTile = () =>
        {
            m_MovePiece.Disable();
            m_LeftShoulderAction.Disable();
            m_PlacePiece.Disable();
            m_PlacePiece_Gamepad.Disable();
            m_StorePiece.Disable();
            PlaceTilePiecesinGame(m_MainPlayGridArea, s_PlaceBomb, s_PlaceTilePiece, s_CheckTileLeftVectorStatus, s_CheckTileRightVectorStatus, s_CheckTileUPVectorStatus, s_CheckTileDownVectorStatus, s_AutomaticTilePlaceCallback, s_SetCurrentPiece, ActiveSinglePlayerInputs, s_RefreshHoldTilePlacement, DecrementScoreCallback);
        };

        if (GamepadManager.Instance.m_Gamepad1 != null)
        {
            if (GamepadManager.Instance.m_Gamepad1.aButton.IsActuated(0.1f))
            {
                placeTile?.Invoke();
            }
        }

    }
    private void MoveTilePieces(InputAction.CallbackContext context)
    {
        Vector2 movementVector = context.ReadValue<Vector2>();
        movementVector = ApplyApproximationonMovementVector(movementVector);
        TriggerTileMovement(m_MainPlayGridArea, s_ActiveBomb, s_DeActiveTilePiece, s_ActiveTilePiece, movementVector);
    }
    private void StoreTilePieces(InputAction.CallbackContext context)
    {
        /*bool activePieceStaus = false;
        Vector2 activePosition = Vector2.zero;
        TilePiece holdedTile = null;
        s_StorePieceTriggered?.Invoke((bool status, Vector2 currenPosition) =>
        {
            activePieceStaus = status;
            activePosition = currenPosition;
        });
        if (activePieceStaus)
        {
            DisableInputs();
            s_ResetCurrentPosition?.Invoke(activePosition);
            s_ActiveUpcomingInputs?.Invoke();
        }
*/
    }
    private void QuitGame(InputAction.CallbackContext context)
    {
        // OnLeavePanelOpens();
    }

    private void SetUpcomingTile(Action<TilePiece> tileAction, Vector2 nextPosition)
    {
        if (!m_IsGameOver)
            PlaceUpcomingTile(m_MainPlayGridArea, m_SideTilePanelArea, m_UpcomingTileParent, tileAction, PieceTypesContainer, s_SetUpcomingBombPositon, s_SetGridTilePosition, DestroyUpcomingPiece, nextPosition, s_NextTileValidPosition);
    }
    private void SetUpcomingTileVirtually(Action<TilePiece> tileAction, Vector2 nextPosition)
    {
        if (!m_IsGameOver)
            PlaceUpcomingTileVirtually(m_MainPlayGridArea, m_SideTilePanelArea, m_UpcomingTileParent, tileAction, PieceTypesContainer, s_SetUpcomingBombPositon, s_SetGridTilePosition, DestroyUpcomingPiece, nextPosition, s_NextTileValidPosition);
    }
    private void DestroyUpcomingPiece()
    {
        Destroy(m_SideTilePanelArea.transform.GetChild(0).GetComponent<TilePiece>().gameObject);
        StartCoroutine(ActiveInputsAfterdelay());
    }

    private void HandleTilePanelGeneration()
    {
        PieceTypesContainer = new SidePieceTye[Constants.TOTAL_SIDEPANELPIECES];
        PiecePercentages();
        AllocateSidePieceContainer(PieceTypesContainer);
        ShuffleSidePieceContainer(PieceTypesContainer);
        RemoveConsecutivePieceFromContainer(PieceTypesContainer);
        FillSidePieceContainer(m_SideTilePanelArea, PieceTypesContainer);
        StartCoroutine(SetDelayinInitialUpcomigTile());
    }

    private void HandleTileGeneration()
    {
        float tileSizeX = CalculateTileSizeX(Constants.SINGLEPLAYERGRIDSIZE, GameData.Instance.GameOptions.SinglePlayerGameRows);
        float tileSizeY = CalculateTileSizeY(Constants.SINGLEPLAYERGRIDSIZE, GameData.Instance.GameOptions.SinglePlayerGameColumns);
        float minSize = (tileSizeX < tileSizeY) ? tileSizeX : tileSizeY;
        AllocateSizeToGrid(m_MainPlayGridArea, minSize, m_BGImage);
        DestroyTileArea(m_MainPlayGridArea);
        GenerateTileAreaTest(m_MainPlayGridArea, m_Tile);
    }

    private void CheckLoopStatus(GridTile startingPiecePosition)
    {

        Debug.Log("Continious loop complete check");
        HandleInputDuringFlow();
        Action<GridTile, Vector2, Vector2> startingPieceStatusCheck = null;
        startingPieceStatusCheck = (GridTile startingPiece, Vector2 nextPiecePosition, Vector2 flowVector) =>
        {
            s_MidCounterLoopStatus?.Invoke(startingPiece, nextPiecePosition, flowVector, startingPieceStatusCheck);
        };
        s_MidCounterLoopStatus?.Invoke(startingPiecePosition, startingPiecePosition.GridPosition + startingPiecePosition.StartingPieceFlowVector, startingPiecePosition.StartingPieceFlowVector, startingPieceStatusCheck);
    }


    private void StartingPieceTimerCallback(GridTile startingPiecePosition)
    {

        Action gameOverAction = () =>
        {
            LoopLastPieceCallback(startingPiecePosition, Constants.INT_ONE);
        };
        Action<GridTile, Vector2, Vector2> startingPieceStatusCheck = null;
        startingPieceStatusCheck = (GridTile startingPiece, Vector2 nextPiecePosition, Vector2 flowVector) =>
        {
            s_CheckStartingPieceLoopStatus?.Invoke(startingPiece, nextPiecePosition, flowVector, startingPieceStatusCheck);
        };
        s_CheckStartingPieceLoopStatus?.Invoke(startingPiecePosition, startingPiecePosition.GridPosition + startingPiecePosition.StartingPieceFlowVector, startingPiecePosition.StartingPieceFlowVector, startingPieceStatusCheck);
        Action continuousFlowAction = () =>
        {
            float flowSpeed = GameData.Instance.GameOptions.FlowSpeed + (m_DiffficultyFlowTimerFlag * GameData.Instance.GameOptions.DifficultFlowSpeed);
            Action<Vector2, Vector2> nextAnimatedTile = null;
            nextAnimatedTile = (Vector2 previousTilePosition, Vector2 nextTilePosition) =>
            {
                s_HandlContinuousFlowAnimation?.Invoke(previousTilePosition, nextTilePosition, nextAnimatedTile, LoopLastPieceCallback, IncrementScoreCallback, startingPiecePosition, s_ResetStartingPiecePositions, CheckLoopStatus, flowSpeed, OnLoopFlowStarts);
            };
            s_HandlContinuousFlowAnimation?.Invoke(startingPiecePosition.GridPosition, startingPiecePosition.StartingPieceLoopPool[Constants.INT_ZERO].GridPosition, nextAnimatedTile, LoopLastPieceCallback, IncrementScoreCallback, startingPiecePosition, s_ResetStartingPiecePositions, CheckLoopStatus, flowSpeed, OnLoopFlowStarts);
        };
        if (startingPiecePosition.StartingPieceLoopPool.Count > Constants.ZERO && startingPiecePosition.transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
            continuousFlowAction?.Invoke();
        else
            gameOverAction?.Invoke();
    }

    private void IncrementScoreCallback(int score, bool isStartingPiece)
    {
        StartCoroutine(IncreaseScoreDuringFlow(score));
        if (isStartingPiece)
        {
            StartCoroutine(CalculateDetachedPieces(false));
            StartCoroutine(DecreaseScoreDuringFlow(m_DetachPiecesCount * GameData.Instance.GameOptions.UnusedTilePenalty + (int)m_TotalPenalty));
            SpawnNewStartingPiece(true);
        }
    }

    private void DecrementScoreCallback(int decrementedScore)
    {
        StartCoroutine(DecreaseScoreDuringFlow(decrementedScore));

    }

    private void LoopLastPieceCallback(GridTile lastTile, float time)
    {

        Vector2 targetVector = GetLastPieceFlowVector(lastTile);
        AssignBrokenPieceeGrid(lastTile, 0f, targetVector);
        StartCoroutine(GameOverCallback(time));
    }

    private void HandleObstacleSpawn()
    {
        CreateObstacleIgnoredArea(m_MainPlayGridArea, m_ObstacleIgnoredArea);
        CreateObstacleSpawnArea(m_MainPlayGridArea, m_ObstacleIgnoredArea, m_ObstacleSpawnArea);
        ShuffleObstacleArea(m_ObstacleSpawnArea);
        SpawnRoundObstacles(m_ObstacleSpawnArea, s_OnRoundObstacleSpawn, GameManager.instace.RoundObstacleCount_SinglePlayer);
        SpawnHorizonatalObstacles(m_ObstacleSpawnArea, s_OnHorizontalObstacleSpawn);
        SpawnVerticalObstacles(m_ObstacleSpawnArea, s_OnVerticalObstacleSpawn);
    }

    private void AssignStartingPieceGrid()
    {
        m_StartingPieceArea.GetComponent<RectTransform>().sizeDelta = m_MainPlayGridArea.GetComponent<RectTransform>().sizeDelta;//new skchanges
        for (int tileIndex = (int)Constants.ZERO; tileIndex < m_MainPlayGridArea.transform.childCount; tileIndex++)
        {
            GridTile gridTile = m_MainPlayGridArea.transform.GetChild(tileIndex).GetComponent<GridTile>();
            GridTile startingTile = (gridTile.IsStartingPiece && !gridTile.IsStartingPieceCounted) ? Instantiate(gridTile, m_StartingPieceArea.transform) as GridTile : gridTile;
        }
    }

    private void AssignBrokenPieceeGrid(GridTile tile, float time, Vector2 targetVector)
    {
        m_BrokenPieceArea.GetComponent<RectTransform>().sizeDelta = m_MainPlayGridArea.GetComponent<RectTransform>().sizeDelta;//new skchanges
        if (tile != null)
        {
            GridTile brokenPieceTile = Instantiate(tile, m_BrokenPieceArea.transform) as GridTile;
            s_TriggerBrokenAnimation?.Invoke(tile.GridPosition, time, targetVector);
        }

    }


    private void ClearStartingPieceArea()
    {
        for (int tileIndex = (int)Constants.ZERO; tileIndex < m_StartingPieceArea.transform.childCount; tileIndex++)
        {
            Destroy(m_StartingPieceArea.transform.GetChild(tileIndex).gameObject);
        }
    }
    private void ClearBrokenPiecePieceArea()
    {
        for (int tileIndex = (int)Constants.ZERO; tileIndex < m_BrokenPieceArea.transform.childCount; tileIndex++)
        {
            Destroy(m_BrokenPieceArea.transform.GetChild(tileIndex).gameObject);
        }
    }
    private void CalculateFinalScore(GridTile startingPiece)
    {
        InvokeScoreManager(startingPiece.PieceLoop);
    }

    private void InvokeScoreManager(List<GridTile> loopPieces)
    {
        int crosspieces = 0;
        int loopLength = 0;
        int startingPiecesCount = 0;
        Vector2 startingPiecePosition = Vector2.zero;
        List<GridTile> startingPieceList = new List<GridTile>();
        if (loopPieces.Count > Constants.ZERO)
        {
            foreach (GridTile piece in loopPieces)
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
        }
        int finalScore = (loopLength * GameData.Instance.GameOptions.LengthPoints) + (crosspieces * GameData.Instance.GameOptions.CrossPoints) + (loopLength * startingPiecesCount * GameData.Instance.GameOptions.ClosedLoopPoints);
        Debug.Log("The Final Score is: " + finalScore);
        StartPipeFlow(loopPieces);
        StartCoroutine(IncreaseScore(finalScore));
    }

    private void HandleInputDuringFlow()
    {
        if (!m_FlowStatus)
        {
            m_PlacePiece.Enable();
            m_PlacePiece_Gamepad.Enable();
            m_MovePiece.Enable();
            m_LeftShoulderAction.Enable();
            m_StorePiece.Enable();
            m_FlowStatus = true;
        }
    }

    #endregion

    #region Coroutines
    private IEnumerator CalculateDetachedPieces(bool isLoopBreaked)
    {
        m_TotalPenalty = 0;
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        /*skchnagesforeach (Transform penaltyTiles in m_MainPlayGridArea.transform)
        {
            GridTile tile = penaltyTiles.GetComponent<GridTile>();
            m_TotalPenalty += tile.PenaltyValue;
        }*/

        Action<int> detachPieceCallback = (int detachPieces) =>
        {
            m_DetachPiecesCount++;
        };

        s_CalculateDetachPieces?.Invoke(detachPieceCallback, isLoopBreaked);
    }

    private void StartPipeFlow(List<GridTile> loopPieces)
    {
        if (loopPieces.Count > Constants.ZERO)
        {
            float speed = 0.5f;
            Vector2 targetVector = Vector2.zero;
            //skchnages  speed = GameData.Instance.GameOptions.FlowSpeed;
            speed = Constants.MAX_FLOWSPEED;
            float time = 1f / speed;
            float totalTimer = time * loopPieces.Count;
            int gridIndex = loopPieces.FindIndex(element => element.IsStartingPiece);
            if (gridIndex != Constants.NULLINDEX)
            {
                GridTile tile = loopPieces[gridIndex];
                loopPieces.RemoveAt(gridIndex);
            }
            bool brokernPieceStatus = CheckBrokenPieceStatus(loopPieces);
            if (brokernPieceStatus)
            {
                targetVector = GetLastPieceFlowVector(loopPieces[loopPieces.Count - 1]);
            }
            StartCoroutine(StartTileFlow(loopPieces, brokernPieceStatus, targetVector));
        }
    }

    private bool CheckBrokenPieceStatus(List<GridTile> loopPieces)
    {

        bool brokenPieceStatus = true;
        int gridIndex = loopPieces.FindIndex(element => element.IsStartingPiece);
        if (gridIndex != Constants.NULLINDEX)
        {
            brokenPieceStatus = false;
        }
        return brokenPieceStatus;
    }

    private Vector2 GetLastPieceFlowVector(GridTile lastTile)
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

    private void UpdateFlowTimer(float totalTimer)
    {
        m_FlowAnimationTimer = totalTimer;
        if (totalTimer > m_MaxGameTimer)
        {
            StopCoroutine(m_GameOverCoroutine);
            m_GameOverCoroutine = StartCoroutine(GameOver(totalTimer, true));
        }
    }
    private IEnumerator Dummy()
    {
        yield return new WaitForSeconds(Constants.SCOREDELAY);
    }

    private IEnumerator StartTileFlow(List<GridTile> loopPieces, bool brokenLoop, Vector2 targetVector)
    {
        m_FlowStatus = false;
        m_GameCheckingStatus = false;
        float speed = 0.5f;
        //skchnages speed = GameData.Instance.GameOptions.FlowSpeed;
        speed = Constants.MAX_FLOWSPEED;
        float time = 1f / speed;
        float totalTimer = time * loopPieces.Count;
        if (brokenLoop && loopPieces.Count > Constants.ZERO)
        {
            AssignBrokenPieceeGrid(loopPieces[loopPieces.Count - Constants.INT_ONE], totalTimer, targetVector);
        }
        UpdateFlowTimer(totalTimer);
        StartCoroutine(VibrateGamepad());
        for (int loopPieceIndex = Constants.INT_ZERO; loopPieceIndex < loopPieces.Count; loopPieceIndex++)
        {
            Debug.Log("Flow Count");
            s_StartFlowAnimation?.Invoke(loopPieces[loopPieceIndex].GridPosition, time, s_ResetStartingPiecePositions, brokenLoop);
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator IncreaseScoreDuringFlow(int increasedScore)
    {
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore;
        float finalScore = m_CurrentScore + increasedScore;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText.text = m_CurrentScore.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore = (int)finalScore;
        m_ScoreText.text = m_CurrentScore.ToString();
    }


    private IEnumerator IncreaseScore(int increasedScore)
    {
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore;
        float finalScore = m_CurrentScore + increasedScore + m_TotalPenalty;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText.text = m_CurrentScore.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore = (int)finalScore;
        m_ScoreText.text = m_CurrentScore.ToString();
        StartCoroutine(DecreaseScore(m_DetachPiecesCount * GameData.Instance.GameOptions.UnusedTilePenalty));
    }

    private IEnumerator DecreaseScoreDuringFlow(int decreasedScore)
    {
        // yield return new WaitForSeconds(0.3f);
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore;
        float finalScore = m_CurrentScore + decreasedScore;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText.text = m_CurrentScore.ToString();
            timeElapsed += Constants.DECREMENT_SCORESPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore = (int)finalScore;
        m_ScoreText.text = m_CurrentScore.ToString();
    }

    private IEnumerator DecreaseScore(int decreasedScore)
    {
        float timeElapsed = 0;
        float lerpDuration = 1;
        float initialScore = m_CurrentScore;
        float finalScore = m_CurrentScore + decreasedScore;
        while (timeElapsed < lerpDuration)
        {
            m_CurrentScore = (int)Mathf.Lerp(initialScore, finalScore, timeElapsed / lerpDuration);
            m_ScoreText.text = m_CurrentScore.ToString();
            timeElapsed += Constants.SCORE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CurrentScore = (int)finalScore;
        m_ScoreText.text = m_CurrentScore.ToString();
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1f);
        s_OnStartingPieceCounterChanges?.Invoke(StartingPieceTimerCallback, CheckLoopStatus, SpawnNewStartingPiece, m_StartingPieceRemainingTime, FlowAnimationCallback);
    }

    private void FlowAnimationCallback(GridTile startingPiece)
    {
        OnLoopFlowStarts();
        StartCoroutine(CalculateScore(startingPiece, false));
    }
    private void SpawnNewStartingPiece(bool isContiniousFlow = false)
    {

        List<GridTile> loopPieces = new List<GridTile>();
        bool horizontalStatus = false;
        bool verticalStatus = false;
        Action normalFlowAction = () =>
        {
            // ActiveSinglePlayerInputs();s
            Debug.Log("The normal flow is triggered");
            for (int startingPieceIndex = Constants.INT_ZERO; startingPieceIndex < m_MainPlayGridArea.transform.childCount; startingPieceIndex++)
            {
                GridTile currenTile = m_MainPlayGridArea.transform.GetChild(startingPieceIndex).GetComponent<GridTile>();
                if (currenTile.IsStartingPiece)
                    loopPieces.AddRange(currenTile.PieceLoop);
            }
            foreach (GridTile loopPiece in loopPieces)
            {
                s_LockLoopPiece?.Invoke(loopPiece.GridPosition);
            }
        };
        Action ContiniousFlowAction = () =>
        {
            Debug.Log("The Continious flow is triggered");
            for (int startingPieceIndex = Constants.INT_ZERO; startingPieceIndex < m_MainPlayGridArea.transform.childCount; startingPieceIndex++)
            {
                GridTile currenTile = m_MainPlayGridArea.transform.GetChild(startingPieceIndex).GetComponent<GridTile>();
                if (currenTile.IsStartingPiece)
                    loopPieces.AddRange(currenTile.ContiniousFlowPieces);
            }
        };
        if (!isContiniousFlow)
            normalFlowAction?.Invoke();
        else
            ContiniousFlowAction?.Invoke();

        m_StartingPieceSpawnSpots.Clear();
        m_StartingPieceHorizontalSpawnSpots.Clear();
        m_StartingPieceVerticalSpawnSpots.Clear();
        /*  s_SpawnNextStartingPiece?.Invoke(s_CheckSafeAreaStatus, m_StartingPieceSpawnSpots);
          if (m_StartingPieceSpawnSpots.Count <= Constants.ZERO)
          {
              s_SpawnStartingPiece_Horizontal?.Invoke(s_CheckStartingPieceHorizontalSafeAreaStatus, loopPieces, m_StartingPieceHorizontalSpawnSpots);
              if (m_StartingPieceHorizontalSpawnSpots.Count <= Constants.ZERO)
              {
                  s_SpawnStartingPiece_Vertical?.Invoke(s_CheckStartingPieceVerticalSafeAreaStatus, loopPieces, m_StartingPieceVerticalSpawnSpots);
              }
          }*/
        float time = (Constants.INT_ONE / Constants.MAX_FLOWSPEED) * loopPieces.Count;
        if (isContiniousFlow)
            time = Constants.INT_ZERO;
        Invoke("SpawnNewStartingPiecewithDelay", time + 1.5f);
        /*changes 04-07-22Action<Vector2> SpawnStartingPiece = (Vector2 RandomPosition) =>
        {
            HandleInputDuringFlow();
            m_StartingPieceBaseTimer -= GameData.Instance.GameOptions.StartingPieceTimeDifference;
            if (m_StartingPieceBaseTimer <= Constants.ZERO)
            {
                StartCoroutine(GameOverCallback());
                return;
            }
            s_SpawnStartingPiece?.Invoke(RandomPosition, (int)m_StartingPieceBaseTimer, horizontalStatus, verticalStatus);
            StartCoroutine(AssignStartingPieceToDummyGrid());
            StartCoroutine(StartTimer());
            StopCoroutine(m_GameOverCoroutine);
            m_GameOverCoroutine = StartCoroutine(GameOver());
        };
        if (m_StartingPieceSpawnSpots.Count > Constants.ZERO)
        {
            Vector2 SpawnPosition = m_StartingPieceSpawnSpots[Constants.INT_ZERO];
            SpawnStartingPiece?.Invoke(SpawnPosition);
        }
        else if (m_StartingPieceHorizontalSpawnSpots.Count > Constants.ZERO)
        {
            Vector2 SpawnPosition = m_StartingPieceHorizontalSpawnSpots[Constants.INT_ZERO];
            horizontalStatus = true;
            SpawnStartingPiece?.Invoke(SpawnPosition);
        }
        else if (m_StartingPieceVerticalSpawnSpots.Count > Constants.ZERO)
        {
            Vector2 SpawnPosition = m_StartingPieceVerticalSpawnSpots[Constants.INT_ZERO];
            verticalStatus = true;
            SpawnStartingPiece?.Invoke(SpawnPosition);

        }
        else
        {
            return;
        }*/

    }

    private void SpawnNewStartingPiecewithDelay()
    {
        List<GridTile> loopPieces = new List<GridTile>();
        s_SpawnNextStartingPiece?.Invoke(s_CheckSafeAreaStatus, m_StartingPieceSpawnSpots);
        if (m_StartingPieceSpawnSpots.Count <= Constants.ZERO)
        {
            s_SpawnStartingPiece_Horizontal?.Invoke(s_CheckStartingPieceHorizontalSafeAreaStatus, loopPieces, m_StartingPieceHorizontalSpawnSpots);
            if (m_StartingPieceHorizontalSpawnSpots.Count <= Constants.ZERO)
            {
                s_SpawnStartingPiece_Vertical?.Invoke(s_CheckStartingPieceVerticalSafeAreaStatus, loopPieces, m_StartingPieceVerticalSpawnSpots);
            }
        }

        bool horizontalStatus = false;
        bool verticalStatus = false;
        Action<Vector2> SpawnStartingPiece = (Vector2 RandomPosition) =>
        {
            HandleInputDuringFlow();
            m_StartingPieceBaseTimer -= (GameData.Instance.GameOptions.StartingPieceTimeDifference + m_IncreaseTimeDifference);
            m_IncreaseTimeDifference = Constants.INT_ZERO;
            m_StartingPieceBaseTimer = (m_StartingPieceBaseTimer <= Constants.ZERO) ? GameData.Instance.GameOptions.MinimumStartingTimer : m_StartingPieceBaseTimer;
            m_DiffficultyFlowTimerFlag = (m_StartingPieceBaseTimer > GameData.Instance.GameOptions.MinimumStartingTimer) ? m_DiffficultyFlowTimerFlag + Constants.INT_ONE : m_DiffficultyFlowTimerFlag;
            /*if (m_StartingPieceBaseTimer <= Constants.ZERO)
            {
                m_StartingPieceBaseTimer = GameData.Instance.GameOptions.MinimumStartingTimer;
                StartCoroutine(GameOverCallback());
                return;
            }*/
            s_SpawnStartingPiece?.Invoke(RandomPosition, (int)m_StartingPieceBaseTimer, horizontalStatus, verticalStatus);
            StartCoroutine(AssignStartingPieceToDummyGrid());
            StartCoroutine(StartTimer());
            StopCoroutine(m_GameOverCoroutine);
            m_GameOverCoroutine = StartCoroutine(GameOver());
        };
        if (m_StartingPieceSpawnSpots.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceSpawnSpots);
            Vector2 SpawnPosition = m_StartingPieceSpawnSpots[Constants.INT_ZERO];
            SpawnStartingPiece?.Invoke(SpawnPosition);
        }
        else if (m_StartingPieceHorizontalSpawnSpots.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceHorizontalSpawnSpots);
            Vector2 SpawnPosition = m_StartingPieceHorizontalSpawnSpots[Constants.INT_ZERO];
            horizontalStatus = true;
            SpawnStartingPiece?.Invoke(SpawnPosition);
        }
        else if (m_StartingPieceVerticalSpawnSpots.Count > Constants.ZERO)
        {
            ShuffleStartingPiecePositions(m_StartingPieceVerticalSpawnSpots);
            Vector2 SpawnPosition = m_StartingPieceVerticalSpawnSpots[Constants.INT_ZERO];
            verticalStatus = true;
            SpawnStartingPiece?.Invoke(SpawnPosition);

        }
        else
        {
            return;
        }
        OnLoopEnds();
        Debug.Log("Loop Completed ");

    }
    private IEnumerator AssignStartingPieceToDummyGrid()
    {
        yield return new WaitForSeconds(0.01f);
        AssignStartingPieceGrid();
    }
    private IEnumerator GenerateStartingPieces()
    {
        s_SpawnNextStartingPiece?.Invoke(s_CheckSafeAreaStatus, m_StartingPieceSpawnSpots);
        yield return new WaitForSeconds(0.01f);
        // ShuffleStartingPiecePositions(GameManager.instace.StartingPiecePoolSinglePlayer);
        ShuffleStartingPiecePositions(m_StartingPieceSpawnSpots);
        // PlaceStarttingPiece(m_MainPlayGridArea, GameManager.instace.StartingPieceCount, GameManager.instace.StartingPiecePoolSinglePlayer, s_SetStartingPositon, Constants.INT_ZERO);
        PlaceStarttingPiece(m_MainPlayGridArea, GameManager.instace.StartingPieceCount, m_StartingPieceSpawnSpots, s_SetStartingPositon, Constants.INT_ZERO);
        AssignStartingPieceGrid();
        yield return new WaitForSeconds(0.21f);
        m_StartingPieceSpawnSpots.Clear();
        HandleObstacleSpawn();
        yield return StartCoroutine(StartTimer());
    }

    private IEnumerator CalculateScore(GridTile startingPiece, bool isLoopBreaked = true)
    {
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        m_DetachPieceCoroutine = StartCoroutine(CalculateDetachedPieces(isLoopBreaked));
        CalculateFinalScore(startingPiece);
    }

    IEnumerator GameOver(float flowTimer = 0f, bool loopFlowDelay = false)
    {
        m_MaxGameTimer = (int)flowTimer;
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        if (!loopFlowDelay)
        {
            for (int tile1Index = (int)Constants.ZERO; tile1Index < m_MainPlayGridArea.transform.childCount; tile1Index++)
            {
                Action<GridTile> startingPieceAction = (GridTile tile) =>
                {
                    m_MaxGameTimer = tile.StartingPieceCounter;
                    m_GameClock = m_MaxGameTimer;
                    if (m_UpdateClockCoroutine != null)
                    {
                        StopCoroutine(m_UpdateClockCoroutine);
                    }
                    m_UpdateClockCoroutine = StartCoroutine(UpdateGameClock());
                    s_StartingPieceCounted?.Invoke(tile.GridPosition);
                };
                GridTile tile = m_MainPlayGridArea.transform.GetChild(tile1Index).GetComponent<GridTile>();
                if (tile.IsStartingPiece && !tile.IsStartingPieceCounted)
                    startingPieceAction?.Invoke(tile);

            }
        }
        while (m_MaxGameTimer > Constants.ZERO)
        {
            m_MaxGameTimer--;
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1.5f);
        /*recent changes 16-06-22
                //skchnagesDisableInputs();
                m_Loader.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                //yield return new WaitForSeconds(Constants.GAMEOVERSCOREDELAY);
                m_Loader.SetActive(false);
                //m_Loader.SetActive(true);
                yield return new WaitForSeconds(Constants.SCOREDELAY);
                //skchnagess_OnGameOvers?.Invoke();
                m_IsGameOver = true;
                s_OnGameManagerCallbacks?.Invoke(m_PlayerName, m_CurrentScore);*/
    }

    IEnumerator StartCheckinggameStatus()
    {
        yield return new WaitForSeconds(1f);
        m_GameCheckingStatus = true;
    }

    /*IEnumerator GameOver(float flowTimer = 0f, bool loopFlowDelay = false)
    {
        m_MaxGameTimer = (int)flowTimer;
        Debug.Log("MaxGameTimer is: " + flowTimer);
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        if (!loopFlowDelay)
        {
            for (int tile1Index = (int)Constants.ZERO; tile1Index < m_MainPlayGridArea.transform.childCount; tile1Index++)
            {
                Action<GridTile> startingPieceAction = (GridTile tile) =>
                {
                    Debug.Log("Testing Tile Status");
                    m_MaxGameTimer = tile.StartingPieceCounter;
                    // m_MaxGameTimer = (m_MaxGameTimer < tile.StartingPieceCounter) ? tile.StartingPieceCounter + difference : m_MaxGameTimer;
                    s_StartingPieceCounted?.Invoke(tile.GridPosition);
                };
                GridTile tile = m_MainPlayGridArea.transform.GetChild(tile1Index).GetComponent<GridTile>();
                if (tile.IsStartingPiece && !tile.IsStartingPieceCounted)
                    startingPieceAction?.Invoke(tile);

            }
        }
        //skchnagesyield return new WaitForSeconds(m_MaxGameTimer);
        yield return new WaitForSeconds(m_MaxGameTimer + 6f);
        // DisableInputs();
        m_Loader.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitForSeconds(Constants.GAMEOVERSCOREDELAY);
        m_Loader.SetActive(false);
        //m_Loader.SetActive(true);
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        s_OnGameOvers?.Invoke();
        m_IsGameOver = true;
        s_OnGameManagerCallbacks?.Invoke(m_PlayerName, m_CurrentScore);
    }*/

    IEnumerator FlowAnimationCompletedTrigger(float timer)
    {

        m_FlowAnimationTimer = timer;
        yield return new WaitForSeconds(m_FlowAnimationTimer);
        DisableInputs();
        yield return new WaitForSeconds(6f);
        s_OnGameOvers?.Invoke(true);
        m_IsGameOver = true;
        Debug.Log("GameOver Triggered successfully");
        m_Loader.SetActive(true);
        yield return new WaitForSeconds(Constants.GAMEOVERSCOREDELAY);
        m_Loader.SetActive(false);
        s_OnGameManagerCallbacks?.Invoke(m_PlayerName, m_CurrentScore);
    }

    IEnumerator SetDelayinInitialUpcomigTile()
    {
        yield return new WaitForSeconds(Constants.INPUT_DELAY);
        SetInitialUpcomingTile(m_UpcomingTile, m_UpcomingTileParent, m_SideTilePanelArea, PieceTypesContainer);
    }

    //  IEnumerator GameOver()

    IEnumerator VibrateGamepad()
    {
        GamepadManager.Instance.m_Gamepad1.SetMotorSpeeds(0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        GamepadManager.Instance.m_Gamepad1.SetMotorSpeeds(0f, 0f);
    }
    IEnumerator GameOverDelay()
    {

        // DisableInputs();
        m_Loader.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitForSeconds(Constants.GAMEOVERSCOREDELAY);
        m_Loader.SetActive(false);
        //m_Loader.SetActive(true);
        yield return new WaitForSeconds(Constants.SCOREDELAY);
        s_OnGameOvers?.Invoke(true);
        m_IsGameOver = true;
        s_OnGameManagerCallbacks?.Invoke(m_PlayerName, m_CurrentScore);
    }
    IEnumerator GameOverCallback(float time = 0)
    {
        DisableInputs();
        s_OnGameOvers?.Invoke(true);
        m_IsGameOver = true;
        StopCoroutine(m_AuxillaryTimerCoroutine);
        Debug.Log("GameOver Callback SucccessFull");
        yield return new WaitForSeconds(time + Constants.INT_ONE);
        m_Loader.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_Loader.SetActive(false);
        s_OnGameManagerCallbacks?.Invoke(m_PlayerName, m_CurrentScore);
    }

    IEnumerator UpdateGameClock()
    {
        s_OnGameOvers?.Invoke(false);
        while (m_GameClock > Constants.ZERO)
        {
            m_GameClock--;
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        yield return new WaitForEndOfFrame();
        //skchnagesDisableInputs();
        //recentchanges 16-06-22 s_OnGameOvers?.Invoke(true);

    }
    IEnumerator StartFlashingStartingPiece()
    {
        yield return new WaitForSeconds(0.1f);
        GridTile startingPieceTile = null;
        for (int tileIndex = Constants.INT_ZERO; tileIndex < m_MainPlayGridArea.transform.childCount; tileIndex++)
        {
            GridTile tile = m_MainPlayGridArea.transform.GetChild(tileIndex).GetComponent<GridTile>();
            if (tile.IsStartingPiece)
            {
                startingPieceTile = tile;
            }
        }
        if (startingPieceTile != null)
        {
            s_FlashStartingPiece?.Invoke(startingPieceTile.GridPosition, true);
        }
        yield return new WaitForSeconds(Constants.FLASH_INTERVAL);
        s_FlashStartingPiece?.Invoke(startingPieceTile.GridPosition, false);

    }

    IEnumerator ShowTutorialPanel()
    {
        yield return new WaitForSeconds(0.8f);
        m_TutorialPanel.gameObject.SetActive(true);
    }

    IEnumerator StartAuxillaryTimer()
    {
        Action colorAction = () =>
        {
            if (m_CurrentAuxillaryTimer < Constants.AUXILLARYTIMER_COUNTDOWN)
                m_DifficultyTimerText.color = Color.red;
            else
                m_DifficultyTimerText.color = Color.white;
        };
       // yield return new WaitForSeconds(1f);
        m_CurrentAuxillaryTimer = GameData.Instance.GameOptions.AuxillaryTimer;
        m_DifficultyTimerText.text = m_CurrentAuxillaryTimer.ToString();
        while (m_CurrentAuxillaryTimer > Constants.INT_ZERO)
        {
            m_CurrentAuxillaryTimer--;
            colorAction?.Invoke();
            m_DifficultyTimerText.text = m_CurrentAuxillaryTimer.ToString();
            yield return new WaitForSeconds(Constants.INT_ONE);
        }
        yield return new WaitForEndOfFrame();
        Debug.Log("Auxillary Time Ended");
        m_IncreaseTimeDifference += (int)GameData.Instance.GameOptions.DifficultyStartingPieceTimeReduction;
        m_AuxillaryTimerCoroutine = StartCoroutine(StartAuxillaryTimer());
    }

    #endregion

}
