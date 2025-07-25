using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class HoldTilePlayer1 : TilePiece
{
    #region Private_Variables
    private SinglePlayerInputs m_HoldTileInputs;
    private InputAction m_XPressed;
    private bool m_IsHolding = false;
    private bool m_isGamePadActive = false;
    private bool m_isEmpty = true;
    public TilePiece HoldedTile;
    #endregion

    #region Public_Variables
    public static Action<TilePiece, Action<TilePiece>, Action<Vector2>> s_StorePiece;
    public static Action<TilePiece> s_ExtractPiece;
    public static Action s_TriggerUpcomingTile;
    public static Action<Action<bool>> s_CheckActiveTileStatus;
    public static Action<Vector2> s_ResetOnHold;
    public static Action<TilePiece> s_ActiveSwapTile;
    #endregion


    #region Unity_Callbacks
    private void Awake()
    {
        m_HoldTileInputs = new SinglePlayerInputs();
    }
    private void OnEnable()
    {
        m_isEmpty = true;
        HoldedTile = null;
        m_isGamePadActive = false;
        TwoPlayerTutorialPanel.s_TutorialEnds += OnTutorialEnds;
    }

    private void OnDisable()
    {
        ViewTwoPlayerGame.s_RefreshHoldTilePlacement_Player1 -= RefreshHoldTilePlacement;
        TwoPlayerTutorialPanel.s_TutorialEnds -= OnTutorialEnds;
        m_XPressed.performed -= StorePiece;
        m_XPressed.Disable();
    }

    private void OnTutorialEnds()
    {
        ViewTwoPlayerGame.s_RefreshHoldTilePlacement_Player1 += RefreshHoldTilePlacement;
        m_isGamePadActive = true;
        m_PieceImage.gameObject.SetActive(false);
        m_SelectedImage.gameObject.SetActive(false);
        m_XPressed = m_HoldTileInputs.TwoPlayers.StorePiecePlayer1;
        m_XPressed.performed += StorePiece;
        m_XPressed.Enable();
        p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
        p_BombImage.gameObject.SetActive(false);
    }
    private void Update()
    {
        StorePieceGamepad();
    }
    #endregion

    #region Private_Methods
    private void StorePiece(InputAction.CallbackContext context)
    {
        if (!p_CanPlaceHoldTile)
        {
            return;
        }
        if (m_isEmpty == true)
        {
            s_StorePiece?.Invoke(HoldedTile, SetHoldTile, s_ResetOnHold);
            m_isEmpty = false;
        }
        else
        {
            p_CanPlaceHoldTile = false;
            s_ActiveSwapTile?.Invoke(this);
            s_StorePiece?.Invoke(this, SetHoldTile, s_ResetOnHold);
        }
    }
    private void StorePieceGamepad()
    {
        if (GamepadManager.Instance.m_Gamepad1 != null && m_isGamePadActive)
        {
            if (GamepadManager.Instance.m_Gamepad1.xButton.wasPressedThisFrame)
            {
                if (!p_CanPlaceHoldTile)
                {
                    return;
                }
                if (m_isEmpty == true)
                {
                    s_StorePiece?.Invoke(HoldedTile, SetHoldTile, s_ResetOnHold);
                    m_isEmpty = false;
                }
                else
                {
                    p_CanPlaceHoldTile = false;
                    s_ActiveSwapTile?.Invoke(this);
                    s_StorePiece?.Invoke(this, SetHoldTile, s_ResetOnHold);
                }
            }
        }
    }

    private void SetHoldTile(TilePiece piece)
    {
        if (piece != null)
        {
            HoldedTile = piece;
            m_IsHolding = true;
            if (piece.ActiveType != Constants.BOMB_TYPEPIECE)
            {
                CreateHoldTile(piece);
            }
            else
            {
                CreateBombTile();
            }
        }

    }
    private void CreateBombTile()
    {
        p_ActivePieceImage.transform.parent.gameObject.SetActive(false);
        CreateBombPiece();
    }
    private void CreateHoldTile(TilePiece piece)
    {
        p_BombImage.gameObject.SetActive(false);
        CreateUpcomingTile(piece);
    }
    
    #endregion
}
