using System;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace BombadiroCrocodilo;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("REPO.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    internal static GameObject BombadiroGameObj = null;
    internal static AudioClip BombadiroChaseSound = null;

    private void TryGetBombadiroAssets() {
        string pathToBombadiroAssets = Path.Combine(Paths.PluginPath, "Randz0-BombadiroCrocodilo/bombadiro");

        Logger.LogInfo("Getting Assets From: " + pathToBombadiroAssets);
        AssetBundle bombadiroAssets = AssetBundle.LoadFromFile(pathToBombadiroAssets);

        if (bombadiroAssets == null) {
            throw new Exception("asset bundle is missing");
        }

        BombadiroGameObj = bombadiroAssets.LoadAsset<GameObject>("bombadiroModel");
        BombadiroChaseSound = bombadiroAssets.LoadAsset<AudioClip>("bombadiroSFX");

        if (BombadiroGameObj == null || BombadiroChaseSound == null) {
            throw new Exception("Could not find model/SFX in assets");
        }
    }

    private void InitializeLogger() {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading!");
    }

    private void Awake()
    {
        InitializeLogger();

        try {
            TryGetBombadiroAssets();
        }
        catch (Exception excep) {
            Logger.LogFatal("Could not load bombadiro, will not load mod \n Error Msg : " + excep.Message);
            return; // Mod will not patch itself into the game files
        }

        Logger.LogInfo("Bombadiro Was Sucessfully loaded.");

        Harmony myPatcher = new Harmony(MyPluginInfo.PLUGIN_GUID);
        myPatcher.PatchAll();
    }
}

[HarmonyPatch(typeof(EnemyParent), nameof(EnemyParent.Awake))]
public class ReplaceEnemyHeadWithBombadiro {
    public static void ReplaceChaseSFX(ref EnemyParent instance) {
        EnemyHeadAnimationSystem enemyHeadAnimationSystem = instance.GetComponentInChildren<EnemyHeadAnimationSystem>(true);

        enemyHeadAnimationSystem.ChaseBegin.Sounds[0] = Plugin.BombadiroChaseSound;
        enemyHeadAnimationSystem.ChaseBegin.Volume = 1;
    }

    public static void ReplaceModel(ref EnemyParent instance) {
        MeshRenderer[] meshRenderers = instance.GetComponentsInChildren<MeshRenderer>(true);
        SkinnedMeshRenderer[] skinnedMeshRenderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        foreach (MeshRenderer meshRenderer in meshRenderers) {
            meshRenderer.enabled = false;
        }

        foreach (SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers) {
            meshRenderer.enabled = false;
        }

        Transform lookAtTransform = instance.transform.GetChild(instance.transform.childCount - 1);
        lookAtTransform = lookAtTransform.GetChild(lookAtTransform.childCount - 1); // Nested structure, but always in the back

        Transform bombadiroTransform = GameObject.Instantiate(Plugin.BombadiroGameObj, lookAtTransform.transform.position, Quaternion.identity, lookAtTransform).transform;
        bombadiroTransform.localRotation = Quaternion.Euler(0, -90, 0);
    }

    public static void Postfix(ref EnemyParent __instance) {
        Plugin.Logger.LogInfo(__instance.enemyName);

        if (__instance.enemyName != "Headman") {
            return;
        }

        ReplaceModel(ref __instance);
        ReplaceChaseSFX(ref __instance);
    }
}