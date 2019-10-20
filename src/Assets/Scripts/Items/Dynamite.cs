﻿using Assets.Scripts;

using UnityEngine;

/// <summary>
/// Gestiona el item de dinamita.
/// Explota todas las bolas en juego hasta alcanzar el nivel mas pequeño.
/// </summary>
public class Dynamite : MonoBehaviour
{
    /// <summary>
    /// The in ground.
    /// Booleano para comprobar si esta en el suelo
    /// </summary>
    private bool inGround;

    private void Update()
    {
        if (!inGround)
        {
            transform.position += Vector3.down * Time.deltaTime * General.Velocidades["lento"];
        }
    }

    /// <summary>
    /// Called when [trigger enter2 d].
    /// Comprueba si el item dinamita colisiono con el suelo o con el jugador
    /// </summary>
    /// <param name="collision">The collision.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            inGround = true;
            Destroy(gameObject, General.Tiempos["item"]);
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            BallManager.bm.Dynamite(5);
        }
    }
}