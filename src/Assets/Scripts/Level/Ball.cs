﻿using Assets.Scripts;

using UnityEngine;

/// <summary>
/// Gestiona la bola que esta en juego
/// </summary>
public class Ball : MonoBehaviour
{
    #region Public Fields

    /// <summary>
    /// The item sonido
    /// Sonido que se reproduce al explotar las bolas
    /// </summary>
    public AudioClip itemSonido;

    /// <summary>
    /// The next ball.
    /// Proxima bola que se generara al explotar
    /// </summary>
    public GameObject nextBall;

    /// <summary>
    /// The power up.
    /// Item que se generara al explotar la bola
    /// </summary>
    public GameObject powerUp;

    /// <summary>
    /// The right.
    /// Establece si se movera a izquierda o derecha
    /// </summary>
    public bool right;

    #endregion Public Fields

    #region Private Fields

    /// <summary>
    /// The current velocity.
    /// Velocidad actual de la bola
    /// </summary>
    private Vector2 currentVelocity;

    /// <summary>
    /// The fuente audio
    /// </summary>
    private AudioSource fuenteAudio;

    /// <summary>
    /// The rb.
    /// Dibuja la vola
    /// </summary>
    private Rigidbody2D rb;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Freezes the ball.
    /// Congela la posicion de las bolas en juego.
    /// </summary>
    /// <param name="balls">The balls.</param>
    public void FreezeBall(params GameObject[] balls)
    {
        foreach (GameObject item in balls)
        {
            if (item != null)
            {
                currentVelocity = item.GetComponent<Rigidbody2D>().velocity;
                item.GetComponent<Rigidbody2D>().isKinematic = true;
                item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// Normals the speed ball.
    /// Recupera el comportanimento por defecto de las bolas tras ser ralentizadas
    /// </summary>
    public void NormalSpeedBall()
    {
        if (rb.velocity.x < Velocidad.NULO)
        {
            rb.velocity = new Vector2((-1f) * Velocidad.NORMAL, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Velocidad.NORMAL, rb.velocity.y);
        }
        rb.gravityScale = Gravedad.NORMAL;
    }

    /// <summary>
    /// Slows the ball.
    /// Establece el comportamiento de las bolas cuando se reduce su velocidad. Se cogio el item de ralentizar el tiempo
    /// </summary>
    public void SlowBall()
    {
        rb.velocity /= Velocidad.MUYLENTO;
        rb.gravityScale = Gravedad.BAJA;
    }

    /// <summary>
    /// Explotan las bolas y se dividen.
    /// Genera un item de premios y puntuacion
    /// </summary>
    public void Split()
    {
        BallManager.bm.SonidoExplosion();

        if (nextBall != null)
        {
            InstantiantePrice();
            GameObject ball1 = Instantiate(nextBall, rb.position + Vector2.right / 4, Quaternion.identity);
            GameObject ball2 = Instantiate(nextBall, rb.position + Vector2.left / 4, Quaternion.identity);

            if (!FreezeManager.fm.freeze)
            {
                ball1.GetComponent<Rigidbody2D>().isKinematic = false;
                ball1.GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 5), ForceMode2D.Impulse);
                ball1.GetComponent<Ball>().right = false;

                ball2.GetComponent<Rigidbody2D>().isKinematic = false;
                ball2.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 5), ForceMode2D.Impulse);
                ball2.GetComponent<Ball>().right = false;
            }
            else
            {
                ball1.GetComponent<Ball>().currentVelocity = new Vector2(2, 5);
                ball2.GetComponent<Ball>().currentVelocity = new Vector2(-2, 5);
            }

            if (!BallManager.bm.spliting)
            {
                BallManager.bm.DestroyBall(gameObject, ball1, ball2);
            }
        }
        else
        {
            BallManager.bm.LastBall(gameObject);
        }
        int score = Puntuacion.BOLA;
        PopUpManager.pm.InstanciatePopUpText(gameObject.transform.position, score);
        ScoreManager.sm.UpdateScore(score);
        GameManager.gm.UpdateBallDestroyed();
    }

    /// <summary>
    /// Starts the force.
    /// Agrega el movimiento a las bolas al inicio de partida o tras una explosion
    /// </summary>
    /// <param name="ball">The ball.</param>
    public void StartForce(GameObject ball)
    {
        ball.GetComponent<Rigidbody2D>().isKinematic = false;
        if (right)
        {
            ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
        }
        else
        {
            ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 0), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Unfreezes the ball.
    /// Reestablece el comportamiento de las bolas tras ser congeladas
    /// </summary>
    /// <param name="balls">The balls.</param>
    public void UnfreezeBall(params GameObject[] balls)
    {
        foreach (GameObject item in balls)
        {
            if (item != null)
            {
                item.GetComponent<Rigidbody2D>().isKinematic = false;
                item.GetComponent<Rigidbody2D>().AddForce(currentVelocity, ForceMode2D.Impulse);
            }
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fuenteAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Instantiantes the price.
    /// Genera el item de premio en la explosion de la bola
    /// </summary>
    private void InstantiantePrice()
    {
        int aleatory = BallManager.bm.AleatoryNumber();
        if (aleatory == 1)
        {
            Instantiate(powerUp, transform.position, Quaternion.identity);
            General.Audio(fuenteAudio, itemSonido);
        }
    }

    #endregion Private Methods
}