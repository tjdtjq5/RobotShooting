using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronSpawn : Singleton<DronSpawn>
{
    [SerializeField] GameObject dronObj;

    public void Spawn()
    {
        Instantiate(dronObj, this.transform);
    }

    public void AllDestroy()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }
}
