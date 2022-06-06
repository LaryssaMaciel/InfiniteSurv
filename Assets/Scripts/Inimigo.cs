using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float cooldown = .5f, dano = 10, vida = 30, speed = .01f;

    public bool playerInRange = false, canAttack = true;
    public bool atacou = false, still = false; // morrendo

    private Vector3 offset;

    private GameObject player, explosion;
    private SpriteRenderer sp;
    public Rigidbody2D rb;
    public Vector2 rbmov;
    public GameObject cam; 
    public AudioSource audioSource; 
    public AudioClip[] som;
    public Animator anim;

    void Awake()
    {
        player = GameObject.Find("Player");
        explosion = this.gameObject.transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("CM vcam1");
        offset = new Vector3(.5f,0,0);
        sp = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        

        if (!still) // se nao ta morrendo
        {
            Ataque();
            Flip();
            if (!atacou) { Mover(); }
            if (vida <= 0) { StartCoroutine(Death()); }
        }

        Animacoes();
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
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    public Vector3 direction;
    void Mover()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position - offset, speed * Time.timeScale);
        // direction = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // this.rbmov.y = direction.y;
        if (player.transform.position.y - this.transform.position.y > 1f) { rbmov.y = 1f; }
        if (player.transform.position.y - this.transform.position.y < -1f) { rbmov.y = -1f; }
    }

    public void AudioManager(int audio)
    {
        audioSource.clip = som[audio];
        audioSource.Play();
    }

    void Ataque()
    {
        if (playerInRange && canAttack && player.GetComponent<Player>().canDano)
        {
            still = true;
            player.GetComponent<Player>().vida -= dano;
            atacou = true;
            swing = true;
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
            this.AudioManager(0);
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

    private bool swing = false; 
    public bool damaged = false;

    void Animacoes()
    {
        if (!damaged && !swing)
        {
            anim.SetFloat("vertical", rbmov.y);
        }

        if (swing) { anim.SetTrigger("swing"); swing = false; }
        if (damaged) { anim.SetTrigger("damage"); damaged = false; }
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
