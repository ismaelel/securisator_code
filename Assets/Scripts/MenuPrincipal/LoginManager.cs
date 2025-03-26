using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField inputUsername;
    public InputField inputPassword;
    public Button loginButton;
    public Text feedbackText;

    private string loginUrl = GlobalConfig.ServerURL + "/login.php"; // URL du script PHP

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    public void OnLoginButtonClicked()
    {
        string username = inputUsername.text;
        string password = inputPassword.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Veuillez saisir votre nom d'utilisateur et votre mot de passe.";
            return;
        }

        // Hacher le mot de passe
        string hashedPassword = HashPassword(password);

        StartCoroutine(SendLoginRequest(username, hashedPassword));
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2")); // Convertir en hexadécimal
            }
            return builder.ToString();
        }
    }

    IEnumerator SendLoginRequest(string username, string hashedPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", hashedPassword);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                Debug.Log("response"+response);
                if (response.StartsWith("success"))
                {
                    feedbackText.text = "Connexion réussie!";
                    string[] parts = response.Split('|');
                    Debug.Log(parts);
                    UserSession.CurrentUserId = int.Parse(parts[1]); // Sauvegarder l'ID utilisateur
                    UserSession.CurrentUsername = username;
                    UserSession.CurrentUserLevel = int.Parse(parts[2]); // Sauvegarde du niveau utilisateur
                    UserSession.CurrentUserParty = int.Parse(parts[3]);
                    // Sauvegarde locale des infos de session
                    PlayerPrefs.SetInt("UserId", UserSession.CurrentUserId);
                    PlayerPrefs.SetString("Username", UserSession.CurrentUsername);
                    PlayerPrefs.SetInt("UserLevel", UserSession.CurrentUserLevel);
                    PlayerPrefs.SetInt("UserParty", UserSession.CurrentUserParty);
                    PlayerPrefs.Save(); // Sauvegarde immédiatement les données

                    Debug.Log("Session sauvegardée localement.");
                    
                    SceneManager.LoadScene("MenuPrincipal"); // Charger la scène "Stats"
                }
                else
                {
                    feedbackText.text = "Pseudo ou mot de passe invalide";
                }
            }
            else
            {
                feedbackText.text = "Erreur de connexion au serveur.";
            }
        }
    }
}
