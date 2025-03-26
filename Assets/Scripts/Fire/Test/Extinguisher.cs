using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    public ParticleSystem sprayParticles; // Jet d'extincteur
    public LayerMask fireLayer; // Cible : feu
    public Transform sprayOrigin; // Position de sortie du jet

    void Start()
    {
        // Désactiver le système de particules au début
        if (sprayParticles != null)
        {
            sprayParticles.Stop(); // Arrêter les particules au lancement de la scène
        }
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique gauche = Active le jet
        {
            sprayParticles.Play();
        }
        if (Input.GetMouseButtonUp(0)) // Relâche = Stop le jet
        {
            sprayParticles.Stop();
        }

        if (sprayParticles.isPlaying) // Si le jet est en cours
        {
            Debug.DrawLine(sprayOrigin.position, sprayOrigin.position + (Vector3)sprayOrigin.right * 5f, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(sprayOrigin.position, Vector2.up, 5f); // Vers le haut
            if (hit.collider != null)
            {
                //Debug.Log("🎯 Raycast a touché : " + hit.collider.name);
                FireController fire = hit.collider.GetComponent<FireController>();
                if (fire != null)
                {
                 //   fire.Extinguish(hit.point);
                }
            }
        }
    }
}