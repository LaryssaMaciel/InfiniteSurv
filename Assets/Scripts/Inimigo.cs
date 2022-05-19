using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float cooldown = .5f, dano = 10, vida = 30;
    private float speed = .01f;

    public bool playerInRange = false, canAttack = true;
    public bool atacou = false, still = false; // morrendo

    private Vector3 offset;

    private GameObject player, explosion;
    private SpriteRenderer sp;
    public Rigidbody2D rb;
    public GameObject cam; 

    void Awake()
    {
        player = GameObject.Find("Player");
        explosion = this.gameObject.transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("CM vcam1");
        offset = new Vector3(.5f,0,0);
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!still) // se nao ta morrendo
        {
            Ataque();
            Flip();
            if (vida <= 0) { StartCoroutine(Death()); }
        }
        if (!atacou && !still) { Mover(); }
        
    }

    IEnumerator Death()
    {
        explosion.SetActive(true);
        // ficar invisivel
        sp.enabled = false;
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
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
            still = true;
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
        if (col.gameObject.tag == "bullet")
        {
            this.vida -= player.GetComponent<Player>().ataqueValor;
            Destroy(col.gameObject);
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
        player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
        player.GetComponent<Player>().canDano = false;
        player.GetComponent<Player>().damaged = true;
        cam.GetComponent<CameraShake>().shake = true;

        yield return new WaitForSeconds(cooldown);

        player.GetComponent<Player>().canDano = true;
        player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        canAttack = true;
        atacou = false;
        still = false;
    }
}
