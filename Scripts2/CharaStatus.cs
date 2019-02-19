using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラのステータスを定義するクラス
/// </summary>
public class CharaStatus : MonoBehaviour {

    public string charaName = "なまえ";
    public float charaHeight = 2.0f;

    public int lv = 1;

    public float maxHP = 500.0f;
    public float currentHp = 500.0f;

    public float ap = 10.0f;

    public float attackRange = 2.0f;
    public float attackCool = 3.0f;
    public float attackPower = 2.0f;

    public float dif;
    public float speed;
    public float dex = 1.0f;
}
