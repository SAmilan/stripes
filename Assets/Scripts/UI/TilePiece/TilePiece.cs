using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;
using TMPro;


public class TilePiece : MonoBehaviour
{

    #region Private_Variables
    [SerializeField]
    protected Animator m_ScorePopUp;
    [SerializeField]
    protected TextMeshProUGUI m_PositiveScore;
    [SerializeField]
    protected TextMeshProUGUI m_NegativeScore;
    [SerializeField]
    protected Image p_FlowFillImage;
    [SerializeField]
    protected Image p_FlowFillImageDualFlow;
    [SerializeField]
    protected Image p_StaringPieceFlowFillImage;
    [SerializeField]
    protected Image p_ActivePieceImage;
    [SerializeField]
    protected float m_ActiveAngle;
    [SerializeField]
    protected float m_ActiveType;

    [SerializeField]
    protected Image m_PieceImage;
    [SerializeField]
    protected Image m_SelectedImage;
    [SerializeField]
    protected Image m_StartingPieceImage;
    [SerializeField]
    protected Mask m_PieceMask;
    [SerializeField]
    protected float m_Angle;
    [SerializeField]
    protected float m_Type;
    [SerializeField]
    protected bool m_isSelected;
    [SerializeField]
    protected bool m_isLoopPiece = false;
    [SerializeField]
    protected Vector2 p_GridPosition;
    protected float p_MovementCount = 0;
    [SerializeField]
    protected bool m_Canplace = true;
    [SerializeField]
    protected bool m_IsBomb = false;
    [SerializeField]
    protected Image p_BombImage;
    [SerializeField]
    protected bool m_IsStartingPiece = false;
    [SerializeField]
    protected bool m_IsStartingPieceCounted = false;
    [SerializeField]
    protected bool m_IsObstacle = false;
    [SerializeField]
    protected bool m_IsPlayer1 = false;
    [SerializeField]
    protected bool m_IsPlayer2 = false;
    [SerializeField]
    protected bool m_IsAttached = false;
    [SerializeField]
    protected bool m_isGameOver = false;
    [SerializeField]
    protected float p_PenaltyValue = 0f;
    [SerializeField]
    protected Animator m_TileAnimator;
    [SerializeField]
    protected bool p_IsCurrentPosition = false;
    [SerializeField]
    protected bool p_IsDualFlow = false;
    [SerializeField]
    protected bool p_IsDualContiniousFlow = false;
    [SerializeField]
    protected bool p_SingleFlowVector = false;
    [SerializeField]
    protected bool p_IsLoopObstacle = false;
    [SerializeField]
    protected Vector2 p_CrossPiecePipe1Vector;
    [SerializeField]
    protected Vector2 p_CrossPiecePipe2Vector;
    [SerializeField]
    protected bool p_CanPlaceHoldTile = true;
    [SerializeField]
    protected Image p_RightConnectionStatus;
    [SerializeField]
    protected Image p_LeftConnectionStatus;
    [SerializeField]
    protected Image p_TopConnectionStatus;
    [SerializeField]
    protected Image p_BottomConnectionStatus;
    [SerializeField]
    protected Sprite p_ValidConnection;
    [SerializeField]
    protected Sprite p_InvalidConnection;

