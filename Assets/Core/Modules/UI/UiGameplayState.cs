using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGameplayState : UiState
{
    [SerializeField] private Button quitButton;

    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private GameObject resultObj;
    [SerializeField] private Slider timer;
    [SerializeField] private GameObject outOfTimeObj;

    [SerializeField] private Image aiThrowImage;
    [SerializeField] private Image playerThrowImage;

    [SerializeField] private List<UiThrowToolIcon> throwIcons;
    [SerializeField] private UiHeartsView heartsView;
    
    private float cycleInterval = 0.05f;

    private Coroutine playerCycleRoutine;
    private Coroutine aiCycleRoutine;

    private bool inputLocked;
    private int currentRoundIndex;

    private float flashThreshold;
    private float flashSpeed = 6f;
    private bool flashing;

    public override void OnEnter(UiManager manager)
    {
        base.OnEnter(manager);
        GameManager.Instance.OnAiThrow += SetAiThrow;
        GameManager.Instance.OnScoreChanged += OnPlayerWonRound;
        GameManager.Instance.OnDraw += ShowDraw;
        GameManager.Instance.OnGameOver += GameOverScreen;
        GameManager.Instance.OnHeartsChanged += OnHeartsChanged;
        GameManager.Instance.OnRoundTimerReset += OnRoundTimerReset;
        
        GameManager.Instance.StartNewRun();
        
        inputLocked = false;
        currentRoundIndex = 1;
        roundText.text = $"Round {currentRoundIndex++}";
        flashing = false;

        resultObj?.SetActive(false);
        outOfTimeObj?.SetActive(false);

        InitThrowIcons();

        quitButton?.onClick.AddListener(OnQuitClicked);

        UpdateHeartsUI(GameManager.Instance.GetHearts());
        
        StartCyclingImages();
    }

    public override void OnTick()
    {
        if (inputLocked || timer == null)
            return;

        float remaining = GameManager.Instance.GetRemainingTime();
        timer.value = remaining;

        if (remaining > 0f && remaining <= flashThreshold)
        {
            flashing = true;
            bool visible = Mathf.PingPong(Time.time * flashSpeed, 1f) > 0.5f;
            outOfTimeObj?.SetActive(visible);
        }
        else if (flashing)
        {
            flashing = false;
            outOfTimeObj?.SetActive(false);
        }
    }
    
    private void OnRoundTimerReset(float duration, bool isDraw)
    {
        if (timer == null)
            return;

        timer.maxValue = duration;
        timer.value = duration;

        flashThreshold = duration * 0.25f;
        flashing = false;

        outOfTimeObj?.SetActive(false);
        resultObj?.SetActive(false);

        inputLocked = false;

        StartCyclingImages();
        
        if(isDraw)
            return;
        
        if (roundText != null)
            roundText.text = $"Round {currentRoundIndex++}";
    }

    private void OnPlayerWonRound(int newScore)
    {
        ShowResult(true);
    }

    private void OnHeartsChanged(int heartsLeft)
    {
        UpdateHeartsUI(heartsLeft);
        ShowResult(false);
    }
    
    private void ShowResult(bool playerWon)
    {
        inputLocked = true;
        StopCyclingImages(); 

        resultObj?.SetActive(true);
        resultText.text = playerWon ? "YOU WIN" : "AI WINS";
    }

    private void ShowDraw()
    {
        inputLocked = true;
        StopCyclingImages();
        
        if (resultObj != null)
            resultObj.SetActive(true);

        if (resultText != null)
            resultText.text = "DRAW";
    }


    private void InitThrowIcons()
    {
        var items = uiManager.GetThrowItems();
        int count = Mathf.Min(items.Count, throwIcons.Count);

        for (int i = 0; i < count; i++)
        {
            throwIcons[i].Init(
                items[i].ThrowTypeName,
                items[i].Icon,
                OnPlayerIconClick
            );
        }
    }
    
    private void OnPlayerIconClick(Items type)
    {
        if (inputLocked)
            return;

        if (playerCycleRoutine != null)
        {
            StopCoroutine(playerCycleRoutine);
            playerCycleRoutine = null;
        }

        var item = uiManager.GetThrowItems()
            .Find(x => x.ThrowTypeName == type);

        if (item != null && playerThrowImage != null)
            playerThrowImage.sprite = item.Icon;

        uiManager.PlayerThrow(type);
    }

    private void SetAiThrow(Items type)
    {
        if (aiCycleRoutine != null)
        {
            StopCoroutine(aiCycleRoutine);
            aiCycleRoutine = null;
        }

        var item = uiManager.GetThrowItems()
            .Find(x => x.ThrowTypeName == type);

        if (item != null && aiThrowImage != null)
            aiThrowImage.sprite = item.Icon;
    }


    private void UpdateHeartsUI(int heartsLeft)
    {
        heartsView?.SetHearts(heartsLeft);
    }

    private void OnQuitClicked()
    {
        uiManager.SwitchToStart();
    }
    
    private void StartCyclingImages()
    {
        StopCyclingImages();

        playerCycleRoutine = StartCoroutine(CyclePlayerImage());
        aiCycleRoutine = StartCoroutine(CycleAiImage());
    }
    
    private IEnumerator CyclePlayerImage()
    {
        var items = uiManager.GetThrowItems();

        while (true)
        {
            var random = items[Random.Range(0, items.Count)];
            if (playerThrowImage != null)
                playerThrowImage.sprite = random.Icon;

            yield return new WaitForSeconds(cycleInterval);
        }
    }

    private IEnumerator CycleAiImage()
    {
        var items = uiManager.GetThrowItems();

        while (true)
        {
            var random = items[Random.Range(0, items.Count)];
            if (aiThrowImage != null)
                aiThrowImage.sprite = random.Icon;

            yield return new WaitForSeconds(cycleInterval);
        }
    }

    private void StopCyclingImages()
    {
        if (playerCycleRoutine != null)
            StopCoroutine(playerCycleRoutine);

        if (aiCycleRoutine != null)
            StopCoroutine(aiCycleRoutine);

        playerCycleRoutine = null;
        aiCycleRoutine = null;
    }

    public void GameOverScreen()
    {
        resultObj?.SetActive(true);
        resultText.text = "YOU LOSE";
    }

    public override void OnExit()
    {
        quitButton?.onClick.RemoveListener(OnQuitClicked);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAiThrow -= SetAiThrow;
            GameManager.Instance.OnScoreChanged -= OnPlayerWonRound;
            GameManager.Instance.OnHeartsChanged -= OnHeartsChanged;
            GameManager.Instance.OnRoundTimerReset -= OnRoundTimerReset;
            GameManager.Instance.OnDraw -= ShowDraw;
            GameManager.Instance.OnGameOver -= GameOverScreen;
        }

        foreach (var icon in throwIcons)
            icon.OnExitButtonClick();

        StopCyclingImages();
        
        base.OnExit();
    }
}