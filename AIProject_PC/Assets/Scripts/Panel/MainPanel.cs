using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Button _imageButton;

    private void Awake()
    {
        _imageButton = transform.Find("Project_1_Button").GetComponent<Button>();

        _imageButton.onClick.AddListener(OnProject_1_ButtonClick);
    }

    private void OnProject_1_ButtonClick()
    {
        UIManager.Instance.PushPanel(EPanelType.ImagePanel);
    }
}
