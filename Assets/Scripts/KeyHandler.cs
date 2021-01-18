using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyHandler : MonoBehaviour
{

    public KeyCode key;
    private Button _button;
    private GameManager gameManager;
    public List<GameObject> surroundingKeys = new List<GameObject>();
    public bool onFire = false;

    private void Awake()
    {
        _button = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        _button.onClick.AddListener(delegate { OnKeyClicked(this.gameObject); });
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            OnKeyClicked(this.gameObject);
        }
    }

    public void OnKeyClicked(GameObject invoker)
    {
        if (gameManager.currentWater > 0)
        {
            GameObject _splash = Instantiate(gameManager.splash, this.transform);
            _splash.transform.SetParent(this.transform);

            if (invoker == this.gameObject)
            {
                gameManager.SprayWater();
                FindObjectOfType<AudioManager>().Play("Splash");
            }

            GameObject child = this.gameObject;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                child = this.transform.GetChild(i).gameObject;
                if (child.CompareTag("Fire"))
                {
                    break;
                }
                else
                {
                    child = this.gameObject;
                }
            }
            if (child != this.gameObject)
            {
                child.GetComponent<Fire>().Sprayed();
            }
        }
        else if (invoker == this.gameObject)
        {
            FindObjectOfType<AudioManager>().Play("Error");
        }
        
        if (gameManager.powerUp && invoker == this.gameObject)
        {
            for (int i = 0; i < surroundingKeys.Count; i++)
            {
                KeyHandler surroundingHandler = surroundingKeys[i].GetComponent<KeyHandler>();
                if (surroundingHandler != null)
                {
                    surroundingHandler.OnKeyClicked(this.gameObject);
                }
            }
        }
    }

    public void OnFire(bool flaming)
    {
        onFire = flaming;
        if (onFire)
        {
            GameObject flame = Instantiate(gameManager.fire, this.transform);
            flame.transform.SetParent(this.transform);
        }
    }
}