    /*protected double[] p_TileCode = new double[4];*/
    protected List<double> p_TileCodeArray = new List<double>(4);
    protected List<double> p_TileCodeArrayPlayer1 = new List<double>(4);
    protected List<double> p_TileCodeArrayPlayer2 = new List<double>(4);
    #endregion
    #region Public_Variables
    public TileType TileType;
    public bool canPlace => m_Canplace;
    public bool IsObstacle => m_IsObstacle;
    public bool isSelected => m_isSelected;
    public bool isLoopPiece => m_isLoopPiece;
    public float Angle => m_Angle;
    public Mask PieceMask => m_PieceMask;
    public Image PieceImage => m_PieceImage;
    public float Type => m_Type;
    public bool IsBomb => m_IsBomb;
    public bool IsAttached => m_IsAttached;
    public bool IsStartingPiece => m_IsStartingPiece;
    public bool IsStartingPieceCounted => m_IsStartingPieceCounted;
    public float PenaltyValue => p_PenaltyValue;
    public bool IsCurrentPosition => p_IsCurrentPosition;
    public float ActiveType => m_ActiveType;
    public bool IsLoopObstacle => p_IsLoopObstacle;
    public static Action<Vector2, double[], bool, bool> s_AssignTileCode;
    public static Action<Vector2, bool, bool> s_DeAttachPieces;


    public Vector2 GridPosition => p_GridPosition;
    #endregion



    public void CreateTilePiece(Sprite PieceImage, int Type, float Angle)
    {
        m_PieceMask.enabled = true; //skmask chnages
        p_ActivePieceImage.sprite = PieceImage;
        p_ActivePieceImage.transform.parent.gameObject.SetActive(true);
        m_ActiveAngle = Angle;
        m_ActiveType = Type;
        m_SelectedImage.gameObject.SetActive(m_isSelected);
        RectTransform pieceImage = p_ActivePieceImage.GetComponent<RectTransform>();
        pieceImage.eulerAngles = (m_ActiveType != Constants.BOMB_TYPEPIECE) ? new Vector3(pieceImage.eulerAngles.x,
                                                                              pieceImage.eulerAngles.y,
                                                                              Constants.ZERO) : pieceImage.eulerAngles;
        m_IsBomb = (m_ActiveType == Constants.BOMB_TYPEPIECE) ? true : false;
    }

    public void CreateGridTile(Vector2 gridPosition, bool isPlayer1, bool isPlayer2, int midRow, int midColumn)
    {
        if ((gridPosition.x == midColumn && gridPosition.y == midRow) || (gridPosition.x == midColumn + Constants.INT_ONE && gridPosition.y == midRow) || (gridPosition.x == midColumn + Constants.INT_TWO && gridPosition.y == midRow))
            p_IsCurrentPosition = true;
        p_GridPosition = gridPosition;
        m_IsPlayer1 = isPlayer1;
        m_IsPlayer2 = isPlayer2;
    }

    #region Private_Methods

    protected void FlashStartingPiece(Vector2 gridPosition, bool flashstatus)
    {
        if (p_GridPosition == gridPosition)
        {
            m_StartingPieceImage.GetComponent<Animator>().SetBool("Flash", flashstatus);
        }
    }

    private void AssignTileCodes(double[] codeArray, bool isPlayer1, bool isPlayer2)
    {
        s_AssignTileCode?.Invoke(p_GridPosition, codeArray, m_IsPlayer1, m_IsPlayer2);
    }

    #endregion

    #region Protected_Methods
    protected void ResetTopCopnnectionStatus(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
            p_TopConnectionStatus.gameObject.SetActive(false);
    }
    protected void ResetBottomCopnnectionStatus(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
            p_BottomConnectionStatus.gameObject.SetActive(false);
    }
    protected void ResetLeftCopnnectionStatus(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
            p_LeftConnectionStatus.gameObject.SetActive(false);
    }
    protected void ResetRightCopnnectionStatus(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
            p_RightConnectionStatus.gameObject.SetActive(false);
    }
    protected void RefreshHoldTilePlacement()
    {
        p_CanPlaceHoldTile = true;
    }

    protected void DisableConnections(Image currentImage)
    {
        currentImage.gameObject.SetActive(false);
    }

    protected void DisableAllConnections()
    {
        p_TopConnectionStatus.gameObject.SetActive(false);
        p_BottomConnectionStatus.gameObject.SetActive(false);
        p_LeftConnectionStatus.gameObject.SetActive(false);
        p_RightConnectionStatus.gameObject.SetActive(false);
    }

