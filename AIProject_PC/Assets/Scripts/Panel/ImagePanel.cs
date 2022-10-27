using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;

public class ImagePanel : BasePanel
{
    private Button _generateButton;
    private Button _backButton;

    private Button _inputButton;
    private Button _outputButton;

    private void Awake()
    {
        _generateButton = transform.Find("GenerateButton").GetComponent<Button>();
        _backButton = transform.Find("BackButton").GetComponent<Button>();
        _generateButton = transform.Find("InputButton").GetComponent<Button>();
        _backButton = transform.Find("OutputButton").GetComponent<Button>();

        _generateButton.onClick.AddListener(OnGenerateButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
        _inputButton.onClick.AddListener(OnInputButtonClick);
        _outputButton.onClick.AddListener(OnOutputButtonClick);
    }

    private void OnGenerateButtonClick()
    {
        //todo
    }

    private void OnInputButtonClick()
    {
        //todo
    }
    private void OnOutputButtonClick()
    {
        //todo
    }

}
