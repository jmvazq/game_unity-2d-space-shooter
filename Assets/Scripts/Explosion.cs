using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _defaultDuration = 3.0f;
    private Animator _anim;

    void Start()
    {
        float duration = _defaultDuration;

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log(name + "'s Animator component not found!");
        } else
        {
            duration = _anim.GetCurrentAnimatorStateInfo(0).length;
        }

        Destroy(this.gameObject, duration);
    }
}
