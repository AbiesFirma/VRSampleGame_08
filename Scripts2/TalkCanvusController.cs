using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 吹き出しのようなキャンバスを開閉するクラス
/// </summary>
public class TalkCanvusController : MonoBehaviour {

    [SerializeField] GameObject TalkCanvus;

    //呼び出されて状況に応じて開閉
    public void Pointer_onClick()
    {
        if (!TalkCanvus.activeSelf)
        {
            TalkCanvus.SetActive(true);
        }
        else
        {
            TalkCanvus.SetActive(false);
        }
    }

    //呼び出されてアタッチされている自身のオブジェクトを閉じる
    public void CloseSelf()
    {
        this.gameObject.SetActive(false);
    }

}
