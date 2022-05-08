using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Background")]
    private float xMin, xMax, yMin, yMax; // bordas pra limitar câmera de sair do mapa
    public SpriteRenderer mapa; // background

    [Header("Joysticks")]
    public FixedJoystick fj, fjatk; // joysticks -> Mov | Ataque

    [Header("Shoot")]
    public GameObject pTiro, posTiro;
    public string tipoAtaque = "axe";
    private bool atk2down = false;

    [Header("Movimentação")]
    public Rigidbody2D rb;
    private float movSpeed = 5, moveforce = 5f;
    public Vector2 mov;
    private bool viraDir = true;

    [Header("Vida")]
    public Image hpBar;
    public float vida, fullvida = 100, cura = 10;

    [Header("Cura")]
    public int vidasExtra = 0;
    private GameObject vidasBtn;
    public Text curasNum;

    [Header("Ataque/Dano")]
    public bool canDano = true; // se pode tomar ataque
    public bool enemyInRange = false, canAttack = true, atacando = false;
    public float ataqueValor = 10, timeBtwAttack, startTimeBtwAttack = .3f, attackRange;
    public Transform attackPos;
    public LayerMask enemiesLayer;

    [Header("Others")]
    public ChangeManager cm;
    private SceneController sm;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vida = fullvida;
        hpBar = GameObject.Find("Life").GetComponent<Image>();
        sm = GameObject.Find("SceneManager").GetComponent<SceneController>();
        curasNum = GameObject.Find("Coletados").GetComponent<Text>();
        fj = GameObject.FindGameObjectWithTag("joystick").GetComponent<FixedJoystick>();
        fjatk = GameObject.FindGameObjectWithTag("fjatk").GetComponent<FixedJoystick>();
        cm = GameObject.Find("Armas").GetComponent<ChangeManager>();
        vidasBtn = GameObject.Find("Curas");
        LimitCamera();
    }
 
    void Update()
    {
        if (timeBtwAttack <= 0) { timeBtwAttack = 0; }
        else { timeBtwAttack -= Time.deltaTime; }

        Ataque2();

        if (vida <= 0) { vida = 0; sm.ChangeScene("GameOver"); }
        else if (vida > fullvida) { vida = fullvida; }
        hpBar.fillAmount = vida / fullvida;

        curasNum.text = vidasExtra.ToString();
        if (vidasExtra <= 0) { vidasBtn.SetActive(false); }
        else { vidasBtn.SetActive(true); }

        if (mov.x > 0 && !viraDir || mov.x < 0 && viraDir) { Flip(); } 

        float xValidPosition = Mathf.Clamp(transform.position.x, xMin, xMax);
        float yValidPosition = Mathf.Clamp(transform.position.y, yMin, yMax);
 
        transform.position = new Vector3(xValidPosition, yValidPosition, 0f);
    }
    
    void FixedUpdate()
    {
        Movimentacao(); 
    }

    void LimitCamera()
    {
        var spriteSize = GetComponent<SpriteRenderer>().bounds.size.y * .5f; // Working with a simple box here, adapt to you necessity
        
        var camHeight = mapa.bounds.size.y/2;
        var camWidth = mapa.bounds.size.x/2;

        yMin = -camHeight + spriteSize; // lower bound
        yMax = camHeight - spriteSize; // upper bound
        
        xMin = -camWidth + spriteSize; // left bound
        xMax = camWidth - spriteSize; // right bound 
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "cura")
        {
            vidasExtra++;
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "gun")
        {
            cm.lista.Add("tiro");
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
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        viraDir =! viraDir;
    }

    public void Ataque() // curta distancia
    {
        if (timeBtwAttack <= 0)
        {
            canAttack = true;
            if (tipoAtaque == "axe")
            {
                ataqueValor = 10;
                Ataque1();
                startTimeBtwAttack = .3f;
            }
            if (tipoAtaque == "tiro")
            {
                ataqueValor = 5;
                atk2down = true;
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else { canAttack = false; }
    }

    public void Atk2Up() { atk2down = false; }

    public void Ataque1() // ataque curta distancia
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemiesLayer);
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Inimigo>().vida -= ataqueValor;
                Vector3 direction = (enemies[i].transform.position - transform.position);
                enemies[i].GetComponent<Inimigo>().rb.AddForce(direction * 500);
                StartCoroutine(AttackCooldown(enemies[i]));
            }
        }
    }

    public void Tiro() // ataque longa distancia
    {
        GameObject a = Instantiate(pTiro, posTiro.transform.position, posTiro.transform.rotation);
        float angle = Mathf.Atan2(fjatk.Horizontal, fjatk.Vertical) * Mathf.Rad2Deg;
        posTiro.transform.eulerAngles = new Vector3(0, 0, -angle);
        a.GetComponent<Rigidbody2D>().AddForce(posTiro.transform.up * 20, ForceMode2D.Impulse);
        StartCoroutine(WaitBullet(a));
    }

    private void Ataque2()
    {
        if (atk2down && canAttack)
        {
            if (timeBtwAttack <= 0)
            {
                startTimeBtwAttack = .2f;
                timeBtwAttack = startTimeBtwAttack;
                Tiro();
            }
        }
    }

    IEnumerator WaitBullet(GameObject a) { yield return new WaitForSeconds(1f); Destroy(a); }

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
