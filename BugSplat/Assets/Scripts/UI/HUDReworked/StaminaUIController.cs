using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class StaminaUIController : GameLoop, IPreprocessBuildWithReport
{
    public Image RechargeBar;
    public Image PointBar;

    public FloatVariable CurrentStamina;
    public FloatVariable MaxStamina;
    public FloatVariable DashCost;

    public GameObject StaminaPoint;

    [SerializeField]
    private GameObject[] StaminaPoints;

    private int _amountOfDashes;

    public int callbackOrder => 2000;

    private void Awake() {
        #if UNITY_EDITOR 
            OnPreprocessBuild(null);
        #endif
        _amountOfDashes = (int) (MaxStamina.Value / DashCost.Value);
        for (var i = 0; i < _amountOfDashes - 1; i++) {
            StaminaPoints[i].gameObject.SetActive(true);
        }
    }

    public override void LoopUpdate(float deltaTime)
    {
        var staminaRatio = CurrentStamina.Value / MaxStamina.Value;
        RechargeBar.fillAmount = staminaRatio;

        var availableDashes = (int) (CurrentStamina.Value / DashCost.Value);
        PointBar.fillAmount = (float) availableDashes / _amountOfDashes;
    }

    public override void LoopLateUpdate(float deltaTime) {}

    public void OnPreprocessBuild(BuildReport report)
    {
        // Subtract one to make room for the seperator object 
        var maxDashCharges = -1 + (int) (MaxStamina.Max / DashCost.Value);

        StaminaPoints = new GameObject[maxDashCharges];
        for (var i = 0; i < maxDashCharges; i++) {
            var staminaPoint = Instantiate(StaminaPoint, PointBar.transform);
            StaminaPoints[i] = staminaPoint;
        }

        var empty = new GameObject();
        empty.name = "SEPERATOR";
        empty.AddComponent<RectTransform>();
        empty.transform.parent = PointBar.transform;
    }
}