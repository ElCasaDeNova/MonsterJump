using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text pieceText;

    // Update is called once per frame
    void Update()
    {
        UpdatepieceText();
        UpdatetimeText();
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
