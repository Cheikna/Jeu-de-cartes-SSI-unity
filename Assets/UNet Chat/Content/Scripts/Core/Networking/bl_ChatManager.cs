
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkIdentity))]
public class bl_ChatManager : NetworkBehaviour
{
    [Header("Settings")]
    [HideInInspector]
    public int GroupID = 0;
    public List<bl_GroupInfo> Groups = new List<bl_GroupInfo>();
    public string ClientName = string.Empty;

    public bool useBothSides = true;
    public bool useGroupPrefix = true;
    public bool ShowPlayerNameInput = true; //If you setup your own method for sen the name, desactive this.
    public int MaxMessages = 15;

    [TextArea(3, 7)] public string BlackList;
    public string ReplaceString = "*";
    [HideInInspector] public bool ShowBlackListEditor;

    public bool FadeMessage = true;
    public float FadeMessageIn = 7;
    public float FadeMessageSpeed = 2;

    private NetworkClient Client;
    private bl_ChatUI ChatUI = null;
    [HideInInspector] public bool ShowGroupsEditor;
    private bool isSetup = false;
    private bool isClientInitializated = false;

    public class MyMsgType
    {
        public static short ChatMsn = MsgType.Highest + 1;
    };

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        ClientName = string.Empty;
        ChatUI = FindObjectOfType<bl_ChatUI>();
        ChatUI.MaxMessages = MaxMessages;
        ChatUI.ShowChat(false);
        ChatUI.ShowPlayerNameUI(false);
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (!isSetup)
        {
            SetupClient();
        }
        //Check if client ready
        if (isClientInitializated && !isServer)
        {
            OnConnected(null);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnStartClient()
    {
        isClientInitializated = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="field"></param>
    public void SendChatText(InputField field)
    {
        string text = field.text;
        if (string.IsNullOrEmpty(text))
            return;

        SendMessageChat(text, GroupID);
        field.text = string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="id"></param>
    void SendMessageChat(string t, int id)
    {
        //create the network message 
        bl_MsnChatInfo info = new bl_MsnChatInfo();
        info.Text = t;
        info.GroupID = id;
        info.Sender = ClientName;

        if (Client.isConnected)
        {
            Client.Send(MyMsgType.ChatMsn, info); // Sending message from client to server
        }
    }

    /// <summary>
    /// Create a client and connect to the server port
    /// </summary>
    public void SetupClient()
    {
        NetworkManager net = FindObjectOfType<NetworkManager>();
        Client = net.client;
        //Register Message listeners
        Client.RegisterHandler(MsgType.Connect, OnConnected);

        //if this are server
        if (isServer)
        {
            //register server message listener
            NetworkServer.RegisterHandler(MyMsgType.ChatMsn, OnChatMessage);
        }
        isSetup = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="netMsg"></param>
    public void OnChatMessage(NetworkMessage netMsg)
    {
        //Read network message
        bl_MsnChatInfo msg = netMsg.ReadMessage<bl_MsnChatInfo>();

        //If this is server
        if (isServer && !msg.PassFroServer)
        {
            //send message to all clients
            msg.PassFroServer = true;
            NetworkServer.SendToAll(MyMsgType.ChatMsn, msg); // Send to all clients
        }
        else if (Client.isConnected)//if this client
        {
            //receive message and show in the chat
            string t = GetMessageFormat(msg.Text, msg.Sender, GetGroup(msg.GroupID));
            bool myTeam = true;
            if (useBothSides)
            {
                myTeam = (msg.GroupID == GroupID) ? true : false;
            }
            //add message in chat
            ChatUI.AddNewLine(t, FadeMessage, FadeMessageIn, FadeMessageSpeed, myTeam);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="netMsg"></param>
    public void OnConnected(NetworkMessage netMsg)
    {
        //if client and name is not setup yet, then show the input field
        if (string.IsNullOrEmpty(ClientName) && Client.isConnected && ShowPlayerNameInput)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            ChatUI.ShowPlayerNameUI(true);
            ClientName = "Anonymous";
        }
        else if (Client.isConnected)//if name already setup
        {
            ChatUI.ShowPlayerNameUI(false);
            ChatUI.ShowChat(true);
            if (string.IsNullOrEmpty(ClientName))
            {
                ClientName = "Anonymous";
                Client.RegisterHandler(MyMsgType.ChatMsn, OnChatMessage);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msn"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public string GetMessageFormat(string msn, string sender, bl_GroupInfo group)
    {
        string m = "";
#if UNITY_5_2 || UNITY_5_3_OR_NEWER

        string hex = ColorUtility.ToHtmlStringRGBA(group.GroupColor);
#else
         string hex = group.GroupColor.ToHexStringRGBA();
#endif
        string filterText = msn;
        //Apply word filter
        if (GetBlackListArray.Length > 0)
        {
            filterText = bl_ChatUtils.FilterWord(GetBlackListArray, msn, ReplaceString);
        }
        if (useGroupPrefix)
        {
            m = string.Format("<color=#{0}>[{1}]{3}: </color>{2}", hex, group.Name, filterText, sender);
        }
        else
        {
            m = string.Format("{2}<color=#{0}>[{1}]</color>", hex, filterText, sender);
        }
        return m;
    }

    /// <summary>
    /// Call this for change of chat group.
    /// </summary>
    /// <param name="id"></param>
    public void ChangeGroup(int id)
    {
        GroupID = id;
    }

    /// <summary>
    /// Set the player name to show in the chat
    /// </summary>
    /// <param name="n">want register in chat or just change the name</param>
    public void SetPlayerName(string n, bool register = false)
    {
        ClientName = n;
        //if first time send the name, then is time for register in chat
        if (register)
        {
            Client.RegisterHandler(MyMsgType.ChatMsn, OnChatMessage);
            ChatUI.ShowChat(true);
        }
    }

    /// <summary>
    /// Call this when want to clean all messages from the chat
    /// </summary>
    public void CleanChat()
    {
        ChatUI.Clean();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bl_GroupInfo GetGroup(int id)
    {
        return Groups[id];
    }

    /// <summary>
    /// 
    /// </summary>
    public string[] GetGroupArray
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < Groups.Count; i++)
            {
                list.Add(Groups[i].Name);
            }
            return list.ToArray();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private string[] GetBlackListArray
    {
        get
        {
            List<string> list = new List<string>();
            string[] split = BlackList.Split(',');
            foreach (string str in split)
            {
                string t = str.Trim();
                if (!string.IsNullOrEmpty(t))
                {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }
    }
}