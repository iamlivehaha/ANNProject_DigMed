using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainPanel : BasePanel
{

    private Button imageButton;
    private Button textButton;
    private Button videoButton;
    private Button audioButton;


    private void Awake()
    {
        //找到图片按钮
        imageButton = transform.Find("ImageButton").GetComponent<Button>();


        //点击事件
        imageButton.onClick.AddListener(OnImageButtonClick);

    }


    //图片按钮的点击事件
    private void OnImageButtonClick()
    {
        
        //再去入栈
        UIManager.Instance.PushPanel(PanelType.ImagePanel);
    }




}
