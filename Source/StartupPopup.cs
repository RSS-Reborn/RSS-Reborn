using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RealSolarSystem
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class StartupPopup : MonoBehaviour
    {
        private const string PreferenceFileName = "RSSFinalizeOrbitWarning";
        private static string PreferenceFilePath => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "PluginData",
            PreferenceFileName);

        public void Start()
        {
            if (AssemblyLoader.loadedAssemblies.Any(a => string.Equals(a.name, "ksp_plugin_adapter", StringComparison.OrdinalIgnoreCase)))
                return;

            if (File.Exists(PreferenceFilePath)) return;

            PopupDialog.SpawnPopupDialog(
                new Vector2(0, 0),
                new Vector2(0, 0),
                new MultiOptionDialog(
                    "RSSStartupDialog",
                    "Warning: This version contains breaking changes for non-Principia users. Significant and necessary fixes have been implemented, which may result in planets changing positions in their orbits. Existing maneuvers and vessels in flight may have dramatically altered future encounters. For existing saves, it is recommended to revert to the prior CKAN version and upgrading is not recommended. Please create backups before attempting to update existing saves.",
                    "Real Solar System",
                    HighLogic.UISkin,
                    new DialogGUIVerticalLayout(
                        new DialogGUIButton("Don't show again", RememberPreference, true),
                        new DialogGUIButton("Ok", () => { }, true)
                        )
                    ),
            true,
                HighLogic.UISkin);
        }

        private static void RememberPreference()
        {
            FileInfo fi = new FileInfo(PreferenceFilePath);
            if (!Directory.Exists(fi.Directory.FullName))
                Directory.CreateDirectory(fi.Directory.FullName);

            // create empty file
            File.Create(PreferenceFilePath).Close();
        }
    }
}
