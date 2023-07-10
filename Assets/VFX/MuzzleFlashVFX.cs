using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MuzzleFlashVFX : MonoBehaviour
{

    [SerializeField] private VisualEffect muzzleVFX;
    [SerializeField] private float fireDelay;
    [SerializeField] private GameObject light;

    // Start is called before the first frame update
    private void Start()
    {
        muzzleVFX = GetComponent<VisualEffect>();
        fireDelay = muzzleVFX.GetFloat("Fire_Delay");
        light = transform.GetChild(0).gameObject;
        fireDelay -= 0.1f;

        StartCoroutine(fire());
    }

    private IEnumerator fire()
    {
        while (true)
        {
            light.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            light.SetActive(false);
            yield return new WaitForSeconds(fireDelay);

            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                muzzleVFX.SetFloat("Height_Collide", hit.distance);
            }
        }
    }
}
