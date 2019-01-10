using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器（銃）をコントローラーのポインター（Sphere）の方向に向けるためのクラス
/// </summary>
public class GunDirectionController : MonoBehaviour
{
    [SerializeField] Transform sphere;
    
    void Update()
    {
        this.transform.LookAt(sphere);
    }
}
