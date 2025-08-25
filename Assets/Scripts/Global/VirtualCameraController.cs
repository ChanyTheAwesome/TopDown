using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    public int currentPriority = 5;
    public int activePriority = 20;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Priority = currentPriority;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            virtualCamera.Priority = activePriority;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            virtualCamera.Priority = currentPriority;
        }
    }
}
