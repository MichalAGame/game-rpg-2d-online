using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MagicBall : MonoBehaviourPun
{
    public float speed;
    public Vector2 moveDirection;
    private Rigidbody2D rb;
    public int damage;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("Attack"))
        {
            damage = PlayerPrefs.GetInt("Attack");
        }

        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyObject", 1);

    }


    
    void Update()
    {
        rb.velocity = moveDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            photonView.RPC("DestroyObject", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
