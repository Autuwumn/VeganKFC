using BepInEx;
using HarmonyLib;
using Jotunn.Utils;
using ModsPlus;
using Photon.Pun;
using System;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using VKFC.Cards;
using VKFC.UnessesaryAddons;

namespace VKFC
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.rarity.lib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.cardtheme.lib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.CrazyCoders.Rounds.RarityBundle", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willis.rounds.modsplus", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class VKFC : BaseUnityPlugin
    {
        private const string ModId = "ancientkoala.rounds.vegankoalasfantasticcards";
        private const string ModName = "Vegan Koalas Fantastic Cards";
        public const string Version = "0.0.1";
        public const string ModInitials = "VKFC";

        public static VKFC instance;

        internal static AssetBundle ArtAssets;

        public ObjectsToSpawn wallBounce;

        internal static GameObject BoxProj;
        internal static int BoxCap = 5;

        void Start()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            instance = this;

            ArtAssets = AssetUtils.LoadAssetBundleFromResources("vkfcassets", typeof(VKFC).Assembly);
            if (ArtAssets == null) UnityEngine.Debug.Log("Vegan Assets no worky");

            BoxProj = ArtAssets.LoadAsset<GameObject>("VKFC_Box");
            BoxProj.AddComponent<OwnerScript>();
            PhotonNetwork.PrefabPool.RegisterPrefab("VKFC_Box", BoxProj);

            CustomCard.BuildCard<BoxedBullets>((card) => { BoxedBullets.card = card; card.SetAbbreviation("Bb"); });
            CustomCard.BuildCard<StayWinning>((card) => { StayWinning.card = card; card.SetAbbreviation("Sw"); });
        }
    }
}
