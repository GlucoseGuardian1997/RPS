using System;
using UnityEngine;
using UnityEngine.UI;

public class UiThrowToolIcon:MonoBehaviour
{
    [SerializeField]
    private Items throwTypeName;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Button btn;
    
    private Action<Items> onPlayerIconClick;

    public void Init(Items throwTypeName,Sprite icon, Action<Items> onPlayerIconClick)
    {
        this.throwTypeName = throwTypeName;
        this.icon.sprite = icon;
        this.onPlayerIconClick = onPlayerIconClick;
        btn.onClick.AddListener(OnButtonClick);
    }
    
    private void OnButtonClick()
    {
        onPlayerIconClick?.Invoke(throwTypeName);
    }
    
    public void OnExitButtonClick()
    {
        btn.onClick.RemoveAllListeners();
    }
    
    
}