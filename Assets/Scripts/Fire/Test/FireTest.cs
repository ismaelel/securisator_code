using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FireTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text timer;
    void Start()
    {
        timer.text = GameTimer.Instance.getTime();

    }

    // Update is called once per frame
    void Update()
    {
        timer.text = GameTimer.Instance.getTime();
    }
}
