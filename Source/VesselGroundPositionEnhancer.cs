using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using static Vessel;

namespace RealSolarSystem
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class VesselGroundPositionEnhancer : MonoBehaviour
    {
        private static bool? _isWorldStabilizerInstalled;

        public static bool IsWorldStabilizerInstalled
        {
            get
            {
                if (!_isWorldStabilizerInstalled.HasValue)
                {
                    _isWorldStabilizerInstalled = AssemblyLoader.loadedAssemblies.Any(a => string.Equals(a.name, "WorldStabilizer", StringComparison.OrdinalIgnoreCase));
                }
                return _isWorldStabilizerInstalled.Value;
            }
        }

        public void Start()
        {
            if (IsWorldStabilizerInstalled) return;

            GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
        }

        public void OnDestroy()
        {
            if (IsWorldStabilizerInstalled) return;

            GameEvents.onVesselGoOffRails.Remove(OnVesselGoOffRails);
        }

        private void OnVesselGoOffRails(Vessel v)
        {
            if (v.Landed && !v.Splashed)
            {
                if (v.situation == Situations.PRELAUNCH)
                {
                    const int framesToSkip = 100;
                    foreach (Part p in v.Parts)
                        if (p.GetComponent<CollisionEnhancer>() is CollisionEnhancer c)
                            c.framesToSkip = framesToSkip;

                    Debug.Log($"[RSS-VGPE] Vessel going off rails in PRELAUNCH, skipping {framesToSkip} frames on CollisionEnhancer.");
                }
                else
                {
                    v.skipGroundPositioning = false;
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    v.CheckGroundCollision();
                    Debug.Log($"[RSS-VGPE] CheckGroundCollision() in {sw.ElapsedMilliseconds}ms");
                    sw.Stop();
                }
            }
            else
            {
                Debug.Log("[RSS-VGPE] not landed");
            }
        }
    }
}
