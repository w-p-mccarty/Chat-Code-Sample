/*
 * ChatBox.cs
 * By: William McCarty
 * Chat Box displayed in the Conversation UI container
 * */
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    #region UI Elements
    //UI Element references
    [SerializeField]
    private Text _SenderLabel;
    [SerializeField]
    private Text _TimeLabel;
    [SerializeField]
    private Image _IconImage;
    [SerializeField]
    private Text _MessageLabel;
    #endregion

    //Data holders to the Chat Data
    private Chat _Chat;
    public Chat Chat
    {
        get
        {
            return _Chat;
        }
    }
    //Icon the Chat box will use
    private Sprite _Icon;


    public void Initialize(Chat chatData, Sprite icon)
    {
        _Chat = chatData;
        _Icon = icon;

        //Set the UI elements to the chat data
        _SenderLabel.text = _Chat.sender;
        _TimeLabel.text = _Chat.timestamp;
        _IconImage.sprite = _Icon;
        _MessageLabel.text = _Chat.message;
    }
}
