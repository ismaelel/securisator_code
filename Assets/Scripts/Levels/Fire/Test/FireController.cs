using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FireController : MonoBehaviour
{
    public ParticleSystem fireParticles; // Feu
    public float fireIntensity = 15f; // Intensité du feu
    public float decreaseRate = 0.0005f; // Vitesse d'extinction si bien visé
    public float increaseRate = 1f; // Augmente si mal visé
    public Text intensityText; // Référence au texte UI pour afficher l'intensité
    public GameObject firePrinter;
    public GameObject afterPrinter;
    public GameObject outro;
    
    void Start()
    {
        afterPrinter.SetActive(false);
        outro.SetActive(false);
    }
    void Update()
    {  
        

        // Mise à jour du texte d'intensité
        if (intensityText != null)
        {
            intensityText.text = "Intensité du feu : " + fireIntensity.ToString("F1");
        }

        // Si l'intensité du feu atteint 0, on arrête les flammes et on détruit l'objet
        if (fireIntensity <= 0)
        {
            fireParticles.Stop(); // Éteindre le feu
            Destroy(gameObject, 2f); // Supprimer après 2s
            firePrinter.gameObject.SetActive(false);
            afterPrinter.gameObject.SetActive(true);
            // await Task.Delay(2000);
            Task.Delay(2000);
            // Met à jour le niveau de l'utilisateur
            outro.SetActive(true);
            // LevelManager levelManager = FindObjectOfType<LevelManager>();
            // if (levelManager == null)
            // {
            //     GameObject obj = new GameObject("LevelManager");
            //     levelManager = obj.AddComponent<LevelManager>();
            // }
            // levelManager.IncreaseUserLevel(PlayerPrefs.GetInt("UserId", -1)); // Remplace "1" par l'ID réel de l'utilisateur
            //
        }

        // 🔹 Gestion du TOUCHER sur mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Récupère le premier toucher
            Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchWorldPosition.z = 0; // Assure-toi que z = 0, car on travaille en 2D

           /// Debug.Log("📱 Touch détecté à " + touchWorldPosition);

            RaycastHit2D hit = Physics2D.Raycast(touchWorldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                FireController fire = hit.collider.GetComponent<FireController>();
                if (fire != null)
                {
                    // Debug.Log("MOUSE DOWN : touchWorldPosition " + touchWorldPosition);
                    fire.Extinguish(touchWorldPosition);
                }
            }
        }

        // 🔹 Gestion de la SOURIS sur PC
        if (Input.GetMouseButtonDown(0)) // Clic gauche
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0; // Assure-toi que z = 0 en 2D

            //Debug.Log("🖱️ Clic souris détecté à " + mouseWorldPosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                FireController fire = hit.collider.GetComponent<FireController>();
                if (fire != null)
                {
                   // Debug.Log("TU SAIS");
                    //fire.Extinguish(mouseWorldPosition);
                }
            }
        }
    }

    // Méthode appelée pour éteindre le feu (en fonction de la position touchée)
    public void Extinguish(Vector3 hitPoint)
    {
        //Debug.Log("🔥 Extincteur utilisé à la position : " + hitPoint);

        // Vérifie si on vise la base du feu
        if (hitPoint.y <= transform.position.y) // Bien visé (base du feu)
        {
            // Debug.Log(decreaseRate);
            fireIntensity -= decreaseRate * Time.deltaTime; // Réduit l'intensité
            //Debug.Log("✅ Feu réduit, intensité : " + fireIntensity);
            //Debug.Log("position" + hitPoint.y + " " + hitPoint.x + "et" + transform.position.y);
        }
        else // Mal visé (haut du feu)
        {
             fireIntensity += increaseRate * Time.deltaTime; // Augmente l'intensité
             // Debug.Log("❌ Feu augmente, intensité : " + fireIntensity);
        }
        // Limiter l'intensité du feu à une plage raisonnable
        fireIntensity = Mathf.Clamp(fireIntensity, 0f, 30f); // Limite l'intensité entre 0 et 10

        // Ajuste la taille du feu
        var main = fireParticles.main;
        main.startSize = Mathf.Clamp(fireIntensity, 0.2f, 5f);

        //main.startSize = Mathf.Lerp(0.2f, 2f, fireIntensity / 10f); // Taille maximale de 2 au lieu de 5
    }
}
