using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoPlayerTutorialPanel : MonoBehaviour
{
    private bool m_canPressed;
    [SerializeField]
    private Image m_TwoPlayerKeyboard;
    public static Action s_TutorialEnds;

    private void OnEnable()
    {
        m_TwoPlayerKeyboard.gameObject.SetActive(false);
        //UpdateCurrentGameStatus();
        StartCoroutine(ActiveInputs());
    }
 
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
        if (GamepadManager.Instance.m_Gamepad1 == null || GamepadManager.Instance.m_Gamepad2 == null)
        {
            m_TwoPlayerKeyboard.gameObject.SetActive(true);
        }
    }
    IEnumerator ActiveInputs()
    {
        yield return new WaitForSeconds(0.5f);
        m_canPressed = true;
    }

    IEnumerator DisableTutorialPanel()
    {
        yield return new WaitForSeconds(2f);
        s_TutorialEnds?.Invoke();
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
}