    protected void ValidConnection(Image currentImage)
    {
        currentImage.sprite = p_ValidConnection;
        StartCoroutine(ToggleConnectionStatus(currentImage));
    }
    protected void InvalidConnection(Image currentImage)
    {
        currentImage.sprite = p_InvalidConnection;
        currentImage.gameObject.SetActive(true);
    }

    protected void CheckTileActiveStatus(Action<bool> activeStatus)
    {
        if (m_isSelected || m_IsBomb)
        {
            activeStatus?.Invoke(true);
        }
    }

    protected void ShowPositiveScore(float Score)
    {
        m_PositiveScore.text = "+" + Score.ToString();
        m_ScorePopUp.SetTrigger(Constants.POSITIVESCORE_ANIMATION);
    }
    protected void ShowNegativeScore(float Score)
    {
        m_NegativeScore.text = Score.ToString();
        m_ScorePopUp.SetTrigger(Constants.NEGATIVESCORE_ANIMATION);
    }


    protected void OnGameOvers(bool status)
    {
        m_isGameOver = status;
    }

    protected void SetPieceActive(Vector2 gridposition)
    {
        p_IsCurrentPosition = (p_GridPosition == gridposition) ? true : false;
    }
    protected void LockLoopPiece(Vector2 gridposition)
    {
        if (p_GridPosition == gridposition)
        {
            m_isLoopPiece = (p_GridPosition == gridposition) ? true : false;
        }
    }
    protected void StartedPieceCounted(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
            m_IsStartingPieceCounted = true;
    }



    protected void CreateRoundObstacle(Vector2 obstaclePosition)
    {
        Action obstacleAction = () =>
        {
            m_TileAnimator.SetTrigger("Place");
            m_Canplace = false;
            m_SelectedImage.gameObject.SetActive(false);
            m_IsObstacle = true;
            m_PieceMask.enabled = false;
            m_PieceImage.sprite = GameData.Instance.Obstacle[Constants.ROUND_OBSTACLE].ObstaclesPieces[Constants.ROUND_PIECE1];
            m_PieceImage.gameObject.SetActive(true);
        };
        if (p_GridPosition == obstaclePosition && m_Canplace && !m_isSelected && !m_IsBomb)
            obstacleAction?.Invoke();
    }
    protected void CreateHorizontalObstacle(Vector2 obstaclePositionPiece1, Vector2 obstaclePositionPiece2)
    {
        Action<int> obstacleAction = (int pieceIndex) =>
        {
            m_Canplace = false;
            m_SelectedImage.gameObject.SetActive(false);
            m_IsObstacle = true;
            m_PieceMask.enabled = false;
            m_PieceImage.sprite = GameData.Instance.Obstacle[Constants.HORIZONTAL_OBSTACLE].ObstaclesPieces[pieceIndex];
            m_PieceImage.gameObject.SetActive(true);
        };
        if (p_GridPosition == obstaclePositionPiece1)
            obstacleAction?.Invoke(Constants.HORIZONTAL_PIECE1);

        if (p_GridPosition == obstaclePositionPiece2)
            obstacleAction?.Invoke(Constants.HORIZONTAL_PIECE2);
    }

    protected void CreateVerticalObstacle(Vector2 obstaclePositionPiece1, Vector2 obstaclePositionPiece2, Vector2 obstaclePositionPiece3)
    {
        Action<int> obstacleAction = (int pieceIndex) =>
        {
            m_Canplace = false;
            m_SelectedImage.gameObject.SetActive(false);
            m_IsObstacle = true;
            m_PieceMask.enabled = false;
            m_PieceImage.sprite = GameData.Instance.Obstacle[Constants.VERTICAL_OBSTACLE].ObstaclesPieces[pieceIndex];
            m_PieceImage.gameObject.SetActive(true);
        };
        if (p_GridPosition == obstaclePositionPiece1)
            obstacleAction?.Invoke(Constants.VERTICAL_PIECE1);

        if (p_GridPosition == obstaclePositionPiece2)
            obstacleAction?.Invoke(Constants.VERTICAL_PIECE2);

        if (p_GridPosition == obstaclePositionPiece3)
            obstacleAction?.Invoke(Constants.VERTICAL_PIECE3);
    }

