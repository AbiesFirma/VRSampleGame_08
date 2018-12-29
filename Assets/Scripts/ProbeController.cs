using System.Collections;
using UnityEngine;
using UnityEngine.VR;

public class ProbeController : MonoBehaviour
{

    ReflectionProbe probe;
    [SerializeField] GameObject PlayerCamera;
    Vector3 camera_pos;

    void Start()
    {
        this.probe = GetComponent<ReflectionProbe>();
    }

    void Update()
    {
        PlayerCamera = GameObject.Find("PlayerRoot");
        camera_pos = PlayerCamera.transform.position;

        this.probe.transform.position = new Vector3(camera_pos.x, (camera_pos.y * -1) - 1.0225f, camera_pos.z);

        probe.RenderProbe();
    }
}
