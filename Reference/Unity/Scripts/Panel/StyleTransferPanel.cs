using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;

public class StyleTransferPanel : BasePanel
{   
    //返回按钮
    private UnityEngine.UI.Button backButton;

    //上传图片的按钮
    private UnityEngine.UI.Button uploadImageButton;
    private UnityEngine.UI.Button uploadStyleButton;
    private UnityEngine.UI.Button createAIButton;

    //图片的显示Image
    private Image imageImage;
    private Image styleImage;
    private Image aiImage;

    bool flag = false;
    string filePath = null;

    private void Update()
    {
        if (flag)
        {
            //更换图片
            StartCoroutine(ChangeImage(@"D:\Project\siki_python\style_transfer\image\" + filePath));
            flag = false;

        }
    }


    private void Awake()
    {
        //赋值
        backButton = transform.Find("BackButton").GetComponent<UnityEngine.UI.Button>();
        uploadImageButton = transform.Find("Image/ChangeImageButton").GetComponent<UnityEngine.UI.Button>();
        uploadStyleButton = transform.Find("Style/ChangeImageButton").GetComponent<UnityEngine.UI.Button>();
        createAIButton = transform.Find("AIImage/CreateImageButton").GetComponent<UnityEngine.UI.Button>();

        //获得图片
        imageImage = transform.Find("Image").GetComponent<Image>();
        styleImage = transform.Find("Style").GetComponent<Image>();
        aiImage = transform.Find("AIImage").GetComponent<Image>();

        //添加点击事件
        backButton.onClick.AddListener(OnBackButtonClick);
        uploadImageButton.onClick.AddListener(OnUploadImageButtonClick);
        uploadStyleButton.onClick.AddListener(OnUploadStyleButtonClick);

        //有两个不同的点击事件，一个是调用Java一个直接调用Python
        createAIButton.onClick.AddListener(OnCreateAIButtonClickWithPython);
    }

    //得到用户的图片
    private void GetUserSelectImage(Image echoImage,string filename)
    {
        //打开文件选择的面板
        OpenFileDialog od = new OpenFileDialog();
        od.Title = "请选择图片";
        od.Multiselect = false;
        od.Filter = "图片文件（*.jpg,*.png）|*.jpg;*.png";
        //如果用户点击了OK
        if(od.ShowDialog() == DialogResult.OK)
        {
            StartCoroutine(GetTexture(od.FileName, echoImage,filename));
        }
    }

    //获得到用户传入的图片。吧图片放道echoImage中
    IEnumerator GetTexture(string url,Image echoImage,string filename)
    {
        WWW www = new WWW("file://"+url);
        yield return www;
        //如果拿到了
        if (www.isDone)
        {
            Texture2D texture2D = new Texture2D(400,500);
            www.LoadImageIntoTexture(texture2D);
            //将Texture变成Sprite，放入Image中
            Sprite sprite = Sprite.Create(texture2D,new Rect(0,0,texture2D.width,texture2D.height),Vector2.zero);
            echoImage.sprite = sprite;


            //把图片保存到本地
            byte[] bytes = texture2D.EncodeToJPG();

            //创建文件
            FileStream file = null;

            if (filename.Equals("Image"))
            {
                file = File.Open(@"D:\Project\Unity_siki\AIProject\Assets\Image\Before\" + filename + ".jpg", FileMode.OpenOrCreate);

            }
            else
            {
                file = File.Open(@"D:\Project\Unity_siki\AIProject\Assets\Image\Style\" + filename + ".jpg", FileMode.OpenOrCreate);
            }
            //文件写入

            BinaryWriter bw = new BinaryWriter(file);
            bw.Write(bytes);

            file.Close();


        }
    }

    //上传图片按钮点击事件
    private void OnUploadImageButtonClick()
    {
        //拿到用户上传的图片
        GetUserSelectImage(imageImage,"Image");
        //回显
    }

    //上传风格按钮点击事件
    private void OnUploadStyleButtonClick()
    {
        //拿到用户上传的图片
        GetUserSelectImage(styleImage,"Style");
    }




    

    //图片的回显
    IEnumerator ChangeImage(string url)
    {
        WWW www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D tex = www.texture;
            Sprite temp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

            aiImage.sprite = temp;
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
        StreamReader reader = new StreamReader(myResponseStream,Encoding.GetEncoding("utf-8"));

        string res = reader.ReadToEnd();

        reader.Close();
        myResponseStream.Close();
        return res;
    }


    //生成风格迁移点击事件
    private void OnCreateAIButtonClickWithPython()
    {
        RunPython();
    }

    private void RunPython()
    {
        //开启命令行的
        Process p = new Process();

        //Python脚本
        string path = @"D:\Project\siki_python\style_transfer\unity.py";

        //Python解释器
        p.StartInfo.FileName = @"D:\_Study\Anaconda\envs\python_3.6_tf_2.0_siki\python.exe";
        p.StartInfo.Arguments = path;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();

        p.BeginOutputReadLine();
        p.OutputDataReceived += new DataReceivedEventHandler(ReceivedHandler);
        
    }

    //输出的信息
    void ReceivedHandler(object sender,DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            
            print(e.Data.ToString());
            filePath = e.Data.ToString();
            flag = true;
            //图片的回显
            //StartCoroutine(ChangeImage(@"D:\Project\siki_python\style_transfer\image\" + e.Data));
            /*WWW www = new WWW("file://"+@"D:\Project\siki_python\style_transfer\image\"+e.Data);
            if (www.isDone)
            {
                print("-----");
                Texture2D tex = www.texture;
                Sprite temp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

                aiImage.sprite = temp;

            }*/

        }
    }

    //生成风格迁移点击事件
    private void OnCreateAIButtonClickWithJava()
    {

        //调用Java服务器。传入两个图片的路径，获得到AI生成图片的路径
        string resp = GetHttpResponse("http://localhost:8080/unity");

        //图片的回显
        StartCoroutine(ChangeImage(@"D:\Project\siki_python\style_transfer\image\" + resp));

    }
}
