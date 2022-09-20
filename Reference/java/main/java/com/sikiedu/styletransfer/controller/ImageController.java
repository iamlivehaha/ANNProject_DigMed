package com.sikiedu.styletransfer.controller;


import org.springframework.boot.SpringApplication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartFile;

import java.io.BufferedReader;
import java.io.File;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.UUID;

@Controller
public class ImageController {

    @RequestMapping("/unity")
    @ResponseBody
    public String unity() throws Exception {
        //处理,传入两个图片的路径image。Style，得到ai处理之后的图片路径
        String path = getAIImagePath("D:\\Project\\Unity_siki\\AIProject\\Assets\\Image\\Before\\Image.jpg","D:\\Project\\Unity_siki\\AIProject\\Assets\\Image\\Style\\Style.jpg");

        return path;
    }




    @RequestMapping("/index")
    public String index(){


        return "index.html";
    }

    @RequestMapping("/getImage")
    @ResponseBody
    public String getImage(
            @RequestParam(value = "image")MultipartFile image,
            @RequestParam(value = "style")MultipartFile style) throws Exception {

        //如果用户没有上传图片
        if(image.isEmpty() || style.isEmpty()){
            System.out.println("没有上传图片");
            return "没有上传图片";
        }

        //把图片保存到本地
        // 文件的名称
        String imageName = UUID.randomUUID() + ".jpg";
        String styleName = UUID.randomUUID() + ".jpg";

        // 文件的路径
        String filePath = "D:\\Project\\siki_python\\style_transfer\\image\\";

        //两张图片的路径
        String imagePath = filePath + imageName;
        String stylePath = filePath + styleName;

        // 创建文件
        File destImage = new File(imagePath);
        File destStyle = new File(stylePath);

        //如果文件夹不存在，则创建(略)
        // 图片的保存
        image.transferTo(destImage);
        style.transferTo(destStyle);

        //调用Python代码，传入2个图片的路径
        String aiPath = getAIImagePath(imagePath,stylePath);

        //System.out.println(aiPath);
        //返回图片的路径

        //让HTML进行回显图片

        return aiPath;
    }

    //调用Python代码，拿到生成图片的路径
    public String getAIImagePath(String imagePath,String stylePath) throws Exception {

        Process proc;
        //python的解释器，python代码，参数
        String[] args = new String[]{"D:\\_Study\\Anaconda\\envs\\python_3.6_tf_2.0_siki\\python","D:\\Project\\siki_python\\style_transfer\\lain.py",imagePath,stylePath};

        //运行cmd
        proc = Runtime.getRuntime().exec(args);

        //使用输入输出流
        BufferedReader in = new BufferedReader(new InputStreamReader(proc.getInputStream()));

        String line = null;
        String content = null;
        while ((line = in.readLine()) != null){
            content = line;
        }

        in.close();
        proc.waitFor();

        return content;
    }


}
