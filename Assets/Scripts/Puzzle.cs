using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    private Transform emptySpace = null;

    [SerializeField]
    private Transform emptySpace2 = null;

    [SerializeField]
    private Transform emptySpace3 = null;

    [SerializeField]
    private float spaceBetween;
    [SerializeField]
    private Tiles[] tiles = null;
    private Camera _camera;

    [SerializeField]
    private int minNumber,maxNumber;
    [SerializeField]
    private GameObject gamePanel = null, newHighscoreText = null;
    [SerializeField]
    private Text timeLeft = null;

    [SerializeField]
    private bool _gameOver;

    private int emptySpaceId1 = 11,
    emptySpaceId2 = 12,
    emptySpaceId3 = 13;

    public ParticleSystem particleBurst;

    private void Start()
    {
        _camera = Camera.main;
        Mixer();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if(hit)
            {
                if(Vector2.Distance(emptySpace.position, hit.transform.position) < spaceBetween)
                {
                    Vector2 lastEmptySpacePosition = emptySpace.position;
                    Tiles thisTile = hit.transform.GetComponent<Tiles>();
                    emptySpace.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptySpacePosition;
                    int tileIndex = findIndex(thisTile);
                    tiles[emptySpaceId1] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceId1 = tileIndex;
                }

                if(Vector2.Distance(emptySpace2.position, hit.transform.position) < spaceBetween)
                {
                    Vector2 lastEmptySpacePosition2 = emptySpace2.position;
                    Tiles thisTile2 = hit.transform.GetComponent<Tiles>();
                    emptySpace2.position = thisTile2.targetPosition;
                    thisTile2.targetPosition = lastEmptySpacePosition2;
                    int tileIndex2 = findIndex(thisTile2);
                    tiles[emptySpaceId2] = tiles[tileIndex2];
                    tiles[tileIndex2] = null;
                    emptySpaceId2 = tileIndex2;
                }

                if(Vector2.Distance(emptySpace3.position, hit.transform.position) < spaceBetween)
                {
                    Vector2 lastEmptySpacePosition3 = emptySpace3.position;
                    Tiles thisTile3 = hit.transform.GetComponent<Tiles>();
                    emptySpace3.position = thisTile3.targetPosition;
                    thisTile3.targetPosition = lastEmptySpacePosition3;
                    int tileIndex3 = findIndex(thisTile3);
                    tiles[emptySpaceId3] = tiles[tileIndex3];
                    tiles[tileIndex3] = null;
                    emptySpaceId3 = tileIndex3;
                }
            }
        }
        if (!_gameOver)
        {
            int correctTiles = 0;
            foreach(var a in tiles)
            {
                if (a != null)
                {
                    if(a.inRightPlace)
                    {
                        correctTiles++;
                    }
                } 
            }

            if(correctTiles == tiles.Length - 10)
            {
                _gameOver = true;
                gamePanel.SetActive(true);
                particleBurst.Emit(100);
                var timeScript = GetComponent<Timer>();
                timeScript.StopTimer();
                timeLeft.text = timeScript.minutes + " : " + timeScript.seconds;
            }
        }
        
    }

    private void Mixer()
    {
        if (emptySpaceId1 != 11 && emptySpaceId2 != 12 && emptySpaceId3 != 13)
        {
            var tileOn11Pos = tiles[11].targetPosition;
            tiles[11].targetPosition = emptySpace.position;
            emptySpace.position = tileOn11Pos;
            tiles[emptySpaceId1] = tiles[11];
            tiles[11] = null;
            emptySpaceId1 = 11;

            var tileOn12Pos = tiles[12].targetPosition;
            tiles[12].targetPosition = emptySpace2.position;
            emptySpace2.position = tileOn12Pos;
            tiles[emptySpaceId2] = tiles[12];
            tiles[12] = null;
            emptySpaceId2 = 12;

            var tileOn13Pos = tiles[13].targetPosition;
            tiles[13].targetPosition = emptySpace3.position;
            emptySpace3.position = tileOn13Pos;
            tiles[emptySpaceId3] = tiles[13];
            tiles[13] = null;
            emptySpaceId3 = 13;
        }
        int invertion;
        do
        {
            for (int i = 0; i <= maxNumber; i++)
            {
                var lastPos = tiles[i].targetPosition;
                int randomIndex = Random.Range(minNumber, maxNumber);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;

                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;
            }
            invertion = GetInversion();
            Debug.Log("Tiles moved");
        } while (invertion % 2 != 0);
    }

    public int findIndex(Tiles ts)
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i] != null)
            {
                if(tiles[i] == ts)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public int GetInversion()
    {
        int inversionSum = 0;
        for(int i = 0; i< tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if(tiles[j] != null)
                {
                    if(tiles[i].position > tiles[j].position)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionSum += thisTileInvertion;
        }
        return inversionSum;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene("Test");
    }
}