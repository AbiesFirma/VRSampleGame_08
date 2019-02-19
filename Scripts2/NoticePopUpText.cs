using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticePopUpText : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] GameObject popUpCanvas;
    [SerializeField] string enterText; 

    
	void Start()
    {
       
    }

	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "playerRoot")
        {
            player = GameObject.FindWithTag("playerRoot");

            //var popPos = GameObject.Find("NoticePopUp");
            var popPos = player.transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject
                               .transform.Find("CenterEyeAnchor").gameObject.transform.Find("NoticePopUp").gameObject;
            var pop = Instantiate(popUpCanvas, popPos.transform.position, popPos.transform.rotation);
            //popUpCanvas.SetActive(true);
            pop.transform.parent = popPos.transform; 
            var text = pop.transform.Find("Text").gameObject;
            text.GetComponent<Text>().text = string.Format("{0}", enterText); 
        }
    }
}
