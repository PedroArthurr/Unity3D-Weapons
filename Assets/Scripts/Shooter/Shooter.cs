#region Libraries
using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
#endregion

public class Shooter : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private SO_Weapon weapon; //SO Asset reference

    [SerializeField] private Transform gunMuzzle;

    [SerializeField] private TextMeshProUGUI bulletCount; //TMPro text asset
    
    [Header("Settings")]
    
    [SerializeField] private KeyCode shotKey;


    private float _time;

    private int _bulletsInMagazine;

    private bool _canShot = true;

    private GunType _currentGunType;
    
    private int _burstCount;

    private void Awake()
    {
        if (weapon) //All the code depends the "weapon" asset
        {
            _bulletsInMagazine = weapon.settings.magazineSize;
            _currentGunType = weapon.settings.gunType;
        }
        else
        {
            Debug.LogError("Add a weapon asset in Inspector !!!!");
        }
    }

    private void Update()
    {
        if (!weapon) return;

        switch (weapon.settings.gunType)
        {
            case GunType.Fullauto: 
                ShotFullAuto();
                break;
            case GunType.Semiauto:
                ShotSemiAuto();
                break;
            case GunType.Shotgun:
                ShotShotgun();
                break;
            case GunType.Burst:
                ShotBurst();
                break;
        }

        if (weapon.settings.gunType != _currentGunType) // In case of changing the weapon asset it will start the Reloading Coroutine
        {
            Debug.Log("Changing weapon...");

            _currentGunType = weapon.settings.gunType;
            StartCoroutine(Reload(weapon.settings.reloadTime));
        }

        UpdateHUD();
    }

    void ShotFullAuto()
    {
        if (Input.GetKey(shotKey) && Time.time > _time && _canShot) //Get key for full auto shooting
        {
            CountShot();
            Shot(
                weapon.fullAutoSettings.fireRate,
                weapon.fullAutoSettings.minSpreadX,
                weapon.fullAutoSettings.minSpreadY,
                weapon.fullAutoSettings.minSpreadZ,
                weapon.fullAutoSettings.maxSpreadX,
                weapon.fullAutoSettings.maxSpreadY,
                weapon.fullAutoSettings.maxSpreadZ,
                weapon.fullAutoSettings.muzzleSpeed,
                weapon.fullAutoSettings.bulletPrefab
                );
        }
    }
    
    private void ShotSemiAuto()
    {
        if (Input.GetKeyDown(shotKey) && Time.time > _time && _canShot) //Get Key Down for semi-auto shootiing
        {
            CountShot();
            Shot(
                weapon.semiAutoSettings.fireRate,
                weapon.semiAutoSettings.minSpreadX,
                weapon.semiAutoSettings.minSpreadY,
                weapon.semiAutoSettings.minSpreadZ,
                weapon.semiAutoSettings.maxSpreadX,
                weapon.semiAutoSettings.maxSpreadY,
                weapon.semiAutoSettings.maxSpreadZ,
                weapon.semiAutoSettings.muzzleSpeed,
                weapon.semiAutoSettings.bulletPrefab
            );
        }
    }
    private void ShotShotgun()
    {
        if (Input.GetKeyDown(shotKey) && Time.time > _time && _canShot) //Get Key Down for semi-auto shooting
        {
            CountShot();

            _time = Time.time + weapon.shotgunSettings.fireRate;

            for (int i = 0; i < weapon.shotgunSettings.pelletCount; i++) //instantiate and shoot "pelletCount" times in 1 frame
            {
                Shot(
                    weapon.shotgunSettings.fireRate,
                    weapon.shotgunSettings.minSpreadX,
                    weapon.shotgunSettings.minSpreadY,
                    weapon.shotgunSettings.minSpreadZ,
                    weapon.shotgunSettings.maxSpreadX,
                    weapon.shotgunSettings.maxSpreadY,
                    weapon.shotgunSettings.maxSpreadZ,
                    weapon.shotgunSettings.muzzleSpeed,
                    weapon.shotgunSettings.bulletPrefab
                );
            }
           
        }
    }

    void ShotBurst()
    {
        if (Input.GetKeyDown(shotKey) && Time.time > _time && _canShot)
        {
            _time = Time.time + weapon.burstSettings.fireRate; 
            StartCoroutine(Burst());
        }

        if (_burstCount == weapon.burstSettings.burstLength) //If the weapon fires "burstLength" times it will stop
        {
            StopCoroutine(Burst()); 
            _burstCount = 0;
        }
    }

    IEnumerator Burst()
    {
        while (_burstCount < weapon.burstSettings.burstLength) 
        {
            if(_bulletsInMagazine == 0)
                yield break;

            yield return new WaitForSeconds(weapon.burstSettings.burstFireRate); 
            CountShot();
            ++_burstCount;
            
            Shot(
                weapon.burstSettings.fireRate,
                weapon.burstSettings.minSpreadX,
                weapon.burstSettings.minSpreadY,
                weapon.burstSettings.minSpreadZ,
                weapon.burstSettings.maxSpreadX,
                weapon.burstSettings.maxSpreadY,
                weapon.burstSettings.maxSpreadZ,
                weapon.burstSettings.muzzleSpeed,
                weapon.burstSettings.bulletPrefab
                );
        }
    }

    IEnumerator Reload(float timeToReload)
    {
        Debug.Log("Reloading...");
        _canShot = false;
        yield return new WaitForSeconds(timeToReload);
        Debug.Log("Reloaded !!");
        _bulletsInMagazine = weapon.settings.magazineSize;
        _canShot = true;
    }

    void CountShot()
    {
        --_bulletsInMagazine;
        if (_bulletsInMagazine == 0)
        {
            StartCoroutine(Reload(weapon.settings.reloadTime));
        }
    }
    
    void Shot(
        float fireRate,
        float minSpreadX,
        float minSpreadY,
        float minSpreadZ,
        float maxSpreadX,
        float maxSpreadY,
        float maxSpreadZ,
        float muzzleSpeed,
        GameObject bulletPrefab)
    {
        _time = Time.time + fireRate;
        
        
        /// Create a new random number for spread
        float rX = Random.Range(maxSpreadX, minSpreadX);
        float rY = Random.Range(maxSpreadY, minSpreadY);
        float rZ = Random.Range(maxSpreadZ, minSpreadZ);
        /// Create a new random number for spread
       
        
        GameObject newBullet = Instantiate(bulletPrefab, gunMuzzle.transform.position, transform.rotation); //Instantiate the bullet in muzzle position in this rotation
        Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>(); 
        
        newBulletRb.rotation = Quaternion.Euler(rX, rY, rZ); //Add the random spread rotation in the bullet Rigidbody
        newBulletRb.AddRelativeForce((newBulletRb.transform.forward * muzzleSpeed)); //Add relative force in the bullet Rigidbody

    }

    void UpdateHUD()
    {
        bulletCount.text = _bulletsInMagazine.ToString() + "/" + weapon.settings.magazineSize.ToString();
    }

}


#region Comments
/*
 * 
 * 
 * 
 * 
 * 
 */
#endregion
