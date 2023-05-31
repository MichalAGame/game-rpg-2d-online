using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    public bool faceRight;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public int damage;
    public float attackRange;
    public float attackDelay;
    public float lastAttackTime;
    
    [HideInInspector]
    public int id;
    public Animator playerAnim;
    public Rigidbody2D rig;
    public Player photonPlayer;
    public SpriteRenderer sr;
    //public HeaderInfo headerInfo;
    public float moveSpeed;
    public int gold;
    public int currentHP;
    public int maxHP;
    public bool dead;

    public static PlayerController me;
    public HeaderInformation headerInfo;

    [PunRPC]
    public void Initialized(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[id - 1] = this;
        headerInfo.Initialized(player.NickName, maxHP);
        GameUI.instance.UpdateHpText(currentHP, maxHP);

        if (PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetInt("Gold");
        }
        GameUI.instance.UpdateGoldText(gold);
        currentHP = maxHP;

        if (player.IsLocal)
            me = this;
        else
            rig.isKinematic = false;
    }

 

    // Update is called once per frame
    void Update()
    {
        
        if (!photonView.IsMine)
            return;
        Move();

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackDelay)
            Attack();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rig.velocity = new Vector2(x, y) * moveSpeed;

        if(x !=0 || y != 0)
        {
            playerAnim.SetBool("Move", true);

            if (x > 0)
            {
                photonView.RPC("FlipRight", RpcTarget.All);
            }
            else
            {
                photonView.RPC("FlipLeft", RpcTarget.All);
            }
           
        }
        else
        {
            playerAnim.SetBool("Move", false);
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        if (faceRight)
        {
            RaycastHit2D hit = Physics2D.Raycast(attackPointRight.position, transform.forward, attackRange);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();

                enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(attackPointLeft.position, transform.forward, attackRange);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();

                enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            }
        }

        playerAnim.SetTrigger("Attack");
    }

    [PunRPC]
    void FlipRight()
    {
        sr.flipX = false;
        faceRight = true;

    }

    [PunRPC]
    void FlipLeft()
    {
        sr.flipX = true;
        faceRight = false;
    }

    [PunRPC]
    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, currentHP);


        if(currentHP <= 0)
        {
            Die();
        }
        else
        {
            photonView.RPC("FlashDamage", RpcTarget.All);
        }

        GameUI.instance.UpdateHpText(currentHP, maxHP);
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
        dead = true;
        rig.isKinematic = true;
        transform.position = new Vector3(0, 90, 0);

        Vector3 spawnPos = GameManager.instance.spawnPoint[Random.Range(0, GameManager.instance.spawnPoint.Length)].position;
        StartCoroutine(Spawn(spawnPos, GameManager.instance.respawnTime));
    }

    IEnumerator Spawn(Vector3 spawnPos, float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);
        dead = false;
        transform.position = spawnPos;
        currentHP = maxHP;
        rig.isKinematic = false;

        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, currentHP);
        GameUI.instance.UpdateHpText(currentHP, maxHP);
    }

    [PunRPC]
    void Heal(int amountToHeal)
    {
        currentHP = Mathf.Clamp(currentHP + amountToHeal, 0, maxHP);
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, currentHP);
        GameUI.instance.UpdateHpText(currentHP, maxHP);
    }
    [PunRPC]
    void GetGold( int goldToGive)
    {
        gold += goldToGive;
        PlayerPrefs.SetInt("Gold", gold);
        GameUI.instance.UpdateGoldText(gold);
    }
}
