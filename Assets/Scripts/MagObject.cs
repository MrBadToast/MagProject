using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagType
{
    NONE,
    POSITIVE,
    NEGATIVE
}

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class MagObject : MonoBehaviour
{
    public float Mass { get { return rBody.mass; }}
    [SerializeField] private bool InfiniteWeight = false;       // 움직이지 않는 고정형 물체에 대한 여부
    [SerializeField] private float AffectDiameter = 10.0f;      // 영향 범위
    [SerializeField] private int MaxInteraction = 10;           // 최대 감지 오브젝트 (최적화용)
    [HideInInspector] public MagType currentMag = MagType.NONE;

    Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MagObject affectedObejct;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, AffectDiameter, Vector3.up ,AffectDiameter, LayerMask.GetMask("MagObject"));
        Debug.Log("hits : " + hits.Length);

        for (int i = 0; i < hits.Length && i < MaxInteraction; i++)
        {
            if (hits[i].collider.TryGetComponent(out affectedObejct))
            {
                affectedObejct.AffectObject(transform.position, this.currentMag);
            }
        }
    }

    static float magPower = 50f;

    public void AffectObject(Vector3 target, MagType targetType)
    {
        if (currentMag == MagType.NONE || targetType == MagType.NONE) return;

        if (InfiniteWeight || rBody.isKinematic) return;

        Debug.Log("AFFECTED");

        if(currentMag == targetType)
        {
            rBody.AddForce((transform.position - target).normalized * magPower / Mass);
        } else
        {
            rBody.AddForce((target - transform.position).normalized * magPower / Mass);
        }

    }

    public void SwitchMagState(MagType type)
    {
        MeshRenderer render;
        Material mat;
        MaterialPropertyBlock matBlock;
        matBlock = new MaterialPropertyBlock();

        if (TryGetComponent(out render))
        {
            mat = render.materials[1];
        }     

        if(type == currentMag)
        {
            matBlock.SetInt("_Enabled", 0);
            currentMag = MagType.NONE;
        }
        else if(type == MagType.POSITIVE)
        {
            matBlock.SetInt("_Enabled", 1);
            matBlock.SetColor("_Color", Color.red);
            currentMag = MagType.POSITIVE;
        }
        else if (type == MagType.NEGATIVE)
        {
            matBlock.SetInt("_Enabled", 1);
            matBlock.SetColor("_Color", Color.blue);
            currentMag = MagType.NEGATIVE;
        }
        else if(type == MagType.NONE)
        {
            matBlock.SetInt("_Enabled", 0);
            currentMag = MagType.NONE;
        }

        render.SetPropertyBlock(matBlock);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AffectDiameter);
    }
}
