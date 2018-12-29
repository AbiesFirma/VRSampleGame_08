using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]

public class CanvasGroupFade : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.DOFade(1.0f, 1.0f).SetEase(Ease.InOutQuart).SetLoops(2, LoopType.Yoyo);
	}
		
}
