using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PickupTypes
{
    Gold,
    Health
}

public class Pickups : MonoBehaviourPun
{
    public PickupTypes type;
    public int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (type == PickupTypes.Gold)
                player.photonView.RPC("GetGold", player.photonPlayer, value);
            else if (type == PickupTypes.Health)
                player.photonView.RPC("Heal", player.photonPlayer, value);

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
