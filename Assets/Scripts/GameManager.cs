using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CardDatabase cardDatabase;
    public CardScanner cardScanner;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
