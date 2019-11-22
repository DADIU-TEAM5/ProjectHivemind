using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnimation : MonoBehaviour
{
    public Animator Animator;

    public void Open() {
        Animator.SetBool("IsOpen", true);
    }

    public void Close() {
        Animator.SetBool("IsOpen", false);
    }
}
