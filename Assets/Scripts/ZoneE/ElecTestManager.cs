using UnityEngine;
using UnityEngine.UI;
public class ElecTestManager : MonoBehaviour
{
    public Text timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
