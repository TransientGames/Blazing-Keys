using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Airlock : MonoBehaviour
{

    private GameManager gameManager;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        _button.onClick.AddListener(OnKeyClicked);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnKeyClicked();
        }
    }

    private void OnKeyClicked()
    {
        if (gameManager.restartable)
        {
            gameManager.Restart();
        }
        gameManager.Airlock();
    }
}
