using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private float movSpeed = 5;
    public Vector2 mov;
    private bool viraDir = true;

    public float vida, fullvida = 100;
    public bool canDano = true; // se pode tomar ataque

    public bool enemyInRange = false, canAttack = true, atacando = false;
    public float ataqueValor = 10, timeBtwAttack, startTimeBtwAttack = .3f, attackRange;
    public Transform attackPos;
    public LayerMask enemiesLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vida = fullvida;
    }

    void Update()
    {
        Ataque();

        if (vida <= 0) { vida = 0; }
        else if (vida > fullvida) { vida = fullvida; }

        if (mov.x > 0 && !viraDir || mov.x < 0 && viraDir)
        {
            Flip();
        } 
    }

    void FixedUpdate()
    {
        Movimentacao();
    }

    void Movimentacao()
    {
        mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.MovePosition(rb.position + mov.normalized * movSpeed * Time.fixedDeltaTime);
    }

    void Flip()
    {
        //gameObject.GetComponent<SpriteRenderer>().flip = 
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        viraDir =! viraDir;
    }

    void Ataque()
    {
        if (timeBtwAttack <= 0)
        {
            canAttack = true;
            if (Input.GetButton("Fire1") && canAttack)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemiesLayer);
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<Inimigo>().vida -= ataqueValor;
                    Vector3 direction = (enemies[i].transform.position - transform.position);
                    enemies[i].GetComponent<Inimigo>().rb.AddForce(direction * 500);
                    StartCoroutine(AttackCooldown(enemies[i]));
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else 
        { 
            timeBtwAttack -= Time.deltaTime; 
            canAttack = false;
        }
    }

    IEnumerator AttackCooldown(Collider2D obj) // no inimigo
    {
        obj.GetComponent<SpriteRenderer>().color = Color.red;
        
        yield return new WaitForSeconds(.5f);

        obj.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }


}
