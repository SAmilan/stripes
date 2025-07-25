using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using TMPro;

public class GridTile : TilePiece
{
    #region Private_Variables
    private SinglePlayerInputs m_SinglePlayerInputs;
    private InputAction m_RotatePieceClockwise;
    private InputAction m_RotatePieceAntiClockwise;
    [SerializeField]
    List<double> m_TileCode = new List<double>();
    [SerializeField]
    protected int p_StartingPieceCounter = 0;
    [SerializeField]
    protected TextMeshProUGUI p_StartingPieceCounterText;
    private int m_StartingPieceIndex;
    [SerializeField]
    private Vector2 m_StartingPieceFlowVector;
    [SerializeField]
    private List<GridTile> m_AttachedPiece = new List<GridTile>();
    [SerializeField]
    private List<GridTile> m_StartingPieceLoopPool = new List<GridTile>();
    [SerializeField]
    private List<GridTile> m_PieceLoop = new List<GridTile>();
    private bool m_ViewStartingPiece;
    [SerializeField]
    private bool m_IsLoopCompleted;
    private bool m_StartedPieceCounted = false;
    public List<double> TileCode => m_TileCode;
    public List<GridTile> AttachedPiece => m_AttachedPiece;
    private Coroutine m_NextPieceAnimationCoroutine;
    public List<GridTile> ContiniousFlowPieces = new List<GridTile>();

    #endregion
    #region Public_Variables
    public List<GridTile> StartingPieceLoopPool => m_StartingPieceLoopPool;
    public List<GridTile> PieceLoop => m_PieceLoop;
    public Vector2 StartingPieceFlowVector => m_StartingPieceFlowVector;
    public int StartingPieceCounter => p_StartingPieceCounter;
    private Action<Vector2, Action<bool>> m_CheckSafeAreaStatus;
    [SerializeField]
    private GameObject m_Right1BrokenImage;
    [SerializeField]
    private GameObject m_Right2BrokenImage;
    [SerializeField]
    private GameObject m_Left1BrokenImage;
    [SerializeField]
    private GameObject m_Left2BrokenImage;
    [SerializeField]
    private GameObject m_UP1BrokenImage;
    [SerializeField]
    private GameObject m_UP2BrokenImage;
    [SerializeField]
    private GameObject m_Down1BrokenImage;
    [SerializeField]
    private GameObject m_Down2BrokenImage;
    [SerializeField]
    private List<BrokenPieceImage> m_BrokenPieceImages = new List<BrokenPieceImage>();

    public static Action<Vector2> s_ResetTopCopnnectionStatus;
    public static Action<Vector2> s_ResetBottomCopnnectionStatus;
    public static Action<Vector2> s_ResetLeftCopnnectionStatus;
    public static Action<Vector2> s_ResetRightCopnnectionStatus;
    #endregion
    #region Unity_CallBacks
    private void Awake()
    {
        m_SinglePlayerInputs = new SinglePlayerInputs();
    }
    private void OnEnable()
    {
        m_RotatePieceClockwise = m_SinglePlayerInputs.SinglePlayer.RotatePieceClockwise;
        m_RotatePieceAntiClockwise = m_SinglePlayerInputs.SinglePlayer.RotatePieceAntiClockwise;
        m_RotatePieceClockwise.performed += RotatePieceClockwise;
        m_RotatePieceAntiClockwise.performed += RotatePieceAntiClockwise;
        SinglePlayerTutorialPanel.s_TutorialEnds += OnTutorialEnds;
       
        ContiniousFlowPieces.Clear();
        ViewSinglePlayerGame.s_SetGridTilePosition += SetGridPiecePosition;
        ViewSinglePlayerGame.s_SetUpcomingBombPositon += SetUpcomingBombPosition;
        ViewSinglePlayerGame.s_ActiveTilePiece += ActiveTilePiece;
        ViewSinglePlayerGame.s_DeActiveTilePiece += DeActiveTilePiece;
        ViewSinglePlayerGame.s_PlaceTilePiece += PlaceTilePiece;
        ViewSinglePlayerGame.s_ActiveBomb += ActiveBomb;
        ViewSinglePlayerGame.s_PlaceBomb += PlaceBombSinglePlayer;
        ViewSinglePlayerGame.s_CheckTileLeftVectorStatus += CheckTileLeftStatus;
        ViewSinglePlayerGame.s_CheckTileRightVectorStatus += CheckTileRightStatus;
        ViewSinglePlayerGame.s_CheckTileUPVectorStatus += CheckTileUpStatus;
        ViewSinglePlayerGame.s_CheckTileDownVectorStatus += CheckTileDownStatus;
        ViewSinglePlayerGame.s_SetStartingPositon += SetStartingPiecePosition;
        ViewSinglePlayerGame.s_OnStartingPieceCounterChanges += OnStartingPieceCounterChanges;
        ViewSinglePlayerGame.s_OnRoundObstacleSpawn += CreateRoundObstacle;
        ViewSinglePlayerGame.s_OnHorizontalObstacleSpawn += CreateHorizontalObstacle;
        ViewSinglePlayerGame.s_OnVerticalObstacleSpawn += CreateVerticalObstacle;
        ViewSinglePlayerGame.s_CheckStartingPieceLoopStatus += UpdateStartingPieceLoopStatus;
        ViewSinglePlayerGame.s_MidCounterLoopStatus += CheckPieceLoopStatus;
        ViewSinglePlayerGame.s_CalculateDetachPieces += CalculateDetachPieces;
        ViewSinglePlayerGame.s_OnGameOvers += OnGameOvers;
        ViewSinglePlayerGame.s_StartingPieceCounted += StartedPieceCounted;
        ViewSinglePlayerGame.s_SpawnNextStartingPiece += CheckStartingPieceSafeArea;
        ViewSinglePlayerGame.s_SpawnStartingPiece_Horizontal += CheckStartingPieceHorizontalSafeArea;
        ViewSinglePlayerGame.s_SpawnStartingPiece_Vertical += CheckStartingPieceVerticalSafeArea;
        ViewSinglePlayerGame.s_CheckSafeAreaStatus += StartingPieceSafeAreaStatus;
        ViewSinglePlayerGame.s_CheckStartingPieceHorizontalSafeAreaStatus += StartingPieceHorizontalSafeAreaStatus;
        ViewSinglePlayerGame.s_CheckStartingPieceVerticalSafeAreaStatus += StartingPieceVerticalSafeAreaStatus;
        ViewSinglePlayerGame.s_SpawnStartingPiece += SpawnStartingPiece;
        ViewSinglePlayerGame.s_StartFlowAnimation += StartFlowAnimation;
        ViewSinglePlayerGame.s_SetCurrentPiece += SetPieceActive;
        ViewSinglePlayerGame.s_LockLoopPiece += LockLoopPiece;
        ViewSinglePlayerGame.s_NextTileValidPosition += ValidNextPosition;
        ViewSinglePlayerGame.s_ResetStartingPiecePositions += ResetStartingPiecePosition;
        ViewSinglePlayerGame.s_TriggerBrokenAnimation += TriggerBrokenPieceAnimation;
        HoldTile.s_StorePiece += HoldCurrentActivePiece;
        ViewSinglePlayerGame.s_StorePieceTriggered += ResetTilePiece;
        ViewSinglePlayerGame.s_ResetCurrentPosition += ResetCurrentPosition;
        ViewSinglePlayerGame.s_FlashStartingPiece += FlashStartingPiece;
        HoldTile.s_ExtractPiece += ExtractHoldPiece;
        HoldTile.s_CheckActiveTileStatus += CheckTileActiveStatus;
        s_AssignTileCode += AssignTilePieceCode;
        s_DeAttachPieces += DeAttachPiece;
        ViewSinglePlayerGame.s_HandlContinuousFlowAnimation += HandleContinousTileFlow;
        s_ResetTopCopnnectionStatus += ResetTopCopnnectionStatus;
        s_ResetBottomCopnnectionStatus += ResetBottomCopnnectionStatus;
        s_ResetLeftCopnnectionStatus += ResetLeftCopnnectionStatus;
        s_ResetRightCopnnectionStatus += ResetRightCopnnectionStatus;
        StartCoroutine(GetWidthHeight());
    }

