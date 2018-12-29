using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_action : MonoBehaviour
{

    [SerializeField] private GameObject player_Root;
    [SerializeField] private GameObject Scene_Changer;

    public void OnClickRe_set()
    {
        if (Input.GetMouseButton(0) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
#if UNITY_EDITOR
            player_Root.GetComponent<UNITY_EDITOR_mover>().Re_set();
#else
            player_Root.GetComponent<OculusGo_Mover>().Re_set();
#endif

        }

    }

    public void OnClickRestart()
    {
        if (Input.GetMouseButton(0) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Scene_Changer.GetComponent<Scene_Changer>().Restart();
        }
    }
}
