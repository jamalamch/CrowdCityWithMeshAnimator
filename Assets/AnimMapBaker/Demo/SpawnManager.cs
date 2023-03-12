using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// demo使用，分配大量的活动的角色
/// </summary>
public class SpawnManager : MonoBehaviour {

    #region 字段

    public GameObject spawnPrefab;
    public int gridWidth;
    public int gridHeight;

    public int countMatrial = 10;
    public float marialOffset = 0.25f;

    Material[] materials;

    #endregion


    #region 方法

    void Start()
    {
        Material material = spawnPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        materials = new Material[countMatrial];
        materials[0] = material;

        float offset = marialOffset;

        for (int i = 1; i < countMatrial; i++)
        {
            materials[i] = new Material(material);
            materials[i].SetFloat("_AnimOffcet", offset);

            offset += marialOffset;
        }

        for (var i = 0; i < gridWidth; i++)
        {
            for(var j = 0; j < gridHeight; j++)
            {
                var GameObject = Instantiate<GameObject>(spawnPrefab, new Vector3(i, 0, j), Quaternion.identity);
                GameObject.GetComponent<MeshRenderer>().sharedMaterial = materials[Random.Range(0, materials.Length)];
            }
        }
    }


    #endregion

}
