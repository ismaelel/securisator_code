using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegistrationManager : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField UsernameInput; 
    public InputField PasswordInput; 
    public Button RegisterButton; 
    public Text FeedbackText; 

    [Header("Server URL")]
    public string registerUrl = GlobalConfig.ServerURL + "register_user.php";

    private void Start()
    {
        RegisterButton.onClick.AddListener(RegisterUser);
    }

    private void RegisterUser()
    {
        Debug.Log("Login " + UsernameInput.text);
        Debug.Log("Password " + PasswordInput.text);
        // On vérifie que les champs ne sont pas vides
        if (string.IsNullOrEmpty(UsernameInput.text) || string.IsNullOrEmpty(PasswordInput.text))
        {
            FeedbackText.text = "Veuillez remplir tous les champs.";
            return;
        }

        // Hachage du mot de passe
        string hashedPassword = HashPassword(PasswordInput.text);
        
        Debug.Log("debut coroutine");
        // Envoi des données au serveur
        StartCoroutine(SendRegistrationData(UsernameInput.text, hashedPassword));
    }

    private string HashPassword(string password)
    {
        // Hacher le mot de passe avec SHA256
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

    IEnumerator SendRegistrationData(string username, string hashedPassword)
    {
        Debug.Log("DANS LA COROUTINE " + username + " : " + hashedPassword);
        // Préparer les données pour l'envoi
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", hashedPassword);
            
        using (UnityWebRequest request = UnityWebRequest.Post(registerUrl, form))
        {
            Debug.Log("AVANT RETURN");
            yield return request.SendWebRequest();
            Debug.Log("APRES RETURN");
            Debug.Log("request.result " + request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene("SignIn"); 
                FeedbackText.text = "pas d'erreur ";
                Debug.Log("PAS D'ERREUR");
            }
            else
            {
                FeedbackText.text = "Erreur lors de l'inscription: " + request.error;
            }
        }
    }
}
