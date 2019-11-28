using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesTrackUI : GameLoop
{

    public IntVariable TotalEnemyCount;
    public EnemyObjectList EnemiesListSO;
    public IntVariable EnemiesKilledSO;
    public IntVariable NumberOfWavesSO;
    public TextMeshProUGUI EKills;
    public TextMeshProUGUI EnemiesAtStart;
    public GameEvent HasWonEvent;
    public MapGenerator MapGen;
    public IntVariable CurrentLevel;
    public GameText WavesCleared, WavesLeft, FinalWave, DefeatAllWaves;
    private ShopLevels _levels;
    private int WaveCount = 0;
    private int eventRaisedCount;
    bool isWave;
    private TextFeedback _textFeedback;

    bool isWon = false;


    private void Start()
    {
        isWon = false;
        // Yes... it looks retarded
        MapGen = FindObjectOfType<MapGenerator>();
        _levels = MapGen.Levels;
        isWave = !_levels.LevelTierPicker[CurrentLevel.Value].IsGauntlet;

        _textFeedback = FindObjectOfType<TextFeedback>();
        if (_textFeedback == null)
            Debug.LogError("No textfeedback founds");

        eventRaisedCount = 0;
        EnemiesKilledSO.Value = 0;
        TotalEnemyCount.Value = 0;

        int enemytotal = EnemiesListSO.Items.Count + TotalEnemyCount.Value;
        EnemiesAtStart.text = enemytotal.ToString();
    }

    public override void LoopUpdate(float deltaTime)
    {
        EKills.text = EnemiesKilledSO.Value.ToString();
    }

    public override void LoopLateUpdate(float deltaTime) { }

    public void UpdateKills()
    {
        Debug.Log("WaveCount: " + WaveCount + ", WaveSO: " + NumberOfWavesSO.Value);
        EnemiesKilledSO.Value++;
        if (EnemiesKilledSO.Value >= TotalEnemyCount.Value && (!isWave || WaveCount + 1 >= NumberOfWavesSO.Value) && !isWon)
        {
            HasWonEvent.Raise();
            isWon = true;
        }
    }

    public void UpdateEnemyCount()
    {
        Debug.Log("Enemy Spawned call");
        int enemytotal = TotalEnemyCount.Value;


        EnemiesAtStart.text = enemytotal.ToString();

    }

    public void NewWave()
    {
        if (WaveCount + 1 < NumberOfWavesSO.Value)
        {
            Debug.Log("New Wave " + WaveCount);
            if (EnemiesKilledSO.Value > 0)
                WaveCount++;
            TotalEnemyCount.Value -= EnemiesKilledSO.Value;
            EnemiesKilledSO.Value = 0;
            WaveTextFeedback();
        }

    }

    public void WaveTextFeedback()
    {
        if (NumberOfWavesSO.Value - WaveCount >= 0)
        {
            var wavesClearedGT = ScriptableObject.CreateInstance<GameText>();
            wavesClearedGT.TextVariations = WavesCleared.TextVariations;

            foreach (var variation in wavesClearedGT.TextVariations) {
                variation.Text = $"{variation.Text} {WaveCount}/{NumberOfWavesSO.Value}";
            }

            _textFeedback.SetTitle(wavesClearedGT);


            int wavesLeft = NumberOfWavesSO.Value - WaveCount;
            if (wavesLeft == 1)
                _textFeedback.SetSubtitle(FinalWave);
            else {
                var wavesLeftGT = ScriptableObject.CreateInstance<GameText>(); 
                wavesLeftGT.TextVariations = WavesLeft.TextVariations;

                foreach (var variation in wavesLeftGT.TextVariations)
                {
                    variation.Text = $"{wavesLeft} Waves Left";
                }

                _textFeedback.SetSubtitle(wavesLeftGT);
            }

            _textFeedback.SetFeedbackActive(true);
        }


    }

    public void EnterArenaTextFeedback()
    {
        _textFeedback.SetLevelTitle(CurrentLevel);
        _textFeedback.SetSubtitle(DefeatAllWaves);
        _textFeedback.SetFeedbackActive(true);
    }
}
