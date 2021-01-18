using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private float health = 2;
    private GameManager gameManager;
    private float growTimer = 0.5f;
    public GameObject splash;
    private float _burnTime = 5f;
    private KeyHandler keyHandle;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        keyHandle = this.transform.parent.gameObject.GetComponent<KeyHandler>();
    }

    private void Start()
    {
        StartCoroutine(Burn());
        health = gameManager.fireHealth;
    }

    public void Sprayed()
    {
        Shrink();
        if (this != null)
        {
            StartCoroutine(grow());
        }
    }

    public void PutOut()
    {
        //GameObject _splash = Instantiate(splash, this.transform);
        //_splash.transform.SetParent(this.transform.parent.transform);
        gameManager.PutOut();
        gameManager.availableKeys.Add(this.transform.parent.gameObject);
        if (keyHandle != null)
        {
            keyHandle.OnFire(false);
        }
        Destroy(this.gameObject);
    }

    IEnumerator grow()
    {
        yield return new WaitForSeconds(growTimer);
        if (this != null)
        {
            health++;
            transform.localScale *= 1.5f;
        }
    }

    void Shrink()
    {
        health--;
        transform.localScale /= 1.5f;
        if (health <= 0)
        {
            PutOut();
        }
    }

    IEnumerator Burn()
    {
        while (gameManager.gameOn)
        {
            yield return new WaitForSeconds(_burnTime);
            gameManager.BurnBuilding();
        }
    }
}
