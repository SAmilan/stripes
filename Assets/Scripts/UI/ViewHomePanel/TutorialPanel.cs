using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialPanel : MonoBehaviour
{
    private bool m_canPressed;
    [SerializeField]
    private Button[] m_GameButtons;
    [SerializeField]
    private Image m_SinglePlayerKeyboard;
    [SerializeField]
    private Image m_TwoPlayerKeyboard;
    private int m_CurrentGame;
    private Button m_CurrentButton;
    [SerializeField]
    private GameObject m_LoadingPanel;
    private void OnEnable()
    {
        SoundManager.Instance.PlayWindowTransitionSound();
        m_canPressed = false;
        StartCoroutine(ActiveInputs());
        m_CurrentGame = Constants.INT_ZERO;
        m_SinglePlayerKeyboard.gameObject.SetActive(false);
        m_TwoPlayerKeyboard.gameObject.SetActive(false);
        UpdateCurrentGameStatus();
    }
    private void OnDisable()
    {
        SoundManager.Instance.PlayCloseWindowSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && m_canPressed)
        {
            StartCoroutine(DisableTutorialPanel());
            GetComponent<Animator>().SetTrigger("Outro");
            m_canPressed = false;
        }
    }
    
    
    private void UpdateCurrentGameStatus()
    {
        m_CurrentGame = GameManager.instace.CurrentGame;
        m_CurrentButton = m_GameButtons[m_CurrentGame];
        Action singlePlayerAction = () =>
            {
                if(GamepadManager.Instance.m_Gamepad1 == null)
                {
                    m_SinglePlayerKeyboard.gameObject.SetActive(true);
                }
            };
        Action twoPlayerAction = () =>
        {
            if (GamepadManager.Instance.m_Gamepad1 == null || GamepadManager.Instance.m_Gamepad2 == null)
            {
                m_TwoPlayerKeyboard.gameObject.SetActive(true);
            }
        };

        if (m_CurrentGame == Constants.INT_ZERO)
            singlePlayerAction?.Invoke();
        else
            twoPlayerAction?.Invoke();
        
    }

    IEnumerator DisableTutorialPanel()
    {
        yield return new WaitForSeconds(2.3f);
        m_LoadingPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        m_CurrentButton.onClick.Invoke();
        this.gameObject.SetActive(false);
        m_LoadingPanel.gameObject.SetActive(false);
    }
    IEnumerator ActiveInputs()
    {
        yield return new WaitForSeconds(0.5f);
        m_canPressed = true;

    }
}
