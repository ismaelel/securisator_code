using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public GameObject scorePrefab; // Prefab contenant un Text standard pour chaque score
    public Transform scoreContainer; // Parent UI où afficher les scores
    private string getScoresURL = GlobalConfig.ServerURL + "get_scores.php";
    public Transform ScoresParent;

    void Start()
    {
        StartCoroutine(LoadScores());
        //StartCoroutine(GetComments());
    }

   IEnumerator LoadScores()
{
    if (scorePrefab == null)
    {
        Debug.LogError("❌ scorePrefab n'est pas assigné dans l'Inspector !");
        yield break;
    }

    if (scoreContainer == null)
    {
        Debug.LogError("❌ scoreContainer n'est pas assigné dans l'Inspector !");
        yield break;
    }

    using (UnityWebRequest www = UnityWebRequest.Get(getScoresURL))
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("📡 Réponse JSON : " + jsonResponse);

            // Désérialiser la réponse
            ScoreData[] scores = JsonHelper.FromJson<ScoreData>(jsonResponse);
            
            // Vider la liste avant d'afficher de nouveaux scores
            foreach (Transform child in scoreContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (ScoreData score in scores)
            {
                Debug.Log($"✅ Score chargé : {score.login}, {score.category}, {score.time_spent}, {score.party_number}");
                
                // Instancier chaque entrée de score dans l'UI
                GameObject entry = Instantiate(scorePrefab, scoreContainer);
                Text textComponent = entry.GetComponentInChildren<Text>();
                Debug.Log("score.login: " + score.login + ", score.category: " + score.category + ", score.time_spent: " + score.time_spent + ", score.party_number: " + score.party_number);
                if (textComponent != null)
                {
                    textComponent.text = $"👤 {score.login} | {score.category}\n ⏱ {score.time_spent}s | 🎮 Partie {score.party_number}";
                }
                else
                {
                    Debug.LogError("❌ Text non trouvé dans le prefab !");
                }
            }
        }
        else
        {
            Debug.LogError("❌ Erreur de récupération des scores : " + www.error);
        }
    }
}


    [System.Serializable]
    public class ScoreData
    {
        public int user_id;
        public string login;  
        public string category;
        public float time_spent;
        public int party_number;
    }
    
    // IEnumerator GetComments()
    // {
    //     using (UnityWebRequest request = UnityWebRequest.Get(getScoresURL))
    //     {
    //         yield return request.SendWebRequest();
    //
    //         if (request.result == UnityWebRequest.Result.Success)
    //         {
    //             string jsonResponse = request.downloadHandler.text;
    //             Debug.Log("Server response: " + jsonResponse);
    //
    //             try
    //             {
    //                 ScoresResponse response = JsonUtility.FromJson<ScoresResponse>(jsonResponse);
    //                 Debug.Log("ICI");
    //                 
    //                 DisplayScores(response.scores);
    //                 Debug.Log("LA");
    //             }
    //             catch (System.Exception e)
    //             {
    //                 Debug.LogError("Error parsing JSON: " + e.Message);
    //                 Debug.Log("Erreur de chargement des commentaires.");
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("Erreur lors du chargement: " + request.error);
    //         }
    //     }
    // }
    //
    private void DisplayScores(Score[] scores)
    
    {
        // Supprime les anciens commentaires
        foreach (Transform child in ScoresParent)
        {
            Destroy(child.gameObject);
        }

        // Ajoute chaque commentaire comme un nouvel élément
        foreach (Score score in scores)
        {
            GameObject scoreObject = Instantiate(scorePrefab, ScoresParent);
            Text scoreText = scoreObject.GetComponentInChildren<Text>();
            scoreText.text = score.category; // Affiche uniquement le commentaire, sans le username
        }
        
        // Force une mise à jour du layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(ScoresParent.GetComponent<RectTransform>());
    }
}

// Utilitaire pour parser un tableau JSON
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
    
    
}

[System.Serializable]
public class Score
{
    public string category;
}

[System.Serializable]
public class ScoresResponse
{
    public Score[] scores;
}