    protected void SetStartingPiece(Vector2 tilePosition)
    {
        m_IsStartingPiece = true;
    }

    protected void CreateUpcomingTile(TilePiece tile)
    {
        m_TileAnimator.SetTrigger("PickUp");
        m_PieceMask.enabled = true;
        p_ActivePieceImage.sprite = tile.p_ActivePieceImage.sprite;
        p_ActivePieceImage.transform.parent.gameObject.SetActive(true);
        m_PieceImage.gameObject.SetActive(false);
        m_ActiveType = tile.m_ActiveType;
        m_ActiveAngle = tile.m_ActiveAngle;
        m_SelectedImage.gameObject.SetActive(true);
        m_IsBomb = tile.m_IsBomb;
        RectTransform pieceImage = p_ActivePieceImage.GetComponent<RectTransform>();
        pieceImage.eulerAngles = (tile.m_ActiveType != Constants.BOMB_TYPEPIECE) ? new Vector3(pieceImage.eulerAngles.x,
                                                                               pieceImage.eulerAngles.y,
                                                                               m_ActiveAngle) : Vector3.zero;
    }

    protected void CreateBombPiece()
    {
        SoundManager.Instance.PlayMovePieceSound();
        m_TileAnimator.SetTrigger("PickUp");
        p_BombImage.gameObject.SetActive(true);
        m_SelectedImage.gameObject.SetActive(true);
        m_ActiveType = Constants.BOMB_TYPEPIECE;
        m_IsBomb = true;
        m_isSelected = false;
        m_SelectedImage.gameObject.SetActive(false);
        p_ActivePieceImage.transform.parent.gameObject.SetActive(false);

    }
    protected void DestroyBombPiece()
    {
        p_BombImage.gameObject.SetActive(false);
        m_SelectedImage.gameObject.SetActive(false);
        m_IsBomb = false;
    }

