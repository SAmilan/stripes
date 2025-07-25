using UnityEngine;
using UnityEngine.UI;
using System.Collections;




[RequireComponent(typeof(Camera))]

public class UIView : MonoBehaviour
{
    #region Private_Variables
    private Camera m_Camera;
    private Canvas m_Canvas;
    #endregion

    #region Public_Variables
    #endregion

    #region Unity_Callbacks
    public virtual void Awake()
    {
        m_Camera = this.GetComponent<Camera>();
        m_Canvas = this.GetComponentInChildren<Canvas>();
    }
    #endregion

    #region Overiden_Methods
    public virtual void Hide ()
    {
        ToggleCanvas(false);
    }
    public virtual void Show()
    {
        ToggleCanvas(true);
    }
    #endregion

    #region Public_Methods
    #endregion

    #region Private_Methods
    /// <summary>
    /// Toggle the canvas and the canera
    /// </summary>
    /// <param name="value">passes by the user to On or Off the Panel</param>
    private void ToggleCanvas(bool value)
    {
        this.gameObject.SetActive(value);
       // m_Camera.enabled = value;
        //m_Canvas.enabled = value;
        Canvas.ForceUpdateCanvases();
    }
    #endregion

}