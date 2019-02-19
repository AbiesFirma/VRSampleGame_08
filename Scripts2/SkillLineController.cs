using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルでスキルを設置する際ラインを表示するクラス
/// </summary>

[RequireComponent(typeof(LineRenderer))]
public class SkillLineController : MonoBehaviour {

    LineRenderer lineRenderer;
    GameObject player;
    GameObject chara;
    GameObject sCharaAttackPoint;

    //[SerializeField] float lineHeight = 1.0f;

    void Start()
    {
        player = GameObject.FindWithTag("playerRoot");
        chara = player.GetComponent<CharacterMoveOrder>().sChara;
        sCharaAttackPoint = chara.transform.Find("AttackPoint").gameObject;
        //point = new Vector3(chara.transform.position.x, chara.transform.position.y + lineHeight, chara.transform.position.z);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, sCharaAttackPoint.transform.position);
        lineRenderer.startWidth = 0.8f;
        lineRenderer.endWidth = 0.8f;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, sCharaAttackPoint.transform.position);
        lineRenderer.startWidth = 0.8f;
        lineRenderer.endWidth = 0.8f;
    }
}

