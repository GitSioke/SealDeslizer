using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Assets.Scripts;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public List<Iceberg> icebergsInBoard;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }

    }

    public int columns = 6;
    public int rows = 6;
        
    // En caso de que queramos crear un numero aleatorio creamos un numero maximo y minimo.
    public Count wallCount = new Count(5,9);

    public GameObject[] iceTiles;
    public GameObject[] waterTiler;
    public GameObject[] enemyTiles;
    public GameObject[] outerWaterTiles;
    public GameObject _start;
    public GameObject _exit;
    public GameObject player;
    public GameObject[] fishTiles;

    public List<Iceberg> _icebergOnBoard = new List<Iceberg>();

    //Prefab to spawn for exit. 
    private Transform boardHolder;

    private List<Vector3> gridPositions = new List<Vector3>();
    private List<Vector3> icePositions = new List<Vector3>();
    private List<Vector3> fishPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
               
  
    }

    void BoardSetup()
    {
        SetFishPositions();

        boardHolder = new GameObject("Board").transform;
        for (int boardCol = -1; boardCol< columns +1; boardCol++)
        {
            for (int boardRow  = -1; boardRow < rows +1 ; boardRow++ )
            {
                Vector3 vector = new Vector3(boardCol, boardRow, 0f);
                GameObject gameObjToInstantiate = waterTiler[0];
                if(boardCol == -1 || boardCol == columns || boardRow == -1 || boardRow == rows)
                {
                    gameObjToInstantiate = outerWaterTiles[Random.Range(0, outerWaterTiles.Length)];

                }
                else
                {
                    foreach(Vector3 vectorIce in icePositions)
                    {
                        if (vector.Equals(vectorIce))
                        {
                            gameObjToInstantiate = iceTiles[Random.Range(0, iceTiles.Length)];
                            break;
                        }
                    }
                }
                GameObject instance = Instantiate(gameObjToInstantiate, 
                    vector, Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);

            }
        }
    }

    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        
        Instantiate(_exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
        _exit.transform.position = new Vector3(columns - 1, rows - 1, 0f);
        
        Instantiate(_start, new Vector3(0, 0, 0f), Quaternion.identity);
        _start.transform.position = new Vector3(0, 0, 0f);

        Instantiate(player, new Vector3(0, 0, 0f), Quaternion.identity);
        
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();
                
        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(iceTiles, wallCount.minimum, wallCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Instantiate the exit and start tile in the upper right hand corner of our game board
        

    }

    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SetFishPositions()
    {
        icePositions.Add(new Vector3(0, 2, 0f));
        icePositions.Add(new Vector3(0, 3, 0f));
        icePositions.Add(new Vector3(2, 0, 0f));
        icePositions.Add(new Vector3(2, 2, 0f));
        icePositions.Add(new Vector3(4, 1, 0f));
        icePositions.Add(new Vector3(4, 2, 0f));

        calculateIcebergs();

        // Place fish
        GameObject gameObjToInstantiate = fishTiles[0];
        
        GameObject instance = Instantiate(gameObjToInstantiate,
                    new Vector3(0, 3.5f, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(boardHolder);
        GameObject gameObjToInstantiate2 = fishTiles[0];

        GameObject instance2 = Instantiate(gameObjToInstantiate2,
                    new Vector3(4, 0.5f, 0f), Quaternion.identity) as GameObject;
        instance2.transform.SetParent(boardHolder);
        GameObject gameObjToInstantiate3 = fishTiles[0];

        GameObject instance3 = Instantiate(gameObjToInstantiate3,
                    new Vector3(1.5f, 2, 0f), Quaternion.identity) as GameObject;
        instance3.transform.SetParent(boardHolder);

        GameManager.instance.remainingFishOnBoard = 3 ;

    }

    private void calculateIcebergs()
    {
        foreach( Vector3 iceVector in icePositions)
        {
            IList<Iceberg> adjacents = new List<Iceberg>(); 
            foreach( Iceberg iceberg in _icebergOnBoard )
            {
                if (iceberg.LookUpAdjdacents(iceVector))
                {
                    adjacents.Add(iceberg);
                }               
            }
            if (adjacents.Count>1)
            {
                MeltIcebergs(adjacents);
            }
            else if (adjacents.Count == 1)
            {
                Iceberg iceberg = adjacents[0];
                iceberg.AddIceVector(iceVector);
            }
            else{

                Iceberg newIceberg = new Iceberg();
                newIceberg.AddIceVector(iceVector);
                _icebergOnBoard.Add(newIceberg);
                GameManager.instance.remainingIceberg.Add(newIceberg);
            }

        }
    }

    private void MeltIcebergs(IList<Iceberg> icebergsToMelt)
    {
        Iceberg meltedIceberg = new Iceberg();
        if (icebergsToMelt.Count > 1)
        {
            foreach (Iceberg iceberg in icebergsToMelt)
            {
                foreach (Vector3 iceVector in iceberg.Ice())
                {
                    meltedIceberg.AddIceVector(iceVector);
                }
            }
            _icebergOnBoard.RemoveAll(item => icebergsToMelt.Contains(item));
            GameManager.instance.remainingIceberg.RemoveAll(item => icebergsToMelt.Contains(item));
            _icebergOnBoard.Add(meltedIceberg);
            GameManager.instance.remainingIceberg.Add(meltedIceberg);
        }
        
    }

    
}
