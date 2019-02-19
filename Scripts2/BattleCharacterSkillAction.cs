using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションイベントによるスキル（結合したので未使用）
/// </summary>

public class BattleCharacterSkillAction : MonoBehaviour {

    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject attackSpherePrefab;

    float attackPower = 2.0f;

    private Animator animator;

    void Start () {
        animator = GetComponent<Animator>();
    }
	
	void Update () {
		
	}

    //animationEvent
    void Kick()
    {
        Instantiate(attackSpherePrefab, attackPoint.position, attackPoint.rotation);
        animator.SetBool("attack", false);
    }

    void Attack()
    {
        GameObject _attackShere = Instantiate(attackSpherePrefab, attackPoint.position, attackPoint.rotation);
        _attackShere.GetComponent<AttackSphere>().charaAttackPower = attackPower;
        animator.SetBool("attack", false);
    }

    
}
