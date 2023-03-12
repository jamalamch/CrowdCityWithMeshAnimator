using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Crowd
{
    [SerializeField] float _rbSpeed = 0.5f;

    TouchPad _touchPad;

    Vector3 _normal;

    public override void Init()
    {
        base.Init();
        _touchPad = TouchPad.instance;

        _normal = Vector3.forward;
    }

    void Update()
    {
        if(_canMove)
            MovementUpdate();

        base.UpdateSphereTriger();
    }

    private void MovementUpdate()
    {
        if (_touchPad.velocityDirection.magnitude > Mathf.Epsilon)
        {
            _normal = _touchPad.velocityDirection.normalized;
        }

        Vector3 newForward = Quaternion.Euler(0, 45, 0) * _normal;
        transform.forward = Vector3.Lerp(transform.forward, newForward, 0.5f);
        transform.position = transform.position + newForward * _rbSpeed * Time.deltaTime;
    }
}
