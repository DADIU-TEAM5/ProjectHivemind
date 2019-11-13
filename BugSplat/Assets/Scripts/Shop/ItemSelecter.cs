using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelecter : MonoBehaviour
{
    public Camera ShopCamera;

    public ShopSlotDisplay ShopDisplay;

    private bool _selected = false;

    // Start is called before the first frame update
    void Start()
    {
        _selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            var mousePos = Input.mousePosition;

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(mousePos);
            Debug.Log(ray);
            if (Physics.Raycast(ray, out hit, 100f)) {
                Debug.Log($"Raycast hit {hit.transform.gameObject.name}");
                if (hit.transform == this.transform) {
                    if (_selected) {
                        ShopDisplay.DeselectItem();
                        _selected = false;
                    } else {
                        ShopDisplay.SelectItem();
                        _selected = true;
                    }
                }
            } else {
                Debug.Log("Didnt hit a thing");
            }
        }
    }


}
