using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace TOGInvincible
{
    public class TOGInvinciblePatch : MonoBehaviour
    {
        private Harmony _harmony;

        public void Inject()
        {
            try
            {
                _harmony = new Harmony("Invincible");
                _harmony.PatchAll(Assembly.GetExecutingAssembly());
                Debug.Log("Invincible Loaded");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }


        // SetPlayerDamage(int dam)

        [HarmonyPatch(typeof(Stats))]
        [HarmonyPatch("SetPlayerDamage")]
        // ReSharper disable once UnusedType.Local
        private static class StatsSetPlayerDamagePatch
        {
            [HarmonyPrefix]
            private static void Prefix(ref int dam)
            {
                dam = 0;
            }
        }

        [HarmonyPatch(typeof(HorseStats))]
        [HarmonyPatch("DecHealth")]
        // ReSharper disable once UnusedType.Local
        private static class HorseStatsDecHealthPatch
        {
            [HarmonyPrefix]
            private static void Prefix(HorseStats __instance, ref int amount)
            {
                if (__instance.isPlayerHorse)
                    amount = 0;
            }
        }

        [HarmonyPatch(typeof(VRCharController))]
        [HarmonyPatch("LateUpdate")]
        // ReSharper disable once UnusedType.Local
        private static class VRCharControllerLateUpdatePatch
        {
            [HarmonyPostfix]
            private static void Postfix(VRCharController __instance)
            {
                if (__instance.LifeManager.Stamina < __instance.LifeManager.MaxStamina)
                {
                    __instance.LifeManager.Stamina = __instance.LifeManager.MaxStamina;
                }
            }
        }
    }
}