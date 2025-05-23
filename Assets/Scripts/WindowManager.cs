using System.Collections;
using UnityEngine;

//temporary for now
public class WindowManager : MonoBehaviour
{
    [SerializeField] private GameObject[] windows;

    private void Start()
    {
        DelayedStart();
    }
    private IEnumerator DelayedStart()
    {
        yield return null;
        DisableAll();
    }
    private void DisableAll()
    {
        foreach(var window in windows)
        {
            window.SetActive(false);
        }
    }
    public void EnableWindow(GameObject windowToEnable)
    {
        DisableAll();
        windowToEnable.SetActive(true);
    }
}
