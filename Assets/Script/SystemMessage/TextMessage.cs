using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class TextMessage : Singleton<TextMessage>
{
    public List<GameObject> objs = new List<GameObject>();

    public Image bg;
    public Text text;

    float fadeTime = 0.35f;
    float waitTime = 1.6f;

    Sequence sequence;

    [Button("Show")]
    public void Show(string _script , System.Action _callback = null)
    {
        text.text = _script;
        text.font = Language.Instance.GetFont();

        for (int i = 0; i < objs.Count; i++)
            objs[i].SetActive(true);

        if (sequence != null)
        {
            sequence.Kill();
        }

        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);

        sequence = DOTween.Sequence();
        sequence.Insert(0, bg.DOFade(0.7f, fadeTime))
                .Insert(0, text.DOFade(1, fadeTime))
                .Insert(waitTime + fadeTime, bg.DOFade(0, fadeTime))
                .Insert(waitTime + fadeTime, text.DOFade(0, fadeTime))
                .InsertCallback(waitTime + fadeTime + fadeTime, () => {
                    for (int i = 0; i < objs.Count; i++)
                        objs[i].SetActive(false);
                    if (_callback != null)
                    {
                        _callback();
                    }
                })
                .Play();
    }

    Sequence testSequence;
    [Button("Test")]
    void Test()
    {
        if (testSequence != null)
        {
            testSequence.Kill();
        }
        testSequence = DOTween.Sequence();
        testSequence.InsertCallback(1, () => Debug.Log(111)).SetLoops(-1, LoopType.Incremental);
        testSequence.Play();
    }

}
