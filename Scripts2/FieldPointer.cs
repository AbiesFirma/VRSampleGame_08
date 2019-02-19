using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドでのプレイヤーのポインターを管理するクラス
/// </summary>
public class FieldPointer : MonoBehaviour {

    [SerializeField]
    private Transform _RightHandAnchor;
    [SerializeField]
    private Transform _LeftHandAnchor;
    [SerializeField]
    private Transform _CenterEyeAnchor;

    //コントローラーの取得
    private Transform Pointer
    {
        get
        {
            var controller = OVRInput.GetActiveController();
            if (controller == OVRInput.Controller.RTrackedRemote)
            {
                return _RightHandAnchor;
            }
            else if (controller == OVRInput.Controller.LTrackedRemote)
            {
                return _LeftHandAnchor;
            }
            // どちらも取れなければ
            return _CenterEyeAnchor;
        }
    }

    //レイ用
    float maxRayDistance = 50.0f;

    float hitdistance;
    GameObject hitObj;
    Vector3 hitpoint;
    Vector3 hitNormal;

    //掴んでいるオブジェクト
    float grabDis;
    bool grabMode;
    GameObject grabObj = null;
    //Rigidbody grabObjRB;

    //マーカー（インスペクターから接続）
    [SerializeField] GameObject pointerMarker;
    [SerializeField] GameObject handMarkerP;
    [SerializeField] GameObject handMarkerG;

    RectTransform pointerMarkerTrans;
    Vector3 currentMarkerScale;

    //LineRenderer lineRenderer;



    void Start()
    {
        pointerMarker.SetActive(false);
        pointerMarkerTrans = pointerMarker.GetComponent<RectTransform>();
        currentMarkerScale = pointerMarkerTrans.localScale;

        handMarkerG.SetActive(false);
        handMarkerP.SetActive(false);

        grabMode = false;
    }

    void Update()
    {
        var pointer = Pointer;
        if (pointer == null)
        {
            return;
        }

        // コントローラー位置からRayを飛ばす
        Ray pointerRay = new Ray(pointer.position, pointer.forward);

        //可視化
        Debug.DrawRay(pointerRay.origin, pointerRay.direction * maxRayDistance, Color.green);
        int layerMask = ~(1 << 15 | 1 << 5);
        RaycastHit hit;

        //レイが当たった時、ヒットしたものによっての処理
        if (Physics.Raycast(pointerRay, out hit, maxRayDistance, layerMask ))
        {
            hitdistance = hit.distance;
            hitObj = hit.collider.gameObject;
            hitpoint = hit.point;
            hitNormal = hit.normal;

            pointerMarker.SetActive(true);
            pointerMarker.transform.position = hit.point;
            pointerMarker.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hitNormal);

            MarkerScalseChange();


            if (grabMode)
            {
                handMarkerP.SetActive(false);
                handMarkerG.SetActive(true);

                if (grabObj != hitObj && grabDis > hitdistance)
                {
                    handMarkerG.transform.position = hitpoint - (pointer.forward * 0.5f);
                    grabObj.transform.position = hitpoint - (pointer.forward * 0.5f);
                }
                else
                {
                    handMarkerG.transform.position = pointer.position + (pointer.forward * grabDis);
                    grabObj.transform.position = pointer.position + (pointer.forward * grabDis);
                }
            }
            else
            {

                //GrabObject
                if (hitObj.layer == 21)
                {
                    handMarkerP.SetActive(true);
                    handMarkerG.SetActive(false);

                    handMarkerP.transform.position = pointerMarker.transform.position;


                    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(1))
                    {
                        handMarkerP.SetActive(false);
                        handMarkerG.SetActive(true);
                        grabObj = hitObj;
                        grabDis = hitdistance;

                        grabMode = true;
                    }

                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButton(1))
                    {
                        handMarkerP.SetActive(false);
                        handMarkerG.SetActive(true);
                    }
                    else
                    {
                        grabMode = false;

                        handMarkerP.SetActive(true);
                        handMarkerG.SetActive(false);
                        handMarkerP.transform.position = pointerMarker.transform.position;

                    }
                }
                else
                {
                    handMarkerP.SetActive(false);
                    handMarkerG.SetActive(false);
                }
            }

            
        }
        //レイが当たってない
        else
        {
            pointerMarker.SetActive(false);

            handMarkerP.SetActive(false);
            handMarkerG.SetActive(false);

            grabMode = false;
        }


        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonUp(1))
        {
            grabMode = false;
        }
    }



    void MarkerScalseChange()
    {
        pointerMarkerTrans.localScale = currentMarkerScale * 0.1f * hitdistance;
    }
}
