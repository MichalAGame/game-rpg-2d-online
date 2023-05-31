using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public string enemyName;
    public float moveSpeed;
    public int currentHp;
    public int maxHp;
    public float chaseRange;
    public float attackRange;
    private PlayerController targetPlayer;
    public float playerdetectRate;
    private float lastPlayerDetectTime;
    public string objectTospawnOnDeath;
    public int damage;
    public float attackrate;
    private float lastattackTime;
    public HeaderInformation healthBar;
    public SpriteRenderer sr;
    public Animator anim;
    public Rigidbody2D rb;

    private void Start()
    {
        healthBar.Initialized(enemyName, maxHp);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if(targetPlayer != null)
        {
            float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);
            float face = targetPlayer.transform.position.x - transform.position.x;

            if(face > 0)
            {
                photonView.RPC("FlipRight", RpcTarget.All);
            }
            else
            {
                photonView.RPC("FlipLeft", RpcTarget.All);
            }

            if(dist < attackRange && Time.time - lastattackTime >= attackrate)
            {
                Attack();
            }
            else if(dist > attackRange)
            {
                Vector3 dir = targetPlayer.transform.position - transform.position;
                rb.velocity = dir.normalized * moveSpeed;
                anim.SetBool("Walk", true);
            }
            else
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Walk", false);
            }
        }

        DetectPlayer();
    }

    [PunRPC]
    void FlipRight()
    {
        sr.flipX = false;
    }

    [PunRPC]
    void FlipLeft()
    {
        sr.flipX = true;
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        lastattackTime = Time.time;
        targetPlayer.photonView.RPC("TakeDamage", targetPlayer.photonPlayer, damage);
    }

    void DetectPlayer()
    {
        if(Time.time -lastPlayerDetectTime > playerdetectRate)
        {
            lastPlayerDetectTime = Time.time;

            foreach(PlayerController player in GameManager.instance.players)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if(player == targetPlayer)
                {
                    if(dist > chaseRange)
                    {
                        targetPlayer = null;
                        anim.SetBool("Walk", false);
                        rb.velocity = Vector2.zero;
                    }
                }else if(dist < chaseRange)
                {
                    if(targetPlayer == null)
                    {
                        targetPlayer = player;
                    }
                }
            }
        }
    }



    [PunRPC]
    public void TakeDamage(int damageAmount)
    {
        currentHp -= damageAmount;
        healthBar.photonView.RPC("UpdateHealthBar", RpcTarget.All, currentHp);


        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            photonView.RPC("FlashDamage", RpcTarget.All);
        }

    }
    [PunRPC]
    void FlashDamage()
    {
        StartCoroutine(DamageFlash());
        IEnumerator DamageFlash()
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
    }

    void Die()
    {
        if (objectTospawnOnDeath != string.Empty)
            PhotonNetwork.Instantiate(objectTospawnOnDeath, transform.position, Quaternion.identity);

        PhotonNetwork.Destroy(gameObject);

    }
}
