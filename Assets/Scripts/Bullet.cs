using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int speed = 20;
    private Rigidbody2D rb;
    public Player player;
    
    void Start()
    {
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // rb = GetComponent<Rigidbody2D>();
        // rb.velocity = transform.right * speed;
        
        // var sAngle = Mathf.Atan2(player.fj.Vertical, player.fj.Horizontal) * Mathf.Rad2Deg;
        
        // var shotdir = Quaternion.AngleAxis(sAngle, Vector3.forward) * Vector3.right;
        // rb.AddForce(shotdir * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
