Thanks for purchase UNet Chat from asset store.

Required:
Unity 5.1++

- Get Started: -------------------------------------------------------------------------------

 - Import UNet Chat Package into your project.

    - For test:
           - Open the example scene in UNet Chat -> Example -> Scene -> ChatUnet.
           - Play Scene and Create a server with LAN Host(H).
           - Write your player name and click on next button.
           - For Test with more clients, just create a build of example scene.
           - And open the game 2 or more, create a server with one of these and others connect to this server (LAN Client(C))

    - For add in your game:
           - Drag the chat prefab "ChatUI" located in UNet Chat -> Content -> Prefab -> Chat -> /*
           - Drag it on the Canvas root if you have one, if not create one.
           - Ready!.

- Integration: ---------------------------------------------------------------------------------

Integrate chat is simple, just required that you send the player name to register a client on the chat,
the chat contain a method for send the name, but if you don't want to use it, you simple need do this:

First create a reference of bl_ChatManager in the script where you manage the player name logic, eg: 

public bl_ChatManager myChat; //assign in inspector

- Then, when you have ready for send the name and register in chat, just call like this:

myChat.SetPlayerName("PlayerNameHere",true);

- Remember if you use this, set the variable "Show Player Name Input" to false for avoid show the input field in the chat.

----------------------------------------------------

For assign / change the group in runtime you can do this:

 - Get a re fence (if not have one) of bl_ChatManager like above, eg: public bl_ChatManager myChat; //assign in inspector
 - Then, just call like this: MyChat.ChangeGroup(group id);
 - Where 'groupid' is the indexOf (int) of group in the 'Groups' list of bl_ChatManager.


Contact: -----------------------------------------------------------------------------------------------------------------

If you have any problem or question, feel free to contact us.
Please if you have a problem, contact us before leave a bad review, we respond in no time.

Forum: http://lovattostudio.com/forum/index.php
Email Form: http://www.lovattostudio.com/en/support/






        