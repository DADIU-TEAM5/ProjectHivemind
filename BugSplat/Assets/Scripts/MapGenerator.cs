using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{


    public FloatVariable CurrentLevelBudget;
    public IntVariable EnemySpawnerCount;


    public int Seed;

    public int Rings = 1;
    public IntVariable CurrentLevel;

    public ShopLevels Levels;

    public GameObject[] Hexagons;

    List<List<GameObject>> SortedHexagons;
    List<Tier> availableTiers;

    public GameObject[] CenterHexagons;

    public GameObjectVariable hexmapParent;

    public GameObject BaseHexagon;
    public GameObject EdgeWall;

    float _hexLength;
    float _hexHeight;
    Vector3 _lastHexPos = Vector3.zero;
    Vector3[] _vertices;

    bool _allTilesGenerated;

    bool _finishedGenerating = false;

    GameObject _Parent;

    List<GameObject> _hexagonsTiles;


    Mesh hexMesh;

    // Start is called before the first frame update
    void Start()
    {
        EnemySpawnerCount.Value = 0;

        CurrentLevelBudget.Value = Levels.LevelTierPicker[CurrentLevel.Value].budget;

        SortedHexagons = new List<List<GameObject>>();
        availableTiers = new List<Tier>();



        for (int i = 0; i < Hexagons.Length; i++)
        {
          Tier tier =   Hexagons[i].GetComponent<Hexagon>().difficultyLevel;
            if (!availableTiers.Contains(tier))
            {
                availableTiers.Add(tier);
            }
           int tierIndex = availableTiers.IndexOf(tier);
            if (SortedHexagons.Count < tierIndex + 1)
            {
                SortedHexagons.Add(new List<GameObject>());
            }

            SortedHexagons[tierIndex].Add(Hexagons[i]);

        }
        print("hexagons have been sorted"+ SortedHexagons.Count);



        if(hexmapParent.Value != null)
        {
            Destroy(hexmapParent.Value);
        }

        Random.InitState(Seed);

        Hexagon.mapGen = this;

        _Parent = new GameObject();
        _Parent.name = "Hex Map";

        _hexagonsTiles = new List<GameObject>();

        hexMesh = BaseHexagon.GetComponent<MeshFilter>().sharedMesh;

        _vertices = hexMesh.vertices;


        _hexLength = Mathf.Abs(_vertices[2].z) + Mathf.Abs(_vertices[4].z);
        _hexHeight = Mathf.Abs(_vertices[0].x) + Mathf.Abs(_vertices[5].x);

        _hexHeight *= BaseHexagon.transform.localScale.z;
        _hexLength *= BaseHexagon.transform.localScale.x;


        GameObject hex = Instantiate(getRandomCenterHexagon());
        Hexagon hexHex= hex.GetComponent<Hexagon>();
        hexHex.RotateTile(Random.Range(0, 5));
        hexHex.IsaccesibleFromMiddle = true;

        hex.name = "middle";
        hex.transform.position = Vector3.zero;

        hex.transform.parent = _Parent.transform;

        _hexagonsTiles.Add(hex);



        StartCoroutine(GeneratAllTheRings());

        StartCoroutine(RotateTilesToMakeMostPossibleConnections());

        hexmapParent.Value = _Parent;
    }

    IEnumerator RotateTilesToMakeMostPossibleConnections()
    {
        yield return new WaitUntil(() => _allTilesGenerated);

        RotateAllPossibleSolutions();
        for (int i = 0; i < _hexagonsTiles.Count; i++)
        {
            Hexagon hexScript = _hexagonsTiles[i].GetComponent<Hexagon>();
           if (!hexScript.IsaccesibleFromMiddle)
            {
                hexScript.OpenAndRotateNeighbour();
                RotateAllPossibleSolutions();
            }

        }

        _Parent.SetActive(false);
        _Parent.transform.Rotate(0, 90, 0);
        _Parent.SetActive(true);
        
        //print("Finished rotatin tiles");

        yield return null;
    }

    void RotateAllPossibleSolutions()
    {
        bool noSolution = false;
        while (!noSolution)
        {
            noSolution = true;



            for (int i = 0; i < _hexagonsTiles.Count; i++)
            {
                Hexagon hexagonScript = _hexagonsTiles[i].GetComponent<Hexagon>();
                if (hexagonScript.HasAMiddleConnectedNeighbourWithAnOpenSideTowardsThisTile() && !hexagonScript.IsaccesibleFromMiddle)
                {
                    noSolution = false;

                    hexagonScript.RotateToFitMiddleConnectedNeighbour();
                }
            }




        }
    }

    GameObject getHexagonBasedOnLevel()
    {
        Tier tier = Levels.LevelTierPicker[CurrentLevel.Value].ChooseTier();

        int index = availableTiers.IndexOf(tier);



        return SortedHexagons[index][Random.Range(0, SortedHexagons[index].Count)];
    }
    GameObject getRandomCenterHexagon()
    {
        return CenterHexagons[Random.Range(0, CenterHexagons.Length)];
    }


    public void GenrateAroundTile(GameObject tile)
    {
        StartCoroutine(GenrateRingAroundHex(tile));


        

        UpdateAllTheNeighbours();

    }

    IEnumerator GeneratAllTheRings()
    {

        for (int i = 0; i < Rings+1; i++)
        {
            int currentNumberOfTiles = _hexagonsTiles.Count;


            for (int j = 0; j < currentNumberOfTiles; j++)
            {
                if (i < Rings)
                {
                    StartCoroutine(GenrateRingAroundHex(_hexagonsTiles[j]));

                    yield return new WaitUntil(() => _finishedGenerating);

                    // yield return new WaitForSeconds(0.1f);

                    UpdateAllTheNeighbours();
                }
                else
                {
                   // GenrateWallsAroundTheTiles(_hexagonsTiles[j]);
                }
            }

        }
        

        _allTilesGenerated = true;
        yield return null;
    }

    void UpdateAllTheNeighbours()
    {
        for (int i = 0; i < _hexagonsTiles.Count; i++)
        {
            Hexagon hexScript = _hexagonsTiles[i].GetComponent<Hexagon>();

            hexScript.LabelNeighbourAccesibility();
            hexScript.GetNeighbours();
        }


       // yield return null;
    }

   /* void GenrateWallsAroundTheTiles(GameObject startHexagon)
    {
        GameObject hex;
        Hexagon hexagonEdge = startHexagon.GetComponent<Hexagon>();

        float multiplierLength = 0;
        float multiplierHeight = 0;
        for (int i = 0; i < _vertices.Length; i++)
        {

            if ((i - 2) % 3 == 0)
            {

                multiplierLength++;
            }
            if (i == 4)
            {
                multiplierLength -= 2;

            }
            if (i == 5)
            {
                multiplierLength -= 4;


            }

            if (i > 1)
            {
                multiplierHeight--;
            }
            if (i > 2)
            {
                multiplierHeight--;
            }
            if (i == 5)
            {
                multiplierHeight++;
            }


            if (hexagonEdge.Neighbours[i] != null)
            {
                continue;
            }





            hex = Instantiate(EdgeWall);


            
            

           


            Vector3 positionToPlaceHex = startHexagon.transform.position;

            float hexStepLength = _hexLength * 0.5f;
            float hexStepHeight = _hexHeight * 0.75f;




            positionToPlaceHex.z += hexStepLength * (1 - (i + multiplierLength) + 1);


            positionToPlaceHex.x += hexStepHeight * (i + multiplierHeight);

            hex.name = "wall";
            hex.transform.position = positionToPlaceHex;


        }
    }
    */

    IEnumerator GenrateRingAroundHex(GameObject startHexagon)
    {
        GameObject hex;
        Hexagon hexagonEdge = startHexagon.GetComponent<Hexagon>();

        float multiplierLength = 0;
        float multiplierHeight = 0;
        for (int i = 0; i < _vertices.Length; i++)
        {

            if ((i - 2) % 3 == 0)
            {

                multiplierLength++;
            }
            if (i == 4)
            {
                multiplierLength -= 2;

            }
            if (i == 5)
            {
                multiplierLength -= 4;


            }

            if (i > 1)
            {
                multiplierHeight--;
            }
            if (i > 2)
            {
                multiplierHeight--;
            }
            if (i == 5)
            {
                multiplierHeight++;
            }


            if (hexagonEdge.Neighbours[i] != null)
            {
                continue;
            }

            

            

            hex = Instantiate(getHexagonBasedOnLevel());

            
            _hexagonsTiles.Add(hex);
            Hexagon hexagonScript = hex.GetComponent<Hexagon>();

            hexagonScript.RotateTile(Random.Range(0, 5)); 

            hexagonEdge.Neighbours[i] = hexagonScript;

            hexagonScript.Neighbours[(i + 3) % 6] = startHexagon.GetComponent<Hexagon>();
              

            Vector3 positionToPlaceHex = startHexagon.transform.position;

            float hexStepLength = _hexLength * 0.5f;
            float hexStepHeight = _hexHeight * 0.75f;


            

            positionToPlaceHex.z += hexStepLength * (1 - (i + multiplierLength) + 1);


            positionToPlaceHex.x += hexStepHeight * (i + multiplierHeight);

            hex.name = "edge " + i;
            hex.transform.position = positionToPlaceHex;

            hex.transform.parent = _Parent.transform;
        }
        _finishedGenerating = true;
        yield return null;

    }


    
}
