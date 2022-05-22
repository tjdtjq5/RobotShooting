using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextSO", menuName = "Scriptable Object/TextSO")]
public class TextSO : ScriptableObject
{
    public List<TextData> textDatas = new List<TextData>();
}
[System.Serializable]
public class TextData
{
    public string code;
    public string ko;
    public string en;
    public string jp;
    public string cn;
}
