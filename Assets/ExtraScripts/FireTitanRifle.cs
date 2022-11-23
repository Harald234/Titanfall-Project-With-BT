using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class FireTitanRifle : NetworkBehaviour
{
    public EnterVanguardTitan enterScript;
    public VanguardMovement moveScript;

    public Animator Arms;
    public Animator titanAnimator;
    public Animator XO16Animator;
    public Animator secondXO16Animator;

    public Camera cam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlashSecondRifle;
    public GameObject impactEffect;

    bool canShoot;
    bool readyToShoot = true;
    bool isReloading = false;

    public float timeBetweenShots, timeBetweenShooting;
    public float spread;

    public int bulletsPerTap, bulletsLeft;
    int bulletsShot;

    void HandleInput()
    {
        canShoot = Input.GetKey(KeyCode.Mouse0);

        if (readyToShoot && canShoot && bulletsLeft > 0 && !isReloading)
        {
            bulletsShot = bulletsPerTap;
            ShootRPC();
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < 500 && !moveScript.isDashing)
        {
            StartCoroutine(Reload());
        }
    }

    void Update()
    {
        if (enterScript.inTitan)
        {
            HandleInput();
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void ShootRPC()
    {
        readyToShoot = false;

        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);

        Vector3 direction = cam.transform.forward + new Vector3(xSpread, ySpread, 0);

        muzzleFlash.Play();
        muzzleFlashSecondRifle.Play();

        if (Physics.Raycast(cam.transform.position, direction, out RaycastHit hit))
        {
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.identity) as GameObject;
            impact.transform.forward = hit.normal;

            Destroy(impact, 1.5f);

            if (hit.collider.CompareTag("Enemy"))
            {
                StartCoroutine(hit.collider.GetComponent<RoninMovement>().TakeDamage(10));
            }
        }

        bulletsLeft--;
        bulletsShot--;

        if (!IsInvoking("ResetShot") && !readyToShoot)
        {
            Invoke("ResetShot", timeBetweenShooting);
        }

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("ShootRPC", timeBetweenShots);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Arms.SetTrigger("reload");
        titanAnimator.SetTrigger("reload");
        XO16Animator.SetTrigger("reload");
        secondXO16Animator.SetTrigger("reload");

        yield return new WaitForSeconds(2.5f);

        bulletsLeft = 500;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
}