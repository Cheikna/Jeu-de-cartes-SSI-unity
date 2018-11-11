using UnityEngine;

public static class bl_ChatUtils
{

    public static string FilterWord(string[] blacklist, string text,string replaceChar = "*")
    {
        foreach (string str2 in blacklist)
        {
            if (text.ToLower().IndexOf(str2) != -1)
            {
                string filter = "";
                for (int i = 0; i < str2.Length; i++)
                {
                    filter += replaceChar;
                }
                text = text.ToLower().Replace(str2, filter);
            }
        }

        return text;
    }
}