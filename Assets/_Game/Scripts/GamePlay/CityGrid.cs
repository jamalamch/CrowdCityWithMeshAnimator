using MatrixAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGrid : MonoBehaviour
{
    public Bool2dArray matrix;
    public float gridSize;

    [SerializeField] GameObject _cube;
    [SerializeField] Player _player;

    Texture2D _texture2D;

    void Start()
    {
        //for (int i = 0; i < matrix.cols; i++)
        //{
        //    for (int j = 0; j < matrix.rows; j++)
        //    {
        //        if (matrix[i, j])
        //        {
        //            GameObject cube = Instantiate(_cube, transform);
        //            cube.transform.position = GetPosition(i, j) + Vector3.one * gridSize * 0.5f;
        //        }
        //    }
        //}
    }

    public int[] GetIntArray()
    {
        int[] matrixInt = new int[matrix.length];
        for (int i = 0; i < matrixInt.Length; i++)
        {
            matrixInt[i] = (matrix[i]) ? 1 : 0;
        }
        return matrixInt;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            (int, int) gredPlayer = GetCellIndex(_player.transform.position);
            Gizmos.DrawWireCube(GetPosition(gredPlayer.Item1, gredPlayer.Item2) + new Vector3(gridSize * 0.5f,0.5f, gridSize * 0.5f), new Vector3(gridSize,1, gridSize));
        }
        Gizmos.color = Color.green;
        for (int i = 0; i < matrix.cols; i++)
        {
            for (int j = 0; j < matrix.rows; j++)
            {
                if (matrix[i, j])
                {
                    Gizmos.DrawWireCube(GetPosition(i,j) + Vector3.one* gridSize*0.5f, Vector3.one * gridSize);
                }
            }
        }
    }

    internal Texture2D GeTextureArray()
    {
        if (_texture2D)
            return _texture2D;


        _texture2D = new Texture2D(matrix.cols, matrix.rows, TextureFormat.RGBA32, false);

        Color one = new Color(1, 1, 1, 1);
        Color zero = new Color(0, 0, 0, 0);

        for (int i = 0; i < matrix.cols; i++)
        {
            for (int j = 0; j < matrix.rows; j++)
            {
                _texture2D.SetPixel(i,j,matrix[i,j]? one: zero);
            }
        }
        _texture2D.Apply();
        return _texture2D;
    }

    public Vector3 GetPosition(int i, int j) => new Vector3(i * gridSize, 0, j * gridSize);
    public (int,int) GetCellIndex(Vector3 position)
    {
        int i = (int)(position.x / gridSize);
        int j = (int)(position.z / gridSize);
        return (i, j);
    }
}
