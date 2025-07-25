using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public class ViewGame : UIView
{
    #region Private_Variables
    private float LtypePiece_1 => GameManager.instace.LtypePiece_1;
    private float LtypePiece_2 => GameManager.instace.LtypePiece_2;
    private float StraighttypePiece => GameManager.instace.StraighttypePiece;
    private float CrosstypePiece => GameManager.instace.CrosstypePiece;
    private float BombtypePiece => GameManager.instace.BombtypePiece;
    private float InversiontypePiece => GameManager.instace.InversiontypePiece;
    private SidePieceTye SIDEPIECE;
    [SerializeField]
    protected List<int> p_SidePiecePercentages = new List<int>((int)Constants.Total_Pieces);
    [SerializeField]
    private TilePiece m_TilePrefab;
    public static Action<int> s_SideLengthChanges;

    /*skchnages[SerializeField]
    private UpcomingTile m_UpcomingTilePrefab;*/
    private float[] m_TileAngles =
    {
        Constants.PIECE_ANGLE0,
        Constants.PIECE_ANGLE90,
        Constants.PIECE_ANGLE180,
        Constants.PIECE_ANGLE270
    };
    #endregion
    #region Public_Variables
    protected enum SidePieceTye
    {
        L_TYPE1,
        L_TYPE2,
        STRAIGHT_TYPE,
        CROSS_TYPE,
        BOMB_TYPE,
        INVERSION //SKCHNAGES
    };
    #endregion
    #region Protected_Variables
    [SerializeField]
    protected Sprite[] p_TileImages;
    protected bool p_IsGamePaused = false;
    #endregion

    #region Unity_Callbacks


    #endregion

    #region Public_Methods

    #endregion

    #region Protected_Methods



    protected Vector2 ApplyApproximationonMovementVector(Vector2 movementVector)
    {
        float xValue = movementVector.x;
        float yValue = movementVector.y;
        movementVector = (xValue > Constants.MAX_VECTORVALUE) ? Vector2.right : (xValue < Constants.MIN_VECTORVALUE) ? Vector2.left : (yValue > Constants.MAX_VECTORVALUE) ? Vector2.up : (yValue < Constants.MIN_VECTORVALUE) ? Vector2.down : movementVector;
        return movementVector;
    }
    protected void OnGameEnds(Action OnGameEnds)
    {
        OnGameEnds.Invoke();
    }
    protected void PlaceTilePiecesinGame(GameObject gameGrid, Action<Vector2> PlaceBomb, Action<Vector2, Action<int>> PlaceTilePiece, Action<GridTile, Vector2> LeftVectorStatus, Action<GridTile, Vector2> RightVectorStatus, Action<GridTile, Vector2> UpVectorStatus, Action<GridTile, Vector2> DownVectorStatus, Action AutomaticTilePlaceCallback, Action<Vector2> SetPieceActive, Action activeInputs, Action RefreshHoldTilePlacement, Action<int> decrementScoreCallback)
    {
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            GridTile activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<GridTile>();
            if (activeTile.IsBomb)
            {
                if (activeTile.isLoopPiece)
                {
                    AutomaticTilePlaceCallback?.Invoke();
                    activeInputs?.Invoke();
                    Debug.Log("Check Looop Piece");
                    return;
                }//recentchanges24-06-22
                PlaceBomb.Invoke(activeTile.GridPosition);
                RefreshHoldTilePlacement?.Invoke();
                SetPieceActive?.Invoke(activeTile.GridPosition);
                
                return;
            }
            if (activeTile.isSelected)
            {
                if(activeTile.isLoopPiece)
                {
                    AutomaticTilePlaceCallback?.Invoke();
                    activeInputs?.Invoke();
                    Debug.Log("Check Looop Piece");
                    return;
                }//recentchanges24-06-22
                PlaceTilePiece?.Invoke(activeTile.GridPosition, decrementScoreCallback);
                RefreshHoldTilePlacement?.Invoke();
                SetPieceActive?.Invoke(activeTile.GridPosition);
                StartCoroutine(CheckAttachedPieceStatusSinglePlayer(LeftVectorStatus, RightVectorStatus, UpVectorStatus, DownVectorStatus, activeTile));
                return;
            }
        }
    }
    protected void PlaceTilePiecesinGameForFirstPlayer(GameObject gameGrid, Action<Vector2> PlaceBomb, Action<Vector2, Action<int>> PlaceTilePiece, Action<GridTilePlayer1, Vector2> LeftVectorStatus, Action<GridTilePlayer1, Vector2> RightVectorStatus, Action<GridTilePlayer1, Vector2> UpVectorStatus, Action<GridTilePlayer1, Vector2> DownVectorStatus, Action AutomaticTilePlaceCallback, Action<Vector2> SetPieceActive, Action activeInputs, Action RefreshHoldTilePlacement, Action<int> decrementScoreCallback)
    {
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            GridTilePlayer1 activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<GridTilePlayer1>();
            if (activeTile.IsBomb)
            {
                if (activeTile.isLoopPiece)
                {
                    AutomaticTilePlaceCallback?.Invoke();
                    activeInputs?.Invoke();
                    Debug.Log("Check Looop Piece");
                    return;
                }//recentchanges24-06-22
                PlaceBomb.Invoke(activeTile.GridPosition);
                RefreshHoldTilePlacement?.Invoke();
                SetPieceActive?.Invoke(activeTile.GridPosition);
                //AutomaticTilePlaceCallback?.Invoke();
                return;
            }
            if (activeTile.isSelected)
            {
                if (activeTile.isLoopPiece)
                {
                    AutomaticTilePlaceCallback?.Invoke();
                    activeInputs?.Invoke();
                    Debug.Log("Check Looop Piece");
                    return;
                }//recentchanges24-06-22
                PlaceTilePiece?.Invoke(activeTile.GridPosition, decrementScoreCallback);
                RefreshHoldTilePlacement?.Invoke();
                SetPieceActive?.Invoke(activeTile.GridPosition);
                //skchngesAutomaticTilePlaceCallback?.Invoke();\

                StartCoroutine(CheckAttachedPieceStatus_Player1(LeftVectorStatus, RightVectorStatus, UpVectorStatus, DownVectorStatus, activeTile));

                /* UpVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.up);
                 DownVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.down);
                 LeftVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.left);
                 RightVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.right);*/
                return;
            }
        }
    }
    protected void PlaceTilePiecesinGameForSecondPlayer(GameObject gameGrid, Action<Vector2> PlaceBomb, Action<Vector2, Action<int>> PlaceTilePiece, Action<GridTilePlayer2, Vector2> LeftVectorStatus, Action<GridTilePlayer2, Vector2> RightVectorStatus, Action<GridTilePlayer2, Vector2> UpVectorStatus, Action<GridTilePlayer2, Vector2> DownVectorStatus, Action AutomaticTilePlaceCallback, Action<Vector2> SetPieceActive, Action activeInputs, Action RefreshHoldTilePlacement, Action<int> decrementScoreCallbak)
    {
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            GridTilePlayer2 activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<GridTilePlayer2>();
            if (activeTile.IsBomb)
            {
                if (activeTile.isLoopPiece)
                {
                    AutomaticTilePlaceCallback?.Invoke();
                    activeInputs?.Invoke();
                    Debug.Log("Check Looop Piece");
                    return;
                }//recentchanges24-06-22
                PlaceBomb.Invoke(activeTile.GridPosition);
                RefreshHoldTilePlacement?.Invoke();
                SetPieceActive?.Invoke(activeTile.GridPosition);
                //skchnges  AutomaticTilePlaceCallback?.Invoke();
                return;
            }
            if (activeTile.isSelected)
            {
                if (activeTile.isLoopPiece)
                {
                    AutomaticTilePlaceCallback?.Invoke();
                    activeInputs?.Invoke();
                    Debug.Log("Check Looop Piece");
                    return;
                }//recentchanges24-06-22
                PlaceTilePiece?.Invoke(activeTile.GridPosition, decrementScoreCallbak);
                RefreshHoldTilePlacement?.Invoke();
                SetPieceActive?.Invoke(activeTile.GridPosition);
                //skchngesAutomaticTilePlaceCallback?.Invoke();
                StartCoroutine(CheckAttachedPieceStatus_Player2(LeftVectorStatus, RightVectorStatus, UpVectorStatus, DownVectorStatus, activeTile));

                /* UpVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.up);
                 DownVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.down);
                 LeftVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.left);
                 RightVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.right);*/
                return;
            }
        }
    }
    protected void PlaceUpcomingTile(GameObject mainGrid, GameObject sidePanelArea, GameObject UpcomingPanelParent, Action<TilePiece> tileAction, SidePieceTye[] PieceTypesContainer, Action<TilePiece, Action> SetUpcomingBombPositon, Action<TilePiece, TilePiece, Action> SetGridTilePosition, Action destroyCallback, Vector2 nextPositon, Action<Vector2, Action<TilePiece>> s_NextTileValidPosition)
    {
        Debug.Log("Upcoming Test");
        TilePiece tilePiece = sidePanelArea.transform.GetChild(0).GetComponent<TilePiece>();
        TilePiece upcomingTile = UpcomingPanelParent.transform.GetChild(0).GetComponent<TilePiece>();
        SetTilePieceInGrid(upcomingTile, mainGrid, SetUpcomingBombPositon, SetGridTilePosition, destroyCallback, nextPositon, s_NextTileValidPosition);
        ShuffleSidePieceContainer(PieceTypesContainer);
        AddSidePanelPiece(sidePanelArea, PieceTypesContainer);
        tileAction?.Invoke(tilePiece);
    }
    protected void PlaceUpcomingTileVirtually(GameObject mainGrid, GameObject sidePanelArea, GameObject UpcomingPanelParent, Action<TilePiece> tileAction, SidePieceTye[] PieceTypesContainer, Action<TilePiece, Action> SetUpcomingBombPositon, Action<TilePiece, TilePiece, Action> SetGridTilePosition, Action destroyCallback, Vector2 nextPositon, Action<Vector2, Action<TilePiece>> s_NextTileValidPosition)
    {
        TilePiece tilePiece = sidePanelArea.transform.GetChild(0).GetComponent<TilePiece>();
        TilePiece upcomingTile = UpcomingPanelParent.transform.GetChild(0).GetComponent<TilePiece>();
        SetTilePieceInGridVirtually(upcomingTile, mainGrid, SetUpcomingBombPositon, SetGridTilePosition, destroyCallback, nextPositon, s_NextTileValidPosition);
        ShuffleSidePieceContainer(PieceTypesContainer);
        AddSidePanelPiece(sidePanelArea, PieceTypesContainer);
        tileAction?.Invoke(tilePiece);
    }

    protected void PlaceStarttingPiece(GameObject mainGrid, int startingPieceCount, List<Vector2> startingPiecePosition, Action<Vector2, int, int> StartingPieceAction, int givenAngle)
    {
        mainGrid.GetComponent<GridLayoutGroup>().enabled = false;
       /* ShuffleStartingPiecePositions(startingPiecePosition);*/
        List<TilePiece> tileList = AddTilePiecesinContainer(mainGrid);
        int pieceIndex = 0;
        for (int startingPieceIndex = (int)Constants.ZERO; startingPieceIndex < startingPieceCount; startingPieceIndex++)
        {
            TilePiece piece = tileList.Find(element => (element.GridPosition == startingPiecePosition[startingPieceIndex]));
            //SetStartingPieceToLastIndex(mainGrid, piece.GridPosition);
            StartingPieceAction?.Invoke(piece.GridPosition, pieceIndex, givenAngle);
            pieceIndex++;
        }
    }
    protected void TriggerTileMovement(GameObject gameGrid, Action<Vector2> s_ActiveBomb, Action<Vector2> DeActiveTilePiece, Action<TilePiece, Vector2> ActiveTilePiece, Vector2 movementVector)
    {
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            TilePiece activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            Vector2 nextPosition;
            Vector2 activePosition;

            if (activeTile.IsBomb)
            {
                nextPosition = movementVector + activeTile.GridPosition;
                activePosition = (nextPosition.x > activeTile.GridPosition.x || nextPosition.y > activeTile.GridPosition.y) ? GetBombsForwardActivePosition(gameGrid, nextPosition, movementVector) : GetBombsBackwardActivePosition(gameGrid, nextPosition, movementVector);
                bool activeStatus = PositionStatusCheck(gameGrid, activePosition);
                if (activeStatus)
                {
                    s_ActiveBomb?.Invoke(activePosition);
                    return;
                }
                /*skchnagesbool activeStatus = BombPositionCheck(gameGrid, nextPosition);
                bool activeStatus = BombPositionCheck(gameGrid, activePosition);
                Vector2 callBackPosition = (activeStatus) ? nextPosition : nextPosition + movementVector;
                if (PositionStatusCheck(gameGrid, callBackPosition))
                {
                    s_ActiveBomb?.Invoke(callBackPosition);
                    return;
                }*/
                return;
            }
            if (activeTile.isSelected)
            {
                nextPosition = movementVector + activeTile.GridPosition;
                activePosition = (nextPosition.x > activeTile.GridPosition.x || nextPosition.y > activeTile.GridPosition.y) ? GetForwardActivePosition(gameGrid, nextPosition, movementVector) : GetBackwardActivePosition(gameGrid, nextPosition, movementVector);
                bool activeStatus = PositionStatusCheck(gameGrid, activePosition);
                if (activeStatus)
                {
                    DeActiveTilePiece?.Invoke(activeTile.GridPosition);
                    ActiveTilePiece?.Invoke(activeTile, activePosition);
                    return;
                }
            }
            /*if (activeTile.isSelected && activeTile.canPlace)
            {
                nextPosition = movementVector + activeTile.GridPosition;
                activePosition = (nextPosition.x > activeTile.GridPosition.x || nextPosition.y > activeTile.GridPosition.y) ? GetForwardActivePosition(gameGrid, nextPosition, movementVector) : GetBackwardActivePosition(gameGrid, nextPosition, movementVector);
                bool activeStatus = PositionStatusCheck(gameGrid, activePosition);
                if (activeStatus)
                {
                    DeActiveTilePiece?.Invoke(activeTile.GridPosition);
                    ActiveTilePiece?.Invoke(activeTile, activePosition);
                    return;
                }
            }*/
        }
    }

    protected void SetInitialUpcomingTile(UpcomingTile upcomingTile, GameObject parent, GameObject pieceListParent, SidePieceTye[] container)
    {
        Debug.Log("Initial Upcoming tile generated");
        DestroyTileArea(parent);
        List<SidePieceTye> containerList = new List<SidePieceTye>();
        containerList.AddRange(container);
        /* TilePiece tile = pieceListParent.transform.GetChild(Constants.INT_ZERO).GetComponent<TilePiece>();
         int tileIndex = containerList.FindIndex(element => element != (SidePieceTye)tile.ActiveType);
         int randomValue = Random.Range(Constants.INT_ONE, container.Length);*/
        TilePiece tile = pieceListParent.transform.GetChild(container.Length - Constants.INT_ONE).GetComponent<TilePiece>();
        int tileIndex = containerList.FindIndex(element => element == (SidePieceTye)tile.ActiveType);
        InstantiateUpcomingTile(upcomingTile, parent, container, tileIndex);
    }
    protected void SetInitialUpcomingTilePlayer1(UpcomingTilePlayer1 upcomingTile, GameObject parent, GameObject pieceListParent, SidePieceTye[] container)
    {
        DestroyTileArea(parent);
        List<SidePieceTye> containerList = new List<SidePieceTye>();
        containerList.AddRange(container);
       /* TilePiece tile = pieceListParent.transform.GetChild(Constants.INT_ZERO).GetComponent<TilePiece>();
        int tileIndex = containerList.FindIndex(element => element != (SidePieceTye)tile.ActiveType);
        int randomValue = Random.Range(0, container.Length);*/
        TilePiece tile = pieceListParent.transform.GetChild(container.Length - Constants.INT_ONE).GetComponent<TilePiece>();
        int tileIndex = containerList.FindIndex(element => element == (SidePieceTye)tile.ActiveType);
        InstantiateUpcomingTilePlayer1(upcomingTile, parent, container, tileIndex);
    }
    protected void SetInitialUpcomingTilePlayer2(UpcomingTilePlayer2 upcomingTile, GameObject parent, GameObject pieceListParent, SidePieceTye[] container)
    {
        DestroyTileArea(parent);
        List<SidePieceTye> containerList = new List<SidePieceTye>();
        containerList.AddRange(container);
       /* TilePiece tile = pieceListParent.transform.GetChild(Constants.INT_ZERO).GetComponent<TilePiece>();
        int tileIndex = containerList.FindIndex(element => element != (SidePieceTye)tile.ActiveType);
        int randomValue = Random.Range(0, container.Length);*/
        TilePiece tile = pieceListParent.transform.GetChild(container.Length - Constants.INT_ONE).GetComponent<TilePiece>();
        int tileIndex = containerList.FindIndex(element => element == (SidePieceTye)tile.ActiveType);
        InstantiateUpcomingTilePlayer2(upcomingTile, parent, container, tileIndex);
    }
    protected void AddSidePanelPiece(GameObject parent, SidePieceTye[] container)
    {
        Debug.Log("Side Panel Piece added");
        List<SidePieceTye> tempSidePieces = new List<SidePieceTye>();
        tempSidePieces.AddRange(container);
        /* TilePiece tile = parent.transform.GetChild(parent.transform.childCount - Constants.INT_ONE).GetComponent<TilePiece>();
         int pieceIndex = tempSidePieces.FindIndex(element => element != (SidePieceTye)tile.ActiveType);
         int randomValue = Random.Range(0, container.Length);*/
        //skchnagesInstantiateSideTile(parent, container, pieceIndex);
        TilePiece tile = parent.transform.GetChild(Constants.INT_ZERO).GetComponent<TilePiece>();
        int pieceIndex = tempSidePieces.FindIndex(element => element == (SidePieceTye)tile.ActiveType);
        InstantiateSideTile(parent, container, pieceIndex);
    }
    protected void FillSidePieceContainer(GameObject parent, SidePieceTye[] container)
    {
        DestroyTileArea(parent);
        for (int containerIndex = 0; containerIndex < container.Length; containerIndex++)
        {
            InstantiateSideTile(parent, container, containerIndex);
        }
        ShuffleSidePieceContainer(container);
        for (int containerIndex = 0; containerIndex < container.Length; containerIndex++)
        {
            InstantiateSideTile(parent, container, containerIndex);
        }
        ShuffleSidePieceContainer(container);
        for (int containerIndex = 0; containerIndex < container.Length; containerIndex++)
        {
            InstantiateSideTile(parent, container, containerIndex);
        }
    }

    protected void AllocateSidePieceContainer(SidePieceTye[] container)
    {
        int containerIndex = (int)Constants.ZERO;
        for (int sidePieceIndex = (int)Constants.ZERO; sidePieceIndex < p_SidePiecePercentages.Count; sidePieceIndex++)
        {
            int pieceOutcome = p_SidePiecePercentages[sidePieceIndex];
            while (pieceOutcome > (int)Constants.ZERO)
            {
                container[containerIndex] = (SidePieceTye)sidePieceIndex;
                containerIndex++;
                pieceOutcome--;
            }
        }
    }
    /*protected void ShuffleSidePieceContainer(SidePieceTye[] container)
    {
        for (int containerIndex = (int)Constants.ZERO; containerIndex < container.Length; containerIndex++)
        {
            int currentIndex = 0;
           for(int containerInnerIndex = containerIndex + Constants.INT_ONE; containerInnerIndex < container.Length; containerInnerIndex++)
            {
                currentIndex++;
                if (container[containerIndex] == container[containerInnerIndex])
                {
                    break;
                }
            }
        }
    }*/
    protected void ShuffleSidePieceContainer(SidePieceTye[] container)
    {
        for (int containerIndex = (int)Constants.ZERO; containerIndex < container.Length; containerIndex++)
        {
            SidePieceTye tempPiece = (SidePieceTye)container[containerIndex];
            SidePieceTye randomPiece = (SidePieceTye)Random.Range(containerIndex, container.Length);
            container[containerIndex] = container[(int)randomPiece];
            container[(int)randomPiece] = tempPiece;
        }
    }

    protected void RemoveConsecutivePieceFromContainer(SidePieceTye[] container)
    {
        for (int containerIndex = (int)Constants.ZERO; containerIndex < container.Length - Constants.INT_ONE; containerIndex++)
        {
            if (container[containerIndex] == container[containerIndex + Constants.INT_ONE])
            {
                int containerInnerIndex;
                for (containerInnerIndex = containerIndex + Constants.INT_ONE; containerInnerIndex < container.Length - Constants.INT_ONE; containerInnerIndex++)
                {
                    if (container[containerIndex] != container[containerInnerIndex])
                    {
                        break;
                    }
                }
                SidePieceTye temp = container[containerInnerIndex];
                container[containerInnerIndex] = container[containerIndex + Constants.INT_ONE];
                container[containerIndex + Constants.INT_ONE] = temp;
            }
        }
    }

    protected float CalculateTileSize(int gridSize)
    {
        float tilesize = gridSize / GameManager.instace.SidelengthSinglePlayers;
        return tilesize;
    }

    protected float CalculateTileSizeX(int gridSize, int rowLength)
    {
        float tilesize = gridSize / rowLength;
        return tilesize;
    }
    protected float CalculateTileSizeY(int gridSize, int columnLength)
    {
        float tilesize = gridSize / columnLength;
        return tilesize;
    }
    protected void SideLengthCallBack(int sideLength)
    {
        s_SideLengthChanges?.Invoke(sideLength);
    }
    protected void PiecePercentages()
    {
        GameData.Instance.PiecePercaentages = PieceDistributionManager.instance.LoadFile();
        /*skchnages int lTypePiece1 = (int)Mathf.Ceil(LtypePiece_1 * Constants.TOTAL_SIDEPANELPIECES);
         int lTypePiece2 = (int)Mathf.Ceil(LtypePiece_2 * Constants.TOTAL_SIDEPANELPIECES);
         int straightTypepiece = (int)Mathf.Ceil(StraighttypePiece * Constants.TOTAL_SIDEPANELPIECES);
         int crossTypepiece = (int)Mathf.Ceil(CrosstypePiece * Constants.TOTAL_SIDEPANELPIECES);
         int bombTypePiece = (int)Mathf.Ceil(BombtypePiece * Constants.TOTAL_SIDEPANELPIECES);
         int inversionTypePiece = (int)Mathf.Ceil(InversiontypePiece * Constants.TOTAL_SIDEPANELPIECES);*/
        int lTypePiece1 = GameData.Instance.PiecePercaentages.LTYPEPIECE_1;
        int lTypePiece2 = GameData.Instance.PiecePercaentages.LTYPEPIECE_2;
        int straightTypepiece = GameData.Instance.PiecePercaentages.STRAIGHTTYPEPIECE;
        int crossTypepiece = GameData.Instance.PiecePercaentages.CROSSTYPEPIECE;
        int bombTypePiece = GameData.Instance.PiecePercaentages.BOMBTYPEPIECE;
        int inversionTypePiece = Constants.INT_ZERO;
        Debug.Log("Piece Action lTypePiece1 " + lTypePiece1+ " Pobability "+ LtypePiece_1);
        Debug.Log("Piece Action lTypePiece2 " + lTypePiece2 + " Pobability " + LtypePiece_2);
        Debug.Log("Piece Action Cross Type Piece " + crossTypepiece + " Pobability " + CrosstypePiece);
        Debug.Log("Piece Action straight " + straightTypepiece + " Pobability " + StraighttypePiece);
        Debug.Log("Piece Action Bomb " + bombTypePiece + " Pobability " + BombtypePiece);
        CreateSidePiecePool(lTypePiece1, lTypePiece2, straightTypepiece, crossTypepiece, bombTypePiece, inversionTypePiece);
        int probabiltySum = GetProbabiltySum();
        Action probabiltyAction;
        if (probabiltySum > Constants.TOTAL_SIDEPANELPIECES)
            probabiltyAction = DecrementProbabilitySum;
        else
            probabiltyAction = IncrementProbabilitySum;
        probabiltyAction?.Invoke();
    }

    protected void CreateSidePiecePool(int ltypepiece1, int ltypePiece2, int straightPiece, int crossTypePiece, int bombTypePiece, int inversionPiece)
    {
        p_SidePiecePercentages.Add(ltypepiece1);
        p_SidePiecePercentages.Add(ltypePiece2);
        p_SidePiecePercentages.Add(straightPiece);
        p_SidePiecePercentages.Add(crossTypePiece);
        p_SidePiecePercentages.Add(bombTypePiece);
        p_SidePiecePercentages.Add(inversionPiece);
    }
    protected void DecrementSidePiecePool(int delta)
    {
        for (int poolIndex = (int)Constants.ZERO; poolIndex < p_SidePiecePercentages.Count; poolIndex++)
        {
            p_SidePiecePercentages[poolIndex] = (p_SidePiecePercentages[poolIndex] > delta) ? p_SidePiecePercentages[poolIndex] - delta : p_SidePiecePercentages[poolIndex];
        }
    }
    protected void IncrementPieceProbability(int delta)
    {
        int poolCount = Mathf.Abs(delta);
        int maxItem = Mathf.Max(p_SidePiecePercentages.ToArray());
        int targetIndex = p_SidePiecePercentages.IndexOf(maxItem);
        while (poolCount != 0)
        {
            maxItem = Mathf.Max(p_SidePiecePercentages.ToArray());
            targetIndex = p_SidePiecePercentages.IndexOf(maxItem);
            p_SidePiecePercentages[targetIndex] = (delta < Constants.ZERO) ? p_SidePiecePercentages[targetIndex] - 1 : p_SidePiecePercentages[targetIndex] + 1;
            poolCount--;
        }
    }
    protected void DecrementProbabilitySum()
    {
        int diffrence = GetProbabiltySum() - Constants.TOTAL_SIDEPANELPIECES;
        float delta = Mathf.Ceil(diffrence / Constants.Total_Pieces);
        DecrementSidePiecePool((int)delta);
        int probabiltySum = GetProbabiltySum();
        Action<int> probabilityAction;
        if (probabiltySum == Constants.TOTAL_SIDEPANELPIECES)
            return;
        else
            probabilityAction = IncrementPieceProbability;
        delta = Constants.TOTAL_SIDEPANELPIECES - GetProbabiltySum();
        probabilityAction?.Invoke((int)delta);
    }

    protected void IncrementProbabilitySum()
    {
        int probabiltySum = GetProbabiltySum();
        int difference = Constants.TOTAL_SIDEPANELPIECES - probabiltySum;
        Action<int> probabiltyAction;
        if (probabiltySum == Constants.TOTAL_SIDEPANELPIECES)
            return;
        else
            probabiltyAction = IncrementPieceProbability;
        probabiltyAction?.Invoke(difference);
    }

    protected int GetProbabiltySum()
    {
        int probabiltySum = (int)Constants.ZERO;
        foreach (int piece in p_SidePiecePercentages)
        {
            probabiltySum += piece;
        }
        Debug.Log("The probabilty sum is: " + probabiltySum);
        return probabiltySum;
    }

    protected void AllocateGridSize(GameObject grid, float tileSize)
    {
        grid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(tileSize, tileSize);
    }

    protected void AllocateSizeToGrid(GameObject grid, float size, RectTransform BgImage)
    {
        grid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(size * GameData.Instance.GameOptions.SinglePlayerGameColumns, size * GameData.Instance.GameOptions.SinglePlayerGameRows);
        RectTransform playAreaRect = grid.GetComponent<RectTransform>();
        BgImage.sizeDelta = new Vector2(playAreaRect.sizeDelta.x + Constants.BG_XDELTA, playAreaRect.sizeDelta.y + Constants.BG_YDELTA);
    }
    protected void AllocateSizeToGridTwoPlayerMode(GameObject grid, float size, RectTransform BgImage)
    {
        grid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(size * GameData.Instance.GameOptions.TwoPlayerGameColumn, size * GameData.Instance.GameOptions.TwoPlayerGameRows);
        RectTransform playAreaRect = grid.GetComponent<RectTransform>();
        BgImage.sizeDelta = new Vector2(playAreaRect.sizeDelta.x + Constants.BG_TWOPLAYER_XDELTA, playAreaRect.sizeDelta.y + Constants.BG_TWOPLATER_YDELTA);
    }



    protected void DestroyTileArea(GameObject grid)
    {
        for (int tileindex = 0; tileindex < grid.transform.childCount; tileindex++)
        {
            Destroy(grid.transform.GetChild(tileindex).gameObject);
        }
    }

    protected void GenerateTileArea(GameObject grid, GridTile maintile)
    {
        int xPosition = (int)Constants.ZERO;
        int yPosition = (int)Constants.ZERO;
        Vector2 piecePosition = new Vector3(xPosition, yPosition);
        for (int tileindexX = 0; tileindexX < GameManager.instace.SidelengthSinglePlayers * GameManager.instace.SidelengthSinglePlayers; tileindexX++)
        {
            piecePosition = (piecePosition.x > (GameManager.instace.SidelengthSinglePlayers - 1)) ? new Vector2(Constants.ZERO, yPosition + 1) : piecePosition;
            xPosition = (int)piecePosition.x;
            yPosition = (int)piecePosition.y;
            GridTile tile = Instantiate(maintile, grid.transform) as GridTile;
            tile.gameObject.name = "Tile " + piecePosition.x + piecePosition.y;
            //skchnages   tile.CreateGridTile(piecePosition, false, false);
            xPosition++;
            piecePosition = new Vector2(xPosition, yPosition);

        }
    }

    protected void GenerateTileAreaTest(GameObject grid, GridTile maintile)
    {
        int xPosition = (int)Constants.ZERO;
        int yPosition = (int)Constants.ZERO;
        Vector2 piecePosition = new Vector3(xPosition, yPosition);

        for (int tileindexY = 0; tileindexY < GameData.Instance.GameOptions.SinglePlayerGameRows; tileindexY++)
        {
            for (int tileindexX = 0; tileindexX < GameData.Instance.GameOptions.SinglePlayerGameColumns; tileindexX++)
            {
                piecePosition = new Vector2(tileindexX, tileindexY);
                GridTile tile = Instantiate(maintile, grid.transform) as GridTile;
                tile.gameObject.name = "Tile " + piecePosition.x + piecePosition.y;
                tile.CreateGridTile(piecePosition, false, false, (GameData.Instance.GameOptions.SinglePlayerGameRows / Constants.INT_TWO), (GameData.Instance.GameOptions.SinglePlayerGameColumns / Constants.INT_TWO));
            }
        }
    }

    protected void GenerateTileAreaPlayer1(GameObject grid, GridTilePlayer1 maintile)
    {
        int xPosition = (int)Constants.ZERO;
        int yPosition = (int)Constants.ZERO;
        Vector2 piecePosition = new Vector3(xPosition, yPosition);

        for (int tileindexY = 0; tileindexY < GameData.Instance.GameOptions.TwoPlayerGameRows; tileindexY++)
        {
            for (int tileindexX = 0; tileindexX < GameData.Instance.GameOptions.TwoPlayerGameColumn; tileindexX++)
            {
                piecePosition = new Vector2(tileindexX, tileindexY);
                GridTilePlayer1 tile = Instantiate(maintile, grid.transform) as GridTilePlayer1;
                tile.gameObject.name = "Tile " + piecePosition.x + piecePosition.y;
                tile.CreateGridTile(piecePosition, true, false, (GameData.Instance.GameOptions.TwoPlayerGameRows / Constants.INT_TWO), (GameData.Instance.GameOptions.TwoPlayerGameColumn / Constants.INT_TWO));
            }
        }
    }

    protected void GenerateTileAreaPlayer2(GameObject grid, GridTilePlayer2 maintile)
    {
        int xPosition = (int)Constants.ZERO;
        int yPosition = (int)Constants.ZERO;
        Vector2 piecePosition = new Vector3(xPosition, yPosition);

        for (int tileindexY = 0; tileindexY < GameData.Instance.GameOptions.TwoPlayerGameRows; tileindexY++)
        {
            for (int tileindexX = 0; tileindexX < GameData.Instance.GameOptions.TwoPlayerGameColumn; tileindexX++)
            {
                piecePosition = new Vector2(tileindexX, tileindexY);
                GridTilePlayer2 tile = Instantiate(maintile, grid.transform) as GridTilePlayer2;
                tile.gameObject.name = "Tile " + piecePosition.x + piecePosition.y;
                tile.CreateGridTile(piecePosition, false, true, (GameData.Instance.GameOptions.TwoPlayerGameRows / Constants.INT_TWO), (GameData.Instance.GameOptions.TwoPlayerGameColumn / Constants.INT_TWO));
            }
        }
    }

    /* protected void GenerateTileArea(GameObject grid, GridTile maintile)
     {
         int xPosition = (int)Constants.ZERO;
         int yPosition = (int)Constants.ZERO;
         Vector2 piecePosition = new Vector3(xPosition, yPosition);
         for (int tileindexX = 0; tileindexX <   GameManager.instace.Sidelength * GameManager.instace.Sidelength; tileindexX++)
         {
             piecePosition = (piecePosition.x > (GameManager.instace.Sidelength - 1)) ? new Vector2(Constants.ZERO, yPosition + 1) : piecePosition;
             xPosition = (int)piecePosition.x;
             yPosition = (int)piecePosition.y;
             GridTile tile = Instantiate(maintile, grid.transform) as GridTile;
             tile.gameObject.name = "Tile " + piecePosition.x + piecePosition.y;
             tile.CreateGridTile(piecePosition, false, false);
             xPosition++;
             piecePosition = new Vector2(xPosition, yPosition);

         }
     }*/
    /* protected void GenerateTileAreaPlayer1(GameObject grid, GridTilePlayer1 maintile)
     {
         int xPosition = (int)Constants.ZERO;
         int yPosition = (int)Constants.ZERO;
         Vector2 piecePosition = new Vector3(xPosition, yPosition);
         for (int tileindexX = 0; tileindexX < GameManager.instace.Sidelength * GameManager.instace.Sidelength; tileindexX++)
         {
             piecePosition = (piecePosition.x > (GameManager.instace.Sidelength - 1)) ? new Vector2(Constants.ZERO, yPosition + 1) : piecePosition;
             xPosition = (int)piecePosition.x;
             yPosition = (int)piecePosition.y;
             GridTilePlayer1 tile = Instantiate(maintile, grid.transform) as GridTilePlayer1;
             tile.gameObject.name = "Tile " + piecePosition.x + piecePosition.y;
             tile.CreateGridTile(piecePosition, true, false);
             xPosition++;
             piecePosition = new Vector2(xPosition, yPosition);

         }
     }*/
    /* //for checking the game area has a empty tile or not during placing a upcoming tile
     protected void CheckGamePlayAreaStatus(GameObject mainGrid)
     {
         for (int gridIndex = 0; gridIndex < mainGrid.transform.childCount; gridIndex++)
         {
             TilePiece gridTile = mainGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
             if (!gridTile.isSelected && gridTile.canPlace && !tile.IsBomb)
             {
                 SetGridTilePosition?.Invoke(gridTile, tile, destroyCallback);
                 return;
             }
         }
     }*/
    /* protected void GenerateTileAreaPlayer2(GameObject grid, GridTilePlayer2 maintile)
     {
         int xPosition = (int)Constants.ZERO;
         int yPosition = (int)Constants.ZERO;
         Vector2 piecePosition = new Vector3(xPosition, yPosition);
         for (int tileindexX = 0; tileindexX < GameManager.instace.Sidelength * GameManager.instace.Sidelength; tileindexX++)
         {
             piecePosition = (piecePosition.x > (GameManager.instace.Sidelength - 1)) ? new Vector2(Constants.ZERO, yPosition + 1) : piecePosition;
             xPosition = (int)piecePosition.x;
             yPosition = (int)piecePosition.y;
             GridTilePlayer2 tile = Instantiate(maintile, grid.transform) as GridTilePlayer2;
             tile.gameObject.name = "Tile " + piecePosition.x + piecePosition.y;
             tile.CreateGridTile(piecePosition, false, true);
             xPosition++;
             piecePosition = new Vector2(xPosition, yPosition);

         }
     }*/

    //Obstacle Spawn code for single player and two players
    protected void CreateObstacleIgnoredArea(GameObject playArea, List<Vector2> ObstacleIgnoredArea)
    {
        for (int obstacleIndex = (int)Constants.ZERO; obstacleIndex < playArea.transform.childCount; obstacleIndex++)
        {
            TilePiece startingPieceTile = playArea.transform.GetChild(obstacleIndex).GetComponent<TilePiece>();
            if (startingPieceTile.IsStartingPiece)
            {
                ObstacleIgnoredArea.Add(startingPieceTile.GridPosition);
                for (int startingPieceSurroundingVectorIndex = (int)Constants.ZERO; startingPieceSurroundingVectorIndex < GameData.Instance.StartingPieceSafeAreaVectors.Count; startingPieceSurroundingVectorIndex++)
                {
                    ObstacleIgnoredArea.Add(startingPieceTile.GridPosition + GameData.Instance.StartingPieceSafeAreaVectors[startingPieceSurroundingVectorIndex]);
                }
                /* for (int startingPieceSurroundingVectorIndex = (int)Constants.ZERO; startingPieceSurroundingVectorIndex < GameData.Instance.StartingPieceSurroundingVectors.Count; startingPieceSurroundingVectorIndex++)
                 {
                     ObstacleIgnoredArea.Add(startingPieceTile.GridPosition + GameData.Instance.StartingPieceSurroundingVectors[startingPieceSurroundingVectorIndex]);
                 }*/
            } 
            
            if(startingPieceTile.isSelected || !startingPieceTile.canPlace || startingPieceTile.IsBomb)
            {
                ObstacleIgnoredArea.Add(startingPieceTile.GridPosition);
            }
        }
    }

    protected void CreateObstacleSpawnArea(GameObject playArea, List<Vector2> ObstacleIgnoredArea, List<TilePiece> ObstacleSpawnArea)
    {
        for (int obstacleIndex = (int)Constants.ZERO; obstacleIndex < playArea.transform.childCount; obstacleIndex++)
        {
            TilePiece startingPieceTile = playArea.transform.GetChild(obstacleIndex).GetComponent<TilePiece>();
            int index = ObstacleIgnoredArea.FindIndex(tilePosition => (tilePosition == startingPieceTile.GridPosition));
            if (index == -1)
            {
                ObstacleSpawnArea.Add(startingPieceTile);
            }
        }
    }
    protected void ShuffleObstacleArea(List<TilePiece> tiles)
    {
        for (int obstacleIndex = (int)Constants.ZERO; obstacleIndex < tiles.Count; obstacleIndex++)
        {
            TilePiece tempTile = tiles[obstacleIndex];
            int randomPiece = Random.Range(obstacleIndex, tiles.Count);
            tiles[obstacleIndex] = tiles[(int)randomPiece];
            tiles[randomPiece] = tempTile;
        }
    }

    protected void ShuffleTwoPlayerObstacleArea(List<TilePiece> Player1Area, List<TilePiece> Player2Area)
    {
        for (int obstacleIndex = (int)Constants.ZERO; obstacleIndex < Player1Area.Count; obstacleIndex++)
        {
            TilePiece tempTile = Player1Area[obstacleIndex];
            TilePiece tempTile2 = Player2Area[obstacleIndex];
            int randomPiece = Random.Range(obstacleIndex, Player1Area.Count - Constants.INT_ONE);
            Debug.Log("Random Index: " + randomPiece);
            Debug.Log("Obstacle Index: "+obstacleIndex);
            Player1Area[obstacleIndex] = Player1Area[randomPiece];
            Player2Area[obstacleIndex] = Player2Area[randomPiece];
            Player1Area[randomPiece] = tempTile;
            Player2Area[randomPiece] = tempTile2;
        }
    }
    protected void SpawnRoundObstacles(List<TilePiece> ObstacleSpawnArea, Action<Vector2> OnRoundObstacleSpawn, int ObstacleCount)
    {
       // ShuffleObstacleArea(ObstacleSpawnArea);
        for (int roundObstacleIndex = (int)Constants.ZERO; roundObstacleIndex < ObstacleCount; roundObstacleIndex++)
        {
            TilePiece roundObstacle = ObstacleSpawnArea[roundObstacleIndex];
            OnRoundObstacleSpawn?.Invoke(roundObstacle.GridPosition);
            int obstacleIndex = ObstacleSpawnArea.FindIndex(element => element.GridPosition == roundObstacle.GridPosition);
            ObstacleSpawnArea.RemoveAt(obstacleIndex);
         //   ShuffleObstacleArea(ObstacleSpawnArea);
        }
    }

    protected void ThrowObstacles(List<TilePiece> ObstacleSpawnArea, Action<Vector2> OnRoundObstacleSpawn, int startingPieceCount, int loopLength)
    {
        if (startingPieceCount > Constants.INT_ZERO && ObstacleSpawnArea.Count > Constants.ZERO)
        {
            Debug.Log("Obstacle Thrown successfully");
            ThrowObstacles obstacleCount = GameData.Instance.ThrownObstacles.Find(element => loopLength >= element.MinLoopLength && loopLength <= element.MaxLoopLength);
            Debug.Log("Obstacle Thrown successfully Count: " + obstacleCount.ObstaceleCount);
            ShuffleObstacleArea(ObstacleSpawnArea);
            for (int roundObstacleIndex = (int)Constants.ZERO; roundObstacleIndex < obstacleCount.ObstaceleCount; roundObstacleIndex++)
            {
                TilePiece roundObstacle = ObstacleSpawnArea[roundObstacleIndex];
                OnRoundObstacleSpawn?.Invoke(roundObstacle.GridPosition);
                int obstacleIndex = ObstacleSpawnArea.FindIndex(element => element.GridPosition == roundObstacle.GridPosition);
                ObstacleSpawnArea.RemoveAt(obstacleIndex);
                ShuffleObstacleArea(ObstacleSpawnArea);
            }
        }
    }

    protected void SpawnHorizonatalObstacles(List<TilePiece> ObstacleSpawnArea, Action<Vector2, Vector2> OnHorizontalObstacleSpawn)
    {
        ShuffleObstacleArea(ObstacleSpawnArea);
        int pieceCount = GameManager.instace.HorizontalObstacleCount;
        for (int obstacleIndex = (int)Constants.ZERO; obstacleIndex < ObstacleSpawnArea.Count; obstacleIndex++)
        {
            TilePiece horizontalObstacle = ObstacleSpawnArea[obstacleIndex];
            Vector2 secondPiecePosition = horizontalObstacle.GridPosition + GameData.Instance.Obstacle[Constants.HORIZONTAL_OBSTACLE].ObstaclePiecePosition[Constants.HORIZONTAL_PIECE1];
            bool piece2Status = CheckObstacleStatus(secondPiecePosition, ObstacleSpawnArea);
            if (piece2Status && pieceCount > (int)Constants.ZERO)
            {
                OnHorizontalObstacleSpawn?.Invoke(horizontalObstacle.GridPosition, secondPiecePosition);
                int piece1Index = ObstacleSpawnArea.FindIndex(element => element.GridPosition == horizontalObstacle.GridPosition);
                ObstacleSpawnArea.RemoveAt(piece1Index);
                int piece2Index = ObstacleSpawnArea.FindIndex(element => element.GridPosition == secondPiecePosition);
                ObstacleSpawnArea.RemoveAt(piece2Index);
                ShuffleObstacleArea(ObstacleSpawnArea);
                pieceCount--;
            }
        }

    }
    protected void SpawnVerticalObstacles(List<TilePiece> ObstacleSpawnArea, Action<Vector2, Vector2, Vector2> OnVerticalObstacleSpawn)
    {
        ShuffleObstacleArea(ObstacleSpawnArea);
        int pieceCount = GameManager.instace.VerticalObstacleCount;
        for (int obstacleIndex = (int)Constants.ZERO; obstacleIndex < ObstacleSpawnArea.Count; obstacleIndex++)
        {
            TilePiece verticalObstacle = ObstacleSpawnArea[obstacleIndex];
            Vector2 secondPiecePosition = verticalObstacle.GridPosition + GameData.Instance.Obstacle[Constants.VERTICAL_OBSTACLE].ObstaclePiecePosition[Constants.VERTICAL_PIECE1];
            Vector2 thirdPiecePosition = verticalObstacle.GridPosition + GameData.Instance.Obstacle[Constants.VERTICAL_OBSTACLE].ObstaclePiecePosition[Constants.VERTICAL_PIECE2];
            bool piece2Status = CheckObstacleStatus(secondPiecePosition, ObstacleSpawnArea);
            bool piece3Status = CheckObstacleStatus(thirdPiecePosition, ObstacleSpawnArea);
            if (piece2Status && piece3Status && pieceCount > (int)Constants.ZERO)
            {
                OnVerticalObstacleSpawn?.Invoke(verticalObstacle.GridPosition, secondPiecePosition, thirdPiecePosition);
                int piece1Index = ObstacleSpawnArea.FindIndex(element => element.GridPosition == verticalObstacle.GridPosition);
                ObstacleSpawnArea.RemoveAt(piece1Index);
                int piece2Index = ObstacleSpawnArea.FindIndex(element => element.GridPosition == secondPiecePosition);
                ObstacleSpawnArea.RemoveAt(piece2Index);
                int piece3Index = ObstacleSpawnArea.FindIndex(element => element.GridPosition == thirdPiecePosition);
                ShuffleObstacleArea(ObstacleSpawnArea);
                pieceCount--;
            }
        }

    }


    #endregion

    #region Private_Methods
    private bool CheckObstacleStatus(Vector2 position, List<TilePiece> ObstacleSpawnArea)
    {
        bool status = false;
        int index = ObstacleSpawnArea.FindIndex(element => element.GridPosition == position);
        status = (index != Constants.NULLINDEX) ? true : status;
        return status;
    }

    private void SetStartingPieceToLastIndex(GameObject grid, Vector2 startingPiecePosition)
    {
        for (int tileIndex = (int)Constants.ZERO; tileIndex < grid.transform.childCount; tileIndex++)
        {
            TilePiece tile = grid.transform.GetChild(tileIndex).GetComponent<TilePiece>();
            if (tile.GridPosition == startingPiecePosition)
            {
                grid.transform.GetChild(tileIndex).SetAsLastSibling();
            }
        }
    }

    protected void ShuffleStartingPiecePositions(List<Vector2> container)
    {
        for (int containerIndex = (int)Constants.ZERO; containerIndex < container.Count; containerIndex++)
        {
            Vector2 tempPiece = container[containerIndex];
            int randomPiece = Random.Range(containerIndex, container.Count);
            container[containerIndex] = container[randomPiece];
            container[randomPiece] = tempPiece;
        }
    }
    private List<TilePiece> AddTilePiecesinContainer(GameObject tileParent)
    {
        List<TilePiece> tilelist = new List<TilePiece>();
        for (int tileIndex = (int)Constants.ZERO; tileIndex < tileParent.transform.childCount; tileIndex++)
        {
            TilePiece Tile = tileParent.transform.GetChild(tileIndex).GetComponent<TilePiece>();
            tilelist.Add(Tile);
        }
        return tilelist;
    }
    private void InstantiateSideTile(GameObject parent, SidePieceTye[] container, int containerIndex)
    {
        
        TilePiece tile = Instantiate(m_TilePrefab, parent.transform) as TilePiece;
        tile.TileType.PieceImage = p_TileImages[(int)container[containerIndex]];
        tile.TileType.Type = (int)container[containerIndex];
        //skchnagestile.TileType.Angle = m_TileAngles[(int)Random.Range(Constants.ZERO, Constants.POSSIBLEANGLE_COUNT)];
        tile.TileType.Angle = Constants.ZERO;
        Debug.Log("InstantiateTile: " + tile.TileType.Angle);
        tile.CreateTilePiece(p_TileImages[(int)container[containerIndex]], (int)container[containerIndex], Constants.ZERO);
    }
    private void InstantiateUpcomingTile(UpcomingTile upcomingTile, GameObject parent, SidePieceTye[] container, int containerIndex)
    {
        UpcomingTile tile = Instantiate(upcomingTile, parent.transform) as UpcomingTile;
        tile.TileType.PieceImage = p_TileImages[(int)container[containerIndex]];
        tile.TileType.Type = (int)container[containerIndex];
        tile.TileType.Angle = Constants.ZERO;
        tile.CreateTilePiece(p_TileImages[(int)container[containerIndex]], (int)container[containerIndex], Constants.ZERO);
    }
    private void InstantiateUpcomingTilePlayer1(UpcomingTilePlayer1 upcomingTile, GameObject parent, SidePieceTye[] container, int containerIndex)
    {
        UpcomingTilePlayer1 tile = Instantiate(upcomingTile, parent.transform) as UpcomingTilePlayer1;
        tile.TileType.PieceImage = p_TileImages[(int)container[containerIndex]];
        tile.TileType.Type = (int)container[containerIndex];
        tile.TileType.Angle = Constants.ZERO;
        tile.CreateTilePiece(p_TileImages[(int)container[containerIndex]], (int)container[containerIndex], Constants.ZERO);

        //skchnages tile.CreateTilePiece();
    }
    private void InstantiateUpcomingTilePlayer2(UpcomingTilePlayer2 upcomingTile, GameObject parent, SidePieceTye[] container, int containerIndex)
    {
        UpcomingTilePlayer2 tile = Instantiate(upcomingTile, parent.transform) as UpcomingTilePlayer2;
        tile.TileType.PieceImage = p_TileImages[(int)container[containerIndex]];
        tile.TileType.Type = (int)container[containerIndex];
        tile.TileType.Angle = Constants.ZERO;
        tile.CreateTilePiece(p_TileImages[(int)container[containerIndex]], (int)container[containerIndex], Constants.ZERO);

        //skchnages tile.CreateTilePiece();
    }

    private void SetTilePieceInGridVirtually(TilePiece tile, GameObject mainGrid, Action<TilePiece, Action> SetUpcomingBombPositon, Action<TilePiece, TilePiece, Action> SetGridTilePosition, Action destroyCallback, Vector2 nextPosition, Action<Vector2, Action<TilePiece>> s_NextTileValidPosition)
    {
        for (int gridIndex = 0; gridIndex < mainGrid.transform.childCount; gridIndex++)
        {
            TilePiece gridTile = mainGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            if (tile.IsBomb && gridTile.IsCurrentPosition)
            {
                SetUpcomingBombPositon?.Invoke(gridTile, destroyCallback);
                return;
            }
            if (!gridTile.IsObstacle && gridTile.IsCurrentPosition)
            {
                SetGridTilePosition?.Invoke(gridTile, tile, destroyCallback);
                return;
            }
            /*skchnagesif (!gridTile.isSelected && gridTile.canPlace && !tile.IsBomb)
            {
                SetGridTilePosition?.Invoke(gridTile, tile, destroyCallback);
                return;
            }*/
        }
        destroyCallback?.Invoke();
    }

    private void SetTilePieceInGrid(TilePiece tile, GameObject mainGrid, Action<TilePiece, Action> SetUpcomingBombPositon, Action<TilePiece, TilePiece, Action> SetGridTilePosition, Action destroyCallback, Vector2 nextPosition, Action<Vector2, Action<TilePiece>> s_NextTileValidPosition)
    {
        for (int gridIndex = 0; gridIndex < mainGrid.transform.childCount; gridIndex++)
        {
            TilePiece gridTile = mainGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            if (tile.IsBomb && gridTile.IsCurrentPosition)
            {
                TilePiece nextTile = null;
                s_NextTileValidPosition?.Invoke((gridTile.GridPosition + nextPosition), (TilePiece tile) =>
                {
                    nextTile = tile;
                });
                if (nextTile != null)
                {
                    SetUpcomingBombPositon?.Invoke(nextTile, destroyCallback);
                }
                else
                {
                    SetUpcomingBombPositon?.Invoke(gridTile, destroyCallback);
                }
                return;
            }
            if (!gridTile.IsObstacle && gridTile.IsCurrentPosition)
            {
                TilePiece nextTile = null;
                s_NextTileValidPosition?.Invoke((gridTile.GridPosition + nextPosition), (TilePiece tile) =>
                {
                    nextTile = tile;
                });
                if (nextTile != null)
                {
                    SetGridTilePosition?.Invoke(nextTile, tile, destroyCallback);
                }
                else
                {
                    SetGridTilePosition?.Invoke(gridTile, tile, destroyCallback);
                }
                return;
            }
            /*skchnagesif (!gridTile.isSelected && gridTile.canPlace && !tile.IsBomb)
            {
                SetGridTilePosition?.Invoke(gridTile, tile, destroyCallback);
                return;
            }*/
        }
        destroyCallback?.Invoke();
    }
    private bool PositionStatusCheck(GameObject gameGrid, Vector2 gridPosition)
    {
        bool status = false;
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            TilePiece activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            if (activeTile.GridPosition == gridPosition)
            {
                status = true;
            }
        }
        return status;
    }
    private bool BombPositionCheck(GameObject gameGrid, Vector2 gridPosition)
    {
        bool status = false;
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            TilePiece activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            status = (activeTile.GridPosition == gridPosition && !activeTile.IsStartingPiece && !activeTile.isLoopPiece) ? true : status;
        }
        return status;
    }
    private Vector2 GetBombsForwardActivePosition(GameObject gameGrid, Vector2 gridPosition, Vector2 differenceVector)
    {
        Vector2 activePosition = gridPosition;
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            TilePiece activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            if (activeTile.GridPosition == activePosition)
            {
                activePosition = (!activeTile.IsStartingPiece && !activeTile.isLoopPiece) ? activeTile.GridPosition : activeTile.GridPosition + differenceVector;
            }
        }
        return activePosition;
    }
    private Vector2 GetBombsBackwardActivePosition(GameObject gameGrid, Vector2 gridPosition, Vector2 differenceVector)
    {
        Vector2 activePosition = gridPosition;
        for (int gridIndex = gameGrid.transform.childCount - 1; gridIndex >= 0; gridIndex--)
        {
            TilePiece activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            if (activeTile.GridPosition == activePosition)
            {
                activePosition = (!activeTile.IsStartingPiece && !activeTile.isLoopPiece) ? activeTile.GridPosition : activeTile.GridPosition + differenceVector;
            }
        }
        return activePosition;
    }

    private Vector2 GetForwardActivePosition(GameObject gameGrid, Vector2 gridPosition, Vector2 differenceVector)
    {
        Vector2 activePosition = gridPosition;
        for (int gridIndex = 0; gridIndex < gameGrid.transform.childCount; gridIndex++)
        {
            TilePiece activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            if (activeTile.GridPosition == activePosition)
            {
                activePosition = (!activeTile.IsObstacle && !activeTile.IsStartingPiece && !activeTile.isLoopPiece) ? activeTile.GridPosition : activeTile.GridPosition + differenceVector;
            }
        }
        return activePosition;
    }
    private Vector2 GetBackwardActivePosition(GameObject gameGrid, Vector2 gridPosition, Vector2 differenceVector)
    {
        Vector2 activePosition = gridPosition;
        for (int gridIndex = gameGrid.transform.childCount - 1; gridIndex >= 0; gridIndex--)
        {
            TilePiece activeTile = gameGrid.transform.GetChild(gridIndex).GetComponent<TilePiece>();
            if (activeTile.GridPosition == activePosition)
            {
                activePosition = (!activeTile.IsObstacle && !activeTile.IsStartingPiece && !activeTile.isLoopPiece) ? activeTile.GridPosition : activeTile.GridPosition + differenceVector;
            }
        }
        return activePosition;
    }

    #endregion

    #region Coroutines
    private IEnumerator CheckAttachedPieceStatusSinglePlayer(Action<GridTile, Vector2> LeftVectorStatus, Action<GridTile, Vector2> RightVectorStatus, Action<GridTile, Vector2> UpVectorStatus, Action<GridTile, Vector2> DownVectorStatus, GridTile activeTile)
    {
        yield return new WaitForSeconds(0.2f);
        UpVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.up);
        DownVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.down);
        LeftVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.left);
        RightVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.right);
    }
    private IEnumerator CheckAttachedPieceStatus_Player1(Action<GridTilePlayer1, Vector2> LeftVectorStatus, Action<GridTilePlayer1, Vector2> RightVectorStatus, Action<GridTilePlayer1, Vector2> UpVectorStatus, Action<GridTilePlayer1, Vector2> DownVectorStatus, GridTilePlayer1 activeTile)
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Detach Check 2");
        UpVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.up);
        DownVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.down);
        LeftVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.left);
        RightVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.right);
    }
    private IEnumerator CheckAttachedPieceStatus_Player2(Action<GridTilePlayer2, Vector2> LeftVectorStatus, Action<GridTilePlayer2, Vector2> RightVectorStatus, Action<GridTilePlayer2, Vector2> UpVectorStatus, Action<GridTilePlayer2, Vector2> DownVectorStatus, GridTilePlayer2 activeTile)
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Detach Check 2");
        UpVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.up);
        DownVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.down);
        LeftVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.left);
        RightVectorStatus?.Invoke(activeTile, activeTile.GridPosition + Vector2.right);
    }

    protected IEnumerator OnPauseGame(GameObject PausePanel)
    {
        PausePanel.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = Constants.ZERO;
    }
    #endregion

}

