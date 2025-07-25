using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTile : TilePiece
{
    public static SwapTile Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        HoldTile.s_ActiveSwapTile += ActiveSwapTile;
    }
    private void OnDisable()
    {
        HoldTile.s_ActiveSwapTile -= ActiveSwapTile;

    }

    private void ActiveSwapTile(TilePiece currentHoldTile)
    {
        if (currentHoldTile.ActiveType != Constants.BOMB_TYPEPIECE)
        {
            CreateHoldTile(currentHoldTile);
        }
        else
        {
            CreateBombTile();
        }
    }

    private void CreateHoldTile(TilePiece piece)
    {
        p_BombImage.gameObject.SetActive(false);
        CreateUpcomingTile(piece);
    }

    private void CreateBombTile()
    {
        p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
        CreateBombPiece();
    }
}
