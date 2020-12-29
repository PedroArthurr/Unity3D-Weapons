#region Libraries
using System;
using UnityEngine;
#endregion

[CreateAssetMenu(fileName="SO_Weapon",menuName="Assets/SO_Weapon")]
public class SO_Weapon : ScriptableObject
{
    [Header("Specialized Settings")]
    public FullAutoSettings fullAutoSettings;
    public SemiAutoSettings semiAutoSettings;
    public ShotgunSettings shotgunSettings;
    public BurstSettings burstSettings;
    
    [Header("General Settings")]
    public GunSettings settings;
}

[Serializable]
public struct GunSettings //General settings (all weapons)
{
    public GunType gunType;
    public int magazineSize;
    public float reloadTime;
}

[Serializable]
public struct FullAutoSettings
{
    public GameObject bulletPrefab; 
    public float muzzleSpeed; //initial force on instantiating the bullet
    public float fireRate; //delay between shots

    [Range(-5, 5)] public float minSpreadX, minSpreadY, minSpreadZ, maxSpreadX, maxSpreadY, maxSpreadZ; //Weapon spray
}

[Serializable]
public struct SemiAutoSettings
{
    public GameObject bulletPrefab;
    public float muzzleSpeed;
    public float fireRate;

    [Range(-5, 5)] public float minSpreadX, minSpreadY, minSpreadZ, maxSpreadX, maxSpreadY, maxSpreadZ;
}

[Serializable]
public struct ShotgunSettings
{
    public GameObject bulletPrefab;
    public float muzzleSpeed;
    public float fireRate;
    public int pelletCount; //How many "mini bullets" are in the buckshot
    
    [Range(-5, 5)] public float minSpreadX, minSpreadY, minSpreadZ, maxSpreadX, maxSpreadY, maxSpreadZ;
}

[Serializable]
public struct BurstSettings
{
    public GameObject bulletPrefab;
    public float muzzleSpeed;
    public int burstLength; //How many bullets per burst
    public float burstFireRate; //Delay between instantiating bullets during the burst
    public float fireRate; //Delay between bursts
    
    [Range(-5, 5)] public float minSpreadX, minSpreadY, minSpreadZ, maxSpreadX, maxSpreadY, maxSpreadZ;
}

public enum GunType
{
    Fullauto,
    Burst,
    Semiauto,
    Shotgun
}