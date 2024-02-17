/*
 * ConversationUI.cs
 * By: William McCarty
 * Controls Conversation UI Page functionality
 * */
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUI : UIPage
{
    #region UI Elements
    [SerializeField]
    private GameObject _ChatContainer;
    [SerializeField]
    private ChatBox _SentToBox;
    [SerializeField]
    private ChatBox _SentFromBox;
    [SerializeField]
    private Button _ShowMoreButton;
    [SerializeField]
    private InputField _MessageInput;
    #endregion

    //User Id of the user is chatting today
    private string _Id;
    //Conversation object of the two users
    private Conversation _Convo;
    //Current Chats displayed
    private int _CurrentChatCount = 0;
    //Max number of chats per page
    private int _ChatsPerPage = 50;

    /// <summary>
    /// Initializes the Conversation UI with the passed in user
    /// </summary>
    /// <param name="id">User Id of the person were chatting to</param>
    public void Initialize(string id)
    {
        _Id = id;
        //Gets the conversation with the passed in id
        _Convo = AppController.Instance.ActiveUser.GetConversation(_Id);
        
        if (_Convo != null)
        {
            //Gets all chat page file names - allows us to find out the most recent version saved
            AppController.Instance.ActiveUser.InitializeLocalChatPageFileNames(_Id, _Convo);
            
            //If we have no pages loaded load starting page
            if (_Convo.PagesLoaded == 0)
            {
                //Loads in page
                AppController.Instance.ActiveUser.LoadNextChatPage(_Convo);
            }
            //Update the conversation display
            UpdateChat();
            //While we dont have the starting number of chats keep loading more chat pages
            while(_CurrentChatCount < _ChatsPerPage)
            {
                //If we failed to load anything else
                if(!AppController.Instance.ActiveUser.LoadNextChatPage(_Convo))
                {
                    //No More Files
                    break;
                }
                //Update the conversation display
                UpdateChat();
            }
            //Update show more button
            UpdateShowMoreButton();
        }
    }

    #region Update UI Function
    /// <summary>
    /// Updates whether to show the load more button
    /// </summary>
    private void UpdateShowMoreButton()
    {
        //If we dont have more chats that can be loaded hide it
        if (_Convo.GetNextChatPageFileToBeLoadedIndex() < 0)
        {
            _ShowMoreButton.gameObject.SetActive(false);
        }
        else
        {
            //Set to the top of the scrollview
            _ShowMoreButton.transform.SetAsFirstSibling();
        }
    }


    /// <summary>
    /// Updates the Chat UI to display loaded Chats
    /// </summary>
    private void UpdateChat()
    {
        //make sure the pagesload is within our chatpage list range
        if (_Convo.PagesLoaded < _Convo.ChatPageList.Count)
        {
            //Get the chat count of our most recent chat page
            int chatsCountInPage = _Convo.ChatPageList[_Convo.PagesLoaded].chats.Count;
            //add items in order of most recent to oldest
            for (int i = chatsCountInPage - 1; i >= 0; i--)
            {
                //Get chat Object in the ChatPAge
                Chat chat = _Convo.ChatPageList[_Convo.PagesLoaded].chats[i];
                ChatBox box;
                Sprite icon;
                //check if the message is from this user or the other user
                if (chat.sender == "You")
                {
                    //Display the send to chat box
                    box = Instantiate(_SentToBox, Vector3.zero, Quaternion.identity, _ChatContainer.transform);
                    //Set the icon to the default version for the user
                    icon = UIManager.Instance.GetIcon("icon_a");
                }
                else
                {
                    //Display the send from chat box
                    box = Instantiate(_SentFromBox, Vector3.zero, Quaternion.identity, _ChatContainer.transform);
                    //set the icon to the users sprite
                    icon = UIManager.Instance.GetIcon(_Convo.photo);
                }
                //Initialize the box to the chat data
                box.Initialize(chat, icon);
                //Set to top so its the recent to older messages in the UI
                box.transform.SetAsFirstSibling();
                //Increase our Chat Count
                _CurrentChatCount++;
            }
            //Increment how many pages were loaded
            _Convo.PagesLoaded++;
        }
    }
    #endregion

    #region Events
    /// <summary>
    /// Loads more Chat Pages and displays them in the scene
    /// </summary>
    public void LoadMoreChats()
    {
        //Load the next chats
        AppController.Instance.ActiveUser.LoadNextChatPage(_Convo);
        //Update the ui
        UpdateChat();
        UpdateShowMoreButton();
    }


    /// <summary>
    /// Sends a message to be added to the conversation
    /// </summary>
    public void OnSendMessage()
    {
        //Create new You Chat + add to container as most recent
        if(!string.IsNullOrEmpty(_MessageInput.text))
        {
            //Get the text typed
            string message = _MessageInput.text;
            //Create the new chat box
            ChatBox box = Instantiate(_SentToBox, Vector3.zero, Quaternion.identity, _ChatContainer.transform);
            Sprite icon = UIManager.Instance.GetIcon("icon_a");
            string timestamp = DateTime.UtcNow.ToString();
            //Set the Chat data
            Chat chat = new Chat("You", message, timestamp);
            //Initialize the box from the chat data
            box.Initialize(chat, icon);
            //Update LastMessage in conversation list
            _Convo.UpdateLastMessageTimestamp(timestamp);
            //Update ChatPages
            _Convo.UpdateChatPages(box, _ChatsPerPage);
            //Reset input field
            _MessageInput.text = "";
            //Increment our Chat count
            _CurrentChatCount++;
        }
    }
    #endregion

    #region Override Functions
    /// <summary>
    /// Override Back Functionaity to also unload the data loaded
    /// </summary>
    public override void Back()
    {
        base.Back();
        _Convo.Unload();
        
    }
    #endregion
}
