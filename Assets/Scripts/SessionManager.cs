using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        // Vérifier si un UserId est stocké
        if (PlayerPrefs.HasKey("UserId"))
        {

            // Charger les données de l'utilisateur
            UserSession.CurrentUserId = PlayerPrefs.GetInt("UserId");
            UserSession.CurrentUsername = PlayerPrefs.GetString("Username");
            UserSession.CurrentUserLevel = PlayerPrefs.GetInt("UserLevel");

            Debug.Log("Session détectée : " + UserSession.CurrentUsername + " (Level " + UserSession.CurrentUserLevel + ")");

            if (currentScene != "MenuPrincipal")
            {
                SceneManager.LoadScene("MenuPrincipal");
            }
        }
        else
        {
            if (currentScene != "SignIn")
            {
                Debug.Log("Aucune session détectée. Redirection vers l'écran de connexion.");
                SceneManager.LoadScene("SignIn"); // Redirige vers l'écran de connexion
            }
            
        }
    }
    
}
