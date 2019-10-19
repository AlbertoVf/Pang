﻿using Assets.Scripts;

using System.Collections;

using UnityEngine;

/// <summary>
/// Gestiona el jugador
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// The animator
    /// </summary>
    private Animator animator;

    /// <summary>
    /// The blink
    /// Comprueba si parpadea, ha perdido el escudo
    /// </summary>
    public bool blink;

    /// <summary>
    /// The left wall
    /// Comprueba si colisiona con el muro derecho
    /// </summary>
    private bool leftWall;

    /// <summary>
    /// The movement
    /// </summary>
    private float movement = General.Velocidades["nulo"];

    /// <summary>
    /// The rb
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// The right wall
    /// Comprueba si colisiona con el muro izquierdo
    /// </summary>
    private bool rightWall;

    /// <summary>
    /// The shield
    /// Comprueba si esta activado el escudo
    /// </summary>
    public GameObject shield;

    /// <summary>
    /// The speed
    /// </summary>
    private float speed = General.Velocidades["normal"];

    /// <summary>
    /// The sr
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// Awakes this instance.
    /// Asigna las variables
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Fixeds the update.
    /// Gestiona la posibilidad de movimiento
    /// </summary>
    private void FixedUpdate()
    {
        if (leftWall)
        {
            if (Input.GetKey(General.Teclas["izquierda"]))
            {
                speed = General.Velocidades["nulo"];
            }
            else if (Input.GetKey(General.Teclas["derecha"]) || Input.GetKeyUp(General.Teclas["izquierda"]))
            {
                speed = General.Velocidades["normal"];
            }
        }
        if (rightWall)
        {
            if (Input.GetKey(General.Teclas["izquierda"]) || Input.GetKeyUp(General.Teclas["derecha"]))
            {
                speed = General.Velocidades["normal"];
            }
            else if (Input.GetKey(General.Teclas["derecha"]))
            {
                speed = General.Velocidades["nulo"];
            }
        }
        rb.MovePosition(rb.position + Vector2.right * movement * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Called when [trigger enter2 d].
    /// Gestiona la perdida de escudo al chocar con una bola
    /// </summary>
    /// <param name="collision">The collision.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (shield.activeInHierarchy)
            {
                shield.SetActive(false);
                StartCoroutine(IEBlinking());
            }
            else
            {
                if (!blink)
                {
                    //fin de partida
                }
            }
        }
    }

    /// <summary>
    /// Comprueba si el jugador ya ha salido de un objeto con el que colisionaba
    /// </summary>
    /// <param name="collision">Objeto con el que estaba colisionando</param>
    private void OnTriggerExit2D(Collider2D collision)
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

    /// <summary>
    /// Comprueba si el jugador esta en contacto con una colision
    /// </summary>
    /// <param name="collision">Objeto con el que esta colisionando</param>
    private void OnTriggerStay2D(Collider2D collision)
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
    /// Updates this instance.
    /// Cambia la direccion del sprite del jugador
    /// </summary>
    private void Update()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetInteger("velX", Mathf.RoundToInt(movement));
        if (movement < General.Velocidades["nulo"])
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    /// <summary>
    /// Ies the blinking.
    /// Corrutina de parpadeo
    /// </summary>
    /// <returns></returns>
    private IEnumerator IEBlinking()
    {
        blink = true;
        for (int i = 0; i < General.Tiempos["cuentaAtras"]; i++)
        {
            if (blink)
            {
                sr.color = new Color(1, 1, 1, 0);
                yield return new WaitForSeconds(General.Tiempos["parpadeo"]);
                sr.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(General.Tiempos["parpadeo"]);
            }
            else
            {
                break;
            }
        }
        blink = false;
    }
}