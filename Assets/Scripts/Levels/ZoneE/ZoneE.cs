using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZoneE : MonoBehaviour
{
   
    public Text timer;

    void Start()
    {
        GameTimer.Instance.StartTimer();
    }
    void Update()
    {
        
        timer.text = GameTimer.Instance.getTime();
        
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"La scène {scene.name} a été chargée.");
       
    }

    
}