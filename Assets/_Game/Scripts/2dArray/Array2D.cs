
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixAlgebra
{
    [System.Serializable]
    public class Array2D<T>
    {
        public int length => m.Length;
        public int rows => y;
        public int cols => x;
        public int x, y;
        public T[] m;
        public Array2D()
        {
        }

        public Array2D(int width, int height)
        {
            x = width;
            y = height;
            m = new T[y * x];
        }

        public Array2D(int width, int height, T[] elemnts)
        {
            x = width;
            y = height;
            m = elemnts;
        }

        public Array2D(Array2D<T> array2D) : this (array2D.x, array2D.y)
        {
            Array.Copy(array2D.m, m, m.Length);
        }

        public T this[int index]
        {
            get { return m[index]; }
            set { m[index] = value; }
        }
        public T this[int i, int j]
        {
            get { return m[j * cols + i]; }
            set { m[j * cols + i] = value; }
        }

        public void Clear()
        {
            m = new T[y * x];
        }
    }

    [System.Serializable]
    public class PPin2dObjectArr : Array2D<GameObject>
    {
        public PPin2dObjectArr(int width, int height) : base(width, height)
        {
        }
    }

    [System.Serializable]
    public class PPin2dGroundArr : Array2D<Ground>
    {
        public PPin2dGroundArr(int width, int height) : base(width, height)
        {
        }
    }
}