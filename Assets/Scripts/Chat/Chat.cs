/*
 * Chat.cs
 * By: William McCarty
 * Used for Holding Chat data for saving and loading
 * */
using Newtonsoft.Json;

public class Chat
{
    [JsonProperty("sender")]
    public string sender;
    [JsonProperty("message")]
    public string message;
    [JsonProperty("timestamp")]
    public string timestamp;

    public Chat() { }
    public Chat(string sender, string message, string timestamp)
    {
        this.sender = sender;
        this.message = message;
        this.timestamp = timestamp;
    }
	
}
