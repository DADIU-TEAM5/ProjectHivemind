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

    private bool Select;

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
        var itemSelector = SlotItemInst.GetComponent<ItemSelecter>();
        itemSelector.ShopCamera = MainCamera;
        itemSelector.ShopDisplay = this;

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
        if (PlayerCurrency.Value < Slot.GetPrice()) {
            BuyButton.GetComponent<Button>().enabled = false;
        }

        yield return RotateItem();
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

        StartCoroutine(SelectItemRoutine(true));
    }

    public void DeselectItem() {
        SlotItemInst.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        StartCoroutine(SelectItemRoutine(false));
    }
}
