/*
 * ProfileUI.cs
 * By: William McCarty
 * Controls Profile UI Page functionality
 * */
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : UIPage
{
    #region UI Elements
    [SerializeField]
    private Image _Portrait;
    [SerializeField]
    private Text _Name;
    #endregion

    /// <summary>
    /// Initializes the UI Page
    /// </summary>
    /// <param name="name">Name of the User</param>
    /// <param name="icon">Users icon key to lookup sprite Icon</param>
    public void Initialize(string name, string icon)
    {
        //Set Name label to user name + Portrait sprite to their icon
        _Name.text = name;
        _Portrait.sprite = UIManager.Instance.GetIcon(icon);
    }
}
