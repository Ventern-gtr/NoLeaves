using BepInEx;
using System.Linq;
using UnityEngine;

namespace NoLeaves.Main
{
    [BepInPlugin("com.noleaves.ventern", "No leaves", "1.9")]
    public class Plugin : BaseUnityPlugin
    {
        private void Start()
        {
            ZoneManagement.OnZoneChange += OnMapLoad;
            DisableLeaves("Environment Objects/LocalObjects_Prefab/Forest");
        }

        private void OnDestroy()
        {
            ZoneManagement.OnZoneChange -= OnMapLoad;
        }

        private void OnMapLoad(ZoneData[] zones)
        {
            foreach (var zone in zones)
            {
                if (zone == null || !zone.active)
                    continue;

                GTZone zoneType = zone.zone;
                // Logger.LogInfo($"[NoLeaves] Active Zone: {zoneType.GetName()}");

                if (zoneType == GTZone.forest)
                    DisableLeaves("Environment Objects/LocalObjects_Prefab/Forest");

                if (zoneType == GTZone.ranked)
                    DisableLeaves("RankedMain/Ranked_Layout/Ranked_Forest_prefab");
            }
        }


        private void DisableLeaves(string path)
        {
            var root = GameObject.Find(path);
            var matchingGroup = root.GetComponentsInChildren<Transform>(true)
                .Where(t => t.name.StartsWith("UnityTempFile"))
                .GroupBy(t => t.name)
                .FirstOrDefault(g => g.Count() == 3);

            string leavesName = matchingGroup?.Key ?? "UnityTempFile";

            foreach (var child in root.GetComponentsInChildren<Transform>(true))
            {
                var go = child.gameObject;
                if (go.name.StartsWith(leavesName))
                    go.SetActive(false);
            }

            string textToUse = path.Contains("Ranked") ? "forest" : "ranked";
            Logger.LogInfo($"[NoLeaves] Disabled leaves in {textToUse}");
        }
    }
}
