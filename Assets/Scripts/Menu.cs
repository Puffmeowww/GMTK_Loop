using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
        Debug.Log("start");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }

}
