/*
 * ConversationListItem.cs
 * By: William McCarty
 * UI Item for displaying a specific Conversation
 * */
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to Display Icons in Inspector
/// </summary>
public class Icon
{
    //Name of the Sprite
    public string Key;
    //The Sprite Associated with it
    public Sprite IconVersion;
}


public class ConversationListItem : MonoBehaviour
{
    #region UI Elements
    [SerializeField]
    private Text _NameLabel;
    [SerializeField]
    private Text _DateLabel;
    [SerializeField]
    private Image _Portrait;
    #endregion

    //Data Fields of the Conversation
    private string _Id;
    private string _Name;
    private string _Date;
    private string _IconKey;

    /// <summary>
    /// Sets the Item UI to what was passed in
    /// </summary>
    /// <param name="id">Id of the user chatting with</param>
    /// <param name="name">Name of user chatting with</param>
    /// <param name="date">Last message timestamp</param>
    /// <param name="icon">User Icon they are using</param>
    public void SetValues(string id, string name, string date, string icon)
    {
        _Id = id;
        _NameLabel.text = name;
        _Name = name;
        _DateLabel.text = date;
        _Date = date;
        _Portrait.sprite = UIManager.Instance.GetIcon(icon);
        _IconKey = icon;
    }

    /// <summary>
    /// Navigates user to their profile
    /// </summary>
    public void OnPortraitTapped()
    {
        //Gets current UIPage
        UIPage prevPage = UIManager.Instance.ActivePage;
        //Moves to next scene
        UIManager.Instance.OnUISelect(UIManager.UIType.Profile);
        //Do Init
        ProfileUI profile = UIManager.Instance.ActivePage as ProfileUI;
        profile.Initialize(_Name, _IconKey);
        //Destroy Previous Page
        Destroy(prevPage.gameObject);
    }

    /// <summary>
    /// Navigates user to their Conversation with the user tapped
    /// </summary>
    public void OnConversationTapped()
    {
        //Gets current UIPage
        UIPage prevPage = UIManager.Instance.ActivePage;
        //Moves to next scene
        UIManager.Instance.OnUISelect(UIManager.UIType.Conversation);
        //Do Init
        ConversationUI convo = UIManager.Instance.ActivePage as ConversationUI;
        convo.Initialize(_Id);
        //Destroy Previous Page
        Destroy(prevPage.gameObject);
    }
}
