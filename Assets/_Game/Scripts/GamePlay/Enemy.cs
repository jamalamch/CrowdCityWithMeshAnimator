using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Crowd
{
    [SerializeField] float _rbSpeed = 0.5f;

    [SerializeField] float _noiseSteep, _noiseMagh;
    float _noisX;
    float _deg;
    Vector3 _noiseNormal;

    public override void Init()
    {
        base.Init();
        _noisX = Random.value * 100;
        _deg = Random.value * 2 * Mathf.PI;
    }

    void Update()
    {
        if (_canMove)
            MovementUpdate();

        base.UpdateSphereTriger();
    }

    private void MovementUpdate()
    {
        _noisX += _noiseSteep * Time.deltaTime;
        _deg += (Mathf.PerlinNoise(_noisX, 0) - 0.5f) * _noiseMagh;
        _noiseNormal.x = Mathf.Cos(_deg);
        _noiseNormal.z = Mathf.Sin(_deg);

        Vector3 target = transform.position + _noiseNormal * _rbSpeed * Time.deltaTime;

        int indexX = (int)(target.x / 50f);
        int indexY = (int)(target.z / 50f);

        transform.rotation = Quaternion.LookRotation(_noiseNormal);

        if (indexX >= 0 && indexY >= 0 && indexY < 20 && indexX < 20)
        {
            if (!FreeBridSpawner.instance.grid.matrix[indexY * 20 + indexX])
            {
                transform.position = target;
            }
        }
    }
}
