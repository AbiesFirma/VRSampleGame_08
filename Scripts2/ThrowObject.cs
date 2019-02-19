using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 斜方投射を再現しスキルのボムなどをなげるためのクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ThrowObject : MonoBehaviour
{

    public Vector3 target;
    public bool targetSet = false;

    [SerializeField] float skillPower = 1.0f;     //スキルの攻撃力
    float damage;
    public float charaAttackPower = 1.0f;   //発動時キャラから上書きされる
    [SerializeField] GameObject bombHitParticlePrefab;
    [SerializeField] GameObject bombHitSphere;

    /// 射出角度
    [SerializeField, Range(0F, 90F)]
    private float ThrowingAngle = 60f;
    //演出用
    [SerializeField] Material explotionMat;
    [SerializeField] float explodeScale = 6.0f;
    Collider col;

    float colTimer;


    private void Awake()
    {
        //target = player.GetComponent<CharacterMoveOrder>().setSkillPos;
    }


    private void Start()
    {
        col = GetComponent<Collider>();

        col.enabled = false;
    }

    private void Update()
    {
        colTimer += Time.deltaTime;

        if (targetSet && target != null)
        {
            ThrowingBall();
            
            targetSet = false;
        }

        if(colTimer >= 0.5f)
        {
            col.enabled = true;
        }

    }

    /// ボールを射出する
    private void ThrowingBall()
    {
        if (target != null)
        {
            // 射出角度
            float angle = ThrowingAngle;

            // 射出速度を算出
            Vector3 velocity = CalculateVelocity(this.transform.position, target, angle);

            // 射出
            Rigidbody rid = GetComponent<Rigidbody>();
            rid.AddForce(velocity * rid.mass, ForceMode.Impulse);     //F=ma
        }

    }


    //標的に命中する射出速度の計算
    //引数１pointA=射出開始座標
    //引数２pointB=標的の座標
    // <returns>射出速度</returns>
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);　　　//y=x*tanθ
        }
    }


    //ヒットしたときの処理
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "floor" || other.tag == "enemy") //(other.tag != "OrderCharactor" || other.tag != "playerRoot")
        {
            GetComponent<Renderer>().material = explotionMat;
            //transform.localScale = new Vector3(explodeScale, explodeScale, explodeScale);

            //着弾地点に演出と当たり判定用オブジェクトを生成
            Instantiate(bombHitParticlePrefab, transform.position, transform.rotation);
            var bombEx = Instantiate(bombHitSphere, transform.position, transform.rotation);
            bombEx.GetComponent<AttackSphere>().charaAttackPower = charaAttackPower;
            Destroy(gameObject);
        }
        /*
        if (other.tag == "enemy")
        {
            damage = skillPower * charaAttackPower;
            other.SendMessage("OnHitAttack", damage, SendMessageOptions.RequireReceiver);            
        }
        */


    }
}