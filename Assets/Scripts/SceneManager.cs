using UnityEngine;
using UnityEngine.SceneManagement; // Nécessaire pour charger des scènes

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad; // Nom de la scène à charger

    
    public void LoadScene()
    {
        Debug.Log("Loading Scene : " + sceneToLoad);
        // Charger la scène spécifiée
        SceneManager.LoadScene(sceneToLoad);
    }
}