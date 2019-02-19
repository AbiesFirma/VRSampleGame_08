using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター上部のスキルオブジェクトの管理クラス
/// </summary>
public class SkillSphere : MonoBehaviour {

    [SerializeField] string skillName = "skillName";   //スキルの名前
    public string _skillName { get; private set; }

    [SerializeField] GameObject skillRange;         //スキルをセレクトした際効果範囲を表すオブジェクト
    public GameObject _skillRange { get; private set; }

    [SerializeField] float clamp = 5.0f;        //射程による移動制限
    public float _clamp { get; private set; }

    [SerializeField] string animationName = "SkillName";    //アニメーションの名前
    public string _animeName { get; private set; }

    [SerializeField] float animationTime = 3.0f;
    public float _animeTime { get; private set; }

    [SerializeField] float useAp = 10.0f;
    public float _useAp { get; private set;}
    

    void Start () {
        _skillRange = skillRange;
        _clamp = clamp;
        _animeName = animationName;
        _skillName = skillName;
        _animeTime = animationTime;
        _useAp = useAp;
    }
	
	
	void Update () {
		
	}

    

}
