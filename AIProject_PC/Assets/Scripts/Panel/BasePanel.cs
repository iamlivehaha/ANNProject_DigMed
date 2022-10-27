using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BasePanel : MonoBehaviour
{
    public virtual void OnEnter()
    {
        transform.localPosition = new Vector3(1920, 0, 0);

        transform.DOLocalMoveX(0, 0.5f);
    }

    public virtual void OnExit()
    {
        transform.DOLocalMoveX(-1920, 0.5f);
    }

    public void OnBackButtonClick()
    {
        UIManager.Instance.PopPanel();
    }
}
