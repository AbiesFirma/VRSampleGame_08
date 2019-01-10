using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIテキストを点滅させるためのクラス
/// </summary>
public class tenmetsuText : MonoBehaviour {
    Text text;
    [SerializeField] float speed = 0.3f;
    float _alpha;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        _alpha = text.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        float current_a = text.color.a;   //元のアルファ値

        if (current_a > 0)
        {
            text.color = new Color(text.color.r,
                                   text.color.g,
                                   text.color.b,
                                   text.color.a - (0.1f * speed));
        }
        else
        {
            text.color = new Color(text.color.r,
                                   text.color.g,
                                   text.color.b,
                                   _alpha);
        }


    }
}