    private void OnDisable()
    {
        SinglePlayerTutorialPanel.s_TutorialEnds -= OnTutorialEnds;
        ViewSinglePlayerGame.s_SetGridTilePosition -= SetGridPiecePosition;
        ViewSinglePlayerGame.s_SetUpcomingBombPositon -= SetUpcomingBombPosition;
        ViewSinglePlayerGame.s_ActiveTilePiece -= ActiveTilePiece;
        ViewSinglePlayerGame.s_DeActiveTilePiece -= DeActiveTilePiece;
        ViewSinglePlayerGame.s_PlaceTilePiece -= PlaceTilePiece;
        ViewSinglePlayerGame.s_ActiveBomb -= ActiveBomb;
        ViewSinglePlayerGame.s_PlaceBomb -= PlaceBombSinglePlayer;
        ViewSinglePlayerGame.s_CheckTileLeftVectorStatus -= CheckTileLeftStatus;
        ViewSinglePlayerGame.s_CheckTileRightVectorStatus -= CheckTileRightStatus;
        ViewSinglePlayerGame.s_CheckTileUPVectorStatus -= CheckTileUpStatus;
        ViewSinglePlayerGame.s_CheckTileDownVectorStatus -= CheckTileDownStatus;
        ViewSinglePlayerGame.s_SetStartingPositon -= SetStartingPiecePosition;
        ViewSinglePlayerGame.s_OnStartingPieceCounterChanges -= OnStartingPieceCounterChanges;
        ViewSinglePlayerGame.s_OnRoundObstacleSpawn -= CreateRoundObstacle;
        ViewSinglePlayerGame.s_OnHorizontalObstacleSpawn -= CreateHorizontalObstacle;
        ViewSinglePlayerGame.s_OnVerticalObstacleSpawn -= CreateVerticalObstacle;
        ViewSinglePlayerGame.s_CheckStartingPieceLoopStatus -= UpdateStartingPieceLoopStatus;
        ViewSinglePlayerGame.s_MidCounterLoopStatus -= CheckPieceLoopStatus;
        ViewSinglePlayerGame.s_CalculateDetachPieces -= CalculateDetachPieces;
        ViewSinglePlayerGame.s_OnGameOvers -= OnGameOvers;
        ViewSinglePlayerGame.s_StartingPieceCounted -= StartedPieceCounted;
        ViewSinglePlayerGame.s_SpawnNextStartingPiece -= CheckStartingPieceSafeArea;
        ViewSinglePlayerGame.s_SpawnStartingPiece_Horizontal -= CheckStartingPieceHorizontalSafeArea;
        ViewSinglePlayerGame.s_SpawnStartingPiece_Vertical -= CheckStartingPieceVerticalSafeArea;
        ViewSinglePlayerGame.s_SpawnStartingPiece -= SpawnStartingPiece;
        ViewSinglePlayerGame.s_CheckSafeAreaStatus -= StartingPieceSafeAreaStatus;
        ViewSinglePlayerGame.s_CheckStartingPieceHorizontalSafeAreaStatus -= StartingPieceHorizontalSafeAreaStatus;
        ViewSinglePlayerGame.s_CheckStartingPieceVerticalSafeAreaStatus -= StartingPieceVerticalSafeAreaStatus;
        ViewSinglePlayerGame.s_StartFlowAnimation -= StartFlowAnimation;
        ViewSinglePlayerGame.s_SetCurrentPiece -= SetPieceActive;
        ViewSinglePlayerGame.s_LockLoopPiece -= LockLoopPiece;
        ViewSinglePlayerGame.s_NextTileValidPosition -= ValidNextPosition;
        ViewSinglePlayerGame.s_ResetStartingPiecePositions -= ResetStartingPiecePosition;
        ViewSinglePlayerGame.s_TriggerBrokenAnimation -= TriggerBrokenPieceAnimation;
        HoldTile.s_StorePiece -= HoldCurrentActivePiece;
        ViewSinglePlayerGame.s_StorePieceTriggered -= ResetTilePiece;
        ViewSinglePlayerGame.s_ResetCurrentPosition -= ResetCurrentPosition;
        HoldTile.s_ExtractPiece -= ExtractHoldPiece;
        HoldTile.s_CheckActiveTileStatus -= CheckTileActiveStatus;
        ViewSinglePlayerGame.s_FlashStartingPiece -= FlashStartingPiece;
        ViewSinglePlayerGame.s_HandlContinuousFlowAnimation -= HandleContinousTileFlow;
        s_AssignTileCode -= AssignTilePieceCode;
        s_DeAttachPieces -= DeAttachPiece;
        s_ResetTopCopnnectionStatus -= ResetTopCopnnectionStatus;
        s_ResetBottomCopnnectionStatus -= ResetBottomCopnnectionStatus;
        s_ResetLeftCopnnectionStatus -= ResetLeftCopnnectionStatus;
        s_ResetRightCopnnectionStatus -= ResetRightCopnnectionStatus;
        MakePlayerInputsDeActive();

    }

    #endregion
    #region Private_Methods

    private void OnTutorialEnds()
    {
        MakePlayerInputsActive();
    }


    private void PlaceBombSinglePlayer(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
        {
            PlaceBomb(gridPosition);
            s_ResetTopCopnnectionStatus?.Invoke(gridPosition + Vector2.down);
            s_ResetBottomCopnnectionStatus?.Invoke(gridPosition + Vector2.up);
            s_ResetLeftCopnnectionStatus?.Invoke(gridPosition + Vector2.right);
            s_ResetRightCopnnectionStatus?.Invoke(gridPosition + Vector2.left);
            StartCoroutine(GameManager.instace.ShakeSinglePlayerCamera(0.6f, 4f));
        }
    }
    private void HoldCurrentActivePiece(TilePiece currentHoldTile, Action<TilePiece> activeTilePiece, Action<Vector2> resetHoldTile)
    {
        if (m_isSelected || m_IsBomb)
        {
            TilePiece currentActiveTile = currentHoldTile;
            activeTilePiece?.Invoke(this);
            if (currentHoldTile != null)
            {
                ExtractHoldPiece(SwapTile.Instance);
                return;
            }
            else
            {
                resetHoldTile?.Invoke(this.GridPosition);
            }
        }
    }
    private void TriggerBrokenPieceAnimation(Vector2 targetPiece, float time, Vector2 flowVector)
    {
        if (p_GridPosition == targetPiece)
        {
            StartCoroutine(StartBrokenEndAnimations(time, flowVector));
        }
    }

