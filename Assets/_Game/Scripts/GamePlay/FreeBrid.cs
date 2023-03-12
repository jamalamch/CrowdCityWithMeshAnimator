using UnityEngine;

public class FreeBrid : MonoBehaviour
{
    [SerializeField] Collider _collider;
    [SerializeField] MeshRenderer _meshRenderer;

    float _offsetAnim;
    float _animLerp = 1;

    void Start()
    {
        _offsetAnim = Random.value;
    }

    public void Free(bool free)
    {
        _collider.enabled = free;
        if (free) 
        {
            tag = "FreeBrid";
        }
        else
        {
            tag = "Brid";
        }
    }

    internal void SetMaterial(Material bridMatrial)
    {
        _meshRenderer.material = bridMatrial;
        _meshRenderer.material.SetFloat("_AnimOffcet", _offsetAnim);
    }

    internal void SetAnimaLerp(float animLerp)
    {
        _animLerp = animLerp;
        _meshRenderer.material.SetFloat("_AnimLerp", _animLerp);
    }
}
