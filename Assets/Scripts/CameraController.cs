using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.me !=null && !PlayerController.me.dead)
        {
            Vector3 targetPosition = PlayerController.me.transform.position;
            targetPosition.z = -10;
            transform.position = targetPosition;
        }
    }
}
