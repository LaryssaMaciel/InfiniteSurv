using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_ : MonoBehaviour
{
    public Transform player, circle, outerCircle;
    public float speed = 5;
    public bool touchStart = false;
    private Vector2 pointA, pointB;

    public Rigidbody2D rb;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

            circle.transform.position = pointA * -1;
            outerCircle.transform.position = pointA * -1;
            circle.GetComponent<SpriteRenderer>().enabled = true;
            outerCircle.GetComponent<SpriteRenderer>().enabled = true;

            if (Input.GetMouseButton(0))
            {
                touchStart = true;
                pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));    
            }
            else 
            {
                touchStart = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointA - pointB;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1f);
            moveCharacter(direction * -1);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y) * -1;
        }
        else
        {
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void moveCharacter(Vector2 dir)
    {
        player.Translate(dir * speed * Time.deltaTime);
    }
}