    protected void SetGridPiecePosition(TilePiece activeGrid, TilePiece upcomingTile, Action destroyUpcomingPieceCallback)
    {
        Action tileAction = () =>
        {
            CreateUpcomingTile(upcomingTile);
            m_isSelected = true;
            m_SelectedImage.gameObject.SetActive(true);
            p_BombImage.gameObject.SetActive(false);
            m_IsBomb = false;
            destroyUpcomingPieceCallback?.Invoke();
        };
        if (activeGrid.p_GridPosition == p_GridPosition)
            tileAction?.Invoke();
    }
    protected void SetUpcomingBombPosition(TilePiece activeGrid, Action destroyUpcomingPieceCallback)
    {
        Action bombAction = () =>
        {
            CreateBombPiece();
            destroyUpcomingPieceCallback?.Invoke();
        };
        if (activeGrid.p_GridPosition == p_GridPosition)
            bombAction?.Invoke();
    }
    protected void PlaceTilePiece(Vector2 gridPosition, Action<int> decrementScoreCallback)
    {
        Action placeAction = () =>
        {
           // DisableAllConnections();
            SoundManager.Instance.PlayTilePlaceSound();
            m_TileAnimator.SetTrigger("Place");
            p_PenaltyValue += (!m_Canplace) ? GameData.Instance.GameOptions.TileReplacementPenalty : Constants.ZERO;
            if (p_PenaltyValue < Constants.ZERO && !m_Canplace)
            {
                ShowNegativeScore(GameData.Instance.GameOptions.TileReplacementPenalty);
                decrementScoreCallback?.Invoke(GameData.Instance.GameOptions.TileReplacementPenalty);
            }
            p_BombImage.gameObject.SetActive(false);
            m_IsBomb = false;
            s_DeAttachPieces?.Invoke(gridPosition, m_IsPlayer1, m_IsPlayer2);
            p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
            p_IsDualFlow = false;//crosspiecechanges
            p_IsDualContiniousFlow = false;//crosspiecechanges
            p_SingleFlowVector = false;//crosspiecechanges
            m_isSelected = false;
            m_Canplace = false;
            m_PieceImage.sprite = p_ActivePieceImage.sprite;
            m_Angle = m_ActiveAngle;
            m_Type = m_ActiveType;
            m_PieceImage.gameObject.SetActive(true);
            GetCurrentTileCode();
            RectTransform pieceImage = m_PieceImage.GetComponent<RectTransform>();
            pieceImage.eulerAngles = (m_Type != Constants.BOMB_TYPEPIECE) ? new Vector3(pieceImage.eulerAngles.x,
                                                                                   pieceImage.eulerAngles.y,
                                                                                   m_Angle) : pieceImage.eulerAngles;
            m_PieceImage.gameObject.SetActive(true);
            m_SelectedImage.gameObject.SetActive(false);
            p_FlowFillImage.sprite = GameData.Instance.FillImages.Find(element => element.Type == m_Type).fillImages;
            p_FlowFillImage.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.x,
                                                                                    p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.y,
                                                                                    m_Angle);
            p_FlowFillImageDualFlow.sprite = GameData.Instance.FillImages.Find(element => element.Type == m_Type).fillImages;
            p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.x,
                                                                                    p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.y,
                                                                                    m_Angle);
            p_FlowFillImage.gameObject.SetActive(true);
            p_FlowFillImageDualFlow.gameObject.SetActive(true);
            p_FlowFillImage.fillAmount = 0f;
            p_FlowFillImageDualFlow.fillAmount = 0f;

        };
        if (p_GridPosition == gridPosition)
            placeAction?.Invoke();
    }

    protected void RotateTilePiece(float angleDiff)
    {
        Action pieceAction = () =>
        {
            //SoundManager.Instance.PlayRotateTilePieceSound();

            // Added by Kyle for Scaling Animation Trigger for Tile Rotation
            m_TileAnimator.SetTrigger("PickUp");
            m_ActiveAngle = m_ActiveAngle + angleDiff;
            RectTransform pieceTransform = p_ActivePieceImage.GetComponent<RectTransform>();
            m_ActiveAngle = (m_ActiveAngle < Constants.ZERO) ? Constants.TOTAL_ANGLE + m_ActiveAngle : (m_ActiveAngle >= Constants.TOTAL_ANGLE) ? Constants.ZERO : m_ActiveAngle;
            pieceTransform.eulerAngles = new Vector3(pieceTransform.eulerAngles.x,
                                                     pieceTransform.eulerAngles.y,
                                                                          m_ActiveAngle);
        };
        if (!m_IsBomb && m_isSelected)
            pieceAction?.Invoke();
    }


    protected void GetCurrentTileCode()
    {
        TileTypes tileType = TileType.TilePiecesCodes.Find(element => element.TilePieceType == (int)m_Type);
        List<TileCode> Codelist = new List<TileCode>();
        Codelist.AddRange(tileType.TileCode);
        TileCode currentCode = Codelist.Find(element => element.tileAngle == m_Angle);
        AssignTileCodes(currentCode.tileCode, m_IsPlayer1, m_IsPlayer2);
    }
    protected void ResetCurrentPosition(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
        {
            p_IsCurrentPosition = true;
        }
        else
        {
            p_IsCurrentPosition = false;
        }
    }

    protected void ExtractHoldPiece(TilePiece previousPiece)
    {
        Action tileAction = () =>
        {
            SoundManager.Instance.PlayMovePieceSound();
            if (!m_Canplace)
            {

                if (m_PieceMask.IsActive())
                    m_PieceMask.enabled = true;
                else
                    m_PieceMask.enabled = false;
                m_PieceImage.gameObject.SetActive(false);
            }
            if (previousPiece.m_IsBomb)
            {
                if (!m_Canplace)
                {
                    m_PieceImage.gameObject.SetActive(true);
                }
                CreateBombPiece();
                p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
                m_isSelected = false;
                m_IsBomb = true;
                return;
            }
            m_TileAnimator.SetTrigger("PickUp");
            m_isSelected = true;
            m_IsBomb = false;
            p_BombImage.gameObject.SetActive(false);
            m_SelectedImage.gameObject.SetActive(true);
            p_ActivePieceImage.sprite = previousPiece.p_ActivePieceImage.sprite;
            p_ActivePieceImage.transform.parent.gameObject.SetActive(true);
            m_ActiveAngle = previousPiece.m_ActiveAngle;
            m_ActiveType = previousPiece.m_ActiveType;
            RectTransform pieceImage = p_ActivePieceImage.GetComponent<RectTransform>();
            pieceImage.eulerAngles = (m_ActiveType != Constants.BOMB_TYPEPIECE) ? new Vector3(pieceImage.eulerAngles.x,
                                                                                   pieceImage.eulerAngles.y,
                                                                                   m_ActiveAngle) : pieceImage.eulerAngles;

        };
        if (m_isSelected || m_IsBomb)
            tileAction?.Invoke();
    }
    protected void ResetTilePiece(Action<bool, Vector2> activePieceStatus)
    {
        Action tileAction = () =>
        {
            activePieceStatus?.Invoke(true, p_GridPosition);
            if (!m_Canplace)
            {
                if (m_PieceMask.IsActive())
                    m_PieceMask.enabled = false;
                else
                    m_PieceMask.enabled = true;
                m_PieceImage.gameObject.SetActive(true);
            }
            m_TileAnimator.SetTrigger(Constants.HOLD_ANIMATION);
            m_isSelected = false;
            m_SelectedImage.gameObject.SetActive(false);
            p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
            p_BombImage.gameObject.SetActive(false);
            m_IsBomb = false;
        };
        if (m_isSelected || m_IsBomb)
            tileAction?.Invoke();
    }

    protected void PlaceBomb(Vector2 gridPosition)
    {
        Action bombAction = () =>
        {
            
            DisableAllConnections();
            SoundManager.Instance.PlayPlaceBombSound();
            m_TileAnimator.SetTrigger("IsBomb");
            p_IsDualFlow = false;
            p_IsDualContiniousFlow = false;
            m_isSelected = false;
            m_Canplace = true;
            m_IsBomb = false;
            m_IsObstacle = false;
            m_SelectedImage.gameObject.SetActive(false);
            m_PieceImage.gameObject.SetActive(false);
            p_BombImage.gameObject.SetActive(false);
            p_FlowFillImage.gameObject.SetActive(false);
            p_FlowFillImageDualFlow.gameObject.SetActive(false);
            p_StaringPieceFlowFillImage.gameObject.SetActive(false);
            m_PieceMask.enabled = true;
            m_IsAttached = false;
            p_SingleFlowVector = false;
            s_DeAttachPieces?.Invoke(gridPosition, m_IsPlayer1, m_IsPlayer2);
        };
        if (p_GridPosition == gridPosition)
            bombAction?.Invoke();

    }

    protected void IgnoreSelectionTile(Vector2 gridPosition)
    {
        m_Canplace = true;
        m_PieceImage.gameObject.SetActive(false);
        m_IsObstacle = false;
        p_IsDualFlow = false;
        p_IsDualContiniousFlow = false;
        p_FlowFillImage.gameObject.SetActive(false);
        m_PieceImage.gameObject.SetActive(false);
        p_FlowFillImageDualFlow.gameObject.SetActive(false);
        p_StaringPieceFlowFillImage.gameObject.SetActive(false);
        m_IsAttached = false;
        p_SingleFlowVector = false;
        s_DeAttachPieces?.Invoke(gridPosition, m_IsPlayer1, m_IsPlayer2);
    }


    protected void ActiveBomb(Vector2 gridPosition)
    {
        Action bombAction;
        if (p_GridPosition == gridPosition)
        {
            bombAction = CreateBombPiece;
        }
        else
        {
            bombAction = DestroyBombPiece;
        }
        bombAction?.Invoke();
    }

    protected void DeActiveTilePiece(Vector2 gridPosition)
    {
        Action tileAction = () =>
        {
            if (!m_Canplace)
            {
                m_PieceMask.enabled = false;
                m_PieceImage.gameObject.SetActive(true);
            }
            m_isSelected = false;
            m_SelectedImage.gameObject.SetActive(false);
            p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
        };
        if (p_GridPosition == gridPosition)
            tileAction?.Invoke();
    }
    protected void ActiveTilePiece(TilePiece previousPiece, Vector2 gridPosition)
    {
        Action tileAction = () =>
        {
            SoundManager.Instance.PlayMovePieceSound();
            if (!m_Canplace)
            {

                m_PieceMask.enabled = true;
                m_PieceImage.gameObject.SetActive(false);
            }
            p_BombImage.gameObject.SetActive(false);
            m_IsBomb = false;
            m_TileAnimator.SetTrigger("PickUp");
            m_isSelected = true;
            m_SelectedImage.gameObject.SetActive(true);
            p_ActivePieceImage.sprite = previousPiece.p_ActivePieceImage.sprite;
            p_ActivePieceImage.transform.parent.gameObject.SetActive(true);
            m_ActiveAngle = previousPiece.m_ActiveAngle;
            m_ActiveType = previousPiece.m_ActiveType;
            RectTransform pieceImage = p_ActivePieceImage.GetComponent<RectTransform>();
            pieceImage.eulerAngles = (m_ActiveType != Constants.BOMB_TYPEPIECE) ? new Vector3(pieceImage.eulerAngles.x,
                                                                                   pieceImage.eulerAngles.y,
                                                                                   m_ActiveAngle) : pieceImage.eulerAngles;
        };
        Action resetAction = () =>
        {
            //added on 24-06-22
            if (!m_Canplace && !IsStartingPiece)
            {
                m_PieceMask.enabled = false;
                m_PieceImage.gameObject.SetActive(true);
            }
            m_isSelected = false;
            m_SelectedImage.gameObject.SetActive(false);
            p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
            p_BombImage.gameObject.SetActive(false);
            m_IsBomb = false;
        };
        if (p_GridPosition == gridPosition)
            tileAction?.Invoke();
        else
            resetAction?.Invoke();

    }
    #endregion

    #region Coroutine

    private IEnumerator ToggleConnectionStatus(Image currentConnection)
    {
        currentConnection.gameObject.SetActive(true);
        yield return StartCoroutine(FadeConnectionImage(currentConnection));      
    }

    private IEnumerator FadeConnectionImage(Image connectionImage)
    {
        yield return new WaitForSeconds(Constants.INPUT_DELAY);
        CanvasGroup currentCanvasGroup = connectionImage.GetComponent<CanvasGroup>();
        float startValue = Constants.INT_ONE;
        float endValue = Constants.INT_ZERO;
        float valueToLerp;
        float timeElapsed = 0;
      
        while (timeElapsed < Constants.CONNECTION_DELAY)
        {
            
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / Constants.CONNECTION_DELAY);
            currentCanvasGroup.alpha = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        currentCanvasGroup.alpha = valueToLerp;
        connectionImage.gameObject.SetActive(false);
        currentCanvasGroup.alpha = startValue;
    }



    #endregion

}
