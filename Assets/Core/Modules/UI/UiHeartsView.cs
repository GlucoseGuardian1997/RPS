using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHeartsView : MonoBehaviour
{
    [SerializeField] private List<Image> heartImages;

    /// <summary>
    /// heartsLeft = remaining lives
    /// </summary>
    public void SetHearts(int heartsLeft)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].enabled = i < heartsLeft;
        }
    }
}