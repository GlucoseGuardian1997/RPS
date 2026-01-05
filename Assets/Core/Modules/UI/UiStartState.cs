using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiStartState : UiState
{
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI highScoreText;

    public override void OnEnter(UiManager manager)
    {
        base.OnEnter(manager);

        highScoreText.text = $"High Score: {GameManager.Instance.GetHighScore()}";
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        uiManager.SwitchToGameplay();
    }

    public override void OnExit()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
        base.OnExit();
    }
}