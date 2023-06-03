using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathAnim : MonoBehaviourPun
{
    
    void Start()
    {
        Invoke("DestroyObject", 1);
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
    
}
