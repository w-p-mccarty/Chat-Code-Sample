/*
 * User.cs
 * By: William McCarty
 * Hold the core data associated to the User Object
 * */
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User
{
    //List of the conversations this user has
    ConversationList _Conversations;

    //Dummy id for the user
    string _Id = "userid";

    /// <summary>
    /// Creates a new user and issues its dummy login
    /// </summary>
    /// <param name="dummyUserName">Dummy login that is used for login</param>
    /// <param name="dummyPW">Dummy pw that is use for login</param>
    public User(string dummyUserName, string dummyPW)
    {
        //Auth validation -- Skipped for now
        //Id field will remain constant for this exercise

        //Logged in yay lets get this users data

        //Simulated Downloaded Data from Server
        string jsonData = "";
        string conversationListPath = Application.persistentDataPath + "/" + _Id + "_conversations";
        //If we have a local version with new data use that otherwise use the default version
        if (File.Exists(conversationListPath))
        {
            jsonData = File.ReadAllText(conversationListPath);
        }
        else
        {
            jsonData = (Resources.Load(_Id + "_conversations") as TextAsset).text;
            //Move our conversation list file to local path for potential modifications
            File.WriteAllText(conversationListPath, jsonData);
        }
        

        //Load Json
        _Conversations = JsonConvert.DeserializeObject<ConversationList>(jsonData);
    }

    #region Accessors
    /// <summary>
    /// Gets the list of conversations this user has
    /// </summary>
    /// <returns>List of conversation objects</returns>
    public List<Conversation> GetConversations()
    {
        return _Conversations.conversations;
    }

    /// <summary>
    /// Get Specfic conversation the user is having with the passed in id
    /// </summary>
    /// <param name="id">Id of the user that this user is having a conservation with</param>
    /// <returns>Conversation object of both users chats</returns>
    public Conversation GetConversation(string id)
    {
        return _Conversations.conversations.Find(x => x.id == id);
    }
    #endregion

    #region Chat Page Initialization

    /// <summary>
    /// Fetchs all Chat Pages in a conversation and sets them for internal use
    /// </summary>
    /// <param name="id">Id of the user that this user is having a conservation with</param>
    /// <param name="convo">Conversation object between both users</param>
    public void InitializeLocalChatPageFileNames(string id, Conversation convo)
    {
        //This will sort the ChatPage filenames in order of oldest to newest
        SortedDictionary<int, string> fileNames = new SortedDictionary<int, string>();
        //Get Files In Resources
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/Resources/PreloadedChats/" + id);
        //Add the found chat pages to the running SortedDictionary
        AddChatPagesTofileList(fileNames, id, fileEntries);

        //If we have a local chat file(ie a new chat item)
        if (Directory.Exists(Application.persistentDataPath + "/Chats/" + id))
        {
            //Get Files In that path location
            fileEntries = Directory.GetFiles(Application.persistentDataPath + "/Chats/" + id);
            //Add the found chat pages to the running SortedDictionary
            AddChatPagesTofileList(fileNames, id, fileEntries);
        }
        //Set the conversations current ChatPage files to the ones found
        convo.InitializeChatPageFiles(fileNames);

    }

    /// <summary>
    /// Goes through a directory and adds all ChatPage files that match with the id
    /// </summary>
    /// <param name="fileNameList">SortedDictionary of the page number and the filename of the ChatPage locally</param>
    /// <param name="id">Id of the user this user is having a conversation with</param>
    /// <param name="fileEntries">List of Filepaths that are needing to be searched</param>
    private void AddChatPagesTofileList(SortedDictionary<int, string> fileNameList, string id, string[] fileEntries)
    {
        //go through each file in the fileEntries
        foreach (string filePath in fileEntries)
        {
            //Split path to get the filename
            string[] splitPath = filePath.Split('\\');
            string fileName = splitPath[splitPath.Length - 1]; 

            //Only continue if the filename contains the id passed in and not a .meta
            if (fileName.Contains(id) && !fileName.Contains(".meta"))
            {
                //Get the page number of the chat
                string[] splitFilename = fileName.Split('_');
                if (splitFilename.Length == 3)
                {
                    //Makes sure to only take the first part of the file extension
                    int pageNumber = Int32.Parse(splitFilename[2].Split('.')[0]);
                    //Verify the key isn't already in the list
                    if (!fileNameList.ContainsKey(pageNumber))
                    {
                        //add to dictionary
                        fileNameList.Add(pageNumber, fileName);
                    }
                }
            }
        }
    }

    #endregion

    #region Loading

    /// <summary>
    /// Loads the Next ChatPage needing to be loaded in
    /// </summary>
    /// <param name="convo">Conversation between this user and another user</param>
    /// <returns>Whether the load was successful or not</returns>
    public bool LoadNextChatPage(Conversation convo)
    {
        //Get the next ChatPath Filename that needs to be loaded
        string fileName = convo.GetNextChatPageFileToBeLoaded();

        //If the local chat directory doesn't exist make it
        if (!Directory.Exists(Application.persistentDataPath + "/Chats/" + convo.id))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Chats/" + convo.id);
        }

        //Path of local chat file location
        string conversationPath = Application.persistentDataPath + "/Chats/" + convo.id + "/" + fileName;
        string resoucesJson = "";
        //If this exists locally use that
        if (File.Exists(conversationPath))
        {
            resoucesJson = File.ReadAllText(conversationPath);
        }
        else if (!string.IsNullOrEmpty(fileName))
        {
            //If its not local then its probably a preloaded chat file
            fileName = fileName.Split('.')[0];
            resoucesJson = (Resources.Load("PreloadedChats/"+ convo.id + "/" + fileName) as TextAsset).text;
        }
        //Convert the ChatPage file to a object
        ChatPage page = JsonConvert.DeserializeObject<ChatPage>(resoucesJson);
        if (page != null)
        {
            //Add the page to the Conversation list
            convo.AddChatPage(page);
            //Was Loaded
            return true;
        }
        //Wasn't Loaded
        return false;
    }
    #endregion

    #region Saving

    /// <summary>
    /// Saves the current User Conversations Object to the local storage
    /// </summary>
    public void SaveConversationList()
    {
        //Json Serializating Settings
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };
        //Serialize the ConversationList Data
        string jsonData = JsonConvert.SerializeObject(_Conversations, settings);
        //Path to the Local Conversation file
        string conversationListPath = Application.persistentDataPath + "/" + _Id + "_conversations";
        //Save Locally
        SaveJsonData(conversationListPath, jsonData);
    }

    /// <summary>
    /// Saves the Most recent Chat messages locally
    /// </summary>
    /// <param name="convo">Conversation between this user and another one</param>
    /// <param name="newPage">Whether the page processed is a new page</param>
    public void SaveConversation(Conversation convo, bool newPage)
    {
        //Json Serializating Settings
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };
        //If this is a new page we add the filename to the list
        if(newPage)
        {
            convo.AddChatPageFileName(convo.GetNumberOfChatPages() + 1, convo.id + "_chat_" + (convo.GetNumberOfChatPages() + 1));
        }
        //This is the last index of the Page File names aka Most recent
        int newPageNumber = convo.GetNumberOfChatPages();

        //Serialize Most Recent ChatPage
        string jsonData = JsonConvert.SerializeObject(convo.ChatPageList[0], settings);
        //Create Directory if doesn't exist
        if (!Directory.Exists(Application.persistentDataPath + "/Chats/" + convo.id))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Chats/" + convo.id);
        }

        //get path to the current Chat Page File
        string conversationListPath = Application.persistentDataPath + "/Chats/" + convo.id + "/" + convo.id + "_chat_" + newPageNumber;
        //if this is a new page well create the file
        if (newPage)
        {
            File.Create(conversationListPath).Close();
        }
        //Save Data Locally
        SaveJsonData(conversationListPath, jsonData);
    }

    /// <summary>
    /// Helper Functions to Write json data to a file if it exists
    /// </summary>
    /// <param name="path">Location of where the data is being written to</param>
    /// <param name="jsonData">Data that is being written to the file</param>
    private void SaveJsonData(string path, string jsonData)
    {
        //verify the file exists
        if (File.Exists(path))
        {
            File.WriteAllText(path, jsonData);
        }
    }

    #endregion
}
