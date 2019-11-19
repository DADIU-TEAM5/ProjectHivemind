using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerTrajectory : GameLoop
{
    //public float Speed = 5;
    //public float RotationSpeed = 5;
    public float Second = 1f;
    public int SaveInSecond = 10;
    public int FrameRateForAnim = 30;
    public int PredictSpeed = 20;
    [Range(0.1f, 2f)]
    public float MoMaUpdateTime = 0.1f;
    [Range(0, 1)]
    public float BlendDegree = 0.5f;
    [Range(1, 30)]
    public int BlendLength = 1;
    [Range(3, 40)]
    public int DifferentClipLength = 10;
    public Vector3Variable Direction;
    public Vector3Variable Velocity;
    public FloatVariable PlayerCurrentSpeedSO;
    public FloatVariable AttackAngle;
    public GameObject PlayerGraphics;



    public AnimationCapsules AnimationTrajectories;
    public AnimationClips AnimationClips;
    public AnimationClips DeadAnimationClips;
    public Result Results;
    public bool Blend = false;
    //it is okay for static?
    public CapsuleScriptObject PlayerTrajectoryCapusule;
    public MagicMotions AttackMotions;



    private Queue<Vector3> _history = new Queue<Vector3>();
    private List<Vector3> _future = new List<Vector3>();
    private float _timer;
    private float _tempMoMaTime;
    private int _stratFrame = 3; //assume we know... todo get it!!!
    private bool _blendFlag = false;
    private int _forBlendPlay = 0;
    private string _attack = null;
    private bool _isDead = false;

    //for test
    private int _beginFrame;
    private int _beginAnimClip;

    private MotionMatcher _motionMatcher;
    private Dictionary<string, Transform> _skeletonJoints = new Dictionary<string, Transform>();
    private float _scale;


    [Header("Events")]
    [SerializeField]
    private GameEvent FootStep;
    //i can't believe it is too long
    void Start()
    {
        GetAllChildren(transform);
        InitializeTrajectory();

        _motionMatcher = new MotionMatcher();
        PlayerTrajectoryCapusule.Capsule = new Capsule();

        _timer = 0;
        _tempMoMaTime = 0;
        Results.FrameNum = 0;
        Results.AnimClipIndex = 0;
        _scale = transform.lossyScale.x;
    }

    // Update is called once per frame
    public override void LoopUpdate(float deltaTime)
    {
        _timer += deltaTime;
        _tempMoMaTime += deltaTime;
        _scale = transform.lossyScale.x;

        int thisClip = Results.AnimClipIndex;
        int thisClipNum = Results.FrameNum;


        var rotationPlayer = Direction.Value;

        //UpdatePlayerState();
        GetRelativeTrajectory(Velocity.Value);


        if (!_isDead)
            if (Blend)
                UpdateWithBlend(thisClip, thisClipNum, rotationPlayer);
            else
                UpdateWithoutBlend(thisClip, thisClipNum, rotationPlayer);

        if (Results.AnimClipIndex == 0 &&
            (Results.FrameNum == 0 || Results.FrameNum == 13
            || Results.FrameNum == 28))
            IsFootStep();

        if (_tempMoMaTime < 0)
        {
            _isDead = false;
            _tempMoMaTime = 0;
        }

    }


    public void GetAttack1()
    {
        if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.25)
            PlayAnimationByIndex(4);
        else if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.5)
            PlayAnimationByIndex(5);
        else if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.75)
            PlayAnimationByIndex(3);
        else
            PlayAnimationByIndex(6);
    }



    //public void AttackComBo()
    //{
    //    if (firsttime)
    //    {
    //        _attacktime = _AttackTime;
    //        GetAttack1();
    //        firsttime = false;
    //    }
    //    else
    //    {
    //        if (_AttackTime - _attacktime > 0 && _AttackTime - _attacktime < 0.7)
    //            GetAttack2();
    //        else
    //            GetAttack1();
    //    }
    //}

    //public void GetComboAttack1()
    //{
    //    if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.25)
    //        PlayAttackAnimationByIndex(3);
    //    else if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.5)
    //        PlayAttackAnimationByIndex(4);
    //    else if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.75)
    //        PlayAttackAnimationByIndex(5);
    //    else
    //        PlayAttackAnimationByIndex(6);
    //}

    //public void GetAttack2()
    //{
    //    if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.25)
    //        PlayAttackAnimationByIndex(3);
    //    else if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.5)
    //        PlayAttackAnimationByIndex(0);
    //    else if (AttackAngle.Value < (AttackAngle.Max - AttackAngle.Min) * 0.75)
    //        PlayAttackAnimationByIndex(1);
    //    else
    //        PlayAttackAnimationByIndex(2);
    //}
    //public void PlayAttackAnimationByIndex(int animIndex)
    //{
    //    StartCoroutine(PlayOneWholeAnimation(AttackAnims.AnimClips[animIndex]));
    //}


    public void debugFootStep()
    {
        Debug.Log("foot");
    }

    public void IsFootStep()
    {
        FootStep.Raise(PlayerGraphics);
    }

    //todo add attack motion
    private void GetRelativeTrajectory(Vector3 inputVel)
    {
        var currentPos = transform.position;
        var currentRot = transform.rotation;


        HistoryTrajectory(currentPos);
        PlayerTrajectoryCapusule.Capsule.TrajectoryHistory = _history.ToArray();


        FuturePredict(currentPos, inputVel, currentRot);
        PlayerTrajectoryCapusule.Capsule.TrajectoryFuture = _future.ToArray();

        transToRelative(PlayerTrajectoryCapusule.Capsule.TrajectoryHistory, currentPos);
        transToRelative(PlayerTrajectoryCapusule.Capsule.TrajectoryFuture, currentPos);
    }

    private void UpdateWithoutBlend(int thisClip, int thisClipNum, Vector3 rotationPlayer)
    {
        //change it to more safety way
        if (_tempMoMaTime < 0)
        {
            _isDead = false;
            _tempMoMaTime = 0;
        }

        if (_tempMoMaTime > MoMaUpdateTime)
        {
            _motionMatcher.GetMotionAndFrame(_attack, AttackMotions, AnimationTrajectories, PlayerTrajectoryCapusule,
                                              Results, AnimationClips, DifferentClipLength);
            _timer = 0;
            _tempMoMaTime = 0;
            _attack = null;
            bool isSimilarMotion = ((thisClip == Results.AnimClipIndex)
                          && (Mathf.Abs(thisClipNum - Results.FrameNum) < DifferentClipLength));

            //todo if same motion, result changes (should have another struct contrl)
            if (isSimilarMotion)
            {
                Results.AnimClipIndex = thisClip;
                Results.FrameNum = thisClipNum;
                Results.FrameNum++;
            }

        }
        else
            Results.FrameNum++;


        PlayAnimationJoints(PlayerTrajectoryCapusule,
                                                Results, AnimationClips, _skeletonJoints);
        //transform.Rotate(rotationPlayer);
    }






    private void UpdateWithBlendInOneFrame(int thisClip, int thisClipNum, Vector3 rotationPlayer)//update in one frame
    {
        if (_tempMoMaTime < 0)
            _tempMoMaTime = 0;
        if (_tempMoMaTime > MoMaUpdateTime)
        {
            _timer = 0;
            _motionMatcher.GetMotionAndFrame(_attack, AttackMotions, AnimationTrajectories, PlayerTrajectoryCapusule,
                                                Results, AnimationClips, DifferentClipLength);
            _tempMoMaTime = 0;
            _attack = null;
            bool isSimilarMotion = ((thisClip == Results.AnimClipIndex)
                            && (Mathf.Abs(thisClipNum - Results.FrameNum) < DifferentClipLength));


            if (isSimilarMotion)
                PlayAnimationJoints(PlayerTrajectoryCapusule,
                                                Results, AnimationClips, _skeletonJoints);
            else
            {
                _beginFrame = thisClipNum;
                _beginAnimClip = thisClip;
                BlendInOneFrame(_skeletonJoints, BlendDegree, _beginFrame, _beginAnimClip,
                    Results, PlayerTrajectoryCapusule, AnimationClips,
                    _forBlendPlay, rotationPlayer);

            }


        }

        else
        {
            Results.FrameNum++;
            PlayAnimationJoints(PlayerTrajectoryCapusule,
                                                Results, AnimationClips, _skeletonJoints);
        }
    }



    private void BlendInOneFrame(Dictionary<string, Transform> skeletonJoints, float blendDegree,
                            int beginFrameIndex, int beginAnimIndex, Result result, CapsuleScriptObject PlayerTrajectoryCapusule,
                            AnimationClips animationClips, int areadlyBlendedTimes, Vector3 rotationEular)
    {
        if (_forBlendPlay >= BlendLength)
            _forBlendPlay = 0;
        else
        {
            _forBlendPlay++;
            PlayBlendAnimation(skeletonJoints, blendDegree, beginFrameIndex, beginAnimIndex,
                        result, PlayerTrajectoryCapusule, animationClips,
                        areadlyBlendedTimes, rotationEular);
        }
    }


    private void UpdateWithBlend(int thisClip, int thisClipNum, Vector3 rotationPlayer)//every frame update
    {
        if (_tempMoMaTime < 0)
            _tempMoMaTime = 0;
        if (_tempMoMaTime > MoMaUpdateTime)
        {
            _motionMatcher.GetMotionAndFrame(_attack, AttackMotions, AnimationTrajectories, PlayerTrajectoryCapusule,
                                                Results, AnimationClips, DifferentClipLength);
            _tempMoMaTime = 0;
            _attack = null;
            bool isSimilarMotion = ((thisClip == Results.AnimClipIndex)
                            && (Mathf.Abs(thisClipNum - Results.FrameNum) < DifferentClipLength));


            if (isSimilarMotion)
                PlayAnimationJoints(PlayerTrajectoryCapusule,
                                                Results, AnimationClips, _skeletonJoints);
            else
            {
                _blendFlag = true;
                _beginFrame = thisClipNum;
                _beginAnimClip = thisClip;
                PlayBlendAnimation(_skeletonJoints, BlendDegree, _beginFrame, _beginAnimClip,
                    Results, PlayerTrajectoryCapusule, AnimationClips,
                    _forBlendPlay, rotationPlayer);

            }


        }
        else if (!_blendFlag)
        {
            PlayAnimationJoints(PlayerTrajectoryCapusule,
                                                Results, AnimationClips, _skeletonJoints);
            Results.FrameNum++;
        }
        else
        {

            if (_forBlendPlay >= BlendLength)
            {
                _blendFlag = false;
                //Results.FrameNum = BlendLength;//_forBlendPlay + _beginFrame;

                _forBlendPlay = 0;

            }
            else
            {
                _forBlendPlay++;
                PlayBlendAnimation(_skeletonJoints, BlendDegree, _beginFrame, _beginAnimClip,
                     Results, PlayerTrajectoryCapusule, AnimationClips,
                     _forBlendPlay, rotationPlayer);
            }
        }
    }


    private void transToRelative(Vector3[] vector3s, Vector3 current)
    {
        for (int i = 0; i < vector3s.Length; i++)
        {
            vector3s[i] = transform.InverseTransformDirection((vector3s[i] - current)); //change it to relative
        }
    }

    private void InitializeTrajectory()
    {
        _isDead = false;
        while (_history.Count < SaveInSecond)
        {
            _history.Enqueue(transform.localPosition);
            _future.Add(transform.localPosition);
        }
    }

    private Vector3 UpdatePlayerState(float deltaTime)
    {

        //get input velocity to move
        var inputVel = Velocity.Value;
        transform.Translate(inputVel * deltaTime);

        return inputVel;
    }

    private void HistoryTrajectory(Vector3 currentPos)
    {
        //save History only in the gap
        if (_timer > 1f / FrameRateForAnim)//(Second / SaveInSecond))
        {
            _timer = 0;
            _history.Dequeue();
            _history.Enqueue(currentPos);
        }
    }


    /* if we do not use the root as the player, it seems we don't need currentRot*/
    private void FuturePredict(Vector3 currentPos, Vector3 inputVel, Quaternion currentRot)
    {
        _future[0] = currentPos;
        Vector3 direct = new Vector3(0, 0, 0);
        direct.y = Direction.Value.y;

        var rotation = Quaternion.Euler(direct);

        for (int i = 1; i < SaveInSecond; i++)
        {
            var increase = Second / SaveInSecond * i;

            //var gap_increase = Quaternion.ToEulerAngles(rotation)* increase;
            //var angle_increase = Quaternion.EulerRotation(gap_increase);
            //var gap = inputVel * increase;
            //gap.x = 0;
            var futureP = currentPos + inputVel * increase;
            //var futureP = (currentPos + angle_increase  * gap);
            _future[i] = futureP;
        }

    }



    //private void FutureFeed(Vector3 currentPos,)
    //{
    //    _future[0] = currentPos;

    //    for(int i = 0; )
    //}


    private void GetAllChildren(Transform trans)
    {
        //SkeletonJoints.Add("Root", trans);
        foreach (Transform child in trans)
        {
            if (child.childCount > 0) GetAllChildren(child);
            _skeletonJoints.Add(child.name, child);
        }
    }

    public void PlayAnimationByIndex(int animIndex)
    {
        StartCoroutine(PlayOneWholeAnimation(AnimationClips.AnimClips[animIndex]));
    }


    public void PlayDeadAnim()
    {
        _isDead = true;
        var deadAnimIndex = Random.Range(0, DeadAnimationClips.AnimClips.Count - 1);
        StartCoroutine(PlayOneWholeAnimation(DeadAnimationClips.AnimClips[deadAnimIndex]));
    }


    private IEnumerator PlayOneWholeAnimation(AnimClip animClip)
    {
        for (int i = 0; i < animClip.Frames.Count; i++)
        {
            FrameToJoints(_skeletonJoints, animClip.Frames[i]);
            yield return null;
        }

    }




    //play animation
    public void PlayAnimationJoints(
                                    CapsuleScriptObject current, Result result,
                                    AnimationClips animationClips,
                                    Dictionary<string, Transform> skeletonJoints)

    {

        current.Capsule.AnimClipIndex = result.AnimClipIndex;
        current.Capsule.AnimClipName = result.ClipName;
        current.Capsule.FrameNum = result.FrameNum;

        //watch out here
        if (result.FrameNum >= animationClips.AnimClips[result.AnimClipIndex].Frames.Count - 1)
            result.FrameNum = 0;


        FrameToJoints(skeletonJoints,
                       animationClips.AnimClips[result.AnimClipIndex].Frames[result.FrameNum]);
        //transform.Rotate(rotationPlayer);
    }

    public void FrameToJoints(
                              Dictionary<string, Transform> skeletonJoints,
                              AnimationFrame frame)
    {
        foreach (var jointPoint in frame.JointPoints)
        {
            if (!skeletonJoints.Keys.Contains(jointPoint.Name))
            {
                continue;
            }
            var joint = skeletonJoints[jointPoint.Name];
            ApplyJointPointToJoint(jointPoint, joint);
        }
    }


    private void ApplyJointPointToJoint(AnimationJointPoint jointPoint, Transform joint)
    {
        joint.rotation = transform.rotation * jointPoint.Rotation;
        joint.position = transform.TransformDirection(jointPoint.Position * _scale) + transform.position;
    }








    //blend animation
    public void PlayBlendAnimation(Dictionary<string, Transform> skeletonJoints, float blendDegree,
                            int beginFrameIndex, int beginAnimIndex, Result result, CapsuleScriptObject PlayerTrajectoryCapusule,
                            AnimationClips animationClips, int areadlyBlendedTimes, Vector3 rotationEular)

    {
        int bestFrameIndex = result.FrameNum;
        PlayerTrajectoryCapusule.Capsule.AnimClipIndex = result.AnimClipIndex;
        PlayerTrajectoryCapusule.Capsule.AnimClipName = result.ClipName;
        PlayerTrajectoryCapusule.Capsule.FrameNum = result.FrameNum;

        if (result.FrameNum >= animationClips.AnimClips[result.AnimClipIndex].Frames.Count - 1)//3 should be start frame 
            result.FrameNum = 0; //3 should be start frame 

        BlendAnimation(beginFrameIndex, bestFrameIndex, skeletonJoints,
            areadlyBlendedTimes, animationClips.AnimClips[beginAnimIndex], animationClips.AnimClips[result.AnimClipIndex], blendDegree);
        //transform.Rotate(rotationEular);
    }


    private void BlendAnimation(int beginFrameIndex, int bestFrameIndex, Dictionary<string, Transform> skeletonJoints,
                            int areadlyBlendedTimes, AnimClip beginClip, AnimClip bestClip, float blendDegree)
    {
        var blendStart = beginFrameIndex + areadlyBlendedTimes;
        var blendEnd = bestFrameIndex - (SaveInSecond - areadlyBlendedTimes);

        BlendFrame(skeletonJoints, transform, beginClip.Frames[blendStart], bestClip.Frames[blendEnd], blendDegree);
    }

    private void BlendFrame(Dictionary<string, Transform> skeletonJoints, Transform transform,
                                    AnimationFrame startFrame, AnimationFrame endFrame, float blendDegree)
    {
        for (int i = 0; i < startFrame.JointPoints.Count; i++)
        {
            var startJoint = startFrame.JointPoints[i];
            var endJoint = endFrame.JointPoints[i];

            var joint = skeletonJoints[startJoint.Name];
            BlendJoints(startJoint, endJoint, joint, blendDegree);
        }
    }



    private void BlendJoints(AnimationJointPoint startjointPoint, AnimationJointPoint endjointPoint,
                             Transform joint, float blendDegree)
    {

        joint.rotation = transform.rotation * Quaternion.Lerp(startjointPoint.Rotation, endjointPoint.Rotation, blendDegree);
        //more cost?
        joint.position = Vector3.Lerp(transform.TransformDirection(startjointPoint.Position) + transform.position,
                                        transform.TransformDirection(endjointPoint.Position) + transform.position, blendDegree);
    }


    public override void LoopLateUpdate(float deltaTime)
    {

    }
}