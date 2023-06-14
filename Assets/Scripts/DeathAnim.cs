using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathAnim : MonoBehaviourPun
{
    public float destroyTime;
    
    void Start()
    {
        Invoke("DestroyObject", destroyTime);
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
    
}
