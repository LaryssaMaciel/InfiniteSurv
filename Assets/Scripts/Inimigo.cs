using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float cooldown = .5f, dano = 10, vida = 30;
    private float speed = .1f;

    private bool playerInRange = false, canAttack = true;
    public bool atacou = false;

    private Vector3 offset;

    private GameObject player;
    public Rigidbody2D rb;
    private GameObject cam;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("CM vcam1");
        offset = new Vector3(.5f,0,0);
    }

    void Update()
    {
        Ataque();
        Flip();

        if (!atacou) { Mover(); }

        if (vida <= 0) { Destroy(this.gameObject); }
    }

    void Flip() 
    {
        if (player.transform.position.x < this.transform.position.x)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void Mover()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position - offset, speed * Time.timeScale);
    }

    void Ataque()
    {
        if (playerInRange && canAttack && player.GetComponent<Player>().canDano)
        {
            player.GetComponent<Player>().vida -= dano;
            atacou = true;
            StartCoroutine(AttackCooldown());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        player.GetComponent<Player>().canDano = false;
        
        cam.GetComponent<CameraShake>().shake = true;

        yield return new WaitForSeconds(cooldown);

        player.GetComponent<Player>().canDano = true;
        canAttack = true;
        atacou = false;
    }
}
