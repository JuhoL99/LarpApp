using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CardDatabase cardDatabase;
    public CardScanner cardScanner;
    public User player;
    [SerializeField] private bool generateNamesForScriptableObjects = false; //temporary

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        if(generateNamesForScriptableObjects) cardDatabase.NamesFromImageFile(); //remove l8r
    }
}
