/*
 * ConversationsListUI.cs
 * By: William McCarty
 * Handles functionality for the ConversationUI Page
 * */
using System.Collections.Generic;
using UnityEngine;

public class ConversationsListUI : UIPage
{
    #region UI ELements
    [SerializeField]
    private GameObject _ScrollviewContainer;
    [SerializeField]
    private ConversationListItem _ConversationListItem;
    #endregion

    public void Awake()
    {
        //Get the Users list of conversations
        List<Conversation> conversations = AppController.Instance.ActiveUser.GetConversations();
        foreach(Conversation conversation in conversations)
        {
            //Create a new ConversationListItem to display what conversations the user has
            ConversationListItem item = Instantiate(_ConversationListItem, _ScrollviewContainer.transform.position, Quaternion.identity, _ScrollviewContainer.transform) as ConversationListItem;
            //Set the values of the Item to display properlly
            item.SetValues(conversation.id, conversation.name, conversation.lastMessageDate, conversation.photo);
        }
    }
}
