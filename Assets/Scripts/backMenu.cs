using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class backMenu : MonoBehaviour
{
    // Start is called before the first frame update
  

    public void Home()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
        Destroy(GameObject.Find("Network Manager"));
    }
}
