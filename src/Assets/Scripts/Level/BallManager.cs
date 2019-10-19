﻿using Assets.Scripts;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Manager para gestionar el comportamiento de las bolas
/// </summary>
public class BallManager : MonoBehaviour
{
    /// <summary>
    /// The bm.
    /// Variable estatica para acceder a la clase desde otras.
    /// </summary>
    public static BallManager bm;

    /// <summary>
    /// The balls
    /// Lista de bolas que estan en juego
    /// </summary>
    public List<GameObject> balls = new List<GameObject>();

    /// <summary>
    /// The spliting
    /// Comprueba si esta explotando
    /// </summary>
    public bool spliting;

    private void Awake()
    {
        if (bm == null)
        {
            bm = this;
        }
        else if (bm != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        balls.AddRange(GameObject.FindGameObjectsWithTag("Ball"));
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SlowTime();
        }
    }

    /// <summary>
    /// Aleatories the number.
    /// Genera una numero aleatorio que se utilizara para obtener items
    /// </summary>
    /// <returns>Aleatorio</returns>
    public int AleatoryNumber()
    {
        return Random.Range(0, 3);
    }

    /// <summary>
    /// Destroys the ball.
    /// Destruye una bola en juego y crea dos bolas de menor tamaño siguiendo la gerarquia
    /// </summary>
    /// <param name="ball">The ball.</param>
    /// <param name="ball1">The ball1.</param>
    /// <param name="ball2">The ball2.</param>
    public void DestroyBall(GameObject ball, GameObject ball1, GameObject ball2)
    {
        balls.Remove(ball);
        Destroy(ball);
        balls.Add(ball1);
        balls.Add(ball2);
    }

    /// <summary>
    /// Dynamites the specified maximum number balls.
    /// Inicia las explosiones de las bolas en juego
    /// </summary>
    /// <param name="maxNumberBalls">The maximum number balls.</param>
    public void Dynamite(int maxNumberBalls)
    {
        StartCoroutine(IEDynamite(maxNumberBalls));
    }

    /// <summary>
    /// Finds the balls. Comprueba todos los componentes que estan en escena buscando las bolas que tengan el mismo tipo que se pasa por parametro y destrullendolas hasta alcancar el nivel mas pequeño
    /// </summary>
    /// <param name="typeOfBall">The type of ball. Tipo de bola numerado desde el 5 hacia bajo, desde el mayor al menor</param>
    /// <returns>Lista con las bolas que se van a destruir</returns>
    private List<GameObject> FindBalls(int typeOfBall)
    {
        List<GameObject> ballsToDestroy = new List<GameObject>();
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].GetComponent<Ball>().name.Contains(typeOfBall.ToString()) && balls[i] != null)
            {
                ballsToDestroy.Add(balls[i]);
            }
        }
        return ballsToDestroy;
    }

    /// <summary>
    /// Lasts the ball.
    /// Destruye la bola mas pequeña sin generar ninguna.
    /// </summary>
    /// <param name="ball">The ball.</param>
    public void LastBall(GameObject ball)
    {
        Destroy(ball);
        balls.Remove(ball);
    }

    /// <summary>
    /// Reloads the list. Recarga la lista de bolas en juego
    /// </summary>
    private void ReloadList()
    {
        balls.Clear();
        balls.AddRange(GameObject.FindGameObjectsWithTag("Ball"));
    }

    /// <summary>
    /// Slows the time.
    /// Inicia la ralentizacion del movimiento de las bolas en juego
    /// </summary>
    public void SlowTime()
    {
        StartCoroutine(IETimeSlow());
    }

    /// <summary>
    /// Starts the game.
    /// Inicia el juego impulsando las bolas a derecha o izquierda de manera aleatoria
    /// </summary>
    public void StartGame()
    {
        foreach (GameObject item in balls)
        {
            if (balls.IndexOf(item) % 2 == 0)
            {
                item.GetComponent<Ball>().right = true;
            }
            else
            {
                item.GetComponent<Ball>().right = false;
            }
            item.GetComponent<Ball>().StartForce(item);
        }
    }

    /// <summary>
    /// Ies the dynamite bh.
    /// Explosion de bolas hasta alcanzar un numero determinado
    /// </summary>
    /// <param name="maxNumberBalls">The maximum number balls.</param>
    /// <returns></returns>
    public IEnumerator IEDynamite(int maxNumberBalls)
    {
        ReloadList();
        spliting = true;
        int numberToFind = 1;
        while (numberToFind < maxNumberBalls)
        {
            foreach (GameObject item in FindBalls(numberToFind))
            {
                item.GetComponent<Ball>().Split();
                Destroy(item);
            }
            yield return new WaitForSeconds(General.Tiempos["parpadeo"]);
            ReloadList();
            numberToFind++;
        }
        spliting = false;
    }

    /// <summary>
    /// Ies the time slow.
    /// Corrutina que ralentiza las bolas
    /// </summary>
    /// <returns></returns>
    public IEnumerator IETimeSlow()
    {
        float time = 0;
        foreach (GameObject item in balls)
        {
            if (item != null)
            {
                item.GetComponent<Ball>().SlowBall();
            }
        }
        while (time < General.Tiempos["cuentaAtras"])
        {
            time += Time.deltaTime;
            yield return null;
        }
        foreach (GameObject item in balls)
        {
            if (item != null)
            {
                item.GetComponent<Ball>().NormalSpeedBall();
            }
        }
    }
}