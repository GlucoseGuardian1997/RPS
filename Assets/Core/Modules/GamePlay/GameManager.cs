using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private const string LifetimeRoundsKey = "LifetimeRounds";
    private const string HighScoreKey = "HighScore";

    private GameConfig gameConfig;

    private ThrowManager throwManager;
    private AI ai;

    private int lifetimeRoundCount;
    private int currentStreak;
    private int highScore;

    private int hearts;
    private const int MaxHearts = 3;

    private float remainingTime;
    private bool isRoundActive;

    public Action<Items> OnAiThrow;
    public Action<int> OnScoreChanged;
    public Action OnDraw;
    public Action OnGameOver;
    public Action<int> OnHeartsChanged;
    public Action<float, bool> OnRoundTimerReset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Init();
    }

    private void Init()
    {
        gameConfig = Resources.Load<GameConfig>("GameConfig");

        throwManager = new ThrowManager();
        ai = new AI(throwManager.ThrowData.ThrowItems.Count);

        lifetimeRoundCount = PlayerPrefs.GetInt(LifetimeRoundsKey, 0);
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

    }
    
    public void StartNewRun()
    {
        currentStreak = 0;
        hearts = MaxHearts;
        StartRound();

        OnScoreChanged?.Invoke(currentStreak);
        OnHeartsChanged?.Invoke(hearts);
    }

    private void StartRound(bool isDraw = false)
    {
        remainingTime = gameConfig.timerDuration;
        isRoundActive = true;
        OnRoundTimerReset?.Invoke(remainingTime, isDraw);
    }

    private void Update()
    {
        if (!isRoundActive)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            EndRound(false);
        }
    }

    public void PlayerThrow(Items playerThrow)
    {
        if (!isRoundActive)
            return;

        isRoundActive = false;

        var aiThrow = ai.GetAiThrow(lifetimeRoundCount);
        OnAiThrow?.Invoke(aiThrow);

        throwManager.ComputeThrow(playerThrow, aiThrow, ComputeResult);
    }

    private void ComputeResult(RoundResult result)
    {
        IncrementLifetimeRounds();

        switch (result)
        {
            case RoundResult.PlayerWin:
                currentStreak++;
                OnScoreChanged?.Invoke(currentStreak);
                StartCoroutine(StartNextRoundAfterDelay());
                Debug.LogError("PlayerWin");
                break;

            case RoundResult.AiWin:
                LoseHeart();
                Debug.LogError("AiWin");
                break;

            case RoundResult.Draw:
                OnDraw?.Invoke();
                StartCoroutine(StartNextRoundAfterDelay(true));
                break;
        }
    }

    private void EndRound(bool playerWon)
    {
        isRoundActive = false;

        if (!playerWon)
            LoseHeart();
    }

    private void LoseHeart()
    {
        hearts--;
        OnHeartsChanged?.Invoke(hearts);

        if (hearts <= 0)
        {
            EndGame();
            return;
        }

        StartCoroutine(StartNextRoundAfterDelay());
    }

    private IEnumerator StartNextRoundAfterDelay(bool isDraw = false)
    {
        yield return new WaitForSeconds(2f);
        StartRound(isDraw);
    }

    private void IncrementLifetimeRounds()
    {
        lifetimeRoundCount++;
        PlayerPrefs.SetInt(LifetimeRoundsKey, lifetimeRoundCount);
        PlayerPrefs.Save();
    }

    private void EndGame()
    {
        if (currentStreak > highScore)
        {
            highScore = currentStreak;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }
        OnGameOver?.Invoke();
        StartCoroutine(GameOverAfterDelay());
    }
    
    private IEnumerator GameOverAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        UiManager.Instance.SwitchToStart();
    }

    // 🔒 EXISTING ACCESSORS (DO NOT REMOVE)
    public float GetRemainingTime() => remainingTime;
    public int GetHearts() => hearts;
    public int GetHighScore() => highScore;

    public List<ThrowItem> GetThrowItems()
    {
        return throwManager.ThrowData.ThrowItems;
    }
}