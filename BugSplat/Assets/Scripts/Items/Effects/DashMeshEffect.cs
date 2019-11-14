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

    public ParticleController TrailParticles;


    private EmptyMono CoroutineBoy;

    public override void Init()
    {
        CoroutineBoy = MakeCoroutineObject();
        CoroutineBoy.name = "Coroutineboy - DashMeshEffect";
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

        var pcs = CreateParticleControllers(target);

        CoroutineBoy.StartCoroutine(Timeout(monitorer, pcs));
    }

    private IEnumerator Timeout(MeshMonitorer monitorer, params ParticleController[] particleControllers) {
        yield return new WaitForSeconds(MeshTimer);

        Destroy(monitorer.gameObject);
        
        for (var i = 0; i < particleControllers.Length; i++) {
            var pc = particleControllers[i];
            var particlesystems = pc.GetComponentsInChildren(typeof(ParticleSystem), false);
            foreach (ParticleSystem ps in particlesystems) {
                var emission = ps.emission;
                emission.rateOverTime = 0;
            }

            Destroy(pc.gameObject, 5);
        }
    }

    private GameObject CreateBox(GameObject endPlace) {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(box.GetComponent<BoxCollider>());
        Destroy(box.GetComponent<Renderer>());

        var direction = endPlace.transform.position - DashInit.StartPos;
        var midPoint = DashInit.StartPos + direction * 0.5f;

        var height = direction.magnitude;

        box.transform.localScale = new Vector3(MeshWidth, MeshDepth, height);
        box.transform.position = new Vector3(midPoint.x, 0, midPoint.z);

        box.transform.LookAt(new Vector3(endPlace.transform.position.x, 0, endPlace.transform.position.z));

        return box;
    }

    private ParticleController[] CreateParticleControllers(GameObject endPlace) {

        var difference = endPlace.transform.position - DashInit.StartPos;
        var distance = difference.magnitude;
        var numberOfPcs = Mathf.CeilToInt(distance);

        Debug.Log($"Number of p-ticles {numberOfPcs}");

        var particleControllers = new ParticleController[Mathf.CeilToInt(distance)];

        for (var i = 0; i < numberOfPcs; i++) {
            float fraction = (float) i / (float) numberOfPcs;
            var pcPosition = DashInit.StartPos + difference * fraction;

            var pc = Instantiate(TrailParticles, pcPosition, endPlace.transform.rotation);

            particleControllers[i] = pc;
        }

        return particleControllers;
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
    }
}
