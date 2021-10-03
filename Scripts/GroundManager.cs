using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundManager : MonoBehaviour
{
    public GameObject tile_prefab;
    public GameObject enemy_prefab;

    GameObject[] tiles;
    public List<GameObject> tile_list;
    float timer;
    float timerB;
    float timerC;
    public float dropInterval;
    public int groundWidth;
    public int groundLength;
    public int enemyCount;

    private void Awake()
    {
       
        for (int i = -groundWidth; i < groundWidth; i++)
        {
            for (int j = -groundLength; j < groundLength; j++)
            {
                GameObject tile = GameObject.Instantiate(tile_prefab, new Vector3(i, 0, j), Quaternion.identity,transform);
                tile_list.Add(tile);
            }
        }

        timerB = 2.5f;
        timerC = 4f;
        //dropInterval = 5;


        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = GameObject.Instantiate(enemy_prefab, new Vector3(Random.Range(-1.0f,1.0f) * groundWidth*0.8f , 1, Random.Range(-1.0f,1.0f) * groundLength * 0.8f), Quaternion.identity);
        }


    }

  
    private void Update()
    {
        timer += Time.deltaTime;
        timerB += Time.deltaTime;
        timerC += Time.deltaTime;
        if (timer > dropInterval)
        {
            timer = 0;
            //DropOutLineTiles();
            GroundCollapse();
            if (dropInterval > 2f)
            {
                dropInterval -= 0.2f;
            }

        }

        if (timerB  > dropInterval)
        {
            timerB = 0;
            DropOutLineTiles();
        }

        if (timerC > dropInterval)
        {
            timerC = 0;
            GroundCollapseRandom();
        }

    }

    void GroundCollapse()
    {
        float f = Random.Range(0.0f, 1.0f);
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        if (f < 0.25f)
        {
            float i = groundWidth;
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.x <= i)
                {
                    i = tile.transform.position.x;
                }
            }
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.x <=i)
                {
                    tile.GetComponent<Tile>().Drop(tile.transform.position.z*0.1f + 1);

                }
            }


        }
        else if (f >= 0.25f && f < 0.5f)
        {
            float i = -groundWidth;
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.x >= i)
                {
                    i = tile.transform.position.x;
                }
            }
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.x >= i)
                {
                    tile.GetComponent<Tile>().Drop(tile.transform.position.z * 0.1f + 1);

                }
            }
        }
        else if (f >= 0.5f && f < 0.75f)
        {
            float j = -groundLength;
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.z >= j)
                {
                    j = tile.transform.position.z;
                }
            }
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.z >= j)
                {
                    tile.GetComponent<Tile>().Drop(tile.transform.position.x * 0.1f + 1);

                }
            }
        }
        else if (f >= 0.75f)
        {
            float j = groundLength;
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.z <= j)
                {
                    j = tile.transform.position.z;
                }
            }
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position.z <= j)
                {
                    tile.GetComponent<Tile>().Drop(tile.transform.position.x * 0.1f + 1);

                }
            }
        }

    }

    void DropOutLineTiles()
    {
        GameObject [] tiles = GameObject.FindGameObjectsWithTag("Tile");
        List<GameObject> outLineTiles = new List<GameObject>();

        float i = -groundWidth;
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.x >=i)
            {
                i = tile.transform.position.x;
            }
        }
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.x >=i)
            {
                outLineTiles.Add(tile);
            }
        }

        for (int j = 0; j < 4; j++)
        {
            int f = Random.Range(0, outLineTiles.Count);
            outLineTiles[f].GetComponent<Tile>().Drop(outLineTiles[f].transform.position.z * 0.1f +1);
        }
        /////
        outLineTiles = new List<GameObject>();
        i = groundWidth;
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.x <= i)
            {
                i = tile.transform.position.x;
            }
        }
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.x <= i)
            {
                outLineTiles.Add(tile);
            }
        }

        for (int j = 0; j < 4; j++)
        {
            int f = Random.Range(0, outLineTiles.Count);
            outLineTiles[f].GetComponent<Tile>().Drop(outLineTiles[f].transform.position.z * 0.1f + 1);
        }

        outLineTiles = new List<GameObject>();
        i = -groundLength;
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.z >= i)
            {
                i = tile.transform.position.z;
            }
        }
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.z >= i)
            {
                outLineTiles.Add(tile);
            }
        }

        for (int j = 0; j < 4; j++)
        {
            int f = Random.Range(0, outLineTiles.Count);
            outLineTiles[f].GetComponent<Tile>().Drop(outLineTiles[f].transform.position.x * 0.1f + 1);
        }

        outLineTiles = new List<GameObject>();
        i = groundLength;
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.z <= i)
            {
                i = tile.transform.position.z;
            }
        }
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.z <= i)
            {
                outLineTiles.Add(tile);
            }
        }

        for (int j = 0; j < 4; j++)
        {
            int f = Random.Range(0, outLineTiles.Count);
            outLineTiles[f].GetComponent<Tile>().Drop(outLineTiles[f].transform.position.x * 0.1f + 1);
        }

    }


    void GroundCollapseRandom()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < 2; i++)
        {
            int f = Random.Range(0, tiles.Length);
            tiles[f].GetComponent<Tile>().Drop(Random.Range(0.0f,1.0f));
        }


    }
}
