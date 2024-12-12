using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // TODO For Score
    public float varExemple = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
