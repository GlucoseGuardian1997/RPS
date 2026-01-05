using System;
using UnityEngine;

public class ThrowManager
{
    public ThrowData ThrowData;

    public ThrowManager()
    {
        ThrowData = GetThrowData();
    }

    private ThrowData GetThrowData()
    {
        var data = Resources.Load<ThrowData>("ThrowData");

        if (data == null)
            Debug.LogError("ThrowData not found in Resources folder!");

        return data;
    }

    public void ComputeThrow(Items playerThrow, Items aiThrow, Action<RoundResult> callback)
    {
        if (ThrowData == null || ThrowData.ThrowItems == null)
        {
            Debug.LogError("ThrowData or ThrowItems not initialized");
            callback?.Invoke(RoundResult.Draw);
            return;
        }

        ComputeThrow(
            ThrowData.ThrowItems.Find(x => x.ThrowTypeName == playerThrow),
            ThrowData.ThrowItems.Find(x => x.ThrowTypeName == aiThrow),
            callback
        );
    }

    private void ComputeThrow(ThrowItem playerThrow, ThrowItem aiThrow, Action<RoundResult> callback)
    {
        if (playerThrow == null)
        {
            callback?.Invoke(RoundResult.AiWin);
            return;
        }

        if (aiThrow == null)
        {
            callback?.Invoke(RoundResult.PlayerWin);
            return;
        }

        if (playerThrow.ThrowTypeName == aiThrow.ThrowTypeName)
        {
            callback?.Invoke(RoundResult.Draw);
            return;
        }

        if (playerThrow.WinItems != null &&
            playerThrow.WinItems.Contains(aiThrow.ThrowTypeName))
        {
            callback?.Invoke(RoundResult.PlayerWin);
        }
        else if (aiThrow.WinItems != null &&
                 aiThrow.WinItems.Contains(playerThrow.ThrowTypeName))
        {
            callback?.Invoke(RoundResult.AiWin);
        }
        else
        {
            Debug.LogWarning("No clear winner determined - check ThrowItem configuration");
            callback?.Invoke(RoundResult.Draw);
        }
    }
}

public enum RoundResult
{
    Draw,
    PlayerWin,
    AiWin
}