using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class OptionController : MonoBehaviour
{
    [SerializeField]
    private int m_NextConrollerIndex;
    [SerializeField]
    private int m_PreviosConrollerIndex;
    [SerializeField]
    private int m_UPConrollerIndex;
    [SerializeField]
    private int m_DownConrollerInmdex;
    [SerializeField]
    private bool m_IsButton = false;
    private bool m_IsSelected = false;
    [SerializeField]
    private bool m_IsSlider = false;
    [SerializeField]
    private Button m_NextButton;
    [SerializeField]
    private Button m_PreviosButton;
    [SerializeField]
    private GameObject[] HighLightedGameObjects;
    [SerializeField]
    private int OptionIndex;
    [SerializeField]
    private Slider m_Slider;

    private void OnEnable()
    {
        ViewLevelOptionsPanel.s_HighlightOption += HighlighOption;
        ViewLevelOptionsPanel.s_ResetOption += ResetOptionPanels;
        ViewLevelOptionsPanel.s_NextButtonTrigger += NextButtonTap;
        ViewLevelOptionsPanel.s_PreviosButtonTrigger += PreviousButtonTap;
        ViewLevelOptionsPanel.s_OnGamepadSubmit += OnSubmitButtonTap;
    }
    private void OnDisable()
    {
        ViewLevelOptionsPanel.s_HighlightOption -= HighlighOption;
        ViewLevelOptionsPanel.s_ResetOption -= ResetOptionPanels;
        ViewLevelOptionsPanel.s_NextButtonTrigger -= NextButtonTap;
        ViewLevelOptionsPanel.s_PreviosButtonTrigger -= PreviousButtonTap;
        ViewLevelOptionsPanel.s_OnGamepadSubmit -= OnSubmitButtonTap;
        m_IsSelected = false;
    }

    private void HighlighOption(int currentIndex, Action<int, int, int, int> nextOptions)
    {
        bool highlightstatus = false;
        
        Action optionActions = () =>
        {
            if (currentIndex == OptionIndex)
            {
                highlightstatus = true;
                SoundManager.Instance.PlayMouseHoverSound();
                nextOptions?.Invoke(m_UPConrollerIndex, m_DownConrollerInmdex, m_PreviosConrollerIndex, m_NextConrollerIndex);
            }
            for (int highlighObjectIndex = Constants.INT_ZERO; highlighObjectIndex < HighLightedGameObjects.Length; highlighObjectIndex++)
            {
                HighLightedGameObjects[highlighObjectIndex].SetActive(highlightstatus);
            }
            if (m_IsSlider)
                m_Slider.interactable = highlightstatus;

        };

        Action submitActions = () =>
        {
            string animationState;
            if (currentIndex == OptionIndex)
            {
                highlightstatus = true;
                SoundManager.Instance.PlayMouseHoverSound();
                nextOptions?.Invoke(m_UPConrollerIndex, m_DownConrollerInmdex, m_PreviosConrollerIndex, m_NextConrollerIndex);
                animationState = "Highlighted";
            }
            else
            {
                animationState = "Normal";
            }
            this.GetComponent<Animator>().SetTrigger(animationState);
        };

        if (m_IsButton)
            submitActions?.Invoke();
        else
            optionActions?.Invoke();

        m_IsSelected = highlightstatus;
    }
    private void NextButtonTap()
    {
        if (m_IsSelected && !m_IsButton)
            m_NextButton.onClick.Invoke();

    }

    private void PreviousButtonTap()
    {
        if (m_IsSelected && !m_IsButton)
            m_PreviosButton.onClick.Invoke();
    }
    private void OnSubmitButtonTap()
    {
        if (m_IsButton && m_IsSelected)
            this.GetComponent<Button>().onClick.Invoke();
    }

    private void ResetOptionPanels()
    {
        m_IsSelected = false;
        if (m_IsSlider)
            m_Slider.interactable = true;
        Action optionActions = () =>
        {
            for (int highlighObjectIndex = Constants.INT_ZERO; highlighObjectIndex < HighLightedGameObjects.Length; highlighObjectIndex++)
            {
                HighLightedGameObjects[highlighObjectIndex].SetActive(false);
            }
        };
        if (!m_IsButton)
            optionActions?.Invoke();
    }


}
