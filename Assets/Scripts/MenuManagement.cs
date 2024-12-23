using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagement : MonoBehaviour
{
    public GameObject Panel;
    public void PlayAction() 
    {
        SceneManager.LoadScene(1);
    } 

    public void ExitAction() 
    {
        SceneManager.LoadScene(0);
    }

    public void QuitAction()
    {
        Application.Quit();
    }

    public void OpenPanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }
    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
    }    
}
