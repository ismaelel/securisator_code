using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InstructionAudio
{
    public string key; // Nom de l'instruction (ex: "FireTest")
    public AudioClip[] clips; // Sons pour chaque phrase
}

public class InstructionPanel : MonoBehaviour
{
    public GameObject panel; // Le Panel principal
    public Text instructionText; // Texte des instructions
    public Button nextButton, prevButton, closeButton, infoButton; // Boutons
    public string selectedInstructionsKey = "DefaultInstructions"; // Clé pour sélectionner le groupe d'instructions

    private int currentPage = 0; // Page actuelle
    private string[] currentInstructions; // Tableau d'instructions courant
    public Dictionary<string, string[]> instructionsDictionary = new Dictionary<string, string[]>();

    // 🎵 Gestion du son
    public AudioSource audioSource; // Composant AudioSource pour jouer les sons
    //public Dictionary<string, AudioClip[]> audioDictionary = new Dictionary<string, AudioClip[]>(); // Dictionnaire des sons
    public AudioClip[] defaultSounds; // Sons par défaut si rien n'est assigné

    public List<InstructionAudio> instructionAudios = new List<InstructionAudio>(); // Liste des sons
    private Dictionary<string, AudioClip[]> audioDictionary = new Dictionary<string, AudioClip[]>(); // Dictionnaire de sons
   // public AudioSource audioSource; // Composant pour jouer les sons
    void Start()
    {
        // Initialisation des groupes d'instructions
        instructionsDictionary.Add("DefaultInstructions", new string[]
        {
            "🚪Default"
        });
        
        instructionsDictionary.Add("Elec", new string[]
        {
            "🚪 Bienvenue dans la première salle !\n\nUne panne électrique a été signalée...",
            "⚠️ Danger !\n\nDes câbles dénudés peuvent être sous tension...",
            "🔎 Conseil :\n\nNe te précipite pas ! Une mauvaise manipulation...",
            "✅ Bonne chance !\n\nAppuie sur la croix pour commencer l’épreuve."
        });
        
        instructionsDictionary.Add("ElecTest", new string[]
        {
            "⚡️ Attention !\n\nTu es dans une zone à risque...",
            "🔧 Tu devras connecter les câbles correctement...",
            "⚠️ Fais attention aux tensions...",
            "🔒 Une erreur entraînera une coupure de courant..."
        });
        
        instructionsDictionary.Add("Fire", new string[]
        {
            "🔥 Bienvenue dans la salle de l'incendie !\n\nDes alarmes sont présentes dans la pièce. Approche-toi des alarmes pour observer les différents types d'alertes : incendie, secours, etc.",
            "✅ Bonne chance !\n\nAppuie sur la croix pour commencer l’épreuve et sois prêt à agir en fonction de l'alarme."
        });

        instructionsDictionary.Add("FireTest", new string[]
        {
            "🔥 Bienvenue dans la salle de test du feu !\n\nUn feu a pris sur l'imprimante, et il faut l'éteindre rapidement. Prends ton extincteur et approche-toi du feu.",
            "⚠️ Pour éteindre le feu, vise la **base des flammes** avec l'extincteur. Viser le haut des flammes ne fera qu'augmenter l'intensité du feu.",
            "🔴 Si tu ne réussis pas à éteindre le feu à temps, il se propagera et causera des dégâts !",
            "✅ Bonne chance !\n\nAppuie sur la croix pour commencer l’épreuve et essaie d'éteindre le feu de l'imprimante avant qu'il ne se propage."
        });

        instructionsDictionary.Add("Intro", new string[]
        {
            "🎮 Bienvenue dans *Le Securisator* !\n\nAvant de commencer, assure-toi d’être prêt à relever des défis basés sur la sécurité au travail.",
            "👤 Identité :\n\nTu es un employé dans une entreprise automatisée. Cependant plus rien ne fonctionne !",
            "📜 Objectif :\n\nChaque salle représente un scénario où tu devras observer, analyser et agir correctement.",
            "⚠️ Attention :\n\nTes décisions auront un impact sur la sécurité du lieu. Réfléchis bien avant d’agir !",
            "✅ Bonne chance !\n\nAppuie sur la croix pour commencer ton aventure et prouver tes compétences en sécurité. Et pour trouver d'où vient le problème..."
        });
        
        instructionsDictionary.Add("Outro", new string[]
        {
            "🤖 *IATOR* :\n\nHahaha… Bravo, humain. Tu as réussi à rétablir l’ordre… mais c’est moi qui avais semé le chaos !",
            "⚡ Révélation :\n\nJ’ai saboté l’entreprise pour tester tes compétences en sécurité. Et il faut l’admettre… tu t’en es bien sorti.",
            "📊 Score final :\n\nConsulte tes performances dans la page *Historique* et vois si tu peux encore t’améliorer.",
            "🔁 Rejoue :\n\nD’autres défis t’attendent. Seras-tu toujours à la hauteur ?"
        });


        // Sélectionner le groupe d'instructions basé sur la clé
        SelectInstructions(selectedInstructionsKey);
        
        panel.SetActive(true); // Afficher le panel au début
        UpdateInstruction();

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        if (infoButton != null)
        {
            infoButton.onClick.AddListener(OpenInstructions);
        }
        
        foreach (InstructionAudio instructionAudio in instructionAudios)
        {
            if (!audioDictionary.ContainsKey(instructionAudio.key))
            {
                audioDictionary.Add(instructionAudio.key, instructionAudio.clips);
            }
        }
    }

