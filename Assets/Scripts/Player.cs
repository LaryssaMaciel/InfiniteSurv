using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("BACKGROUND")]
    public SpriteRenderer mapa; // background
    private float xMin, xMax, yMin, yMax; // bordas pra limitar câmera de sair do mapa

    [Header("JOYSTICKS")]
    public FixedJoystick fj; // Movement
    public FixedJoystick fjatk; // Ataque

    [Header("MOVIMENTAÇÃO")]
    public Rigidbody2D rb;
    private float movSpeed = 5, moveforce = 5f;
    public Vector2 mov, rbmov;
    private bool viraDir = true;

    [Header("VIDA")]
    public Image hpBar;
    public float vida, fullvida = 100, cura = 25;

    [Header("CURA")]
    public int vidasExtra = 0;
    private GameObject vidasBtn;
    public Text curasNum;

    [Header("ATAQUE/DANO")]
    public string tipoAtaque = "axe";
    public bool canDano = true; // se pode tomar ataque
    public bool damaged = false;
    public bool enemyInRange = false, canAttack = true, atacando = false;
    public float ataqueValor = 10, timeBtwAttack, startTimeBtwAttack = .3f, attackRange;
    public Transform attackPos;
    public LayerMask enemiesLayer;

    [Header("TIRO")]
    public GameObject pTiro;
    public GameObject posTiro;
    private bool atk2down = false;
    
    [Header("Animacao")]
    public string animState = "idle";
    private bool swing = false, dead = false;
    public Animator animator;

    // others
    private ChangeManager cm;
    private SceneController sm;
    private GameObject imgB;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        vidasBtn = GameObject.Find("Curas");
        hpBar = GameObject.Find("Life").GetComponent<Image>();
        sm = GameObject.Find("SceneManager").GetComponent<SceneController>();
        curasNum = GameObject.Find("Coletados").GetComponent<Text>();
        cm = GameObject.Find("Armas").GetComponent<ChangeManager>();
        
        mapa = GameObject.FindGameObjectWithTag("bg").GetComponent<SpriteRenderer>();
        imgB = GameObject.FindGameObjectWithTag("imgB");
        fj = GameObject.FindGameObjectWithTag("joystick").GetComponent<FixedJoystick>();
        fjatk = GameObject.FindGameObjectWithTag("fjatk").GetComponent<FixedJoystick>();
        audioSource = GetComponent<AudioSource>();
        audioSource1 = GameObject.Find("AtaqueRange").GetComponent<AudioSource>();
        audioSource2 = GameObject.Find("ShootPoint").GetComponent<AudioSource>();

        imgB.SetActive(false);
        vida = fullvida;
        LimitCamera();

        //if (Application.isMobilePlatform) { QualitySettings.vSyncCount = 0; }
    }
 
    void Update()
    {
        rbmov = mov;

        if (timeBtwAttack <= 0) { timeBtwAttack = 0; }
        else { timeBtwAttack -= Time.deltaTime; }

        if (dead) { canDano = false; }
        else { 
            CuraManager();
            Ataque2(); 
            FlipManager();
        }

        VidaManager();
        Animacoes();
    } 
    
    void FixedUpdate()
    {
        if (!dead) { Movimentacao(); }
    }

    public AudioSource audioSource, audioSource1, audioSource2; 
    public void AudioManager(AudioSource audios, int audio)
    {
        audios.clip = sm.som[audio];
        audios.Play();
    }

    void Animacoes()
    {
        if (!damaged && !swing && !dead)
        {
            animator.SetFloat("speed", mov.sqrMagnitude);
            animator.SetFloat("vertical", rbmov.y);
        }

        if (swing) 
        {
            animator.SetTrigger("swing");
            if (tipoAtaque != "tiro") 
            {
                swing = false;
            }
        }
        if (damaged) { animator.SetTrigger("damaged"); AudioManager(audioSource, 0); damaged = false; }
        if (dead) { animator.SetTrigger("dead"); }
    } 

    void LimitCamera()
    {
        var spriteSize = imgB.GetComponent<SpriteRenderer>().bounds.size.y * .5f; // Working with a simple box here, adapt to you necessity
        
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

        if (col.gameObject.tag == "Enemy")
        {
            obj = col.gameObject;
        }
    }

    private GameObject obj;

    void VidaManager()
    {
        if (vida <= 0) { vida = 0; dead = true; StartCoroutine(Death()); }
        else if (vida > fullvida) { vida = fullvida; }
        hpBar.fillAmount = vida / fullvida;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(2f);
        sm.ChangeScene("GameOver"); 
    }

    public void Curar()
    {
        if (vida < fullvida && vidasExtra > 0)
        {
            vida += cura;
            vidasExtra--;
        }
    }
    
    void CuraManager()
    {
        curasNum.text = vidasExtra.ToString();
        if (vidasExtra <= 0) { vidasBtn.SetActive(false); }
        else { vidasBtn.SetActive(true); }
    }
    //private bool ismoving = false;
    void Movimentacao()
    {
        if (!canDano) { movSpeed = 4f; }
        else { movSpeed = 5;  }

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

    void FlipManager()
    {
        if (mov.x > 0 && !viraDir || mov.x < 0 && viraDir) { Flip(); } 

        float xValidPosition = Mathf.Clamp(transform.position.x, xMin, xMax);
        float yValidPosition = Mathf.Clamp(transform.position.y, yMin, yMax);
 
        transform.position = new Vector3(xValidPosition, yValidPosition, 0f);
    }

    public void Ataque() // curta distancia
    {
        if (timeBtwAttack <= 0 && !dead)
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
            swing = true;
        }
        else 
        { 
            swing = false;
            canAttack = false; 
        }
    }

    public void Atk2Up() { atk2down = false; swing = false; }

    public void Ataque1() // ataque curta distancia
    {
        AudioManager(audioSource1, 3);
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
        AudioManager(audioSource1, 4);
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
                startTimeBtwAttack = .25f;
                timeBtwAttack = startTimeBtwAttack;
                Tiro();
            }
        }
    }

    IEnumerator WaitBullet(GameObject a) { yield return new WaitForSeconds(1f); Destroy(a); }

    IEnumerator AttackCooldown(Collider2D obj) // no inimigo
    {
        obj.GetComponent<SpriteRenderer>().color = Color.red;
        obj.GetComponent<Inimigo>().damaged = true;
        this.obj.GetComponent<Inimigo>().AudioManager(0);
        
        yield return new WaitForSeconds(.5f);

        obj.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    //public IEnumerator TakeDamageCoolDown(float cooldown)
    //{
    //    obj.GetComponent<Inimigo>().canAttack = false;
    //    gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
    //    canDano = false;
    //    damaged = true;
    //    obj.GetComponent<Inimigo>().cam.GetComponent<CameraShake>().shake = true;

    //    yield return new WaitForSeconds(cooldown);

    //    canDano = true;
    //    gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

    //    obj.GetComponent<Inimigo>().canAttack = true;
    //    obj.GetComponent<Inimigo>().atacou = false;
    //}
}
