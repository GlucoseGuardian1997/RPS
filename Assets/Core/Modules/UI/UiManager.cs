using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] private UiState startState;
    [SerializeField] private UiState gameplayState;

    private UiState currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SwitchState(startState);
    }

    private void Update()
    {
        currentState?.OnTick();
    }

    public void SwitchToStart()    => SwitchState(startState);
    public void SwitchToGameplay() => SwitchState(gameplayState);

    public List<ThrowItem> GetThrowItems()
    {
        return GameManager.Instance.GetThrowItems();
    }

    public void PlayerThrow(Items type)
    {
        GameManager.Instance.PlayerThrow(type);
    }

    private void SwitchState(UiState newState)
    {
        if (currentState == newState)
            return;

        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
}