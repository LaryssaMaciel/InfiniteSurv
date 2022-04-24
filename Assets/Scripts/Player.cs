using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float xMin, xMax, yMin, yMax;
    public SpriteRenderer mapa;

    public FixedJoystick fj;

    private SceneController sm;

    public Rigidbody2D rb;
    private float movSpeed = 5, moveforce = 5f;
    public Vector2 mov;
    private bool viraDir = true;

    public Image hpBar;
    public float vida, fullvida = 100, cura = 10;

    public int vidasExtra = 0;
    private Text curasNum;

    public bool canDano = true; // se pode tomar ataque

    public bool enemyInRange = false, canAttack = true, atacando = false;
    public float ataqueValor = 10, timeBtwAttack, startTimeBtwAttack = .3f, attackRange;
    public Transform attackPos;
    public LayerMask enemiesLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vida = fullvida;
        hpBar = GameObject.Find("Life").GetComponent<Image>();
        sm = GameObject.Find("SceneManager").GetComponent<SceneController>();
        curasNum = GameObject.Find("Coletados").GetComponent<Text>();
        fj = GameObject.FindGameObjectWithTag("joystick").GetComponent<FixedJoystick>();

        var spriteSize = GetComponent<SpriteRenderer>().bounds.size.y * .5f; // Working with a simple box here, adapt to you necessity
 
        var camHeight = mapa.bounds.size.y/2;
        var camWidth = mapa.bounds.size.x/2;

        yMin = -camHeight + spriteSize; // lower bound
        yMax = camHeight - spriteSize; // upper bound
        
        xMin = -camWidth + spriteSize; // left bound
        xMax = camWidth - spriteSize; // right bound 
    }
 
    void Update()
    {
        //Ataque();
        
        if (timeBtwAttack <= 0) { timeBtwAttack = 0; }
        else { timeBtwAttack -= Time.deltaTime; }

        if (vida <= 0) { vida = 0; sm.ChangeScene("GameOver"); }
        else if (vida > fullvida) { vida = fullvida; }

        hpBar.fillAmount = vida / fullvida;

        curasNum.text = vidasExtra.ToString();

        if (mov.x > 0 && !viraDir || mov.x < 0 && viraDir) { Flip(); } 


        float xValidPosition = Mathf.Clamp(transform.position.x, xMin, xMax);
        float yValidPosition = Mathf.Clamp(transform.position.y, yMin, yMax);
 
        transform.position = new Vector3(xValidPosition, yValidPosition, 0f);
    }
    
    void FixedUpdate()
    {
        Movimentacao(); 
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "cura")
        {
            vidasExtra++;
            Destroy(col.gameObject);
        }
    }

    public void Curar()
    {
        if (vida < fullvida && vidasExtra > 0)
        {
            vida += cura;
            vidasExtra--;
        }
    }

    void Movimentacao()
    {
        mov = new Vector2(fj.Horizontal * moveforce, fj.Vertical * moveforce);
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

    public void Ataque()
    {
        if (timeBtwAttack <= 0)
        {
            canAttack = true;
            if (canAttack)
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
