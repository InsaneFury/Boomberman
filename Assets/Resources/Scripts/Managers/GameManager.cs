﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [Header("Settings")]
    [Tooltip("Player Spawn Point")]
    public Transform spawnPoint;
    Player player;
    Portal portal;
    ScoreManager sManager;
    float gameTimer = 0f;
    public bool gameOver = false;
    public bool victory = false;
    public string time = " ";

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        player = Player.Get();
        player.OnPlayerDie += ResetPlayer;
        portal = Portal.Get();
        sManager = ScoreManager.Get();
    }

    void Update()
    {
        if (player.lives <= 0 && player.canMove)
        {
            GameOver();
        }
        else if (player.lives > 0 && player.canMove && portal.playerIsOnPortal)
        {
            Victory();
        }
        if (!gameOver)
        {
            time = GetRealTime();
        }
    }

    void ResetPlayer(Player player)
    {
        if (spawnPoint)
        {
            player.transform.position = spawnPoint.position;
        }
        player.Revive();
    }

    void GameOver()
    {
        victory = false;
        player.StopPlayer();
        gameOver = true;
        ScenesManager.Get().LoadScene("GameOver");
    }

    void Victory()
    {
        victory = true;
        player.StopPlayer();
        sManager.AddHighScore();
        gameOver = true;
        ScenesManager.Get().LoadScene("GameOver");
    }

    public string GetRealTime()
    {
        gameTimer += Time.deltaTime;

        int seconds = (int)(gameTimer % 60);
        int minutes = (int)(gameTimer / 60) % 60;

        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

        return timerString;
    } 
}