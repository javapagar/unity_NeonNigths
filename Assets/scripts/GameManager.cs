using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    public GameObject startUI;
    public GameObject endUI;
    public GameObject startLevel;

    public int myLevel = 0;
    public Player player;

    public bool getControlOfPlayerAtStart = false;
    public bool resPawn=false;
    public Text scoreText;
    private TextMesh screenScore;
    private TextMesh screenSpeed;
    public static int score = 0;
    public Key key;//para gestionar la funcionalidad de la llave

    // Use this for initialization
    void Start()
    {
        //Used to be able to play directly from play when testing levels
        if (getControlOfPlayerAtStart)
        {
            player.hasControl = false;
            
        }
        //If we are at a start level
        if (startUI != null)
        {
            startUI.SetActive(true);
            
        }
        else
        {
            startGame();
           
        }
        //If we are at an end level
        if (endUI != null)
        {
            endUI.SetActive(false);
        }

        player.manager = this;
        key.manager = this;
        screenScore = GameObject.Find("Score").GetComponent< TextMesh >();
        screenSpeed = GameObject.Find("Speed").GetComponent<TextMesh>();
    }


    private void Update()
    {
       if (startLevel != null)
        {
            StartCoroutine(InitLevel());
        }
        screenScore.text = "Score :" + score;
        screenSpeed.text = "Speed :" + (int) Player.currentSpeed;

        if (resPawn)//aquí debería de colocar los elementos como al inicio
        {
            
            
            key.resPawn();//activo la llave
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    


    /// <summary>
    /// This is called from the UI button
    /// </summary>
    public void startGame()
    {
       
        if (startUI != null)
        {
            startUI.SetActive(false);
            score = 0;
        }

        player.hasControl = true;
    }

    /// <summary>
    /// This is called when the player ends a level
    /// </summary>
    public void endLevel()
    {
        if (endUI != null)
        {
            endUI.gameObject.SetActive(true);
            scoreText.text= "Your score: " + score;
        }
        else
        {
            loadLevel(myLevel + 1);
        }

    }
    protected IEnumerator InitLevel()
    {
        Debug.Log("level2");
        yield return new WaitForSeconds(2);
        startLevel.SetActive(false);
    }

        public void loadLevel(int level)
    {
        player.initLevel();
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
       
    }

    public void startAgain()
    {
        myLevel= 0;
        score = 0;
        loadLevel(myLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
