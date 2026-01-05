using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ThrowData")]
public class ThrowData : ScriptableObject
{
    public List<ThrowItem> ThrowItems;
}