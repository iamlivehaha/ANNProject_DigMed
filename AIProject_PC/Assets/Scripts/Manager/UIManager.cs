using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPanelType
{
    MainPanel,
    ImagePanel
}

public class UIManager
{
    private static UIManager _instance = null;

    public static UIManager Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new UIManager();
            }

            return _instance;
        }
    }

    private Dictionary<EPanelType, string> _panelDictionary = null;

    private Stack<BasePanel> _panelStack = null;

    private Transform _canvasTransform = null;

    private Dictionary<EPanelType, BasePanel> _basePanelDic;

    private UIManager()
    {
        if (_panelDictionary==null)
        {
            _panelDictionary = new Dictionary<EPanelType, string>();
        }
        _panelDictionary.Add(EPanelType.MainPanel, "Panel/MainPanel");
        _panelDictionary.Add(EPanelType.ImagePanel, "Panel/ImagePanel");

        if (_panelStack ==null)
        {
            _panelStack = new Stack<BasePanel>();
        }

        if (_canvasTransform==null)
        {
            _canvasTransform = GameObject.Find("Canvas").transform;
        }

        if (_basePanelDic == null)
        {
            _basePanelDic = new Dictionary<EPanelType, BasePanel>();
        }
    }

    public void PopPanel()
    {
        if (_panelStack.Count<=0)
        {
            Debug.LogError("PanelStack is empty");
        }

        BasePanel panel = _panelStack.Pop();

        panel.OnExit();

        Debug.Log("pop panel" + panel);
    }

    public void PushPanel(EPanelType panelType)
    {
        BasePanel panel = GetPanel(panelType);

        panel.OnEnter();

        if (panel)
        {
            _panelStack.Push(panel);
        }
        Debug.Log("push panel" + panel);
    }

    private BasePanel GetPanel(EPanelType panelType)
    {
        BasePanel basePanel = _basePanelDic.GetValue(panelType);
        if (basePanel==null)
        {
            string panelPath = _panelDictionary.GetValue(panelType);
            GameObject panelGo = Object.Instantiate(Resources.Load<GameObject>(panelPath), _canvasTransform, false);
            basePanel = panelGo.GetComponent<BasePanel>();
            _basePanelDic.Add(panelType,basePanel);
        }

        return basePanel;
    }
}
