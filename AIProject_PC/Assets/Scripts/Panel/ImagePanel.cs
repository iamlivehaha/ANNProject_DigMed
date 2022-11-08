using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using Button = UnityEngine.UI.Button;

public class ImagePanel : BasePanel
{
    private Button _generateButton;
    private Button _backButton;

    private Button _inputButton;
    private Button _outputButton;

    private Image _inputImage;
    private Image _outputImage;

    bool flag = false;
    string filePath = null;

    private void Awake()
    {
        _generateButton = transform.Find("GenerateButton").GetComponent<Button>();
        _backButton = transform.Find("BackButton").GetComponent<Button>();
        _inputButton = transform.Find("InputButton").GetComponent<Button>();
        _outputButton = transform.Find("OutputButton").GetComponent<Button>();

        _generateButton.onClick.AddListener(OnGenerateButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
        _inputButton.onClick.AddListener(OnInputButtonClick);
        _outputButton.onClick.AddListener(OnOutputButtonClick);

        _inputImage = transform.Find("InputButton").GetComponent<Image>();
        _outputImage = transform.Find("OutputButton").GetComponent<Image>();
    }

    private void Update()
    {
        if (flag)
        {
            //异步更换最终生成图片
            StartCoroutine(ChangeResultImage(@"D:\UnityProject\ANNProject_DigMed\AIProject_PC\Assets\ImageSource\Results\" + filePath));
            flag = false;

        }
    }

    private void OnGenerateButtonClick()
    {
        ////1. 调用Java服务器。传入两个图片的路径，获得到AI生成图片的路径
        //string resp = GetHttpResponse("http://localhost:8080/unity");

        //2. 调用本地python代码
 
        string promt = "--prompt " + "\"a close-up portrait of a cat by pablo picasso, vivid, abstract art, colorful, vibrant\"";
        string plms = "--plms";
        string n_iter = "--n_iter 5";
        string height = "--H 512";
        string width = "--W 512";
        string samples = "--n_samples 1";
        string[] arg = new string[6] { promt,plms,n_iter,height,width,samples};
        RunPython();
    }

    private void OnInputButtonClick()
    {
        //todo
        GetUserSelectedImage(_inputImage);
    }
    private void OnOutputButtonClick()
    {
        //todo
        GetUserSelectedImage(_outputImage);
    }

    private void GetUserSelectedImage(Image selectImage)
    {
        OpenFileDialog od = new OpenFileDialog();
        od.Title = "请选择图片";
        od.Multiselect = false;
        od.Filter = "图片文件（*.jpg,*,png）|*.jpg;*.png";
        if(od.ShowDialog() == DialogResult.OK)
        {
            UnityEngine.Debug.Log("image selected!");
            StartCoroutine(GetTexture(od.FileName, selectImage));
        }
    }
    
    IEnumerator GetTexture(string url, Image selectImage)
    {
        WWW www = new WWW("file://"+url);
        yield return null;
        if(www.isDone)
        {
            Texture2D texture2d = new Texture2D(720, 576);
            www.LoadImageIntoTexture(texture2d);
            Sprite sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
            selectImage.sprite = sprite;
        }

    }

    //图片的回显
    IEnumerator ChangeResultImage(string url)
    {
        WWW www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D tex = www.texture;
            Sprite temp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

            _outputImage.sprite = temp;
        }

    }

    private string GetHttpResponse(string url)
    {
        //创建HTTP请求
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "Get";
        request.ContentType = "application/json;charset=utf-8";
        request.UserAgent = null;
        request.Timeout = 500000;

        //创建HTTP的相应（发起HTTP请求）
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //使用流接受
        Stream myResponseStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

        string res = reader.ReadToEnd();

        reader.Close();
        myResponseStream.Close();
        return res;
    }

    private void RunPython(string[] argvs = null)
    {
        //开启命令行的
        Process process = new Process();

        //Python脚本
        string path = @"D:\UnityProject\ANNProject_DigMed\(Deprecated)Document-Image-Unwarping-pytorch-master\playground.py";
        //参数输入
        if(argvs!=null)
        {
            foreach (string temp in argvs)
            {
                path += " " + temp;
            }
        }

        //Python解释器
        process.StartInfo.FileName = @"D:\Tools\Miniconda3\envs\unwarping_assignment\python.exe";

        UnityEngine.Debug.Log("run python " + path);

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.Arguments = path;     // 路径+参数
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = false;        // 不显示执行窗口

        // 开始执行，获取执行输出，添加结果输出委托
        process.Start();

        process.BeginOutputReadLine();
        process.OutputDataReceived += new DataReceivedEventHandler(ReceivedHandler);
        process.WaitForExit();
 
    }

    //输出的信息
    void ReceivedHandler(object sender, DataReceivedEventArgs e)
    {
        print("recievd data: "+e.Data.ToString());
        if (!string.IsNullOrEmpty(e.Data))
        {

            
            //图片的回显
            filePath = e.Data.ToString();
            flag = true;
        }
    }
}
