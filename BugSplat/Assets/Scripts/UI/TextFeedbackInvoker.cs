using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TextFeedbackInvoker : MonoBehaviour
{
    public TextFeedback TextFeedback;

    public LevelText LevelText;

    public IntVariable NumberOfWaves, CurrentWave;

    [Header("GameTexts")]
    public GameText DefeatAllWaves;
    public GameText KillAllEnemies;
    public GameText WavesCleared;
    public GameText WavesLeft;
    public GameText FinalWave;

    private bool _isWave;

    public void SetIsWave(bool isWave) {
        _isWave = isWave;
    }

    public void EnterArenaText() {
        TextFeedback.SetLevelTitle(LevelText);
        TextFeedback.SetSubtitle((_isWave) ? DefeatAllWaves : KillAllEnemies);
        TextFeedback.SetFeedbackActive();
    }
        
    public void NewWaveText()
    {
        var stringBuilder = new StringBuilder();

        var wavesClearedGT = Instantiate(WavesCleared);
        foreach (var variation in wavesClearedGT.TextVariations)
        {
            stringBuilder.Append(variation.Text);
            stringBuilder.Append(" ");
            stringBuilder.Append(CurrentWave.Value);
            stringBuilder.Append('/');
            stringBuilder.Append(NumberOfWaves.Value);
            
            variation.Text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        TextFeedback.SetTitle(wavesClearedGT);


        var wavesLeft = NumberOfWaves.Value - CurrentWave.Value;
        if (wavesLeft == 1)
            TextFeedback.SetSubtitle(FinalWave);
        else {
            var wavesLeftGT = Instantiate(WavesLeft);
            foreach (var variation in wavesLeftGT.TextVariations)
            {
                stringBuilder.Append(wavesLeft);
                stringBuilder.Append(" ");
                stringBuilder.Append(variation.Text);

                variation.Text = stringBuilder.ToString();
                stringBuilder.Clear();
            }

            TextFeedback.SetSubtitle(wavesLeftGT);
        }

        TextFeedback.SetFeedbackActive();
    }
}