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
    private void Start()
    {
        //temp
        player = new User("Player", 0);
        return;
        for(int i = 0; i < 10; i++)
        {
            User user = new User($"user {i + 1}", i + 1);
            player.AddRelationToUser(user);
        }
    }
}
