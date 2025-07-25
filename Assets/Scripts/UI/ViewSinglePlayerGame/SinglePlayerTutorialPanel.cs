using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SinglePlayerTutorialPanel : MonoBehaviour
{
    private bool m_canPressed;
   
    [SerializeField]
    private Image m_SinglePlayerKeyboard;
    public static Action s_TutorialEnds;
    
    private void OnEnable()
    {
        m_SinglePlayerKeyboard.gameObject.SetActive(false);
       // UpdateCurrentGameStatus();
        StartCoroutine(ActiveInputs());
    }
    void Start()
    {
        
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
        if (GamepadManager.Instance.m_Gamepad1 == null)
        {
            m_SinglePlayerKeyboard.gameObject.SetActive(true);
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
