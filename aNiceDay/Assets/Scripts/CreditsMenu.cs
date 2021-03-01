using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour
{
    Button BackButton;
    private bool IgnoreFirstUpdate = true;
    private float IgnoreTimer = 0.0f;

    void Awake()
    {
        BackButton = GetComponentInChildren<Button>();
    }

    void Start()
    {
    }

    public void GotoCreditsMenu()
    {
        gameObject.SetActive(true);
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
                BackButton.Select();
                IgnoreFirstUpdate = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton.onClick.Invoke();
        }

        //Vector2 move = gamepad.leftStick.ReadValue();
    }
}
