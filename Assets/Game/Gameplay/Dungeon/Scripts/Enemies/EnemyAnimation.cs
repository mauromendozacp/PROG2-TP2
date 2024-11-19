using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetAnimator(string name, bool value)
    {
        _anim.SetBool(name, value);
    }

    public void SetAnimator(string name)
    {
        _anim.SetTrigger(name);
    }

    public int GetCurrentAnimationStateHash()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        return _anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
    }

    public void Play(int animationStateHash)
    {
        _anim.Play(animationStateHash);
    }
}
