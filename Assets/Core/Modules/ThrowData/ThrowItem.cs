using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ThrowItem")]
public class ThrowItem : ScriptableObject
{
    public Items ThrowTypeName;

    public List<Items> WinItems;

    public Sprite Icon;
}

public enum Items
{
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock
}