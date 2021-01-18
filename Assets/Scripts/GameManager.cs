using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject[] keys;
    public List<GameObject> availableKeys = new List<GameObject>();
    public GameObject fire;
    private int _maxNumOfFires = 47;
    public int _numOfFires = 0;
    public float _fireIntervals = 0.7f;
    public float _firesPutOut;
    public bool gameOn = true;
    public float maxWater = 100f;
    public float currentWater;
    public Water waterSlider;
    private float sprayAmount = 10f;
    public Slider _healthSlider;
    private float _buildingHealth = 100f;
    public GameObject IntroText;
    public GameObject GoodJobText;
    public GameObject NotGoodText;
    public GameObject DoBetterText;
    public GameObject GameOverText;
    public GameObject AirlockTexthalf;
    public GameObject AirlockTextWhy;
    public GameObject AirlockTextClose;
    public GameObject restartText;
    public GameObject splash;
    public GameObject ftlParticle;
    public GameObject gameOverPanel;
    public Text spaceText;
    public List<GameObject> dashFlames = new List<GameObject>();
    public GameObject dash;
    public bool powerUp = true;
    private int _numofCrew = 600;
    private int _crewLeft;
    private int _crewEscaped = 0;
    private int _crewPerished = 0;
    public Text crewLeftText;
    public Text crewEscapedText;
    public Text crewPerishedText;
    private float _burnRate = 1f;
    public bool restartable = false;
    private bool onFire = false;
    private bool alertOn = false;
    public float fireHealth = 2f;

    void Start()
    {
        _crewLeft = _numofCrew;
        UpdateCrewLeftText();
        UpdateCrewEscapedText();
        UpdateCrewPerishedText();
        StartCoroutine(CrewBurning());
        for (int i = 0; i < keys.Length; i++)
        {
            availableKeys.Add(keys[i].gameObject);
        }
        StartCoroutine(SpawnFire());
        currentWater = maxWater;
        FindObjectOfType<AudioManager>().Stop("Survived");
        FindObjectOfType<AudioManager>().Stop("Died");
        FindObjectOfType<AudioManager>().Play("Bgm");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (restartable)
            {
                Restart();
            }
        }

        if (_numOfFires > 0 && onFire == false)
        {
            onFire = true;
            FindObjectOfType<AudioManager>().Play("Fire");
        }
        
        if (_numOfFires <= 0 && onFire == true)
        {
            onFire = false;
            FindObjectOfType<AudioManager>().Stop("Fire");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Airlock()
    {
        GameOver(2);
    }

    IEnumerator SpawnFire()
    {
        yield return new WaitForSeconds(3f);
        while (gameOn)
        {
            if (_numOfFires < _maxNumOfFires && availableKeys.Count > 0)
            {
                float fireLocation = Random.Range(0f, availableKeys.Count);
                GameObject key = availableKeys[(int)fireLocation];
                if (key != null) {
                    KeyHandler keyHandle = key.GetComponent<KeyHandler>();
                    if (keyHandle != null && keyHandle.onFire == false)
                    {
                        keyHandle.OnFire(true);
                    }
                    _numOfFires++;
                    availableKeys.Remove(key);
                }
            }
            yield return new WaitForSeconds(_fireIntervals);
        }
    }


    public void PutOut()
    {
        _numOfFires--;
        _firesPutOut++;
        switch (_firesPutOut)
        {
            case 10:
                _fireIntervals = 0.6f;
                break;
            case 20:
                _fireIntervals = 0.5f;
                break;
            case 50:
                _fireIntervals = 0.4f;
                break;
        }
    }

    public void SprayWater()
    {
        currentWater -= 10f;
        waterSlider.SprayWaterSlider(sprayAmount);
    }

    public void BurnBuilding()
    {
        if (gameOn)
        {
            _buildingHealth--;
            _healthSlider.value--;

            if (_buildingHealth <= 30f && alertOn == false)
            {
                alertOn = true;
                FindObjectOfType<AudioManager>().Play("Alert");
            }
            if (_buildingHealth <= 0f)
            {
                GameOver(1);
            }
        }
    }

    private void GameOver(int method)
    {
        gameOn = false;
        IntroText.SetActive(false);
        if (method == 1)
        {

            StartCoroutine(ShipDestruct());
            FindObjectOfType<AudioManager>().Play("Died");
            FindObjectOfType<AudioManager>().Stop("Bgm");
        }
        else if (method == 2)
        {

            _crewPerished += _crewLeft;
            _crewLeft = 0;
            UpdateCrewLeftText();
            UpdateCrewPerishedText();

            GameObject[] allFlames = GameObject.FindGameObjectsWithTag("Fire");
            if (allFlames.Length != 0)
            {
                foreach (GameObject flame in allFlames)
                {

                    FindObjectOfType<AudioManager>().Play("Airlock");

                    _numOfFires--;
                    _firesPutOut++;
                    Destroy(flame);
                }
            }

            if (_buildingHealth >= 60)
            {
                AirlockTextWhy.SetActive(true);
            }
            else if (_buildingHealth < 20)
            {
                AirlockTextClose.SetActive(true);
            }
            else
            {
                AirlockTexthalf.SetActive(true);
            }
            StartCoroutine(RestartText());
            FindObjectOfType<AudioManager>().Play("Survived");
            FindObjectOfType<AudioManager>().Stop("Bgm");
            if (alertOn)
            {
                FindObjectOfType<AudioManager>().Stop("Alert");
            }
        }
        else if (_crewPerished <= 200)
        {
            GoodJobText.SetActive(true);
            StartCoroutine(RestartText());
            FindObjectOfType<AudioManager>().Play("Survived");
            FindObjectOfType<AudioManager>().Stop("Bgm");
            if (alertOn)
            {
                FindObjectOfType<AudioManager>().Stop("Alert");
            }
        }
        else if (_crewPerished <= 300)
        {
            DoBetterText.SetActive(true);
            StartCoroutine(RestartText());
            FindObjectOfType<AudioManager>().Play("Survived");
            FindObjectOfType<AudioManager>().Stop("Bgm");
            if (alertOn)
            {
                FindObjectOfType<AudioManager>().Stop("Alert");
            }
        }
        else
        {
            NotGoodText.SetActive(true);
            StartCoroutine(RestartText());
            FindObjectOfType<AudioManager>().Play("Survived");
            FindObjectOfType<AudioManager>().Stop("Bgm");
            if (alertOn)
            {
                FindObjectOfType<AudioManager>().Stop("Alert");
            }
        }
    }

    IEnumerator RestartText()
    {
        yield return new WaitForSeconds(1f);
        restartText.SetActive(true);
        restartable = true;
        spaceText.text = "RESTART";
    }

    private void UpdateCrewLeftText()
    {
        crewLeftText.text = _crewLeft.ToString();
    }

    private void UpdateCrewEscapedText()
    {
        crewEscapedText.text = _crewEscaped.ToString();
    }

    private void UpdateCrewPerishedText()
    {
        crewPerishedText.text = _crewPerished.ToString();
    }

    private void KillCrew()
    {
        int deaths = (int)(_numOfFires / 2.5f);
        int possibleCrewLeft = _crewLeft - deaths;
        int absoluteDeaths = deaths;
        if (possibleCrewLeft < 0)
        {
            absoluteDeaths = _crewLeft;
            _crewLeft = 0;
        }
        else
        {
            _crewLeft -= deaths;
        }
        _crewPerished += absoluteDeaths;
        UpdateCrewLeftText();
        UpdateCrewPerishedText();
        if (_crewLeft == 0)
        {
            if (gameOn)
            {
                GameOver(0);
            }
        }
    }

    private void CrewEscape()
    {
        _crewLeft--;
        _crewEscaped++;
        UpdateCrewLeftText();
        UpdateCrewEscapedText();
        if (_crewLeft == 0)
        {
            if (gameOn)
            {
                GameOver(0);
            }
        }
    }

    IEnumerator CrewBurning()
    {
        while (gameOn)
        {
            yield return new WaitForSeconds(_burnRate);
            CrewEscape();
            CrewEscape();
            CrewEscape();
            CrewEscape();
            KillCrew();
        }
    }

    IEnumerator ShipDestruct()
    {
        ftlParticle.SetActive(true);
        gameOverPanel.SetActive(true);
        for (int i = 0; i < dashFlames.Count; i++)
        {
            FindObjectOfType<AudioManager>().Play("SmallExplosion");
            dashFlames[i].SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }
        FindObjectOfType<AudioManager>().Play("LargeExplosion");
        FindObjectOfType<AudioManager>().Stop("Alert");
        

        GameObject[] allFlames = GameObject.FindGameObjectsWithTag("Fire");
        if (allFlames.Length != 0)
        {
            foreach (GameObject flame in allFlames)
            {
                _numOfFires--;
                Destroy(flame);
            }
        }
        dash.SetActive(false);

        restartable = true;
    }
}
