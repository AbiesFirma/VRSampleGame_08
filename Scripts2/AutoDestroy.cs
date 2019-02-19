using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 指定秒数で自動的に破壊されるためのクラス
/// </summary>
public class AutoDestroy : MonoBehaviour {

    [SerializeField] float lifetime = 5.0f;
    float time;

    //透明度を変えながら？
    [SerializeField] bool alphaWave = false;
    MeshRenderer mRenderer;
    float _alpha;
    float speed;
    float change_a;

    //イメージの透明度を変えながら？
    [SerializeField] bool imageAlphaWave = false;
    Color imageColor;
    float i_alpha;
    float i_speed;
    float i_change_a;

    //大きさを変えながら？
    [SerializeField] bool scaleWave = false;
    Vector3 scale;
    float _speed;



    void Start () {
        time = 0.0f;

        if (alphaWave)
        {
            mRenderer = GetComponent<MeshRenderer>();
            _alpha = mRenderer.material.color.a;
            speed = _alpha / (lifetime / 2);
            change_a = 0.0f;

            mRenderer.material.color = new Color(mRenderer.material.color.r,
                                                 mRenderer.material.color.g,
                                                 mRenderer.material.color.b,
                                                 change_a);
        }

        if (imageAlphaWave)
        {
            imageColor = GetComponent<Image>().color;
            _alpha = imageColor.a;
            speed = _alpha / (lifetime / 2);
            change_a = 0.0f;

            imageColor = new Color(imageColor.r,
                                   imageColor.g,
                                   imageColor.b,
                                   change_a);
        }

        if (scaleWave)
        {
            scale = GetComponent<Transform>().localScale;
            _speed = 1 / (lifetime * 0.5f);
            transform.localScale = Vector3.zero;
        }

        Destroy(gameObject, lifetime);
    }
	
	
	void Update () {
        time += Time.deltaTime;

        if (alphaWave)
        {
            if (time <= (lifetime / 2))
            {
                change_a += speed * Time.deltaTime;
            }
            else
            {
                change_a -= speed * Time.deltaTime;
            }
            mRenderer.material.color = new Color(mRenderer.material.color.r,
                                                 mRenderer.material.color.g,
                                                 mRenderer.material.color.b,
                                                 change_a);
        }

        if (imageAlphaWave)
        {
            if (time <= (lifetime / 2))
            {
                change_a += speed * Time.deltaTime;
            }
            else
            {
                change_a -= speed * Time.deltaTime;
            }
            imageColor = new Color(imageColor.r,
                                   imageColor.g,
                                   imageColor.b,
                                   change_a);
        }

        if (scaleWave)
        {
            if (time <= (lifetime / 2))
            {
                transform.localScale += new Vector3 (scale.x * _speed * Time.deltaTime, scale.y * _speed * Time.deltaTime, scale.z * _speed * Time.deltaTime);
            }
            else
            {
                transform.localScale -= new Vector3(scale.x * _speed * Time.deltaTime, scale.y * _speed * Time.deltaTime, scale.z * _speed * Time.deltaTime);
            }

        }

    }
}
