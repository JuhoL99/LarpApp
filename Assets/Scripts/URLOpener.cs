using UnityEngine;

public class URLOpener : MonoBehaviour
{
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void OpenFile(string pdfPath)
    {
        if (System.IO.File.Exists(pdfPath))
        {
            Application.OpenURL("file://" + pdfPath);
        }
        else
        {
            Debug.LogError("PDF file not found: " + pdfPath);
        }
    }
}