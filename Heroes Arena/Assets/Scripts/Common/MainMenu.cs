using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public InputField PlayerNameField;
    public string PlayerName = "";

	private void Start ()
	{
	    MainMenu[] menus = FindObjectsOfType<MainMenu>();
	    if (menus.Length > 1)
	    {
	        for (int i = menus.Length - 1; i >= 0; --i)
	        {
	            MainMenu menu = menus[i];
	            if (menu.PlayerName != "")
	            {
	                PlayerNameField.text = menu.PlayerName;
                    Destroy(menu.gameObject);
	                break;
	            }
	        }
	    }
	    DontDestroyOnLoad(this);
	}

    public void OnStartClick()
    {
        if (PlayerNameField.text == "")
        {
            string[] names ={"Arngrim", "Bjorn", "Einherjar", "Guomundr", "Hrothgar", "Ingvar", "Jonark", "Kjarr","Niohad", "Orvar", "Palnatoke", "Ragnar", "Sigmund", "Volsung", "Weohstan", "Yrsa"};
            PlayerNameField.text = names[Random.Range(0, names.Length)];
        }
        PlayerName = PlayerNameField.text;
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
