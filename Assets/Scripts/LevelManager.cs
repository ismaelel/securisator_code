using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    private string updateLevelURL = GlobalConfig.ServerURL + "update_level.php";
    private string getLevelURL = GlobalConfig.ServerURL + "get_level.php"; // Nouveau script PHP
    private string endGameURL = GlobalConfig.ServerURL + "end_game.php";
    private string insertScoreURL = GlobalConfig.ServerURL + "insert_score.php"; // Remplace par ton URL PHP
    
    
    private void ProcessResponse(string jsonResponse)
    {
        // Désérialiser la réponse JSON
        ServerResponse response = JsonUtility.FromJson<ServerResponse>(jsonResponse);
        Debug.Log("REPONSE    " + response);
        // Vérifier si la réponse est un succès
        if (response.success)
        {
            // Afficher le niveau récupéré
            Debug.Log($"Niveau récupéré : {response.level}");
            Debug.Log($"Party récupérée : {response.party}");

            Debug.Log("AVANT" + UserSession.CurrentUserLevel);
            UserSession.SetUser(UserSession.CurrentUserId, UserSession.CurrentUsername, response.level, response.party);
            Debug.Log("APRES" + UserSession.CurrentUserLevel);
        }
        else
        {
            Debug.LogError($"Erreur : {response.message}");
        }
    }

    [System.Serializable]
    public class ServerResponse
    {
        public bool success;
        public string message;
        public int level;
        public int party;
    }   
    
    
    public void IncreaseUserLevel(int userId)
    {
        Debug.Log($"📡 Augmentation du niveau pour UserId: {userId}");
        
        if (this == null)
        {
            Debug.LogError("❌ LevelManager est NULL !");
            return;
        }
    
        Debug.Log($"📌 StartCoroutine(UpdateLevel({userId})) est sur le point d'être appelé !");
        //StartCoroutine(UpdateLevel(userId));
        UpdateLevelAsync(userId);
        Debug.Log("FINI?");

    }

    public async void UpdateLevelAsync(int userId)
    {
        Debug.Log(userId);
        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);

        using (UnityWebRequest www = UnityWebRequest.Post(updateLevelURL, form))
        {
            Debug.Log("AVANT LEVEL" + UserSession.CurrentUserLevel);
            Debug.Log("Category : " + getCurrentCategory());
            if (getCurrentCategory() != "")
            {
                Debug.Log("GAME TIMER" + GameTimer.Instance.StopTimer() + " " + UserSession.CurrentUserParty);
                AddScore(getCurrentCategory(), GameTimer.Instance.StopTimer(), UserSession.CurrentUserParty);
            }

            var operation = www.SendWebRequest();
        
            while (!operation.isDone)
            {
                await Task.Delay(100);
                Debug.Log("En attente...");
            }
        
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("APRES LEVEL" + UserSession.CurrentUserLevel);
                Debug.Log("✅ Niveau mis à jour avec succès !");
                GetUserLevel(userId);
            }
            else
            {
                Debug.LogError("❌ Erreur de mise à jour : " + www.error);
            }
        }
    }

 


    public void IncreaseCurrentUserLevel()
    {
        int userIdFromPrefs = PlayerPrefs.GetInt("UserId", -1);
        if (userIdFromPrefs != -1)
        {
            IncreaseUserLevel(userIdFromPrefs);
            
        }
        else
        {
            Debug.LogError("❌ UserId non trouvé dans PlayerPrefs !");
        }
    }

    public async void EndGame()
    {
        int userIdFromPrefs = PlayerPrefs.GetInt("UserId", -1);
        if (userIdFromPrefs != -1)
        {
            
            WWWForm form = new WWWForm();
            form.AddField("user_id", userIdFromPrefs);
            Debug.Log("Données envoyées : user_id = " + userIdFromPrefs);
            Debug.Log("Envoi de la requête à : " + endGameURL);

            using (UnityWebRequest www = UnityWebRequest.Post(endGameURL, form))
            {
                Debug.Log("Données envoyées : user_id = " + userIdFromPrefs);
                Debug.Log("Envoi de la requête à : " + endGameURL);
                var operation = www.SendWebRequest();
                Debug.Log("Réponse du serveur : " + www.downloadHandler.text);

                while (!operation.isDone)
                {
                    await Task.Delay(100);
                    Debug.Log("En attente...");
                }
        
                Debug.Log("ENDGAME?");
                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("✅ OHOHONiveau mis à jour avec succès !");
                    Debug.Log("✅ Réponse serveur : " + www.downloadHandler.text);
                    GetUserLevel(userIdFromPrefs);
                }
                else
                {
                    Debug.LogError("❌ Erreur de mise à jour : " + www.error);
                    Debug.LogError("❌ Erreur HTTP : " + www.responseCode + " | " + www.error);
                }
            }
        }
        else
        {
            Debug.LogError("❌ UserId non trouvé dans PlayerPrefs !");
        }
    }

    public void GetCurrentUserLevel()
    {
        int userIdFromPrefs = PlayerPrefs.GetInt("UserId", -1);
        if (userIdFromPrefs != -1)
        {
            GetUserLevel(userIdFromPrefs);
        }
        else
        {
            Debug.LogError("❌ UserId non trouvé dans PlayerPrefs !");
        }
    }

    public void GetUserLevel(int userId)
    {

        try
        {
            string url = getLevelURL + "?user_id=" + userId; // URL avec paramètre GET
            Debug.Log(url);
            WebRequest wrGETURL = WebRequest.Create(url);
            wrGETURL.Method = "GET"; // S'assurer que la requête est bien une requête GET

            using (WebResponse response = wrGETURL.GetResponse())
            using (Stream objStream = response.GetResponseStream())
            using (StreamReader objReader = new StreamReader(objStream))
            {
                string sLine;
                while ((sLine = objReader.ReadLine()) != null)
                {
                    Debug.Log("Réponse du serveur: " + sLine);
                    ProcessResponse(sLine); // Appeler la fonction pour traiter la réponse
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Erreur lors de la requête GET : " + ex.Message);
        }

    }


    public async void AddScore(string category, float timeSpent, int partyNumber)
    {
      
        Debug.Log("DEBUT ADDSCORE");
        int userId = PlayerPrefs.GetInt("UserId", -1);
        if (userId == -1)
        {
            Debug.LogError("❌ UserID introuvable !");
            return;
        }
        Debug.Log("USER : " + userId + " category ; " + category + " time spent : " + (int)timeSpent + " party number : " + partyNumber);
        
        
        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);
        form.AddField("category", category);
        //form.AddField("time_spent", timeSpent.ToString("F2").Replace(",", ".")); // Format US
        form.AddField("time_spent", (int)timeSpent); // Format US
        form.AddField("party_number", partyNumber);
        
        using (UnityWebRequest www = UnityWebRequest.Post(insertScoreURL, form))
        {Debug.Log("AVANT SEND INSERTSCORE  :::: " + insertScoreURL);
            var operation = www.SendWebRequest();
            Debug.Log("APRES SEND INSERTSCORE  :::: " + insertScoreURL);
            while (!operation.isDone)
            {
                await Task.Delay(100);
            }
            Debug.Log("RESULT SEND INSERTSCORE  :::: " + www.result);
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Score ajouté avec succès !");
            }
            else
            {
                Debug.LogError("❌ Erreur lors de l'ajout du score : " + www.error);
            }
        }
    }

    public string getCurrentCategory()
    {
        if (UserSession.CurrentUserLevel == 1) return "Electricité";
        else if (UserSession.CurrentUserLevel == 2) return "Incendie";
        else if (UserSession.CurrentUserLevel == 3) return "Mécanique";
        else return "";
    }
    
}
