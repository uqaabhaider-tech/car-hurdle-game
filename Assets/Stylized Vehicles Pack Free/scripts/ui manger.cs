using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoaderByIndex : MonoBehaviour
{

    public static GameObject gameOverPanel;

    private void Awake()
    {

        gameOverPanel = GameObject.FindGameObjectWithTag("LostPanel");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    public void level1()
    {
       
        SceneManager.LoadScene(1);
    }
    public void level2()
    {

        SceneManager.LoadScene(2);
    }
    public void level3()
    {

        SceneManager.LoadScene(3);
    }
    public void level4()
    {

        SceneManager.LoadScene(4);
    }
    public void level5()
    {

        SceneManager.LoadScene(5);
    }
    public void level6()
    {

        SceneManager.LoadScene(6);
    }
    public void nextlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
   
    public void quit()
    {
        Application.Quit();
        
    }
}