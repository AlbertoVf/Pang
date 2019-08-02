﻿using Assets.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = General.velocidades["normal"];
    float movement = General.velocidades["nulo"];
    bool rightWall;
    bool leftWall;
    private Rigidbody2D rb;
    private Animator animator;
    SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetInteger("velX", Mathf.RoundToInt(movement));
        if (movement < General.velocidades["nulo"])
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    void FixedUpdate()
    {
        if (leftWall)
        {
            if (Input.GetKey(General.teclas["izquierda"]))
            {
                speed = General.velocidades["nulo"];
            }
            else if (Input.GetKey(General.teclas["derecha"]) || Input.GetKeyUp(General.teclas["izquierda"]))
            {
                speed = General.velocidades["normal"];
            }
        }
        if (rightWall)
        {
            if (Input.GetKey(General.teclas["izquierda"]) || Input.GetKeyUp(General.teclas["derecha"]))
            {
                speed = General.velocidades["normal"];
            }
            else if (Input.GetKey(General.teclas["derecha"]))
            {
                speed = General.velocidades["nulo"];
            }
        }
        rb.MovePosition(rb.position + Vector2.right * movement * Time.fixedDeltaTime);
        //newX = Mathf.Clamp(transform.position.x, General.limites["izquierda"], General.limites["derecha"]);
        //transform.position = new Vector2(newX, transform.position.y);
    }


    /// <summary>
    /// Comprueba si el jugador esta en contacto con una colision
    /// </summary>
    /// <param name="collision">Objeto con el que esta colisionando</param>
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Left")
        {
            leftWall = true;
        }
        else if (collision.gameObject.tag == "Right")
        {
            rightWall = true;
        }
    }

    /// <summary>
    /// Comprueba si el jugador ya ha salido de un objeto con el que colisionaba
    /// </summary>
    /// <param name="collision">Objeto con el que estaba colisionando</param>
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Left")
        {
            leftWall = false;
        }
        else if (collision.gameObject.tag == "Right")
        {
            rightWall = false;
        }
    }
}
