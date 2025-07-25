using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TitleButtonController : MonoBehaviour
{
    [SerializeField]
    private int m_XIndex;
    [SerializeField]
    private int m_YIndex;
    Action<int, int> m_TurnOffCallback;


    private void OnEnable()
    {
        ViewTitlePanel.s_HighlightButtonCallback += HighLightButton;
        m_TurnOffCallback += ResetButton;
    }
    private void OnDisable()
    {
        ViewTitlePanel.s_HighlightButtonCallback -= HighLightButton;
        m_TurnOffCallback -= ResetButton;
    }
    private void ResetButton(int xIndex, int yIndex)
    {
        if(m_XIndex != xIndex || m_YIndex != yIndex)
        {
            string currentAnimation = "Normal"; ;
            this.GetComponent<Animator>().SetTrigger(currentAnimation);
        }
    }

    private void HighLightButton(int xIndex, int yIndex)
    {
        string currentAnimation;
        if (m_XIndex == xIndex && m_YIndex == yIndex)
        {
            m_TurnOffCallback?.Invoke(m_XIndex, m_YIndex);
            currentAnimation = "Highlighted";
        }
        else
        {
            currentAnimation = "Normal";
        }
        this.GetComponent<Animator>().SetTrigger(currentAnimation);
    }

    private void OnButtonClicked(int xIndex, int yIndex)
    {
        if (m_XIndex == xIndex && m_YIndex == yIndex)
        {
            this.GetComponent<Button>().onClick.Invoke();
        }
        
    }
}
