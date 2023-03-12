using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeBridSpawner : MonoBehaviour
{
    public static FreeBridSpawner instance;

    [SerializeField] FreeBrid _freeBridGenerator;
    [SerializeField] CityGrid _cityGrid;
    [SerializeField] float _timeToSpwan;
    [SerializeField] int _targetCount;
    [SerializeField] float _spawnRadius;


    [SerializeField] private ComputeShader cshader;
    [SerializeField] private Texture2D noiseTexture;
    private int _kernelHandle;

    [SerializeField] private List<FreeBrid> _boidsFree;
    [SerializeField] private GPUFreeBoid[] _boidsData;

    float _timer;
    int _count;
    bool _enableCShader = true;

    public PoolComponent<FreeBrid> poolBrid { get; set;}
    public CityGrid grid => _cityGrid;
    void Awake()
    {
        instance = this;
        poolBrid = new PoolComponent<FreeBrid>(_freeBridGenerator, 50, this);

        _boidsFree = new List<FreeBrid>();

        FirstSpawn(_targetCount);
    }

    [ContextMenu("Calue avretge noise")]
    void CalculeAvregeOffTexture()
    {
        float r = 0;
        for (int i = 0; i < noiseTexture.width; i++)
        {
            for (int j = 0; j < noiseTexture.height; j++)
            {
                r += noiseTexture.GetPixel(i, j).r - 0.4518434f;
            }
        }
        print("Avrege Value : " + r / (noiseTexture.width * noiseTexture.height));
    }

    public void FirstSpawn(int firstCount)
    {
        _boidsData = new GPUFreeBoid[firstCount];

        for (int i = 0; i < firstCount; i++)
        {
            FreeBrid freeBrid = poolBrid.Pick();

            int rX = Random.Range(0, _cityGrid.matrix.cols);
            int rY = Random.Range(0, _cityGrid.matrix.rows);

            while (_cityGrid.matrix[rX,rY])
            {
                rX = Random.Range(0, _cityGrid.matrix.cols);
                rY = Random.Range(0, _cityGrid.matrix.rows);
            }

            Vector3 Rpo = _cityGrid.GetPosition(rX, rY);

            var pos = new Vector3(Rpo.x + Random.value * _cityGrid.gridSize, 0, Rpo.z + Random.value * _cityGrid.gridSize);
            GPUFreeBoid gPUFree = CreateBoidDataAtPosition(pos);
            freeBrid.transform.position = pos;
            freeBrid.transform.rotation = Quaternion.LookRotation(gPUFree.rot);
            _boidsFree.Add(freeBrid);
            _boidsData[i] = gPUFree;
        }

        _count = firstCount;

        if (_enableCShader)
        {
            _kernelHandle = cshader.FindKernel("CSMain");

            CityGrid cityGrid = grid;
            Texture gridMapArray = cityGrid.GeTextureArray();
            int rows = cityGrid.matrix.rows;
            int cols = cityGrid.matrix.cols;
            float gridSize = cityGrid.gridSize;

            cshader.SetTexture(_kernelHandle, "grids", gridMapArray);
            cshader.SetTexture(_kernelHandle, "noise", noiseTexture);
            cshader.SetInt("rows", rows);
            cshader.SetInt("cols", cols);
            cshader.SetFloat("gridSize", gridSize);
        }
    }

    public void AddNewBrid()
    {
        FreeBrid freeBrid = poolBrid.Pick();
        var pos = transform.position + Random.insideUnitSphere * _spawnRadius;
        pos.y = 0;
        GPUFreeBoid gPUFree = CreateBoidDataAtPosition(pos);
        freeBrid.transform.position = pos;
        freeBrid.transform.rotation = Quaternion.LookRotation(gPUFree.rot);
        _boidsFree.Add(freeBrid);

        System.Array.Resize(ref _boidsData, _boidsData.Length + 1);
        _boidsData[_boidsData.Length - 1] = gPUFree;
        _count++;
    }

    public void RemovedBrid(FreeBrid freeBrid)
    {
        int index = _boidsFree.IndexOf(freeBrid);
        _boidsFree.RemoveAt(index);

        for (int i = index; i < _boidsData.Length - 1; i++)
        {
            _boidsData[i] = _boidsData[i + 1];
        }

        System.Array.Resize(ref _boidsData, _boidsData.Length - 1);
    }

    public GPUFreeBoid CreateBoidDataAtPosition(Vector3 pos)
    {
        var boidData = new GPUFreeBoid();
        pos.y = 0f;
        boidData.pos = pos;
        boidData.noise = Random.value * 100;
        boidData.deg = Random.value * 2 * Mathf.PI;

        Vector3 rot = Vector3.zero;
        rot.x = Mathf.Cos(boidData.deg);
        rot.z = Mathf.Sin(boidData.deg);

        boidData.rot = rot;
        return boidData;
    }


    void Update()
    {
        //if(_timer < Time.time)
        //{
        //    _timer += _timeToSpwan;
        //    if (_count < _targetCount)
        //    {
        //        _count++;

        //   }
        //}

        if (!_enableCShader)
            return;

        var buffer = new ComputeBuffer(_count, 32);
        buffer.SetData(_boidsData);

        cshader.SetBuffer(_kernelHandle, "boidBuffer", buffer);
        cshader.SetFloat("deltaTime", Time.deltaTime);
        cshader.Dispatch(_kernelHandle, _count, 1, 1);
        buffer.GetData(_boidsData);
        buffer.Release();


        for (int i = 0; i < _boidsData.Length; i++)
        {
            _boidsFree[i].transform.position = _boidsData[i].pos;

            if (!_boidsData[i].rot.Equals(Vector3.zero))
            {
                _boidsFree[i].transform.rotation = Quaternion.LookRotation(_boidsData[i].rot);
            }
        }
    }

    public void Removed(FreeBrid freeBrid)
    {
        
    }
}
