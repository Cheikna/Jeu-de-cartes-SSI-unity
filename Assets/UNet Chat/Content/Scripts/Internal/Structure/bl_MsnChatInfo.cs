using UnityEngine.Networking;

public class bl_MsnChatInfo : MessageBase
{
    public string Sender;
    public string Text;
    public int GroupID;
    public bool PassFroServer = false;
}