using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// componenta Singleton, ce se asigura ca pe tot parcursul jocului va exista o singura "camera"
public class RPGCameraManager : MonoBehaviour
{
    private static RPGCameraManager sharedInstance;
    
    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        } else
        {
            sharedInstance = this;
        }
        var vCamGameObject = GameObject.FindWithTag("VirtualCamera");
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
}
