using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class MapController : MonoBehaviour, IPreprocessBuildWithReport
{
    public int callbackOrder => 2000;

    public Hexagon[] OuterTiles, WaveTiles;
    public Hexagon[] CenterTiles, WaveCenterTiles;

    public IntVariable CurrentLevel;

    public ShopLevels Levels;

    public bool UseRandomSeed;
    public int Seed;

    [SerializeField]
    public Dictionary<Tier, List<Hexagon>> TierToHexDic;

    [SerializeField]
    private Vector3[] _hexPositions;
    [SerializeField]
    private Quaternion[] _hexRotations;

    [SerializeField]
    private GameObject _parentObject;

    [SerializeField]
    private GameObject BaseHex;

    private float HexHeight;

    [Header("Events")]
    public GameEvent GauntletEvent;
    public GameEvent WaveEvent;


    public void OnPreprocessBuild(BuildReport report)
    {
        TierToHexDic = new Dictionary<Tier, List<Hexagon>>();
        for (int i = 0; i < OuterTiles.Length; i++)
        {
            var hexagon = OuterTiles[i];
            var tier = hexagon.difficultyLevel;
            if (TierToHexDic.ContainsKey(tier)) {
                var hexagonList = TierToHexDic[tier];
                hexagonList.Add(hexagon);
            } else {
                Debug.Log($"New tier: {tier.name}");
                TierToHexDic.Add(tier, new List<Hexagon>() { hexagon });
            }
        }

        _parentObject = new GameObject();
        _parentObject.name = "MapGenny";

        var baseHexRenderer = BaseHex.GetComponent<MeshRenderer>();
        var bounds = baseHexRenderer.bounds;
        HexHeight = bounds.size.z;

        _hexPositions = new Vector3[] {
            new Vector3(0f, 0f, bounds.size.z),
            new Vector3(0f, 0f, -bounds.size.z),
            new Vector3(bounds.size.x, 0f, bounds.size.z * 0.5f),
            new Vector3(bounds.size.x, 0f, bounds.size.z * 0.5f),
            new Vector3(bounds.size.x, 0f, -bounds.size.z * 0.5f),
            new Vector3(bounds.size.x, 0f, -bounds.size.z * 0.5f),
        };

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
            OuterTiles = WaveTiles;
            CenterTiles = WaveCenterTiles;
        }
        
        var centerHex = Instantiate(GetRandomCenterHexagon(), Vector3.zero, Quaternion.identity, _parentObject.transform);

        for (int i = 0; i < _hexPositions.Length; i++) {
            var hex = Instantiate(GetHexagonBasedOnLevel(), Vector3.zero, _hexRotations[i], _parentObject.transform);
            hex.transform.localPosition += hex.transform.forward * HexHeight; 
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