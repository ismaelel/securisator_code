using UnityEngine;
using UnityEngine.UI;

public class Meca : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;  // Texte de la question
        public string[] answers;     // Liste des réponses (A, B, C)
        public int correctAnswerIndex;  // Index de la bonne réponse (0 pour A, 1 pour B, etc.)
       
    }

    public Text timer;
    public Question[] questions;  // Liste de toutes les questions
    private int currentQuestionIndex = 0;  // Index de la question courante
    private int currentScore = 0;
    public Text questionText;  // Texte de la question à afficher
    public Button buttonA;     // Bouton A pour la réponse
    public Button buttonB;     // Bouton B pour la réponse
    public Button buttonC;     // Bouton C pour la réponse
    public Button buttonEnd;
    public Text answerFeedbackText;  // Texte de retour pour la réponse (correcte ou incorrecte)
    public Text score;
    void Start()
    {
        timer.text = GameTimer.Instance.getTime().ToString();
        // GameObject gameManager = GameObject.Find("GameManagerQCM");
        //
        // if (gameManager != null)
        // {
        //     Debug.Log("✅ GameManager du QCM trouvé !");
        // }
        // else
        // {
        //     Debug.LogError("⚠️ GameManager du QCM introuvable !");
        // }
        //
        // if (gameManager == null)
        // {
        //     Debug.LogWarning("⚠️ GameManager du QCM introuvable, création d’un nouveau !");
        //     gameManager = new GameObject("GameManagerQCM");
        //     gameManager.AddComponent<Meca>();  // Ajoute le script manquant
        // }
        Debug.Log("START QCM");
        ShowQuestion();
        GameTimer.Instance.StartTimer();
        buttonEnd.gameObject.SetActive(false);
        
    }
    
    void Update()
    {
        timer.text = GameTimer.Instance.getTime();
    }
    void ShowQuestion()
    {
        Debug.Log("currentQuestionIndex" + currentQuestionIndex);
        Debug.Log(" : question.length " + questions.Length);
        if (currentQuestionIndex 
            < questions.Length)
        {
            // Afficher la question
            questionText.text = questions[currentQuestionIndex].questionText;

            // Afficher les réponses
            buttonA.GetComponentInChildren<Text>().text = questions[currentQuestionIndex].answers[0];
            buttonB.GetComponentInChildren<Text>().text = questions[currentQuestionIndex].answers[1];
            buttonC.GetComponentInChildren<Text>().text = questions[currentQuestionIndex].answers[2];

            // Associer les actions aux boutons
            buttonA.onClick.RemoveAllListeners();
            buttonB.onClick.RemoveAllListeners();
            buttonC.onClick.RemoveAllListeners();

            buttonA.onClick.AddListener(() => CheckAnswer(0));
            buttonB.onClick.AddListener(() => CheckAnswer(1));
            buttonC.onClick.AddListener(() => CheckAnswer(2));
        }
        else
        {
            EndQuiz();  // Fin du quiz
        }
    }

    void CheckAnswer(int chosenAnswerIndex)
    {
        if (chosenAnswerIndex == questions[currentQuestionIndex].correctAnswerIndex)
        {
            answerFeedbackText.text = "Bonne réponse !";
            currentScore++;
            score.text = currentScore.ToString();
            answerFeedbackText.color = Color.green;
        }
        else
        {
            answerFeedbackText.text = "Mauvaise réponse ! Pénalité de 10 secondes !";
            answerFeedbackText.color = Color.red;
            GameTimer.Instance.AddPenalty(10f);
        }

        // Passer à la question suivante après un délai
        currentQuestionIndex++;
        Invoke("ShowQuestion", 1f);  // Attendre 2 secondes avant de passer à la question suivante
    }

    void EndQuiz()
    {
        answerFeedbackText.text = "Quiz terminé !";
        buttonA.gameObject.SetActive(false);
        buttonB.gameObject.SetActive(false);
        buttonC.gameObject.SetActive(false);
        questionText.gameObject.SetActive(false);
        buttonEnd.gameObject.SetActive(true);

        // Ajouter ici la logique pour terminer le jeu ou avancer dans le scénario
    }
}
