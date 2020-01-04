using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class MapController : SerializedMonoBehaviour, IPreprocessBuildWithReport
{
    public int callbackOrder => 2000;

    [TabGroup("Wave")]
    public Hexagon[] WaveOuterTiles, WaveCenterTiles;

    [TabGroup("Gauntlet")]
    public Hexagon[] OuterTiles, CenterTiles;

    
    [BoxGroup("Randomness")]
    public bool UseRandomSeed;
    [DisableIf("UseRandomSeed")]
    [BoxGroup("Randomness")]
    public int Seed;

    [BoxGroup("Wall System")]
    public GameObject EdgeWall;
    [BoxGroup("Wall System")]
    public GameObject WallConnector, SideWallConnector;

    [BoxGroup("Other dependencies")]
    public IntVariable CurrentLevel;

    [BoxGroup("Other dependencies")]
    public ShopLevels Levels;


    [HideInInspector]
    public Dictionary<Tier, List<Hexagon>> TierToHexDic;

    [SerializeField]
    [ReadOnly]
    private Quaternion[] _hexRotations;

    [SerializeField]
    [HideInInspector]
    private GameObject _parentObject;

    [SerializeField]
    [BoxGroup("Other dependencies")]
    private GameObject BaseHex;

    [SerializeField]
    [HideInInspector]
    private float HexHeight;

    private Hexagon[] MapHexagons;

    [BoxGroup("Events")]
    public GameEvent GauntletEvent;
    [BoxGroup("Events")]
    public GameEvent WaveEvent;


    public void OnPreprocessBuild(BuildReport report)
    {
        TierToHexDic = new Dictionary<Tier, List<Hexagon>>();
        for (int i = 0; i < OuterTiles.Length; i++)
        {
            var hexagon = OuterTiles[i];
            var tier = hexagon.Tier;
            if (tier == null) print(hexagon.name);
            if (TierToHexDic.ContainsKey(tier)) {
                var hexagonList = TierToHexDic[tier];
                hexagonList.Add(hexagon);
            } else {
                TierToHexDic.Add(tier, new List<Hexagon>() { hexagon });
            }
        }

        _parentObject = new GameObject();
        _parentObject.name = "MapGenny";

        var baseHexRenderer = BaseHex.GetComponent<MeshRenderer>();
        var bounds = baseHexRenderer.bounds;
        HexHeight = bounds.size.z;
        
        _hexRotations = new Quaternion[6];
        for (int i = 0; i < _hexRotations.Length; i++) {
            var angle = 180 + (60 * i);

            _hexRotations[i] = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void Awake()
    {
        #if UNITY_EDITOR
            OnPreprocessBuild(null);
        #endif
            
        if (!UseRandomSeed)
            Random.InitState(Seed);

        var isGauntlet = Levels.LevelTierPicker[CurrentLevel.Value].IsGauntlet;
        if (!isGauntlet) {
            OuterTiles = WaveOuterTiles;
            CenterTiles = WaveCenterTiles;
            WaveEvent?.Raise();
        } else {
            GauntletEvent?.Raise();
        }
        
        var centerHex = Instantiate(GetRandomCenterHexagon(), Vector3.zero, Quaternion.identity, _parentObject.transform);

        MapHexagons = new Hexagon[6];
        for (int i = 0; i < MapHexagons.Length; i++) {
            var hex = Instantiate(GetHexagonBasedOnLevel(), Vector3.zero, _hexRotations[i], _parentObject.transform);

            hex.transform.localPosition += hex.transform.forward * HexHeight; 
            MapHexagons[i] = hex;
        }

        if (!isGauntlet) RotateAllPossibleSolutions();
    }

    private void RotateTilesToMakeMostPossibleConnections()
    {

        RotateAllPossibleSolutions();
        for (int i = 0; i < MapHexagons.Length; i++)
        {
            var hex = MapHexagons[i];

            if (!hex.IsaccesibleFromMiddle)
            {
                hex.OpenAndRotateNeighbour();
                RotateAllPossibleSolutions();
            }
            
        }

        for (int i = 0; i < MapHexagons.Length; i++)
        {
            var hex = MapHexagons[i];
            hex.RemoveOuterWalls();
        }

        for (int i = 0; i < MapHexagons.Length; i++)
        {
            var hex = MapHexagons[i];
            hex.CreateConnectors();
        }
    }

    void RotateAllPossibleSolutions()
    {
        bool noSolution = false;
        while (!noSolution)
        {
            noSolution = true;

            for (int i = 0; i < MapHexagons.Length; i++)
            {
                var hex = MapHexagons[i];

                if (hex.HasAMiddleConnectedNeighbourWithAnOpenSideTowardsThisTile() && !hex.IsaccesibleFromMiddle)
                {
                    noSolution = false;

                    hex.RotateToFitMiddleConnectedNeighbour();
                }
            }
        }
    }

    private Hexagon GetHexagonBasedOnLevel()
    {
        var tier = Levels.LevelTierPicker[CurrentLevel.Value].ChooseTier();
        var availableHexes = TierToHexDic[tier];

        return availableHexes[Random.Range(0, availableHexes.Count)];
    }

    private Hexagon GetRandomCenterHexagon()
    {
        return CenterTiles[Random.Range(0, CenterTiles.Length)];
    }
}