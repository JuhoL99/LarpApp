using UnityEngine;

public class TestListener : MonoBehaviour
{
    [SerializeField] TestSender sender;
    private void Start()
    {
        sender.testEvent.AddListener(Test);
    }
    private void Test()
    {
        Debug.Log("received");
    }
}
