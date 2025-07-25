using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class UpcomingTile : TilePiece
{
    #region Public_Variable
    public static Action<Action<TilePiece>, Vector2> s_OnTileMove;
    public static Action<Action<TilePiece>, Vector2> s_OnTileMoveVirtually;
    public static Action s_InitialTileSet;
    #endregion
    #region Private_Variable
    private SinglePlayerInputs m_UpcomingButtonActions;
    private InputAction m_UpcomingButtonTap;
    private InputAction m_PlacePiece;
    private InputAction m_PlacePiece_Gamepad;
    #endregion
    #region Unity_CallBacks
    private void Awake()
    {
        m_UpcomingButtonActions = new SinglePlayerInputs();
    }
    private void OnEnable()
    {
        EnableAllEvents();
    }
    private void OnDisable()
    {
        DisableAllEvents();
    }
    #endregion
    #region Public_Methods
    private void EnableAllEvents()
    {
        SinglePlayerTutorialPanel.s_TutorialEnds += OnTutorialEnds;
    }
    private void DisableAllEvents()
    {
        SinglePlayerTutorialPanel.s_TutorialEnds -= OnTutorialEnds;
        m_UpcomingButtonTap.Disable();
        m_PlacePiece.Disable();
        m_PlacePiece_Gamepad.Disable();
        m_UpcomingButtonTap.performed -= MovePieceToGrid;
        m_PlacePiece.performed -= EnableUpcoming;
        m_PlacePiece_Gamepad.performed -= EnableUpcoming_Gamepads;
        ViewSinglePlayerGame.s_OnGameOvers -= OnGameOvers;
        ViewSinglePlayerGame.s_AutomaticTilePlaceCallback -= MoveTile;
        ViewSinglePlayerGame.s_ActiveUpcomingInputs -= EnableUpcomingInputs;
        HoldTile.s_TriggerUpcomingTile -= OnStorePieceTap;
    }
    private void OnTutorialEnds()
    {
        m_UpcomingButtonTap = m_UpcomingButtonActions.SinglePlayer.UpcomingActions;
        m_PlacePiece = m_UpcomingButtonActions.SinglePlayer.PlacePiece;
        m_PlacePiece_Gamepad = m_UpcomingButtonActions.SinglePlayer.PlacePieceGamepad;
        m_UpcomingButtonTap.performed += MovePieceToGrid;
        m_PlacePiece.performed += EnableUpcoming;
        m_PlacePiece_Gamepad.performed += EnableUpcoming_Gamepads;
        ViewSinglePlayerGame.s_OnGameOvers += OnGameOvers;
        ViewSinglePlayerGame.s_AutomaticTilePlaceCallback += MoveTile;
        ViewSinglePlayerGame.s_ActiveUpcomingInputs += EnableUpcomingInputs;
        HoldTile.s_TriggerUpcomingTile += OnStorePieceTap;
        StartCoroutine(StartingInputDelay());
    }
    private void MoveTile()
    {
        Action moveAction = () =>
        {
            m_SelectedImage.gameObject.SetActive(true);
            m_UpcomingButtonTap.Disable();
        };
        if (!m_isGameOver)
            moveAction?.Invoke();
    }
    private void SetTile(TilePiece tile)
    {
        CreateUpcomingTile(tile);
    }
    private void EnableUpcoming(InputAction.CallbackContext context)
    {
        Action enableAction = () =>
        {
            Debug.Log("Upcoming trigger 1");
            m_UpcomingButtonTap.Enable();
            m_SelectedImage.gameObject.SetActive(true);
        };
        if (!m_isGameOver && !GameManager.instace.SinglePlayerGamePauseStatus)
            enableAction?.Invoke();

    }
    private void EnableUpcoming_Gamepads(InputAction.CallbackContext context)
    {
        Action enableAction = () =>
        {
            if (GamepadManager.Instance.m_Gamepad1 != null)
            {
                if (GamepadManager.Instance.m_Gamepad1.aButton.isPressed)
                {
                    Debug.Log("Upcoming trigger 1");
                    m_UpcomingButtonTap.Enable();
                    m_SelectedImage.gameObject.SetActive(true);
                }
            }
        };
        if (!m_isGameOver && !GameManager.instace.SinglePlayerGamePauseStatus)
            enableAction?.Invoke();

    }

    private void EnableUpcomingInputs()
    {
        Action enableAction = () =>
        {
            Debug.Log("Upcoming trigger 2");
            m_UpcomingButtonTap.Enable();
            m_SelectedImage.gameObject.SetActive(true);
        };
        if (!m_isGameOver && !GameManager.instace.SinglePlayerGamePauseStatus)
            enableAction?.Invoke();
    }
    private void OnStorePieceTap()
    {
        Action moveAction = () =>
        {
            s_OnTileMoveVirtually?.Invoke(SetTile, Vector2.zero);
            m_SelectedImage.gameObject.SetActive(true);
            m_UpcomingButtonTap.Disable();
        };
        if (!m_isGameOver && !GameManager.instace.SinglePlayerGamePauseStatus)
            moveAction?.Invoke();
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
            m_SelectedImage.gameObject.SetActive(true);
            m_UpcomingButtonTap.Disable();
        };
        if (!m_isGameOver && !GameManager.instace.SinglePlayerGamePauseStatus)
        {
            moveAction?.Invoke();
        }
    }
    #endregion

    IEnumerator StartingInputDelay()
    {
        yield return new WaitForSeconds(0.1f);
        m_UpcomingButtonTap.Enable();
        m_PlacePiece.Enable();
        m_PlacePiece_Gamepad.Enable();
    }
}
