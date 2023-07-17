using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PhotonPrefabs
{
    public const string PHOTON_PREFABS = "PhotonPrefabs";

    public enum Prefab
    {
        PlayerManager,
        Player,
        PlayerHolder,
        PlayerCamera,
        Enemy,
        SpawnManager
    }

    public static string SpawnPrefab(Prefab prefab)
    {
        return Path.Combine("PhotonPrefabs", prefab.ToString());
    }

}
