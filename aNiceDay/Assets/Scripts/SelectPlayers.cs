using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPlayers : MonoBehaviour
{
    int buttonIndex = 0;

    Button[] buttons;
    public Button SecretBackButton;
    private float IgnoreTimer = 0.0f;
    private bool IgnoreFirstUpdate = true;
    void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void GotoSelectMenu()
    {
        gameObject.SetActive(true);
        buttonIndex = 0;
        IgnoreFirstUpdate = true;
        IgnoreTimer = 0.0f;
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
        else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            buttons[buttonIndex].onClick.Invoke();
        }
    }
}
