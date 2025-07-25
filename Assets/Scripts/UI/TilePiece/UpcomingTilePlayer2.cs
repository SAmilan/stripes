using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class UpcomingTilePlayer2 : TilePiece
{
    #region Public_Variable
    public static Action<Action<TilePiece>, Vector2> s_OnTileMove;
    public static Action s_InitialTileSet;
    #endregion
    #region Private_Variable
    private SinglePlayerInputs m_UpcomingButtonActions;
    private InputAction m_UpcomingButtonTap;
    private InputAction m_UpcomingButtonTapGamepad;
    private InputAction m_PlacePiece;
    private InputAction m_PlacePieceGamepad;
    #endregion
    #region Unity_CallBacks
    private void Awake()
    {
        m_UpcomingButtonActions = new SinglePlayerInputs();
    }
    private void OnEnable()
    {
        TwoPlayerTutorialPanel.s_TutorialEnds += OnTutorialEnds;
    }
    private void OnDisable()
    {
        TwoPlayerTutorialPanel.s_TutorialEnds -= OnTutorialEnds;
        DisableAllEvents();
    }
    #endregion
    #region Public_Methods
    private void EnableAllEvents()
    {
        m_UpcomingButtonTap = m_UpcomingButtonActions.TwoPlayers.UpcomingActionsPlayer2;
        m_UpcomingButtonTapGamepad = m_UpcomingButtonActions.TwoPlayers.UpcomingActionsPlayer2Gamepads;
        m_PlacePiece = m_UpcomingButtonActions.TwoPlayers.PlacePiecePlayer2;
        m_PlacePieceGamepad = m_UpcomingButtonActions.TwoPlayers.PlacePiecePlayer2Gamepads;
        m_UpcomingButtonTap.performed += MovePieceToGrid;
        m_UpcomingButtonTapGamepad.performed += MovePieceToGridGamePad;
        m_PlacePiece.performed += EnableUpcoming;
        m_PlacePieceGamepad.performed += EnableUpcomingGamepad;
        ViewTwoPlayerGame.s_OnGameOvers_Player2 += OnGameOvers;
        ViewTwoPlayerGame.s_AutomaticTilePlaceCallback_Player2 += MoveTile;
        ViewTwoPlayerGame.s_ActiveUpcomingInputs_Player2 += EnableUpcomingInputs;
        StartCoroutine(StartingInputDelay());
    }
    private void DisableAllEvents()
    {
        m_UpcomingButtonTap.Disable();
        m_UpcomingButtonTapGamepad.Disable();
        m_PlacePiece.Disable();
        m_PlacePieceGamepad.Disable();
        m_UpcomingButtonTap.performed -= MovePieceToGrid;
        m_UpcomingButtonTapGamepad.performed -= MovePieceToGridGamePad;
        m_PlacePiece.performed -= EnableUpcoming;
        m_PlacePieceGamepad.performed -= EnableUpcomingGamepad;
        ViewTwoPlayerGame.s_OnGameOvers_Player2 -= OnGameOvers;
        ViewTwoPlayerGame.s_AutomaticTilePlaceCallback_Player2 -= MoveTile;
        ViewTwoPlayerGame.s_ActiveUpcomingInputs_Player2 -= EnableUpcomingInputs;
    }

    private void OnTutorialEnds()
    {
        EnableAllEvents();
    }
    private void MoveTile()
    {
        Action moveAction = () =>
        {
            m_SelectedImage.gameObject.SetActive(false);
            m_UpcomingButtonTap.Disable();
            m_UpcomingButtonTapGamepad.Disable();
        };

        if (!m_isGameOver && !GameManager.instace.TwoPlayerGamePauseStatus && !GameManager.instace.SecondPlayerPauseStatus)
            moveAction?.Invoke();
    }

    private void SetTile(TilePiece tile)
    {
        CreateUpcomingTile(tile);
    }

    private void EnableUpcomingInputs()
    {
        Action enableAction = () =>
        {
            m_UpcomingButtonTap.Enable();
            m_UpcomingButtonTapGamepad.Enable();
            m_SelectedImage.gameObject.SetActive(true);
        };
        if (!m_isGameOver && !GameManager.instace.TwoPlayerGamePauseStatus && !GameManager.instace.SecondPlayerPauseStatus)
            enableAction?.Invoke();
    }

    private void EnableUpcoming(InputAction.CallbackContext context)
    {
        Action enableAction = () =>
        {
            m_UpcomingButtonTap.Enable();
            m_UpcomingButtonTapGamepad.Enable();
            m_SelectedImage.gameObject.SetActive(true);
        };
       if (!m_isGameOver && !GameManager.instace.TwoPlayerGamePauseStatus && !GameManager.instace.SecondPlayerPauseStatus)
            enableAction?.Invoke();
    }
    private void MovePieceToGrid(InputAction.CallbackContext context)
    {
        bool UPKeyStatus = false;
        bool DownKeyStatus = false;
        bool LeftKeyStatus = false;
        bool RightKeyStatus = false;
        Action<Vector2> XAction = (Vector2 gamepadPosition) =>
        {
            UPKeyStatus = false;
            DownKeyStatus = false;
            if (gamepadPosition.x <= Constants.ZERO)
                LeftKeyStatus = true;
            else
                RightKeyStatus = true;
        };
        Action<Vector2> YAction = (Vector2 gamepadPosition) =>
        {
            LeftKeyStatus = false;
            RightKeyStatus = false;
            if (gamepadPosition.y <= Constants.ZERO)
                DownKeyStatus = true;
            else
                UPKeyStatus = true;
        };
        Vector2 gamepadPos = context.ReadValue<Vector2>();
        float XPosition = Mathf.Abs(gamepadPos.x);
        float YPositon = Mathf.Abs(gamepadPos.y);
        if (YPositon > XPosition)
            YAction?.Invoke(gamepadPos);
        else
            XAction?.Invoke(gamepadPos);
        Vector2 nextPosition = (UPKeyStatus) ? GameData.Instance.LEFTSTICKUPVECTOR : (DownKeyStatus) ? GameData.Instance.LEFTSTICKDOWNVECTOR : (LeftKeyStatus) ? GameData.Instance.LEFTSTICKLEFTVECTOR : (RightKeyStatus) ? GameData.Instance.LEFTSTICKRIGHTVECTOR : GameData.Instance.LEFTSTICKUPVECTOR;
        Action moveAction = () =>
        {
            s_OnTileMove?.Invoke(SetTile, nextPosition);
            m_SelectedImage.gameObject.SetActive(false);
            m_UpcomingButtonTap.Disable();
            m_UpcomingButtonTapGamepad.Disable();
        };

        if (!m_isGameOver && !GameManager.instace.TwoPlayerGamePauseStatus && !GameManager.instace.SecondPlayerPauseStatus)
            moveAction?.Invoke();
    }

    private void MovePieceToGridGamePad(InputAction.CallbackContext context)
    {
        bool UPKeyStatus = false;
        bool DownKeyStatus = false;
        bool LeftKeyStatus = false;
        bool RightKeyStatus = false;
        Action<Vector2> XAction = (Vector2 gamepadPosition) =>
        {
            UPKeyStatus = false;
            DownKeyStatus = false;
            if (gamepadPosition.x <= Constants.ZERO)
                LeftKeyStatus = true;
            else
                RightKeyStatus = true;
        };
        Action<Vector2> YAction = (Vector2 gamepadPosition) =>
        {
            LeftKeyStatus = false;
            RightKeyStatus = false;
            if (gamepadPosition.y <= Constants.ZERO)
                DownKeyStatus = true;
            else
                UPKeyStatus = true;
        };
        Vector2 gamepadPos = context.ReadValue<Vector2>();
        float XPosition = Mathf.Abs(gamepadPos.x);
        float YPositon = Mathf.Abs(gamepadPos.y);
        if (YPositon > XPosition)
            YAction?.Invoke(gamepadPos);
        else
            XAction?.Invoke(gamepadPos);
        Vector2 nextPosition = (UPKeyStatus) ? GameData.Instance.LEFTSTICKUPVECTOR : (DownKeyStatus) ? GameData.Instance.LEFTSTICKDOWNVECTOR : (LeftKeyStatus) ? GameData.Instance.LEFTSTICKLEFTVECTOR : (RightKeyStatus) ? GameData.Instance.LEFTSTICKRIGHTVECTOR : GameData.Instance.LEFTSTICKUPVECTOR;
        Action moveAction = () =>
        {
            s_OnTileMove?.Invoke(SetTile, nextPosition);
            m_SelectedImage.gameObject.SetActive(false);
            m_UpcomingButtonTap.Disable();
            m_UpcomingButtonTapGamepad.Disable();
        };

        if (!m_isGameOver && !GameManager.instace.TwoPlayerGamePauseStatus && !GameManager.instace.SecondPlayerPauseStatus)
        {
            if (GamepadManager.Instance.m_Gamepad2 != null)
            {
                if (GamepadManager.Instance.m_Gamepad2.dpad.IsPressed() || GamepadManager.Instance.m_Gamepad2.leftStick.IsPressed())
                {
                    moveAction?.Invoke();
                }
            }
        }
    }

    private void EnableUpcomingGamepad(InputAction.CallbackContext context)
    {
        Action enableAction = () =>
        {
            m_UpcomingButtonTap.Enable();
            m_UpcomingButtonTapGamepad.Enable();
            m_SelectedImage.gameObject.SetActive(true);
        };
        if (!m_isGameOver && !GameManager.instace.TwoPlayerGamePauseStatus && !GameManager.instace.SecondPlayerPauseStatus)
        {
            if (GamepadManager.Instance.m_Gamepad2 != null)
            {
                if (GamepadManager.Instance.m_Gamepad2.aButton.isPressed)
                {
                    enableAction?.Invoke();
                }
            }
        }
    }
    #endregion

    IEnumerator StartingInputDelay()
    {
        yield return new WaitForSeconds(0.2f);
        m_UpcomingButtonTap.Enable();
        m_UpcomingButtonTapGamepad.Enable();
        m_PlacePiece.Enable();
        m_PlacePieceGamepad.Enable();
    }
}
