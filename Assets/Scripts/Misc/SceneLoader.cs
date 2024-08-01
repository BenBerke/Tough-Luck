using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }   

    public void ActivateAI(bool activate)
    {
        if (activate) PlayerPrefs.SetInt("ai", 1);
        else PlayerPrefs.SetInt("ai", -1);
    }
}
