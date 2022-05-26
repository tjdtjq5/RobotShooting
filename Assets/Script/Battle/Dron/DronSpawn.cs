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
}
