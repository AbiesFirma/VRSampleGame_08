using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コントローラから出るレーザーをUIからオンオフするクラス
/// </summary>
public class LineChanger : MonoBehaviour {

    public GameObject LineObj;
    private LineRenderer lr;
    //Toggle Laser_tgg;
    bool nonLaser_bool = false;
    

    //トグルの状態が変わるたびに呼び出される
    //最初はチェックが入ってるので、入ってる時がnonLaser_bool=falseになる
    //トグルにチェックが入ってる、nonLaser_bool = falseがレーザーあり
    //トグルにチェックが入っいない、nonLaser_bool = trueがレーザーなし
    public void LaserChange()
    {
        lr = LineObj.GetComponent<LineRenderer>();
        if (nonLaser_bool)
        {
            lr.startWidth = 0.008f;
            lr.endWidth = 0.005f;

            nonLaser_bool = false;
        }
        else
        {
            lr.startWidth = 0.0f;
            lr.endWidth = 0.0f;

            nonLaser_bool = true;
        }
    }
    

}
