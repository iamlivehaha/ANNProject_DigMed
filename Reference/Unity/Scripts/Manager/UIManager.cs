using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Panel的类型
public enum PanelType
{
    MainPanel,
    ImagePanel,
    StyleTransferPanel
}

public class UIManager
{
    //单例模式
    private static UIManager _instance = null;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    //开始的时候。需要创建GameObject(UI面板)，GameObject.Instantiate去创建，使用Resources.Load加载游戏物品，所以先要
    //拿到这些路径
    private Dictionary<PanelType, string> panelDict = null;
    //栈
    private Stack<BasePanel> panelStack = null;
    //定位的
    private Transform canvasTransform = null;
    private UIManager()
    {   
        //创建资源对应的路径
        if (panelDict == null)
        {
            panelDict = new Dictionary<PanelType, string>();
        }
        panelDict.Add(PanelType.MainPanel, "Panel/MainPanel");
        panelDict.Add(PanelType.ImagePanel, "Panel/ImagePanel");
        panelDict.Add(PanelType.StyleTransferPanel, "Panel/StyleTransferPanel");
        
        //核心的栈管理
        if(panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        //对Canvas的位置赋值
        if(canvasTransform == null)
        {
            canvasTransform = GameObject.Find("Canvas").transform;
        }
       


    }

    private Dictionary<PanelType, BasePanel> basePanelDict = new Dictionary<PanelType, BasePanel>();
    
    //得到Panel
    private BasePanel GetPanel(PanelType panelType)
    {
        //先查看basePanelDict里面有没有我们想要的Panel，如果有直接返回，如果没有就加载
        BasePanel panel = basePanelDict.GetValue(panelType);

        //如果在dict中没有找到。则加载
        if (panel == null)
        {
            //获得路径
            string path = panelDict.GetValue(panelType);

            //通过路径定位到GameObject
            GameObject panelGO = GameObject.Instantiate(Resources.Load<GameObject>(path), canvasTransform, false);
            panel = panelGO.GetComponent<BasePanel>();
            //添加
            basePanelDict.Add(panelType, panel);
        }
      
        return panel;
    }

    //压栈
    public void PushPanel(PanelType panelType)
    {   
        //先移动出去
        if (panelStack.Count > 0)
        {
            BasePanel top = panelStack.Peek();
            top.OnExit();
        }

        //先得到Panel
        BasePanel panel = GetPanel(panelType);

        panel.OnEnter();
        //将Panel压栈
        panelStack.Push(panel);


    }

    //出栈，出一个面板。上一个面板进入
    public void PopPanel()
    {
        if(panelStack.Count <= 0)
        {
            return;
        }

        //出栈
        BasePanel panel = panelStack.Pop();
        panel.OnExit();

        if(panelStack.Count > 0)
        {
            BasePanel top = panelStack.Peek();
            top.OnEnter();
        }

    }


}
