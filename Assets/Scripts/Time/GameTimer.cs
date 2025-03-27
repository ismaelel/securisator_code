using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    private float startTime;
    private bool isRunning = false;
   // public Text timerText; 
    private float penaltyTime = 0f; // Temps ajouté en cas de mauvaise réponse

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garde l'objet actif même après un changement de scène
        }
        else
        {
           // Destroy(gameObject);
        }
        
        
    }

    private void Update()
    {
        if (isRunning) 
        {
            float elapsedTime = Time.time - startTime + penaltyTime;
            //UpdateTimerUI(elapsedTime);
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
        penaltyTime = 0f; // Reset les pénalités

        Debug.Log("⏳ Timer démarré !");
    }

    public float StopTimer()
    {
        isRunning = false;
        float elapsedTime = Time.time - startTime + penaltyTime;
        Debug.Log("⏳ Temps écoulé : " + elapsedTime.ToString("F2") + " secondes");
        return elapsedTime;
    }

    // private void UpdateTimerUI(float elapsedTime)
    // {
    //     if (timerText != null)
    //     {
    //         timerText.text = "⏳ Temps : " + elapsedTime.ToString("F2") + "s";
    //     }
    //     else
    //     {
    //         Debug.LogWarning("⚠️ TimerText n'est pas assigné, désactivation du Timer !");
    //         isRunning = false;  // Stoppe le timer pour éviter les mises à jour inutiles
    //     }
    // }

    public string getTime()
    {
        float elapsedTime = Time.time - startTime + penaltyTime;
        return  "⏳ Temps : " + elapsedTime.ToString("F2") + "s";;
    }
    public void AddPenalty(float extraTime)
    {
        penaltyTime += extraTime;
        Debug.Log("❌ Pénalité ajoutée : +" + extraTime + "s");
        
    }
}