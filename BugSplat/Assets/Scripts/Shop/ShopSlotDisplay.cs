using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotDisplay : MonoBehaviour
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

        if (SlotObject == null)
        {
            SlotObject = Slot?.GetItemPrefab();
        }

        SlotItemInst = Instantiate(SlotObject, SlotPlaceholder);
        PriceText.text = Slot?.GetPrice().ToString();
    }

    private IEnumerator SelectItemRoutine() {
        yield return MoveItem();

        BuyButton.SetActive(true);

        yield return RotateItem();
    }

    private IEnumerator MoveItem() {
        var startPos = SlotItemInst.transform.position;
        var endPos = AnimTargetPos.position;

        var startRot = Quaternion.Euler(20, 0, 0);
        var endRot = Quaternion.Euler(0, 0, 0);

        for (var time = 0f; time < AnimationPositionTime; time += Time.deltaTime) {
            //float curveTime = 1 - (_lerpTime / AnimationPositionTime);
            var curveTime = time / AnimationPositionTime;

            _nextPos = Vector3.Lerp(startPos, endPos, AnimPosCurve.Evaluate(curveTime));
            _nextRotation = Quaternion.Lerp(startRot, endRot, AnimPosCurve.Evaluate(curveTime) * 4);

            SlotItemInst.transform.position = _nextPos;
            SlotItemInst.transform.rotation = _nextRotation;

            yield return null;
        }
   }

   private IEnumerator RotateItem() {
        for (var time = 0f; time < AnimationRotationTime; time += Time.deltaTime) {
            float curveTime = time / AnimationRotationTime;
            float curveAnimTime = AnimRotationCurve.Evaluate(curveTime);

            Quaternion nextRotation = Quaternion.Euler(0, -360 * curveAnimTime, 0);
            SlotItemInst.transform.rotation = nextRotation;

            yield return null;
        }
   }

    public void SelectItem()
    {
        SlotItemInst.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        StartCoroutine(SelectItemRoutine());
    }
}
