using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(FollowController))]
public class MonsterAnimator : MonoBehaviour
{
    private Animator _animator;
    readonly string[] NAMES =
    {
        "Movement_idle",
        "Movement_walk",
        "Movement_run"
    };
    
    public void generateAnimator(string id)
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Templates/animTemplate") as RuntimeAnimatorController;
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = animatorOverrideController;
        foreach (var s in NAMES)
        {
            string path = "Monsters/"+id + "/Files/Animations/" + s;
            var animClip = Resources.Load<AnimationClip>(path);
            if (animClip != null)
            {
                animatorOverrideController[s] = animClip;
            }
        }
        
    }
    
}
