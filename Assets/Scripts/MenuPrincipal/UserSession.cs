using UnityEngine;

public static class UserSession
{
    public static int CurrentUserId;
    public static string CurrentUsername;
    public static int CurrentUserLevel;
    public static int CurrentUserParty;
    public static void SetUser(int userId, string username, int level, int party)
    {
        CurrentUserId = userId;
        CurrentUsername = username;
        CurrentUserLevel = level;
        CurrentUserParty = party;
        
        // Mettre à jour PlayerPrefs
        PlayerPrefs.SetInt("UserId", userId);
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetInt("UserLevel", level);
        PlayerPrefs.SetInt("UserParty", party);
        PlayerPrefs.Save();

        Debug.Log($"🔄 OKOKOKOKOKOKOK UserSession mise à jour : {username} (Level {level})");
    }

    public static void Logout()
    {
        CurrentUserId = 0;
        CurrentUsername = null;
        CurrentUserLevel = 0;

        PlayerPrefs.DeleteKey("UserId");
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("UserLevel");
        PlayerPrefs.DeleteKey("UserParty");
        PlayerPrefs.Save();
    }
}