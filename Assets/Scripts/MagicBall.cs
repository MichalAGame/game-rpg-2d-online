using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MagicBall : MonoBehaviourPun
{
    public float speed;
    private int attackerId;
    private bool isMine;
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
        Destroy(gameObject, 2);

    }


    
    void Update()
    {
        rb.velocity = moveDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy"&&isMine)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient,this.attackerId, damage);
            
            Destroy(gameObject);
        }
    }

    public void Initialized(int attackId, bool isMine)
    {
        this.attackerId = attackId;
        this.isMine = isMine;
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