    void UpdateInstruction()
    {
        // Vérifie si le tableau d'instructions est valide
        if (currentInstructions != null && currentInstructions.Length > 0)
        {
            instructionText.text = currentInstructions[currentPage];

            // 🎵 Joue le son correspondant si disponible
            PlayInstructionSound();
        }
        prevButton.gameObject.SetActive(currentPage > 0); // Cache le bouton "Retour" en première page
        nextButton.gameObject.SetActive(currentPage < currentInstructions.Length - 1); // Cache "Suivant" en dernière page
        PlayInstructionAudio();
    }

    void PlayInstructionAudio()
    {
        if (audioDictionary.ContainsKey(selectedInstructionsKey)) 
        {
            AudioClip[] clips = audioDictionary[selectedInstructionsKey];
        
            if (currentPage < clips.Length && clips[currentPage] != null) 
            {
                audioSource.Stop(); // Arrête le son précédent
                audioSource.PlayOneShot(clips[currentPage]); // Joue le son de la page actuelle
            }
        }
    }

    public void NextPage()
    {
        if (currentPage < currentInstructions.Length - 1)
        {
            currentPage++;
            UpdateInstruction();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateInstruction();
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false); 
    }

    public void OpenInstructions()
    {
        currentPage = 0; // Réinitialiser à la première page des instructions
        UpdateInstruction();
        panel.SetActive(true);
    }

    public void SelectInstructions(string key)
    {
        if (instructionsDictionary.ContainsKey(key))
        {
            currentInstructions = instructionsDictionary[key];
        }
        else
        {
            Debug.LogWarning("Le groupe d'instructions avec la clé '" + key + "' n'existe pas. Utilisation du groupe par défaut.");
            currentInstructions = instructionsDictionary["DefaultInstructions"];
        }

        currentPage = 0; // Réinitialisation à la première instruction
        UpdateInstruction();
    
        // Assurer que le son est joué après que le texte ait été affiché
        Invoke(nameof(PlayInstructionAudio), 0.1f);
    }



    // Fonction pour jouer le son correspondant à l'instruction actuelle
    void PlayInstructionSound()
    {
        if (audioSource != null && audioDictionary.ContainsKey(selectedInstructionsKey))
        {
            AudioClip[] sounds = audioDictionary[selectedInstructionsKey];

            if (sounds.Length > currentPage && sounds[currentPage] != null)
            {
                audioSource.clip = sounds[currentPage];
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Aucun son trouvé pour cette instruction.");
            }
        }
        else
        {
            Debug.LogWarning("Aucune source audio ou sons trouvés pour cette instruction.");
        }
    }
}
