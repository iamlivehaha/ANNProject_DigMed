using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : BasePanel
{
    private Button backButton;
    private Button startButton;

    private void Awake()
    {
        //赋值
        backButton = transform.Find("StyleTransferProject/BackButton").GetComponent<Button>();
        startButton = transform.Find("StyleTransferProject/StartButton").GetComponent<Button>();


        //点击事件
        backButton.onClick.AddListener(OnBackButtonClick);
        startButton.onClick.AddListener(OnStartButtonClick);
    }



    //开始按钮的点击事件
    private void OnStartButtonClick()
    {
        //压栈
        UIManager.Instance.PushPanel(PanelType.StyleTransferPanel);
    }


}
