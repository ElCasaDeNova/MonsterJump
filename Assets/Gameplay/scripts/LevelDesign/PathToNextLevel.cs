using UnityEngine;
using UnityEngine.SceneManagement;

public class PathToNextLevel : MonoBehaviour
{
    [SerializeField]
    private Chronometer chronometer;
    [SerializeField]
    private string nameOfScene;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering has the tag "Player"
        if (other.CompareTag("Player"))
        {
            SetScore();

            SendToNextLevel();
        }
    }

    private void SetScore() {
        chronometer.StopChronometer();
        Score.time = (int)chronometer.elapsedTime;
    }

    private void SendToNextLevel() {
        SceneLoader.nextScene = nameOfScene;
        SceneManager.LoadScene("LoadingScene");
    }
}
