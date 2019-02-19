using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIを点滅させるためのクラス
/// </summary>
public class tenmetsuText : MonoBehaviour {
    Text text;
    Image image;
    [SerializeField] float speed = 0.3f;
    float _alpha;

    [SerializeField] bool textTenmetsu = true;
    [SerializeField] bool imageTenmetsu = false;

    float timer;
    [SerializeField] float tenmetsuTime = 0.5f;

    float current_a;

    void Start()
    {
        if (textTenmetsu)
        {
            text = GetComponent<Text>();
            _alpha = text.color.a;
        }
        if(imageTenmetsu)
        {
            image = GetComponent<Image>();
            _alpha = image.color.a;
        }

        current_a = _alpha;   //元のアルファ値
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (textTenmetsu)
        {
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

        if (imageTenmetsu)
        {
            if (timer < tenmetsuTime)
            {
                image.color = new Color(image.color.r,
                                       image.color.g,
                                       image.color.b,
                                       image.color.a - (0.1f * speed));
            }
            else
            {
                image.color = new Color(image.color.r,
                                       image.color.g,
                                       image.color.b,
                                       _alpha);
                timer = 0.0f;
            }
        }


    }
}
