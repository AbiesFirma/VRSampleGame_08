using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 敵を撃破したときのポイントをポップアップするためのクラス
/// </summary>
[RequireComponent(typeof(TextMesh))]

public class PopupText : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        var textMesh = GetComponent<TextMesh>();
        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(0.3f, 0.2f));
        sequence.Append(transform.DOMoveY(3.0f, 3.0f).SetRelative());

        var color = textMesh.color;
        color.a = 0.0f;

        sequence.Join(DOTween.To(() => textMesh.color,
        c => textMesh.color = c, color, 0.3f).SetEase(Ease.InOutQuart));

        sequence.OnComplete(() => Destroy(gameObject));
	}
		
}
