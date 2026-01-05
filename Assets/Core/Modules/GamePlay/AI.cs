using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private List<Items> riggedFlow;
    private int throwItemCount;

    public AI(int throwItemCount)
    {
        this.throwItemCount = throwItemCount;
        riggedFlow = LoadRiggedFlow();
    }

    private List<Items> LoadRiggedFlow()
    {
        var data = Resources.Load<RiggedData>("RiggedData");

        if (data == null || data.RiggedItems == null)
        {
            Debug.LogWarning("RiggedData missing or empty. AI will be fully random.");
            return new List<Items>();
        }

        return data.RiggedItems;
    }
    
    public Items GetAiThrow(int lifetimeRoundIndex)
    {
        if (lifetimeRoundIndex < riggedFlow.Count)
        {
            return riggedFlow[lifetimeRoundIndex];
        }
        return GetRandomThrow();
    }

    private Items GetRandomThrow()
    {
        if (throwItemCount <= 0)
        {
            Debug.LogError("Invalid throwItemCount for AI");
            return default;
        }

        return (Items)Random.Range(0, throwItemCount);
    }
}