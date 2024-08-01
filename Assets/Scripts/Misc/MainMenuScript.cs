
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject sfxCrossMark;

    private void Start()
    {
        if (PlayerPrefs.GetInt("sfx") == 1) sfxCrossMark.SetActive(false); 
        else sfxCrossMark.SetActive(true);
        
    }
    public void ChangeSFX()
    {
        if (PlayerPrefs.GetInt("sfx") == 1)
        {
            PlayerPrefs.SetInt("sfx", -1);
            sfxCrossMark.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("sfx", 1);
            sfxCrossMark.SetActive(false);
        }
    }
}
