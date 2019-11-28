using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : GameLoop
{

    public GameObject PlayerGraphics;
    // Setup ScriptableObjects for holding the PlayerMovementInfo
    public Vector3Variable MoveDirectionSO;

    public GameEvent Swipe;
    public GameEvent AttackTapSO;
    public FloatVariable InputMoveMinThresholdSO;
    public FloatVariable InputMoveMaxThresholdSO;
    public FloatVariable InputSwipeTapTimeSO;
    public FloatVariable InputSwipeThresholdSO; // Percentage of the screen width
    public BoolVariable PlayerControlOverrideSO;
    public BoolVariable IsStunned;

    public GameObjectVariable LockedTarget;
    public GameObject UICanvas;
    public RectTransform UIMenuButton;
    private Vector2 _uiOffset = new Vector2(0, 0);

    // Setup the private variables needed for the calculations in the current script
   // private Vector3 _inputTouch;
    private bool[] _recordPositions = { true, true, true, true };
    private Vector3[] _recordedInputPositions;
    public Vector3[][] _currentInputPosition;
    private bool[] _inputMoved;
    private float[] _inputTime;
    private float _runtimeInputMax;
    private float _runtimeInputMin;
    private int[] _inputFrames;
    private int _inputSwipeThreshold;

    // Debug UI stuff
    [Header("Debug stuff")]
    public Text DebugText;
    public GameObject TouchUIDotCurrent;
    public GameObject TouchUIDotRecorded;
    public Transform TouchCanvas;

    private GameObject[] _uiRecord;
    private GameObject[] _uiCurrent;


    // Start is called before the first frame update
    void Start()
    {
        _recordedInputPositions = new Vector3[4];
        _inputTime = new float[4];
        _uiRecord = new GameObject[4];
        _uiCurrent = new GameObject[4];
        _inputMoved = new bool[4];

        _inputFrames = new int[4];



        MoveDirectionSO.Value = Vector3.zero;
        // Disable Multitouch for the phone touch to fix problems with multiple touches. However, multiple touches should be implemented at a later stage.
        Input.multiTouchEnabled = true;



        _currentInputPosition = new Vector3[4][];
        for (int i = 0; i < 4; i++)
        {

            _inputFrames[i] = Mathf.RoundToInt(InputSwipeTapTimeSO.Value);

            _currentInputPosition[i] = new Vector3[_inputFrames[i]];

            _inputFrames[i]--;

        }





        _inputSwipeThreshold = Mathf.RoundToInt(Screen.width * (InputSwipeThresholdSO.Value / 100));

       // _inputTouch = new Vector3();

        _runtimeInputMax = Screen.width * (InputMoveMaxThresholdSO.Value / 100);
        _runtimeInputMin = Screen.width * (InputMoveMinThresholdSO.Value / 100);

        if (PlayerControlOverrideSO.Value == false)
        {
        }

        if (UIMenuButton != null)
        {

            _uiOffset = new Vector2(UIMenuButton.position.x - (UIMenuButton.sizeDelta.x/2), UIMenuButton.position.y + (UIMenuButton.sizeDelta.y / 2));
            //Debug.Log("UI Position: " + _uiOffset);
            //Debug.Log("sizeDelta: " + UIMenuButton.sizeDelta.x / 2);
        }
    }


    public override void LoopUpdate(float deltaTime)
    {
        if (PlayerControlOverrideSO.Value == false)
        {
            if (IsStunned.Value == false)
            {
                // Detect Touch
                if (Input.touchCount <= _recordPositions.Length)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);


                        Vector3 touchPosition = touch.position;


                            if (touchPosition.x < _uiOffset.x || touchPosition.y > _uiOffset.y)
                            {
                                switch (touch.phase)
                                {
                                    case TouchPhase.Moved:
                                        BeginMove(touchPosition, i);
                                        break;
                                    case TouchPhase.Ended:
                                        EndMove(touchPosition, i);
                                        break;
                                }
                            }


                    }
                }




                // Simulate touch with mouse, if mouse present
                if (Input.mousePresent)
                {
                    Vector3 inputPosition = Input.mousePosition;

                    if (Input.GetMouseButton(0))
                    {

                            if (inputPosition.x < _uiOffset.x || inputPosition.y > _uiOffset.y)
                            {
                                BeginMove(inputPosition,0);
                            }


                    }
                    if (Input.GetMouseButtonUp(0))
                    {

                            if (inputPosition.x < _uiOffset.x || inputPosition.y > _uiOffset.y)
                            {
                                EndMove(Input.mousePosition,0);
                            }

                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < _uiCurrent.Length; i++)
            {
                if (_uiCurrent[i] != null)
                {
                    MoveDirectionSO.Value = Vector3.zero;
                    ClearInputUI(i);
                }
            }
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {

    }

    private void PopulatePositionIndex(Vector3[] inputArray,int index)
    {
        for (int i = 0; i < _inputFrames[index]; i++)
        {
            inputArray[i] = inputArray[i+1];
        }
    }


    private void BeginMove(Vector3 inputPosition, int index)
    {
        PopulatePositionIndex(_currentInputPosition[index], index); // Move old positions one frame backwards in array

        if(inputPosition.x < _uiOffset.x || inputPosition.y > _uiOffset.y)
        {
            ReturnInputPosition(inputPosition, index); // Record Start Pos

            ReturnInputPosition(inputPosition, index); // Recording Current Pos
        }


        //DebugText.text = "MOVING!";

        _inputMoved[index] = true;


        if (Vector3.Distance(_currentInputPosition[index][_inputFrames[index]],_recordedInputPositions[index]) > InputMoveMinThresholdSO.Value)
        {
            Vector3 heading;
            float distance;
            Vector3 direction;

            heading = _currentInputPosition[index][_inputFrames[index]] - _recordedInputPositions[index];

            // Calculates the outer rim position of the "joystick" if the player moves the finger beyond the borders of the designated joystick space
            float x = 0f;
            float y = 0f;
            float a = Mathf.Atan2(heading.y, heading.x);

            float circleCalc = (heading.x * heading.x) + (heading.y * heading.y);

            if (circleCalc > (_runtimeInputMax * _runtimeInputMax))
            {
                x = _runtimeInputMax * Mathf.Cos(a);
                heading.x = x;

                y = _runtimeInputMax * Mathf.Sin(a);
                heading.y = y;
            }
            else
            {
                x = heading.x;
                y = heading.y;
            }

            // Normalize the direction and speed vector
            distance = heading.magnitude;
            direction = heading.normalized;

            // UI debug stuff
            _uiCurrent[index].transform.localPosition = new Vector3(x, y);

            // Export direction and speed vector to the PlayerSpeedDirectionSO
            MoveDirectionSO.Value.x = direction.x;
            MoveDirectionSO.Value.z = direction.y;
        }
    }


    private void ReturnInputPosition(Vector3 touchPos,int index)
    {
        if(_recordPositions[index]) // Record first position of touch
        {
            _recordedInputPositions[index] = touchPos;
            _inputTime[index] = Time.time;
            _recordPositions[index] = false;

            // UI Debug Stuff
            _uiRecord[index] = Instantiate(TouchUIDotRecorded, TouchCanvas);
            _uiRecord[index].transform.position = touchPos;
        }
        else
        {
            _currentInputPosition[index][_inputFrames[index]] = touchPos;
        }

        // UI Debug Stuffv
        if (_uiCurrent[index] == null)
        {
            _uiCurrent[index] = Instantiate(TouchUIDotCurrent, _uiRecord[index].transform.position, Quaternion.identity, _uiRecord[index].transform);
        }
        else
        {
            _uiCurrent[index].transform.position = _uiRecord[index].transform.position;
        }
    }


    private void EndMove(Vector3 touchPosition, int index)
    {
        ClearInputUI(index);

        // Check if TAP has happened
        if (Vector3.Distance(_currentInputPosition[index][_inputFrames[index]], _recordedInputPositions[index]) < _runtimeInputMin)
        {
            float endTime = Time.time - _inputTime[index];

            if (endTime < InputSwipeTapTimeSO.Value)
            {
                //Debug.Log("ATTACKED!");
                AttackTapSO.Raise(PlayerGraphics);
            }
        }
        // Check if SWIPE has happened
        else if (Vector3.Distance(_currentInputPosition[index][0], _currentInputPosition[index][_inputFrames[index]]) > _inputSwipeThreshold)
        {
            //DebugText.text = "DODGED!";
            Swipe.Raise(PlayerGraphics);
        }

        // Check if MOVE has happened
        if (_inputMoved[index])
        {
            //DebugText.text = "MOVED!";
        }

        MoveDirectionSO.Value = Vector3.zero;
        _inputMoved[index] = false;
        _recordPositions[index] = true;
        _inputFrames[index] = Mathf.RoundToInt(InputSwipeTapTimeSO.Value);
        _currentInputPosition[index] = new Vector3[_inputFrames[index] + 1];
    }

    private void ClearInputUI(int index)
    {
        // UI Debug Stuff
        if (_uiRecord[index] != null)
        {
            Destroy(_uiRecord[index]);
        }

        if (_uiCurrent[index] != null)
        {
            Destroy(_uiCurrent[index]);
        }
    }

}
