using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotDisplay : GameLoop
{
    public ShopSlot Slot;

    [SerializeField]
    private GameObject SlotObject;

    [SerializeField]
    private Transform SlotPlaceholder;

    [SerializeField]
    private TMPro.TextMeshPro PriceText;

    private bool _runOnce;

    private bool Select;

    private GameObject SlotItemInst;

    public Transform AnimTargetPos;

    public float AnimationPositionTime;
    public float AnimationRotationTime;

    public AnimationCurve AnimPosCurve;
    public AnimationCurve AnimRotationCurve;

    private Vector3 _nextPos;
    private float _lerpTime;
    private Quaternion _nextRotation;
    public GameObject BuyButton;


    public void Start() {
        Slot.Init();
        _lerpTime = AnimationPositionTime;
    }

    public override void LoopUpdate(float deltaTime)
    {
        if (_runOnce == false)
        {
            if (SlotObject == null)
            {
                SlotObject = Slot?.GetItemPrefab();
            }
            SlotItemInst = Instantiate(SlotObject, SlotPlaceholder);
            PriceText.text = Slot?.GetPrice().ToString();
            _runOnce = true;
        }

        if (Select)
        {

            Vector3 startPos = SlotItemInst.transform.position;

            _lerpTime -= Time.deltaTime;

            if (_lerpTime > 0)
            {
                float curveTime = 1 - (_lerpTime / AnimationPositionTime);
                _nextPos = Vector3.Lerp(startPos, AnimTargetPos.position, AnimPosCurve.Evaluate(curveTime)/AnimationPositionTime);
                _nextRotation = Quaternion.Lerp(Quaternion.Euler(20, 0, 0), Quaternion.Euler(0, 0, 0), AnimPosCurve.Evaluate(curveTime)*4);
                SlotItemInst.transform.position = _nextPos;
                SlotItemInst.transform.rotation = _nextRotation;
            }

            if (_lerpTime <= 0 && _lerpTime >= -AnimationRotationTime)
            {
                BuyButton.SetActive(true);
                float curveTime = 1 - (-_lerpTime / AnimationRotationTime);
                float curveAnimTime = AnimRotationCurve.Evaluate(curveTime);
                Quaternion nextRotation = Quaternion.Euler(0, -360*curveAnimTime, 0);
                SlotItemInst.transform.rotation = nextRotation;
            }

            if (_lerpTime <= -AnimationRotationTime)
            {
                _lerpTime = 0;
            }

            
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }

    public void Buy() {
        Slot.Buy();
        Debug.Log("ITEM BOUGHT");
        SlotItemInst.SetActive(false);
        Select = false;
    }

    public void SelectItem()
    {
        Select = true;

        SlotItemInst.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }
}
