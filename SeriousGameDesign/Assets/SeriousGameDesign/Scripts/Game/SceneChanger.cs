using UnityEngine.SceneManagement;

public class SceneChanger
{
    public static void TransitionScene(string _nextScene)
    {
        SceneManager.LoadScene(_nextScene);   
    }
}
