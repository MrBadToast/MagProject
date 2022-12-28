using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MagGun : MonoBehaviour
{

    [SerializeField] private Animator anim;
    [SerializeField] LayerMask magLayer;
    [SerializeField] GameObject cubeObj;

    public void OnFire_Pos()
    {
        anim.SetTrigger("Fire");
        RaycastHit hit;
        Camera cam = Camera.main;
        if (!Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, 100f, magLayer)) return;

        MagObject mag;
        if(hit.collider.TryGetComponent(out mag))
        {
            mag.SwitchMagState(MagType.POSITIVE);
        }
    }

    public void OnFire_Neg()
    {
        anim.SetTrigger("Fire");
        RaycastHit hit;
        Camera cam = Camera.main;
        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, magLayer)) return;

        if (hit.IsUnityNull()) return;

        MagObject mag;
        if (hit.collider.TryGetComponent(out mag))
        {
            mag.SwitchMagState(MagType.NEGATIVE);
        }
    }

    public void OnSpawnCube()
    {
        anim.SetTrigger("Fire");
        RaycastHit hit;
        Camera cam = Camera.main;
        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f)) return;

        GameObject.Instantiate(cubeObj, hit.point + Vector3.up, Quaternion.identity);
    }

    public void OnEscape()
    {
        Application.Quit();
    }
}
