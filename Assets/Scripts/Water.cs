using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    public Slider Slider;
    public float refillAmount = 3f;
    public float refillTime = 0.1f;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(Refill());
    }

    public void SprayWaterSlider(float amount)
    {
        Slider.value -= amount;
    }

    IEnumerator Refill()
    {
        while (true)
        {
            if (Slider.value < Slider.maxValue)
            {
                float futureValue = Slider.value + refillAmount;
                if (futureValue > Slider.maxValue)
                {
                    Slider.value = Slider.maxValue;
                    gameManager.currentWater = gameManager.maxWater;
                }
                else
                {
                    Slider.value += refillAmount;
                    gameManager.currentWater += refillAmount;
                }
            }
            yield return new WaitForSeconds(refillTime);
        }
    }

}
