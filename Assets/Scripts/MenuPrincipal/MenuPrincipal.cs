using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPrincipal : MonoBehaviour
{
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Méthode pour aller à l'écran de connexion
    public void GoToLogin()
    {
        SceneManager.LoadScene("SignIn");
    }

    // Méthode pour aller à l'écran d'inscription
    public void GoToSignup()
    {
        SceneManager.LoadScene("SignUp");
    }
    
    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void Play()
    {
        Debug.Log("Play LEVEL" + UserSession.CurrentUserLevel);
        Debug.Log("PREFS LEVEL" + PlayerPrefs.GetInt("UserLevel"));
        switch (UserSession.CurrentUserLevel)
        {
            case 0:
                SceneManager.LoadScene("Intro");
                break;
            case 1:
                SceneManager.LoadScene("Elec");
                break;
            case 2:
                SceneManager.LoadScene("Fire");
                break;
            case 3:
                SceneManager.LoadScene("Meca");
                break;
        }
       
    }
    
    // 🛑 Bouton Déconnexion : Effacer la session et revenir au menu principal
    public void Logout()
    {
        PlayerPrefs.DeleteKey("UserId");
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("UserLevel");
        PlayerPrefs.Save(); // Sauvegarde les changements

        Debug.Log("Déconnexion réussie. Retour au menu principal.");
        SceneManager.LoadScene("SignIn"); // Redirige vers le menu principal
    }

    // ❌ Bouton Quitter : Fermer complètement l'application
    public void QuitGame()
    {
        Logout();
        Debug.Log("Fermeture du jeu.");
        Application.Quit(); // Quitte le jeu (ne fonctionne pas dans l'éditeur)

        // Permet de simuler la fermeture dans l'éditeur Unity
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void Historique()
    {
        SceneManager.LoadScene("Score"); 
    }
    
    public void LoadMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal"); // Redirige vers le menu principal
    }
}
