using UnityEngine;
using UnityEngine.UI;
public class Intro : MonoBehaviour
{
    public GameObject continueButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cross()
    {
        continueButton.SetActive(true);
    }
}
