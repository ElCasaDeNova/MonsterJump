using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required to work with UI components

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro killText;
    [SerializeField]
    private TextMeshPro pieceText;
    [SerializeField]
    private TextMeshPro timeText;

    void Start()
    {
        // Update the text at the start
        UpdatekillText();
        UpdatepieceText();
        UpdatetimeText();
    }

    private void UpdatekillText()
    {
        // Update the text with the value of the variable of kill
        killText.text = Score.killPoints.ToString();
    }

    private void UpdatepieceText()
    {
        // Update the text with the value of the variable of pieces collected
        pieceText.text = Score.piecePoints.ToString();
    }

    private void UpdatetimeText()
    {
        // Update the text with the value of the variable of the chronometer
        int minutes = Score.time / 60;
        int seconds = Score.time % 60;

        timeText.text = minutes + "m " + seconds + "s";
    }
}
