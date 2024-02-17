/*
 * ChatPage.cs
 * By: William McCarty
 * The list of Chats apart of a Single Chat page for pagination
 * */
using Newtonsoft.Json;
using System.Collections.Generic;

public class ChatPage 
{
    [JsonProperty("chats")]
    public List<Chat> chats = new List<Chat>();
}
