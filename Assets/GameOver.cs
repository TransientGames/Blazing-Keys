using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        if (Input.touchCount > 0 && _gameManager.restartable)
        {
            Restart();
        }
        if (Input.GetMouseButtonUp(0) && _gameManager.restartable)
        {
            Restart();
        }
    }

    private void Restart()
    {
        _gameManager.Restart();
    }
}
