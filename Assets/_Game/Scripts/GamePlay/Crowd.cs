using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    public bool initOnStart;

    public int startCount;
    public GPUFlock gPUFlockBrid;
    [SerializeField] SphereCollider _sphereCollider;
    [SerializeField] Material _bridMatrial;
    protected bool _onFight;
    protected bool _canMove = true;

    public int bridsCount => gPUFlockBrid.boidsCount;

    protected void Start()
    {
        if (initOnStart)
            Init();
    }

    public virtual void Init()
    {
        gPUFlockBrid.FirstSpawn(startCount);

        foreach (var item in gPUFlockBrid.boidsGo)
        {
            item.SetMaterial(_bridMatrial);
        }
        ResetSphereColliderRaduis();
    }

    public void Collect(FreeBrid freeBrid)
    {
        if (gPUFlockBrid.enabled)
        {
            FreeBridSpawner.instance.RemovedBrid(freeBrid);
            CollectFrom(freeBrid);
            freeBrid.SetMaterial(_bridMatrial);
        }
    }

    public void CollectFrom(FreeBrid freeBrid)
    {
        freeBrid.Free(false);
        GPUBoid gPUBoid = gPUFlockBrid.CreateBoidDataAtPosition(freeBrid.transform.position);
        gPUFlockBrid.AddBoidsGo(gPUBoid, freeBrid);
        ResetSphereColliderRaduis();
    }

    public void StartFight(Crowd crowd,bool goinTowin)
    {
        if (!_onFight)
        {
            _onFight = true;
            if (goinTowin)
            {
                foreach (var item in crowd.gPUFlockBrid.boidsGo)
                {
                    CollectFrom(item);
                }

                StartCoroutine(EnumeratorAddCrowd(crowd.gPUFlockBrid.boidsGo));

                _onFight = false;
            }
            else
            {
                _canMove = false;
                gPUFlockBrid.enabled = false;
                Destroy(gameObject, 0.5f);
            }
        }
    }

    IEnumerator EnumeratorAddCrowd(List<FreeBrid> freeBrid)
    {
        foreach (var item in freeBrid)
        {
            yield return new WaitForSeconds(0.1f);
            item.SetMaterial(_bridMatrial);
        }
    }

    protected virtual void UpdateSphereTriger()
    {
        _sphereCollider.transform.position = gPUFlockBrid.centreBoids;
    }

    void ResetSphereColliderRaduis() => _sphereCollider.radius = Interpolation(10, 10f, 1000, 40, startCount);

    public  float Interpolation(float xa, float ya, float xb, float yb, float newX)
    {
        return ya + (newX - xa) * (yb - ya) / (xb - xa);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FreeBrid")
        {
            Collect(other.GetComponent<FreeBrid>());
        }

        if(other.tag == "Crawd")
        {
            if(!_onFight)
            {
                print("name : " + other.name);
                Crowd crowd = other.GetComponentInParent<Crowd>();
                if (crowd != this && !crowd._onFight)
                {
                    bool win = (bridsCount > crowd.bridsCount);

                    StartFight(crowd, win);
                    crowd.StartFight(this, !win);
                }
            }
        }
    }

}
