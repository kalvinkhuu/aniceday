using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button[] buttons;
    int buttonIndex = 0;

    private bool IgnoreFirstUpdate = true;
    private float IgnoreTimer = 0.0f;

    void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    void Start()
    {
        BackToMainMenu();
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(true);
        buttonIndex = 0;
    }

    public void BackToMainMenuFromCredits()
    {
        gameObject.SetActive(true);
        buttonIndex = 2;
    }

    public void BackToMainMenuFromTutorial() 
    {
        gameObject.SetActive(true);
        buttonIndex = 1;
    }

    void Update()
    {
        if (IgnoreFirstUpdate)
        {
            IgnoreTimer += Time.deltaTime;
            if (IgnoreTimer > 0.25f)
            {
                buttons[buttonIndex].Select();
                IgnoreFirstUpdate = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (buttonIndex < buttons.Length - 1)
            {
                buttonIndex++;
                buttons[buttonIndex].Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (buttonIndex > 0)
            {
                buttonIndex--;
                buttons[buttonIndex].Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            buttons[buttonIndex].onClick.Invoke();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}