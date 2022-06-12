using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgSpawn : Singleton<DmgSpawn>
{
    public GameObject obj;

    List<GameObject> objList = new List<GameObject>();

    public void Spawn(Vector2 _pos ,string _dmgScript, bool isCri, bool isSupper)
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

        temp.GetComponent<DmgObj>().Spawn(_pos, _dmgScript, isCri, isSupper);
    }

    public void Spawn(Transform _pos, string _dmgScript, bool isCri, bool isSupper)
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

        temp.GetComponent<DmgObj>().Spawn(_pos.position, _dmgScript, isCri, isSupper);
    }

    public void MissSpawn(Vector2 _pos)
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

        temp.GetComponent<DmgObj>().MissSpawn(_pos);
    }
}
