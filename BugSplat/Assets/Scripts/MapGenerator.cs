using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public IntVariable EnemySpawnerCount;

    public GameEvent DoneGenerating;
    public GameEvent GauntletEvent;
    public GameEvent WaveEvent;

    public bool UseRandomSeed;
    public int Seed;

    public int Rings = 1;
    public IntVariable CurrentLevel;

    public ShopLevels Levels;

    //public bool IsGauntlet;

    public GameObject[] Hexagons;

    public GameObject[] GauntletHexagons;

    List<List<GameObject>> SortedHexagons;
    List<Tier> availableTiers;

    public GameObject[] CenterHexagons;
    public GameObject[] GauntletCenterHexagons;

    public GameObjectVariable hexmapParent;

    public GameObject BaseHexagon;
    public GameObject EdgeWall;

    [HideInInspector]
    public float HexLength;
    [HideInInspector]
    public float HexHeight;

    Vector3 _lastHexPos = Vector3.zero;

    [HideInInspector]
    public Vector3[] Vertices;

    bool _allTilesGenerated;

    bool _finishedGenerating = false;

    GameObject _Parent;

    List<GameObject> _hexagonsTiles;


    Mesh hexMesh;

    public GameObject WallConnector;
    public GameObject SideWallConnector;


    public bool DontGenerate;

    // Start is called before the first frame update
    void Start()
    {
            var isGauntlet = Levels.LevelTierPicker[CurrentLevel.Value].IsGauntlet;
            if (isGauntlet)
            {
                Hexagons = GauntletHexagons;
                CenterHexagons = GauntletCenterHexagons;
            EnemySpawner.IsWave = false;
        }
        else
        {
            EnemySpawner.IsWave = true;
            
        }


            EnemySpawnerCount.Value = 0;
           // Debug.Log("THE MAPGEN SET UP THE VALUES!!");
            EnemySpawner.LevelBudget = Levels.LevelTierPicker[CurrentLevel.Value].budget;

        int[] MakeSureItsNotARef = new int[Levels.LevelTierPicker[CurrentLevel.Value].WaveBudgets.Length];

        for (int i = 0; i < MakeSureItsNotARef.Length; i++)
        {
            MakeSureItsNotARef[i] = Levels.LevelTierPicker[CurrentLevel.Value].WaveBudgets[i];
        }
            
            
            
            EnemySpawner.WaveLevelBudget = MakeSureItsNotARef;



            //print("spawner budget set to " + EnemySpawner.LevelBudget);
            //print("spawner budget should be set to " + Levels.LevelTierPicker[CurrentLevel.Value].budget);

            SortedHexagons = new List<List<GameObject>>();
            availableTiers = new List<Tier>();



            for (int i = 0; i < Hexagons.Length; i++)
            {
                Tier tier = Hexagons[i].GetComponent<Hexagon>().Tier;
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
            //print("hexagons have been sorted "+ SortedHexagons.Count);



            if (hexmapParent.Value != null)
            {
                Destroy(hexmapParent.Value);
            }

            if (!UseRandomSeed)
                Random.InitState(Seed);

            Hexagon.mapGen = this;

            _Parent = new GameObject();
            _Parent.name = "Hex Map";

            _hexagonsTiles = new List<GameObject>();

            hexMesh = BaseHexagon.GetComponent<MeshFilter>().sharedMesh;

            Vertices = hexMesh.vertices;


            HexLength = Mathf.Abs(Vertices[2].z) + Mathf.Abs(Vertices[4].z);
            HexHeight = Mathf.Abs(Vertices[0].x) + Mathf.Abs(Vertices[5].x);

            HexHeight *= BaseHexagon.transform.localScale.z;
            HexLength *= BaseHexagon.transform.localScale.x;

        if (!DontGenerate)
        {

            GameObject hex = Instantiate(getRandomCenterHexagon());
            Hexagon hexHex = hex.GetComponent<Hexagon>();
            //hexHex.RotateTile(Random.Range(0, 5));
            hexHex.IsaccesibleFromMiddle = true;

            hex.name = "middle";
            hex.transform.position = Vector3.zero;

            hex.transform.parent = _Parent.transform;

            _hexagonsTiles.Add(hex);


            StartCoroutine(GeneratAllTheRings());

            _hexagonsTiles.Remove(hex);



            StartCoroutine(RotateTilesToMakeMostPossibleConnections());

            hexmapParent.Value = _Parent;
        }
    }

    IEnumerator RotateTilesToMakeMostPossibleConnections()
    {
        yield return new WaitUntil(() => _allTilesGenerated);

        if (!Levels.LevelTierPicker[CurrentLevel.Value].IsGauntlet)
        {
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

            for (int i = 0; i < _hexagonsTiles.Count; i++)
            {
                Hexagon hexScript = _hexagonsTiles[i].GetComponent<Hexagon>();


                hexScript.RemoveOuterWalls();

                
                //print(hexScript.name + " nieghbour 0 " + (hexScript.Neighbours[0] != null));

            }
            for (int i = 0; i < _hexagonsTiles.Count; i++)
            {
                Hexagon hexScript = _hexagonsTiles[i].GetComponent<Hexagon>();


                

                hexScript.CreateConnectors();
                //print(hexScript.name + " nieghbour 0 " + (hexScript.Neighbours[0] != null));

            }


        }
        _Parent.SetActive(false);
        _Parent.transform.Rotate(0, 90, 0);
        _Parent.SetActive(true);

        if(DoneGenerating != null)
        DoneGenerating.Raise();

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
                //print(hexagonScript.name + " nieghbour 0 " + (hexagonScript.Neighbours[0] != null));

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

            //print(hexScript.name + " nieghbour 0 " +( hexScript.Neighbours[0] != null));

            hexScript.LabelNeighbourAccesibility();
            hexScript.GetNeighbours();

            //print(hexScript.name + " second check nieghbour 0 " + (hexScript.Neighbours[0] != null));
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
        for (int i = 0; i < Vertices.Length; i++)
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


            if (Levels.LevelTierPicker[CurrentLevel.Value].IsGauntlet)
            {

                if(i == 0)
                    hexagonScript.RotateTile(3);
                else if (i == 1)
                    hexagonScript.RotateTile(4);
                else if (i == 2)
                    hexagonScript.RotateTile(5);
                else if (i == 3)
                    hexagonScript.RotateTile(0);
                else if (i == 4)
                    hexagonScript.RotateTile(1);
                else if (i == 5)
                    hexagonScript.RotateTile(2);

            }
            else
            {
                hexagonScript.RotateTile(Random.Range(0, 5));
            }
            

            hexagonEdge.Neighbours[i] = hexagonScript;

            hexagonScript.Neighbours[(i + 3) % 6] = startHexagon.GetComponent<Hexagon>();
              

            Vector3 positionToPlaceHex = startHexagon.transform.position;

            float hexStepLength = HexLength * 0.5f;
            float hexStepHeight = HexHeight * 0.75f;


            

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
