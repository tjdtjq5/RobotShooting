using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSpawn : Singleton<BreakSpawn>
{
    public GameObject obj;

    List<GameObject> objList = new List<GameObject>();

    public void Spawn(Vector2 _pos)
    {
        GameObject temp = null;
        for (int i = 0; i < objList.Count; i++)
        {
            if (!objList[i].activeSelf)
            {
                objList[i].SetActive(true);
                temp = objList[i];
                break;
            }
        }
        if (temp == null)
        {
            temp = Instantiate(obj, this.transform);
            objList.Add(temp);
        }

        temp.GetComponent<BreakObj>().Spawn(_pos);
    }
}
