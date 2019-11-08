using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Dash Mesh")]
public class DashMeshEffect : Effect
{
    [Header("Dependencies")]
    public DashEffect DashInit;
    public Effect OnMeshEffect;


    [Header("Mesh parameters")]
    public float MeshWidth;
    public float MeshDepth;
    public float MeshTimer;



    public override void Init()
    {
    }

    public override void Trigger(GameObject target = null)
    {
        if (target == null) return;

        // TODO: This might need object pooling
        // MAKE MESH
        var mesh = CreateBox(target);
        
        // TODO: Maybe add this to another layer that only collides with Enemies 
        var monitorer = mesh.AddComponent<MeshMonitorer>();
        monitorer.ZoneEffect = OnMeshEffect;

        Destroy(mesh, MeshTimer);
    }

    

    private GameObject CreateBox(GameObject endPlace) {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(box.GetComponent<BoxCollider>());

        var direction = endPlace.transform.position - DashInit.StartPos;
        Debug.Log("Dir: " + endPlace.transform.position);
        var height = direction.magnitude;

        box.transform.localScale = new Vector3(MeshWidth, MeshDepth, height);
        box.transform.position = DashInit.StartPos + direction * 0.5f;
        box.transform.LookAt(direction + endPlace.transform.position);

        return box;
    }

    internal class MeshMonitorer : MonoBehaviour {
        internal Effect ZoneEffect;

        private Mesh MeshObj;

        void Start() {
            MeshObj = GetComponent<MeshFilter>().mesh;
        }

        void FixedUpdate() {
            var enemies = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation, (1 << 8));
           

            foreach (var enemy in enemies) {
                ZoneEffect.Trigger(enemy.gameObject);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
