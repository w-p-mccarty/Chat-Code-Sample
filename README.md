# Chat Code Sample
##### Author: William McCarty
##### 2024
----
### Description
----

This is a Unity Project, written in C# that Creates a Chat Application with the following features:
* Show a list of conversations.
* Display a friend name, icon/photo, time of the last message.
* Clicking on name should display a conversation with that person.
* Friend's profile.
* Clicking on the friend's icon in conversation list should display a profile dialog showing name and a larger version of photo.
* Display a conversation with a friend (list of messages).
* Show your icon, friend's icon, message and date/time message was sent.
* Data loaded and saved in JSON files, emulating data that would be received from the server.
* Allows user to add new items to the conversation from themselves.

----
### Installation
----

In order to test the Project please do the following steps:
1. Open the Project and open the Main.unity scene
2. The Scene should open up and should just contain some core functionality components
3. By hitting the play button the First UI should become visible(Conversations List)

You can also make a build in unity and run it directly.  

***NOTE:***   The Main.unity scene starts off with no UI and the UI is loaded in Dynamically as needed

----
### Usage
----

####  Modifying Conversation List
There are about 30 different Conversations active that are preloaded within the app.  This can be modified by modifying `userid_conversations` in the resources folder.  Keep in mind a local copy gets made for modifications and the application uses that if it exists.  So in order to change this field in the project and see the reflection you must delete the version under `LocalLow/WilliamMcCarty/Chat-Code-Sample` or simply modify it there

----
####  Conversations

Each conversation has 300 messages preloaded.  The conversations in this example are the same.  They are broken up into Page files each page file contains about 50 chats.  When the application enters the Conversation section it looks up the most recent chat page file and loads it if the amount is under 50 it will load the next one and will keep doing this till its greater than or equal to 50.  Worst case 99 elements are loaded during startup, but in default loading 50 chats are loaded.  After this if you scroll up all the way a Show more label is visible if more pages can be loaded.  Tapping that will give you the next 50 chats.

----
####  JSON Processing
There are two basic Jsons/Types:

1.  userid_conversations located in Resources + local version is in `LocalLow/WilliamMcCarty/Chat-Code-Sample`
	- This controls what conversations the user has open.  Each entry in the list contains:
		-    **id**: user id they are communicating to
		-    **name**: name of the user they are communicating to
		-    **photo**: icon the user they are communicating to is using
		-    **lastMessageDate**: Time of the last message in the conversation
2. `<id>_chat_<pageNumber>` located in `Resources/PreloadedChats/<id>` or locally in `LocalLow/WilliamMcCart/Chat-Code-Sample/Chats/<id>`
These contain the chats for each conversation with that user.  The `id` is the users id being communicated to, essentially the id in the conversation list.  The `pageNumber` is the paged data that will be loaded.  The larger the page number, the more recent the chat is. There can be X pageNumbers associated to and id, but are loaded one at a time.  Each entry has:
   - **sender**: name of the user that sent the message,"You" will be shown if its from the user looking at the application itself
   - **message**: text being sent to the other user
   - **timestamp**: time of the message sent
----
####  Core Class Summaries

`Appcontroller` - Controls the main app flow and the user using the app itself
`ChatFrom` - Chat messages sent from a different user that isn't using the app
`ChatSent` - Chat messages sent from the user using the app
`ConversationListItem` - Conversation between both users in the Conversations list
`ConversationListUI` - UI Page to display all conversation of the User
`ConversationUI` - UI Page to display the conversation between both users
`ProfileUI` - UI Page to that shows the user info
`UIManager` - Manages UI Page transitions and navigation