    private void ResetStartingPiecePosition(Vector2 gridPosition)
    {

        if (p_GridPosition == gridPosition)
        {
            m_isLoopPiece = false;
            m_IsStartingPiece = false;
            m_PieceLoop.Clear();
            m_StartingPieceLoopPool.Clear();
            ContiniousFlowPieces.Clear();
            m_IsStartingPiece = false;
            m_IsObstacle = false;
            m_IsLoopCompleted = false;
            m_StartingPieceImage.gameObject.SetActive(false);
            p_StartingPieceCounterText.gameObject.SetActive(false);
            m_IsStartingPieceCounted = false;
            m_StartedPieceCounted = false;
            PlaceBomb(gridPosition);
            if (this.transform.parent.name == Constants.STARTINGPIECEAREAPARENTSINGLEPLAYER)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void ValidNextPosition(Vector2 gridPosition, Action<GridTile> validTile)
    {
        if (!m_IsObstacle && !m_IsStartingPiece && (p_GridPosition == gridPosition) && !isLoopPiece)
        {
            validTile?.Invoke(this);
        }
    }

    private void StartFlowAnimation(Vector2 GridPosition, float time, Action<Vector2> startingPiecePosition, bool brokernLoopStatus)
    {
        if (p_GridPosition == GridPosition)
        {
            Action normalPieceAction = () =>
            {
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_FlowFillImage.fillClockwise = currentFillImage.Clockwise;
                    if (!brokernLoopStatus)
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);
                    }
                    else
                    {
                        ShowNegativeScore(GameData.Instance.GameOptions.UnusedTilePenalty);
                    }
                    StartCoroutine(StartFillingFlowImage(time));
                }
            };
            Action<Action<Vector2>> startingPieceAction = (Action<Vector2> startingPiecePosition) =>
            {
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    p_StaringPieceFlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_StaringPieceFlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_StaringPieceFlowFillImage.fillClockwise = currentFillImage.Clockwise;
                    if (!brokernLoopStatus)
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.ClosedLoopPoints);
                    }
                    else
                    {
                        ShowNegativeScore(GameData.Instance.GameOptions.UnusedTilePenalty);
                    }
                    StartCoroutine(StartStartingPieceFillingFlowImage(time));
                    StartCoroutine(ExplodeStarttingPiece(this, time, startingPiecePosition));
                }
            };
            Action CrossPieceAction = () =>
            {
                CrossTypeAnimation currentFillImage = null;
                if (!p_IsDualFlow)
                {
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe1Vector);
                }
                else
                {
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe2Vector);
                }

                if (currentFillImage != null)
                {

                    if (!p_IsDualFlow)
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);
                        p_FlowFillImage.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                        StartCoroutine(StartFillingFlowImage(time));
                        p_IsDualFlow = true;
                    }
                    else
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.CrossPoints);

                        p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImageDualFlow.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImageDualFlow.fillOrigin = currentFillImage.fillOrigin;
                        StartCoroutine(StartFillingDualImage(time));
                    }
                }
            };

            if (m_Type != Constants.CROSS_TYPEPIECE)
                normalPieceAction?.Invoke();
            if (m_Type == Constants.CROSS_TYPEPIECE)
                CrossPieceAction?.Invoke();
            if (m_Type == Constants.STARTING_TYPEPIECE)
                startingPieceAction?.Invoke(startingPiecePosition);
        }
    }

    private void HandleContinousTileFlow(Vector2 previousTile, Vector2 currentTile, Action<Vector2, Vector2> nextPieceCallback, Action<GridTile, float> lastPieceCallback, Action<int, bool> incrementScoreCallback, GridTile startingPiece, Action<Vector2> resetStartingPosition, Action<GridTile> loopCompleteCallback, float flowSpeed, Action onflowStarts)
    {
        if (p_GridPosition == currentTile)
        {
            //skchangesfloat speed = GameData.Instance.GameOptions.FlowSpeed;
            float speed = flowSpeed;
            float time = 1f / speed;
            Action normalPieceAction = () =>
            {
                m_StartingPieceFlowVector = currentTile - previousTile;
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    LockLoopPiece(this.p_GridPosition);
                    p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_FlowFillImage.fillClockwise = currentFillImage.Clockwise;
                    startingPiece.ContiniousFlowPieces.Add(this);
                    ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);
                    incrementScoreCallback?.Invoke(GameData.Instance.GameOptions.LengthPoints, false);
                    StartCoroutine(StartFillingContinousFlowForNextPosition(nextPieceCallback, lastPieceCallback, time, startingPiece, loopCompleteCallback, onflowStarts));
                }
            };
            Action crossPieceAction = () =>
            {
                LockLoopPiece(this.p_GridPosition);
                m_StartingPieceFlowVector = currentTile - previousTile;
                CrossTypeAnimation currentFillImage = null;
                if (!p_IsDualFlow)
                {
                    p_CrossPiecePipe1Vector = m_StartingPieceFlowVector;
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe1Vector);
                }
                else
                {
                    p_CrossPiecePipe2Vector = m_StartingPieceFlowVector;
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe2Vector);
                }

                if (currentFillImage != null)
                {

                    if (!p_IsDualFlow)
                    {
                        p_FlowFillImage.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                        ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);
                        startingPiece.ContiniousFlowPieces.Add(this);
                        incrementScoreCallback?.Invoke(GameData.Instance.GameOptions.LengthPoints, false);
                        StartCoroutine(StartFillingContinousFlowForNextPosition(nextPieceCallback, lastPieceCallback, time, startingPiece, loopCompleteCallback, onflowStarts));
                        p_IsDualFlow = true;
                    }
                    else
                    {
                        p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImageDualFlow.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImageDualFlow.fillOrigin = currentFillImage.fillOrigin;
                        ShowPositiveScore(GameData.Instance.GameOptions.CrossPoints);
                        startingPiece.ContiniousFlowPieces.Add(this);
                        incrementScoreCallback?.Invoke(GameData.Instance.GameOptions.CrossPoints, false);
                        StartCoroutine(StartFillingContinousFlowForDualImage(nextPieceCallback, lastPieceCallback, time, startingPiece, loopCompleteCallback, onflowStarts));
                    }
                }
            };

            Action startingPieceAction = () =>
            {
                m_StartingPieceFlowVector = currentTile - previousTile;
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    startingPiece.ContiniousFlowPieces.Add(this);
                    p_StaringPieceFlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_StaringPieceFlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_StaringPieceFlowFillImage.fillClockwise = currentFillImage.Clockwise;
                    //ShowPositiveScore(startingPiece.ContiniousFlowPieces.Count * GameData.Instance.GameOptions.ClosedLoopPoints);
                    ShowPositiveScore(GameData.Instance.GameOptions.ClosedLoopPoints);
                    if (transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
                    {
                        incrementScoreCallback?.Invoke(startingPiece.ContiniousFlowPieces.Count * GameData.Instance.GameOptions.ClosedLoopPoints, true);
                    }
                    StartCoroutine(StartStartingPieceFillingFlowImageContiniousFlow(time, startingPiece));
                    StartCoroutine(ExplodeContiniousFlowPiece(startingPiece, time, resetStartingPosition));
                }
            };
            if (IsStartingPiece)
            {
                startingPieceAction?.Invoke();
                return;
            }
            if (m_Type == Constants.CROSS_TYPEPIECE)
                crossPieceAction?.Invoke();
            else
                normalPieceAction?.Invoke();
        }
    }

    private void StartingPieceSafeAreaStatus(Vector2 currentPosition, Action<bool> pieceStatus)
    {
        if (p_GridPosition == currentPosition && m_Canplace)
        {
            pieceStatus?.Invoke(true);
        }
    }
    private void StartingPieceHorizontalSafeAreaStatus(Vector2 currentPosition, List<GridTile> loopPieces, Action<bool> pieceStatus)
    {
        if (p_GridPosition == currentPosition)
        {
            int tileIndex = loopPieces.FindIndex(element => element.GridPosition == p_GridPosition);
            if (tileIndex == Constants.NULLINDEX)
                pieceStatus?.Invoke(true);
        }
    }
    private void StartingPieceVerticalSafeAreaStatus(Vector2 currentPosition, List<GridTile> loopPieces, Action<bool> pieceStatus)
    {
        if (p_GridPosition == currentPosition)
        {
            int tileIndex = loopPieces.FindIndex(element => element.GridPosition == p_GridPosition);
            if (tileIndex == Constants.NULLINDEX)
                pieceStatus?.Invoke(true);
        }
    }

    private void CalculateDetachPieces(Action<int> pieceCount, bool isLoopBreaked)
    {
        int pieces = 0;
        if (!m_isLoopPiece && !m_Canplace && !m_IsStartingPiece && !m_IsObstacle && !m_isSelected && !m_IsBomb)
        {
            pieces++;
            pieceCount?.Invoke(pieces);
            if (!isLoopBreaked)
            {
                ShowNegativeScore(GameData.Instance.GameOptions.UnusedTilePenalty);
                StartCoroutine(ExplodeUnusedPiece());//recentchanges
            }
        }
    }
    private void CheckStartingPieceSafeArea(Action<Vector2, Action<bool>> safeAreaStatusCallback, List<Vector2> spawnSpots)
    {
        if (m_Canplace)
        {
            int pieceCount = 0;
            Action<bool> statusAction = (bool status) =>
                {
                    pieceCount++;
                };

            foreach (Vector2 safeAreaPosition in GameData.Instance.StartingPieceSafeAreaVectors)
            {
                safeAreaStatusCallback(safeAreaPosition + p_GridPosition, statusAction);
            }
            if (pieceCount == GameData.Instance.StartingPieceSafeAreaVectors.Count)
            {
                spawnSpots.Add(p_GridPosition);
            }
        }
    }

    private void CheckStartingPieceHorizontalSafeArea(Action<Vector2, List<GridTile>, Action<bool>> safeAreaStatusCallback, List<GridTile> loopPieces, List<Vector2> spawnSpots)
    {
        if (m_Canplace)
        {
            int pieceCount = 0;
            Action<bool> statusAction = (bool status) =>
            {
                pieceCount++;
            };

            foreach (Vector2 safeAreaPosition in GameData.Instance.StartingPieceHorizontalSafeAreaVectors)
            {
                safeAreaStatusCallback(safeAreaPosition + p_GridPosition, loopPieces, statusAction);
            }
            if (pieceCount == GameData.Instance.StartingPieceHorizontalSafeAreaVectors.Count)
            {
                spawnSpots.Add(p_GridPosition);
            }
        }
    }

    private void CheckStartingPieceVerticalSafeArea(Action<Vector2, List<GridTile>, Action<bool>> safeAreaStatusCallback, List<GridTile> loopPieces, List<Vector2> spawnSpots)
    {
        if (m_Canplace)
        {
            int pieceCount = 0;
            Action<bool> statusAction = (bool status) =>
            {
                pieceCount++;
            };

            foreach (Vector2 safeAreaPosition in GameData.Instance.StartingPieceVerticalSafeAreaVectors)
            {
                safeAreaStatusCallback(safeAreaPosition + p_GridPosition, loopPieces, statusAction);
            }
            if (pieceCount == GameData.Instance.StartingPieceVerticalSafeAreaVectors.Count)
            {
                spawnSpots.Add(p_GridPosition);
            }
        }
    }


    private void CheckPieceLoopStatus(GridTile startingPiece, Vector2 NextPiecePosition, Vector2 flowVector, Action<GridTile, Vector2, Vector2> SendNextAttachedPiece)
    {

        if (p_GridPosition == NextPiecePosition && transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            m_IsAttached = true;
            GridTile nextPiece = null;
            int startingPieceIndex = m_AttachedPiece.FindIndex(element => element.IsStartingPiece);
            int startingPieceIndexinLoop = startingPiece.m_PieceLoop.FindIndex(element => element.IsStartingPiece);
            if (startingPieceIndex == Constants.NULLINDEX && startingPieceIndexinLoop == Constants.NULLINDEX)
            {
                return;
            }
            startingPiece.m_PieceLoop.Add(this);
            m_StartingPieceFlowVector = flowVector;
            if (m_Type == Constants.CROSS_TYPEPIECE)
            {
                if (!p_SingleFlowVector)//recent
                {
                    p_CrossPiecePipe1Vector = m_StartingPieceFlowVector;
                    p_SingleFlowVector = true;
                }
                else
                {
                    p_CrossPiecePipe2Vector = m_StartingPieceFlowVector;
                }
                int crossTypeIndex = m_AttachedPiece.FindIndex(element => element.GridPosition == (p_GridPosition + flowVector));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece && (element.GridPosition == (p_GridPosition + flowVector) || element.GridPosition == (p_GridPosition - flowVector))); /*&& element.GridPosition == (p_GridPosition)*/
                if (crossTypeIndex != Constants.NULLINDEX)
                {
                    if (!m_AttachedPiece[crossTypeIndex].IsStartingPiece)
                    {
                        nextPiece = m_AttachedPiece[crossTypeIndex];
                        if (nextPiece.Type == Constants.CROSS_TYPEPIECE)  //recent
                        {
                            if (!nextPiece.p_SingleFlowVector)
                            {
                                nextPiece.p_CrossPiecePipe1Vector = nextPiece.GridPosition - GridPosition;
                                nextPiece.p_SingleFlowVector = true;
                            }
                            else
                            {
                                nextPiece.p_CrossPiecePipe2Vector = nextPiece.GridPosition - GridPosition;
                            }
                        }
                        flowVector = nextPiece.GridPosition - GridPosition;

                    }
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_PieceLoop.Add(m_AttachedPiece[startPiece]);
                }
            }
            else
            {
                int pieceIndex = m_AttachedPiece.FindIndex(element => element.GridPosition != (p_GridPosition - flowVector) && (!element.IsStartingPiece));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece); /*&& element.GridPosition == (p_GridPosition)*/
                if (pieceIndex != Constants.NULLINDEX)
                {
                    nextPiece = m_AttachedPiece[pieceIndex];
                    flowVector = nextPiece.GridPosition - GridPosition;
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_PieceLoop.Add(m_AttachedPiece[startPiece]);
                }

            }

            if (nextPiece != null)
                SendNextAttachedPiece?.Invoke(startingPiece, nextPiece.GridPosition, flowVector);

        }
    }

    private void UpdateStartingPieceLoopStatus(GridTile startingPiece, Vector2 NextPiecePosition, Vector2 flowVector, Action<GridTile, Vector2, Vector2> SendNextAttachedPiece)
    {

        if (p_GridPosition == NextPiecePosition && transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            m_IsAttached = true;
            GridTile nextPiece = null;
            int startingPieceIndex = m_AttachedPiece.FindIndex(element => element.IsStartingPiece);
            int startingPieceIndexinLoop = startingPiece.m_StartingPieceLoopPool.FindIndex(element => element.IsStartingPiece);
            if (startingPieceIndex == Constants.NULLINDEX && startingPieceIndexinLoop == Constants.NULLINDEX)
            {
                return;
            }
            startingPiece.m_StartingPieceLoopPool.Add(this);
            m_StartingPieceFlowVector = flowVector;
            if (m_Type == Constants.CROSS_TYPEPIECE)
            {
                if (!p_SingleFlowVector)
                {
                    p_CrossPiecePipe1Vector = m_StartingPieceFlowVector;
                    p_SingleFlowVector = true;
                }
                int crossTypeIndex = m_AttachedPiece.FindIndex(element => element.GridPosition == (p_GridPosition + flowVector));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece && (element.GridPosition == (p_GridPosition + flowVector) || element.GridPosition == (p_GridPosition - flowVector)));
                if (crossTypeIndex != Constants.NULLINDEX)
                {
                    if (!m_AttachedPiece[crossTypeIndex].IsStartingPiece)
                    {
                        nextPiece = m_AttachedPiece[crossTypeIndex];
                        if (nextPiece.Type == Constants.CROSS_TYPEPIECE)
                        {
                            if (!nextPiece.p_SingleFlowVector)
                            {
                                nextPiece.p_CrossPiecePipe1Vector = nextPiece.GridPosition - GridPosition;
                                nextPiece.p_SingleFlowVector = true;
                            }
                            else
                            {
                                nextPiece.p_CrossPiecePipe2Vector = nextPiece.GridPosition - GridPosition;
                            }
                        }
                        flowVector = nextPiece.GridPosition - GridPosition;
                    }
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_StartingPieceLoopPool.Add(m_AttachedPiece[startPiece]);
                }
            }
            else
            {
                int pieceIndex = m_AttachedPiece.FindIndex(element => element.GridPosition != (p_GridPosition - flowVector) && (!element.IsStartingPiece));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece); /*&& element.GridPosition == (p_GridPosition)*/
                if (pieceIndex != Constants.NULLINDEX)
                {
                    nextPiece = m_AttachedPiece[pieceIndex];
                    if (nextPiece.Type == Constants.CROSS_TYPEPIECE)
                    {
                        if (!nextPiece.p_SingleFlowVector)
                        {
                            nextPiece.p_CrossPiecePipe1Vector = nextPiece.GridPosition - GridPosition;
                            nextPiece.p_SingleFlowVector = true;
                        }
                        else
                        {
                            nextPiece.p_CrossPiecePipe2Vector = nextPiece.GridPosition - GridPosition;
                        }
                    }
                    flowVector = nextPiece.GridPosition - GridPosition;
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_StartingPieceLoopPool.Add(m_AttachedPiece[startPiece]);
                }
            }

            if (nextPiece != null)
                SendNextAttachedPiece?.Invoke(startingPiece, nextPiece.GridPosition, flowVector);
        }
    }

    private void SpawnNewStartingPiece(int timer, bool horizontalStatus, bool verticalStatus)
    {
        m_PieceImage.gameObject.SetActive(false);//recentchanges
        m_Canplace = false;
        m_IsStartingPiece = true;
        p_BombImage.gameObject.SetActive(false);
        m_SelectedImage.gameObject.SetActive(false);
        float currentWidth = GetComponent<RectTransform>().sizeDelta.x;
        RectTransform pieceImage = m_StartingPieceImage.GetComponent<RectTransform>();
        m_Type = Constants.STARTING_TYPEPIECE;
        var startingPieceCounterValues = GameData.Instance.StartingPiecesIndex.Find(element => element.StartingPieceIndex == m_StartingPieceIndex);
        p_StartingPieceCounter = timer;
        p_StartingPieceCounterText.text = p_StartingPieceCounter.ToString();
        p_StartingPieceCounterText.gameObject.SetActive(true);
        List<float> angleList;
        if (horizontalStatus)
        {
            angleList = new List<float>()
            {
                 Constants.PIECE_ANGLE90,
                 Constants.PIECE_ANGLE270,
            };
        }
        else if (verticalStatus)
        {
            angleList = new List<float>()
            {
                 Constants.PIECE_ANGLE0,
                 Constants.PIECE_ANGLE180,
            };
        }
        else
        {
            angleList = new List<float>()
            {
                 Constants.PIECE_ANGLE0,
                 Constants.PIECE_ANGLE90,
                 Constants.PIECE_ANGLE180,
                 Constants.PIECE_ANGLE270,
            };

        }
        int randomIndex = (int)Random.Range(Constants.ZERO, (angleList.Count - Constants.INT_ONE));
        m_Angle = angleList[randomIndex];
        var FlowVector = GameData.Instance.StartingPieceFlowVectors.Find(angle => angle.Angle == m_Angle);
        var StartingPieceSprite = GameData.Instance.StartingPieceAnglesVariations.Find(element => element.Angle == m_Angle);
        var StartingPieceFillImages = GameData.Instance.StartingPieceFlowFillImages.Find(element => element.Angle == m_Angle);
        m_StartingPieceImage.sprite = StartingPieceSprite.StartingPieceSprite;
        p_StaringPieceFlowFillImage.sprite = StartingPieceFillImages.StartingPieceSprite;
        p_StaringPieceFlowFillImage.gameObject.SetActive(true);
        pieceImage.sizeDelta = (StartingPieceSprite.StartingPieceSprite.rect.width < StartingPieceSprite.StartingPieceSprite.rect.height) ? new Vector2(currentWidth, currentWidth * Constants.STARTTINGPIECEBALANCERATIO) : new Vector2(currentWidth * Constants.STARTTINGPIECEBALANCERATIO, currentWidth);
        p_StaringPieceFlowFillImage.GetComponent<RectTransform>().sizeDelta = pieceImage.sizeDelta;
        p_StaringPieceFlowFillImage.fillAmount = Constants.ZERO;
        m_StartingPieceFlowVector = FlowVector.flowVector;
        m_StartingPieceImage.gameObject.SetActive(true);
        GetCurrentTileCode();
    }

    private void CreateStartingPiece(int startingPieceIndex)
    {
        m_Canplace = false;
        m_IsStartingPiece = true;
        p_BombImage.gameObject.SetActive(false);
        m_SelectedImage.gameObject.SetActive(false);
        m_StartingPieceIndex = startingPieceIndex;
        float currentWidth = GetComponent<RectTransform>().sizeDelta.x;
        RectTransform pieceImage = m_StartingPieceImage.GetComponent<RectTransform>();
        m_Type = Constants.STARTING_TYPEPIECE;
        var startingPieceCounterValues = GameData.Instance.StartingPiecesIndex.Find(element => element.StartingPieceIndex == m_StartingPieceIndex);
        p_StartingPieceCounter = GameData.Instance.GameOptions.StartingPieceTimer;
        p_StartingPieceCounterText.text = p_StartingPieceCounter.ToString();
        p_StartingPieceCounterText.gameObject.SetActive(true);
        List<float> angleList = new List<float>()
        {
            Constants.PIECE_ANGLE0,
            Constants.PIECE_ANGLE90,
            Constants.PIECE_ANGLE180,
            Constants.PIECE_ANGLE270,
        };
        int randomIndex = (int)Random.Range(0f, 4f);
        m_Angle = angleList[randomIndex];
        var FlowVector = GameData.Instance.StartingPieceFlowVectors.Find(angle => angle.Angle == m_Angle);
        var StartingPieceSprite = GameData.Instance.StartingPieceAnglesVariations.Find(element => element.Angle == m_Angle);
        var StartingPieceFillImages = GameData.Instance.StartingPieceFlowFillImages.Find(element => element.Angle == m_Angle);
        m_StartingPieceImage.sprite = StartingPieceSprite.StartingPieceSprite;
        p_StaringPieceFlowFillImage.sprite = StartingPieceFillImages.StartingPieceSprite;
        p_StaringPieceFlowFillImage.gameObject.SetActive(true);
        pieceImage.sizeDelta = (StartingPieceSprite.StartingPieceSprite.rect.width < StartingPieceSprite.StartingPieceSprite.rect.height) ? new Vector2(currentWidth, currentWidth * Constants.STARTTINGPIECEBALANCERATIO) : new Vector2(currentWidth * Constants.STARTTINGPIECEBALANCERATIO, currentWidth);
        p_StaringPieceFlowFillImage.GetComponent<RectTransform>().sizeDelta = pieceImage.sizeDelta;
        p_StaringPieceFlowFillImage.fillAmount = Constants.ZERO;
        m_StartingPieceFlowVector = FlowVector.flowVector;
        m_StartingPieceImage.gameObject.SetActive(true);
        GetCurrentTileCode();
    }

    private void DecrementStartingPieceCounter(Action<GridTile> startingPiece, Action<GridTile> loopCompleteCallback, Action<bool> increaseTimer, int[] remainingTime, Action<GridTile> FlowAnimationCallback)
    {
        StartCoroutine(DecrementCounter(startingPiece, loopCompleteCallback, increaseTimer, remainingTime, FlowAnimationCallback));
    }

    private void OnStartingPieceCounterChanges(Action<GridTile> startingPiece, Action<GridTile> loopCompletecallback, Action<bool> increaseTimer, int[] remainingTime, Action<GridTile> FlowAnimationCallback)
    {
        Action startingPieceAction = () =>
        {
            DecrementStartingPieceCounter(startingPiece, loopCompletecallback, increaseTimer, remainingTime, FlowAnimationCallback);
            m_StartedPieceCounted = true;
        };

        if (IsStartingPiece && !m_StartedPieceCounted)
            startingPieceAction?.Invoke();
    }

    private void SetStartingPiecePosition(Vector2 positon, int startingPieceIndex, int angle)
    {
        if (p_GridPosition == positon)
        {
            CreateStartingPiece(startingPieceIndex);
        }
    }

    private void SpawnStartingPiece(Vector2 positon, int timer, bool horizontalStatus, bool verticalStatus)
    {
        if (p_GridPosition == positon && m_Canplace)
        {
            SpawnNewStartingPiece(timer, horizontalStatus, verticalStatus);
        }
    }

    private void MakePlayerInputsActive()
    {
        m_RotatePieceClockwise.Enable();
        m_RotatePieceAntiClockwise.Enable();
    }
    private void MakePlayerInputsDeActive()
    {
        m_SinglePlayerInputs.Disable();
        m_RotatePieceClockwise.performed -= RotatePieceClockwise;
        m_RotatePieceAntiClockwise.performed -= RotatePieceAntiClockwise;
        m_RotatePieceClockwise.Disable();
        m_RotatePieceAntiClockwise.Disable();
    }
    private void DeAttachPiece(Vector2 targetPiece, bool Player1, bool Player2)
    {
        if (targetPiece == p_GridPosition)
        {
            for (int PieceCount = (int)Constants.ZERO; PieceCount < m_AttachedPiece.Count; PieceCount++)
            {
                int index = m_AttachedPiece[PieceCount].m_AttachedPiece.FindIndex(element => element.GridPosition == targetPiece);
                if (index != Constants.NULLINDEX)
                    m_AttachedPiece[PieceCount].m_AttachedPiece.RemoveAt(index);
            }
            m_AttachedPiece.Clear();
        }
    }

    private void RotatePieceClockwise(InputAction.CallbackContext context)
    {
        Action RotateAction = () =>
        {
            RotateTilePiece(-Constants.ANGLE_DIFF);
            SoundManager.Instance.PlayRotateTilePieceSound();
        };
        if (!m_isGameOver && m_isSelected)
            RotateAction?.Invoke();


    }
    private void RotatePieceAntiClockwise(InputAction.CallbackContext context)
    {
        Action RotateAction = () =>
        {
            RotateTilePiece(Constants.ANGLE_DIFF);
            SoundManager.Instance.PlayRotateTilePieceSound();
        };
        if (!m_isGameOver && m_isSelected)
            RotateAction?.Invoke();
    }
    private void AssignTilePieceCode(Vector2 currentPosition, double[] tileCodes, bool isPlayer1, bool isPlayer2)
    {
        Action tileAction = () =>
           {
               m_TileCode.Clear();
               for (int tileIndex = 0; tileIndex < tileCodes.Length; tileIndex++)
               {
                   m_TileCode.Add(tileCodes[tileIndex]);
               }

           };

        if (currentPosition == GridPosition)
            tileAction?.Invoke();
    }
    protected void CheckTileUpStatus(GridTile currentTile, Vector2 UpTilePosition)
    {
        if (UpTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] == m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                currentTile.ValidConnection(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
                ValidConnection(p_BottomConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_TopConnectionStatus);
                InvalidConnection(p_BottomConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
            }
        }

        // FOR DUMMY STARTING PIECE
        if (UpTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] == m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                currentTile.ValidConnection(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
                ValidConnection(p_BottomConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_TopConnectionStatus);
                InvalidConnection(p_BottomConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
            }
        }
    }
    protected void CheckTileDownStatus(GridTile currentTile, Vector2 DownTilePosition)
    {
        if (DownTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] == m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                currentTile.ValidConnection(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
                ValidConnection(p_TopConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_BottomConnectionStatus);
                InvalidConnection(p_TopConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
            }
        }

        // FOR DUMMY STARTING PIECE
        if (DownTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] == m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                currentTile.ValidConnection(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
                ValidConnection(p_TopConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_BottomConnectionStatus);
                InvalidConnection(p_TopConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
            }
        }

    }

    protected void CheckTileLeftStatus(GridTile currentTile, Vector2 leftTilePosition)
    {
        if (leftTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] == m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                currentTile.ValidConnection(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
                ValidConnection(p_RightConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_LeftConnectionStatus);
                InvalidConnection(p_RightConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
            }
        }

        //FOR DUMMY STARTING PIECE
        if (leftTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] == m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {

                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                currentTile.ValidConnection(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
                ValidConnection(p_RightConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_LeftConnectionStatus);
                InvalidConnection(p_RightConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
            }
        }
    }
    protected void CheckTileRightStatus(GridTile currentTile, Vector2 rightTilePosition)
    {
        if (rightTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] == m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                currentTile.ValidConnection(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
                ValidConnection(p_LeftConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_RightConnectionStatus);
                InvalidConnection(p_LeftConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
            }
        }

        //FOR DUMMY STARTING PIECE
        if (rightTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] == m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                currentTile.ValidConnection(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
                ValidConnection(p_LeftConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_RightConnectionStatus);
                InvalidConnection(p_LeftConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
            }
        }
    }

    private void CheckLoopPieces(List<GridTile> loopPieces)
    {
        Action loopCompleteAction = () =>
        {
            foreach (GridTile tile in loopPieces)
            {
                tile.m_isLoopPiece = true;
            }
        };
        GridTile startingPiece = loopPieces.Find(element => element.IsStartingPiece);
        int startingPieceCount = 0;
        foreach (GridTile tile in loopPieces)
        {
            if (tile.IsStartingPiece && tile.GridPosition == startingPiece.GridPosition)
            {
                startingPieceCount++;
            }
        }
        m_IsLoopCompleted = (startingPieceCount >= Constants.STARTINGPIECEPAIR) ? true : false;
        if (m_IsLoopCompleted)
            loopCompleteAction?.Invoke();
    }

    #endregion

    #region    

    private IEnumerator StartFillingFlowImage(float time)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        while (timeElapsed < time)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImage.fillAmount = valueToLerp;

    }


    private IEnumerator StartFillingContinousFlowForNextPosition(Action<Vector2, Vector2> nextPieceCallback, Action<GridTile, float> lastPieceCallback, float time, GridTile startingPiece, Action<GridTile> loopCompleteCallback, Action onflowStarts)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        if (startingPiece.m_IsLoopCompleted)
        {
            time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
            onflowStarts?.Invoke();
        }
        while (timeElapsed < time)
        {
            if (!startingPiece.m_IsLoopCompleted)
            {
                startingPiece.m_PieceLoop.Clear();
                loopCompleteCallback?.Invoke(startingPiece);
                startingPiece.CheckLoopPieces(startingPiece.m_PieceLoop);
            }
            if (startingPiece.m_IsLoopCompleted)
            {
                time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
                onflowStarts?.Invoke();
            }
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImage.fillAmount = valueToLerp;
        if (m_Type != Constants.CROSS_TYPEPIECE)
        {
            m_NextPieceAnimationCoroutine = StartCoroutine(NextPieceAnimationAttachedPieceStatus(nextPieceCallback, lastPieceCallback, time));
        }
        else
        {
            m_NextPieceAnimationCoroutine = StartCoroutine(NextPieceAnimationAttachedPieceStatusForCrossPiece(nextPieceCallback, lastPieceCallback, time));
        }
    }
    private IEnumerator StartFillingContinousFlowForDualImage(Action<Vector2, Vector2> nextPieceCallback, Action<GridTile, float> lastPieceCallback, float time, GridTile startingPiece, Action<GridTile> loopCompleteCallback, Action onflowStarts)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        if (startingPiece.m_IsLoopCompleted)
        {
            time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
            onflowStarts?.Invoke();
        }
        while (timeElapsed < time)
        {
            if (!startingPiece.m_IsLoopCompleted)
            {
                startingPiece.m_PieceLoop.Clear();
                loopCompleteCallback?.Invoke(startingPiece);
                startingPiece.CheckLoopPieces(startingPiece.m_PieceLoop);
            }
            if (startingPiece.m_IsLoopCompleted)
            {
                time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
                onflowStarts?.Invoke();
            }
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImageDualFlow.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImageDualFlow.fillAmount = valueToLerp;
        m_NextPieceAnimationCoroutine = StartCoroutine(NextPieceAnimationAttachedPieceStatusForCrossPiece(nextPieceCallback, lastPieceCallback, time));
    }
    private IEnumerator NextPieceAnimationAttachedPieceStatus(Action<Vector2, Vector2> nextPieceCallback, Action<GridTile, float> lastPieceCallback, float time)
    {
        yield return new WaitForSeconds(Constants.ZERO);
        GridTile nextTilePosition = m_AttachedPiece.Find(tile => tile.GridPosition != (p_GridPosition - m_StartingPieceFlowVector));
        if (nextTilePosition != null)
        {
            nextPieceCallback?.Invoke(p_GridPosition, nextTilePosition.GridPosition);
        }
        else
        {
            lastPieceCallback?.Invoke(this, time);
        }
    }
    private IEnumerator NextPieceAnimationAttachedPieceStatusForCrossPiece(Action<Vector2, Vector2> nextPieceCallback, Action<GridTile, float> lastPieceCallback, float time)
    {
        yield return new WaitForSeconds(Constants.ZERO);
        Vector2 crossPieceFlowVector = p_CrossPiecePipe1Vector;
        if (!p_IsDualContiniousFlow)
        {
            crossPieceFlowVector = p_CrossPiecePipe1Vector;
            p_IsDualContiniousFlow = true;
        }
        else
        {
            crossPieceFlowVector = p_CrossPiecePipe2Vector;

        }
        GridTile nextTilePosition = m_AttachedPiece.Find(tile => tile.GridPosition == (p_GridPosition + crossPieceFlowVector));
        if (nextTilePosition != null)
        {
            nextPieceCallback?.Invoke(p_GridPosition, nextTilePosition.GridPosition);
        }
        else
        {
            lastPieceCallback?.Invoke(this, time);
        }
    }
    private IEnumerator StartFillingDualImage(float time)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        while (timeElapsed < time)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImageDualFlow.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImageDualFlow.fillAmount = valueToLerp;

    }

    private IEnumerator StartStartingPieceFillingFlowImageContiniousFlow(float time, GridTile startingPiece)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        if (startingPiece.m_IsLoopCompleted)
        {
            time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
        }
        while (timeElapsed < time)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_StaringPieceFlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_StaringPieceFlowFillImage.fillAmount = valueToLerp;

    }

    private IEnumerator StartStartingPieceFillingFlowImage(float time)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        while (timeElapsed < time)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_StaringPieceFlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_StaringPieceFlowFillImage.fillAmount = valueToLerp;

    }

    private IEnumerator ExplodeContiniousFlowPiece(GridTile startingPiece, float time, Action<Vector2> startingPiecePosition)
    {
        time = (startingPiece.m_IsLoopCompleted) ? Constants.INT_ONE / Constants.MAX_FLOWSPEED : time;
        yield return new WaitForSeconds(time + 0.3f);
        for (int loopPieceIndex = Constants.INT_ZERO; loopPieceIndex < startingPiece.ContiniousFlowPieces.Count; loopPieceIndex++)
        {
            if (!startingPiece.ContiniousFlowPieces[loopPieceIndex].IsStartingPiece)
            {
                startingPiece.ContiniousFlowPieces[loopPieceIndex].m_isLoopPiece = false;
                startingPiece.ContiniousFlowPieces[loopPieceIndex].PlaceBomb(startingPiece.ContiniousFlowPieces[loopPieceIndex].GridPosition);
            }
        }
        startingPiecePosition?.Invoke(startingPiece.GridPosition);
    }

    private IEnumerator ExplodeStarttingPiece(GridTile startingPiece, float time, Action<Vector2> startingPiecePosition)
    {
        yield return new WaitForSeconds(time + 0.8f);
        for (int loopPieceIndex = Constants.INT_ZERO; loopPieceIndex < startingPiece.m_PieceLoop.Count; loopPieceIndex++)
        {
            if(startingPiece.m_PieceLoop[loopPieceIndex].IsBomb || startingPiece.m_PieceLoop[loopPieceIndex].isSelected)
            {
                startingPiece.m_PieceLoop[loopPieceIndex].m_isLoopPiece = false;
                startingPiece.m_PieceLoop[loopPieceIndex].IgnoreSelectionTile(startingPiece.m_PieceLoop[loopPieceIndex].GridPosition);
            }
            if (!startingPiece.m_PieceLoop[loopPieceIndex].IsStartingPiece && !startingPiece.m_PieceLoop[loopPieceIndex].IsBomb && !startingPiece.m_PieceLoop[loopPieceIndex].isSelected)
            {
                startingPiece.m_PieceLoop[loopPieceIndex].m_isLoopPiece = false;
                startingPiece.m_PieceLoop[loopPieceIndex].PlaceBomb(startingPiece.m_PieceLoop[loopPieceIndex].GridPosition);
            }
        }
        startingPiecePosition?.Invoke(startingPiece.GridPosition);
    }

    IEnumerator ExplodeUnusedPiece()
    {
        yield return new WaitForSeconds(1f);
        PlaceBomb(p_GridPosition);
    }
    IEnumerator GetWidthHeight()
    {
        yield return new WaitForSeconds(2f);
        Vector2 tileSize = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y);
        RectTransform Right1 = m_Right1BrokenImage.GetComponent<RectTransform>();
        RectTransform Right2 = m_Right2BrokenImage.GetComponent<RectTransform>();
        RectTransform Left1 = m_Left1BrokenImage.GetComponent<RectTransform>();
        RectTransform Left2 = m_Left2BrokenImage.GetComponent<RectTransform>();
        RectTransform UP1 = m_UP1BrokenImage.GetComponent<RectTransform>();
        RectTransform UP2 = m_UP2BrokenImage.GetComponent<RectTransform>();
        RectTransform Down1 = m_Down1BrokenImage.GetComponent<RectTransform>();
        RectTransform Down2 = m_Down2BrokenImage.GetComponent<RectTransform>();
        Right1.sizeDelta = Constants.INT_TWO * tileSize;
        Right1.anchoredPosition = new Vector2(tileSize.x, Right1.anchoredPosition.y);
        Right2.sizeDelta = Constants.INT_TWO * tileSize;
        Right2.anchoredPosition = new Vector2(tileSize.x, Right2.anchoredPosition.y);
        Left1.sizeDelta = Constants.INT_TWO * tileSize;
        Left1.anchoredPosition = new Vector2(-tileSize.x, Left1.anchoredPosition.y);
        Left2.sizeDelta = Constants.INT_TWO * tileSize;
        Left2.anchoredPosition = new Vector2(-tileSize.x, Left2.anchoredPosition.y);
        UP1.sizeDelta = Constants.INT_TWO * tileSize;
        UP1.anchoredPosition = new Vector2(UP1.anchoredPosition.x, tileSize.y);
        UP2.sizeDelta = Constants.INT_TWO * tileSize;
        UP2.anchoredPosition = new Vector2(UP2.anchoredPosition.x, tileSize.y);
        Down1.sizeDelta = Constants.INT_TWO * tileSize;
        Down1.anchoredPosition = new Vector2(Down1.anchoredPosition.x, -tileSize.y);
        Down2.sizeDelta = Constants.INT_TWO * tileSize;
        Down2.anchoredPosition = new Vector2(Down2.anchoredPosition.x, -tileSize.y);
    }
    private IEnumerator DecrementCounter(Action<GridTile> startingPiece, Action<GridTile> loopCompleteCallback, Action<bool> increaseTimer, int[] remainingTime, Action<GridTile> FlowAnimationCallback)
    {
        Action loopCompleteAction = () =>
        {
            remainingTime[Constants.INT_ZERO] = p_StartingPieceCounter;
            increaseTimer?.Invoke(false);
        };

        while (p_StartingPieceCounter > (int)(Constants.ZERO) && !m_IsLoopCompleted)
        {
            m_PieceLoop.Clear();
            if (transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
                loopCompleteCallback?.Invoke(this);
            CheckLoopPieces(m_PieceLoop);
            if (m_IsLoopCompleted)
            {
                float flowTime = ((Constants.INT_ONE) / Constants.MAX_FLOWSPEED) * m_PieceLoop.Count;
                Debug.Log("The Flow Time is: " + flowTime);
                loopCompleteAction?.Invoke();
                FlowAnimationCallback?.Invoke(this);
            }
            p_StartingPieceCounter--;
            p_StartingPieceCounterText.color = (p_StartingPieceCounter < Constants.COUNTERWARNING) ? Color.red : Color.white;
            if (p_StartingPieceCounter < Constants.COUNTERWARNING && transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
                SoundManager.Instance.PlayCountDownSound();
            p_StartingPieceCounterText.text = p_StartingPieceCounter.ToString();
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForEndOfFrame();
        if (!m_IsLoopCompleted)
        {
            if (transform.parent.name == Constants.MAINGAMEAREAPARENTSINGLEPLAYER)
            {
                yield return new WaitForSeconds(p_StartingPieceCounter);
                startingPiece?.Invoke(this);
            }
        }
    }

    private IEnumerator StartBrokenEndAnimations(float time, Vector2 targetVector)
    {
        yield return new WaitForSeconds(time);
        int tileCodeIndex = GameData.Instance.BrokenPieceVectors.SurroundingVectors.FindIndex(element => element == targetVector);
        if (tileCodeIndex != Constants.NULLINDEX)
        {
            GameObject targetBrokenPiece = m_BrokenPieceImages.Find(brokenpiece => brokenpiece.FlowVector == targetVector).targetImages.Find(element => element.tileCode == m_TileCode[tileCodeIndex]).BrokenFlowImage;
            if (targetBrokenPiece != null)
            {
                targetBrokenPiece.gameObject.SetActive(true);
            }
        }
    }
    #endregion
}
