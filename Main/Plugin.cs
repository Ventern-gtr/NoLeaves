using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NoLeaves.Main
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Start()
        {
            DisableLeavesInForest("Environment Objects/LocalObjects_Prefab/Forest");
            DisableLeavesInForest("RankedMain/Ranked_Layout/Ranked_Forest_prefab");
        }

        private void DisableLeavesInForest(string path)
        {
            var forest = GameObject.Find(path);
            if (forest == null) return;

            var matchingGroup = forest.GetComponentsInChildren<Transform>(true)
                .Where(t => t.name.StartsWith("UnityTempFile"))
                .GroupBy(t => t.name)
                .FirstOrDefault(g => g.Count() == 3);

            string leavesName = matchingGroup?.Key ?? "UnityTempFile";
            foreach (var child in forest.GetComponentsInChildren<Transform>(true))
            {
                var go = child.gameObject;
                if (go.name == leavesName || go.name.StartsWith(leavesName))
                {
                    go.SetActive(false);
                }
            }
        }
    }
}
