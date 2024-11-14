using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        GameManager.Instance.varExemple++;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
