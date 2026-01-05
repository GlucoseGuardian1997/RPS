using UnityEngine;

public class UiGameEndState : UiState
{
    public override void OnEnter(UiManager manager)
    {
        base.OnEnter(manager);
        Debug.Log("GameEnd UI Enter");
    }

    public override void OnExit()
    {
        Debug.Log("GameEnd UI Exit");
        base.OnExit();
    }
}