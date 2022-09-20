using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class BasePanel : MonoBehaviour
{
    //当面板进入的时候调用
    public virtual void OnEnter()
    {
        //设置初始的位置
        transform.localPosition = new Vector3(1920,0,0);

        //移动到屏幕中间
        transform.DOLocalMoveX(0,0.5f);
    }


    //当面板离开的时候调用
    public virtual void OnExit()
    {
        transform.DOLocalMoveX(-1920,0.5f);
    }



    //返回按钮的点击事件
    public void OnBackButtonClick()
    {
        //出栈
        UIManager.Instance.PopPanel();
    }

}
