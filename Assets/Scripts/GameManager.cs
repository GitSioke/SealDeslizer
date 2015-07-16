using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public BoardManager boardScript;

    public int level = 3;

    public int playerFoodPoints = 0;

    [HideInInspector]
    public bool playersTurn = true;

    [HideInInspector]
    public int remainingFishOnBoard;

    [HideInInspector]
    public int remainingVisitedIceberg
    {   
        get
        {
            return remainingIceberg.Count;
        }
    }

    public List<Iceberg> remainingIceberg =  new List<Iceberg>();
     
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GameOver(bool goodEnd)
    {
        // If good end, the game end with a winning. Bad if not.

        enabled = false;

        if (goodEnd)
        {
            // do good finish
            //GUI.Label(new Rect(0, 0, 100, 50), "BLAH");
            GUIStyle style = new GUIStyle();
            style.richText = true;
            GUILayout.Label("<size=30>Some <color=yellow>RICH</color> text</size>", style);
        }
        else
        {
            // do a bad finish
        }
    }
    
    public void UpdateRemainingIceberg (Vector3 vector)
    { 
        remainingIceberg.RemoveAll(iceberg => iceberg.ContainsIceCube(vector));
    }
}
