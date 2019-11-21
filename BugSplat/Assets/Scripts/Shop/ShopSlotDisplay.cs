using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotDisplay : MonoBehaviour
{
    public ShopSlot Slot;

    public IntVariable PlayerCurrency;

    public Camera MainCamera;

    [SerializeField]
    private GameObject SlotObject;

    [SerializeField]
    private Transform SlotPlaceholder;

    [SerializeField]
    private TMPro.TextMeshPro PriceText;

    private bool _runOnce;

    private bool Select = false;

    private GameObject SlotItemInst;

    public Transform AnimTargetPos;

    public float AnimationPositionTime;
    public float AnimationRotationTime;

    public AnimationCurve AnimPosCurve;
    public AnimationCurve AnimRotationCurve;

    private float _lerpTime;

    private Vector3 StartPos;
    private Quaternion StartRot, EndRot;

    public GameObject BuyButton;


    public void Start() {
        Slot.Init();
        _lerpTime = AnimationPositionTime;

        if (SlotObject == null)
        {
            SlotObject = Slot?.GetItemPrefab();
        }

        SlotItemInst = Instantiate(SlotObject, SlotPlaceholder);
        SlotItemInst.transform.localPosition = Vector3.zero;
        PriceText.text = Slot?.GetPrice().ToString();

        StartPos = SlotItemInst.transform.position;
        StartRot = Quaternion.Euler(20, 0, 0);
        EndRot = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator SelectItemRoutine(bool select) {
        yield return select ?
            MoveItem(StartPos, AnimTargetPos.position, StartRot, EndRot)
            : MoveItem(AnimTargetPos.position, StartPos, EndRot, StartRot);

        BuyButton.SetActive(select);

        if (select) StartCoroutine(RotateItem());
    }

    private IEnumerator MoveItem(Vector3 from, Vector3 to, Quaternion fromRot, Quaternion toRot) {

        for (var time = 0f; time < AnimationPositionTime; time += Time.deltaTime) {
            //float curveTime = 1 - (_lerpTime / AnimationPositionTime);
            var curveTime = time / AnimationPositionTime;

            var nextPos = Vector3.Lerp(from, to, AnimPosCurve.Evaluate(curveTime));
            var nextRotation = Quaternion.Lerp(fromRot, toRot, AnimPosCurve.Evaluate(curveTime) * 4);

            SlotItemInst.transform.position = nextPos;
            SlotItemInst.transform.rotation = nextRotation;

            yield return null;
        }
   }

   private IEnumerator RotateItem() {
        var curveTime = 0f;
        while (true) {
            float curveAnimTime = AnimRotationCurve.Evaluate(curveTime);
            Quaternion nextRotation = Quaternion.Euler(0, -360 * curveAnimTime, 0);
            SlotItemInst.transform.rotation = nextRotation;

            curveTime += Time.deltaTime;

            yield return null;
        }
   }

    public void SelectItem() {
        if (Select) return;
        StopAllCoroutines();

        SlotItemInst.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        StartCoroutine(SelectItemRoutine(true));

        Select = true;
    }

    public void DeselectItem() {
        if (!Select) return;
        StopAllCoroutines();

        SlotItemInst.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        StartCoroutine(SelectItemRoutine(false));

        Select = false;
    }

    public void ToggleItem() {
        if (Select) {
            DeselectItem();
        } else {
            SelectItem();
        }
    }
}
