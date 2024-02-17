/*
 * Conversation.cs
 * By: William McCarty
 * Holds data for a conversation
 * */
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// Holds a list of conversations a User has
/// </summary>
public class ConversationList
{
    [JsonProperty("conversations")]
    public List<Conversation> conversations = new List<Conversation>();

    public ConversationList() { }
}

/// <summary>
/// Data associated to a specfic conversation with another user
/// </summary>
public class Conversation
{

    #region Json Loaded Data
    [JsonProperty("id")]
    public string id = "";
    [JsonProperty("name")]
    public string name = "";
    [JsonProperty("photo")]
    public string photo = "";
    [JsonProperty("lastMessageDate")]
    public string lastMessageDate = "";
    #endregion

    //Chat Pages loaded in the ChatPageList
    private int _PagesLoaded = 0;
    [JsonIgnore]
    public int PagesLoaded
    {
        get
        {
            return _PagesLoaded;
        }
        set
        {
            _PagesLoaded = value;
        }
    }

    //List of ChatPage FileNames ordered in 0 is the oldest - N is the newest
    private SortedDictionary<int, string> _ChatPageFileNames;
    //List of active Chat Pages
    private List<ChatPage> _ChatPageList = new List<ChatPage>();
    [JsonIgnore]
    public List<ChatPage> ChatPageList
    {
        get
        {
            return _ChatPageList;
        }
    }

    public Conversation() { }

    /// <summary>
    /// Sets the Passed in Sorted Dictionary to the Current ChatPageFileList
    /// </summary>
    /// <param name="chatPageFileNames">SortedDictionary of the page number and the file names asscoitated with them</param>
    public void InitializeChatPageFiles(SortedDictionary<int, string> chatPageFileNames)
    {
        _ChatPageFileNames = chatPageFileNames;
    }

    #region Getters/Setters
    /// <summary>
    /// Gets the next ChatPage Filename
    /// </summary>
    /// <returns>Next ChatPageFileName</returns>
    public string GetNextChatPageFileToBeLoaded()
    {
        //every index should be in order and not skip
        //we include the PagesLoaded to offset the next most recent entry to load
        int pageIndex = _ChatPageFileNames.Count - _PagesLoaded - 1;
        //Assures it has the page number
        if(_ChatPageFileNames.ContainsKey(pageIndex))
        {
            //Returns the filename of that page
            return _ChatPageFileNames[pageIndex];
        }
        //That page doesn't exist - most likely at the end of all saved ChatPages
        return "";
    }

    /// <summary>
    /// Get index of the next ChatPageFileName
    /// </summary>
    /// <returns>index of next ChatPageFileName</returns>
    public int GetNextChatPageFileToBeLoadedIndex()
    {
        //every index should be in order and not skip
        //we include the PagesLoaded to offset the next most recent entry to load
        return _ChatPageFileNames.Count - _PagesLoaded - 1;
    }

    /// <summary>
    /// Get the of known ChatPages we have total
    /// </summary>
    /// <returns>number of how many pages there are total</returns>
    public int GetNumberOfChatPages()
    {
        return _ChatPageFileNames.Count - 1;
    }

    /// <summary>
    /// Add a new ChatPage to the ChapPageList
    /// </summary>
    /// <param name="page">Chatpage that is needing to be added</param>
    public void AddChatPage(ChatPage page)
    {
        _ChatPageList.Add(page);
    }

    /// <summary>
    /// Add a ChatPageFile to the Known ChatPageFile List
    /// </summary>
    /// <param name="pageNumber">Number of the page</param>
    /// <param name="fileName">Name of the ChatPage File</param>
    public void AddChatPageFileName(int pageNumber, string fileName)
    {
        _ChatPageFileNames.Add(pageNumber, fileName);
    }
    #endregion

    #region Saving + Loading
    /// <summary>
    /// Clears out loaded ChatPages in a session
    /// </summary>
    public void Unload()
    {
        _PagesLoaded = 0;
        _ChatPageList.Clear();
    }

    /// <summary>
    /// Updates the Conversation last time message was added
    /// </summary>
    /// <param name="timestamp"></param>
    public void UpdateLastMessageTimestamp(string timestamp)
    {
        lastMessageDate = timestamp;
        //Saves the new timestamp to the ConversationList
        AppController.Instance.ActiveUser.SaveConversationList();
    }

    /// <summary>
    /// Updates the List of Chats and adds new ChatPages if needed
    /// </summary>
    /// <param name="box">The New Chatbox being added to the conversation</param>
    /// <param name="maxChatCount">The Max chats in a Page</param>
    public void UpdateChatPages(ChatBox box, int maxChatCount)
    {
        if(_ChatPageList.Count > 0)
        {
            bool newPage = false;
            //if we can add to our current last page add to end of list
            if(_ChatPageList[0].chats.Count >= maxChatCount)
            {
                newPage = true;
                ChatPage page = new ChatPage();
                //Add it to the front as its the most recent
                _ChatPageList.Insert(0, page);
            }
            //we need to make a new page
            _ChatPageList[0].chats.Add(new Chat(box.Chat.sender, box.Chat.message, box.Chat.timestamp));
            //Issue save locally
            AppController.Instance.ActiveUser.SaveConversation(this, newPage);
        }
    }
    #endregion
}
