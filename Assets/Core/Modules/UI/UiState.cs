using UnityEngine;

public abstract class UiState : MonoBehaviour
{
    protected UiManager uiManager;

    public virtual void OnEnter(UiManager manager)
    {
        uiManager = manager;
        gameObject.SetActive(true);
    }

    public virtual void OnExit()
    {
        gameObject.SetActive(false);
        uiManager = null;
    }

    public virtual void OnTick()
    {
    }
}