using TMPro;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "TutorialArea")
        {
            tutorialText.text = other.GetComponent<TutorialArea>().tutorialText;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        tutorialText.text = null; 
    }
}
