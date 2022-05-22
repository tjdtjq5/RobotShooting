using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AniManager : MonoBehaviour
{
    public Animator _anim;
    string c_aniName;

    private void Awake()
    {
        InitAnimPair();
    }
   
    private AnimationClip[] _animationClips;
    private Dictionary<string, int> _nameToHashPair = new Dictionary<string, int>();

    private void InitAnimPair()
    {
        _nameToHashPair.Clear();
        _animationClips = _anim.runtimeAnimatorController.animationClips;
        foreach (var clip in _animationClips)
        {
            int hash = Animator.StringToHash(clip.name);
            _nameToHashPair.Add(clip.name, hash);
        }
    }

    Sequence playSequence;

    [Button("PlayAnimation")]
    // 이름으로 애니메이션 실행
    public float PlayAnimation(string name, bool isSkip = true, System.Action _callback = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            return 0;
        }
        if (name == c_aniName && isSkip)
        {
            return 0;
        }

        if (playSequence != null)
            playSequence.Kill();

        c_aniName = name;
        foreach (var animationName in _nameToHashPair)
        {
            if (animationName.Key.ToLower() == name.ToLower())
            {
                _anim.Play(animationName.Value, 0);
            }
        }
        for (int i = 0; i < _anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (_anim.runtimeAnimatorController.animationClips[i].name == name)
            {
                float time = _anim.runtimeAnimatorController.animationClips[i].length;
                playSequence = DOTween.Sequence();
                playSequence.InsertCallback(time, () => {
                    if (_callback != null)
                        _callback();
                });
                return time;
            }
        }
        return 0;
    }

    public void SetTriger(string _triger)
    {
        _anim.SetTrigger(_triger);
    }

    public void AniSpeed(float _speed)
    {
        _anim.speed = _speed;
    }
}
