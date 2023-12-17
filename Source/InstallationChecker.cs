using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RealSolarSystem
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class RSSInstallationCheck : MonoBehaviour
    {
        public void Start()
        {
            try
            {
                CheckRealSolarSystem();
                CheckTexturesInstalled();
                CheckCompatibleKopInstalled();
                CheckTerrainInstalled();
            }
            catch (Exception ex)
            {
                Debug.Log("[RealSolarSystem] RSSInstallationCheck.Start() caught an exception: " + ex);
            }
            finally
            {
                Destroy(this);
            }
        }

        private void CheckRealSolarSystem()
        {
            string szUserMessage = "The old RealSolarSystem folder has been detected in your GameData, please remove the folder from GameData.";
            string szRSSFolderPath = $"{KSPUtil.ApplicationRootPath}GameData{Path.AltDirectorySeparatorChar}RealSolarSystem";
            if (Directory.Exists(szRSSFolderPath))
            {
                Debug.Log($"[RealSolarSystem | EXC] Old RealSolarSystem found!");
                PopupDialog.SpawnPopupDialog
                (
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 0.0f),
                    new MultiOptionDialog
                    (
                        "RSSInstallCheck",
                        szUserMessage,
                        "RealSolarSystem Folder Detected",
                        HighLogic.UISkin,
                        new Rect(0.25f, 0.75f, 320f, 80f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIButton
                        (
                            "Understood",
                            delegate {},
                            140.0f,
                            30.0f,
                            true
                        )
                    ),
                    false,
                    HighLogic.UISkin,
                    true,
                    string.Empty
                );
            }
        }
        private void CheckTexturesInstalled()
        {
            string szTextureFolderPath = $"{KSPUtil.ApplicationRootPath}GameData{Path.AltDirectorySeparatorChar}RSS-Textures{Path.AltDirectorySeparatorChar}PluginData{Path.AltDirectorySeparatorChar}";

            int errors = 0;
            string szUserMessage = "";


            //Thanks I hate this
            string[] texturePaths = new string[10];
            texturePaths[0] = szTextureFolderPath + "00_Sun";           //Inner System
            texturePaths[1] = szTextureFolderPath + "03_Earth";         //Earth
            texturePaths[2] = szTextureFolderPath + "03-01_Moon";       //Luna
            texturePaths[3] = szTextureFolderPath + "04_Mars";          //Mars
            texturePaths[4] = szTextureFolderPath + "05_Jupiter";       //Jupiter Pack
            texturePaths[5] = szTextureFolderPath + "06_Saturn";        //Saturn Pack
            texturePaths[6] = szTextureFolderPath + "07_Uranus";        //Uranus Pack
            texturePaths[7] = szTextureFolderPath + "08_Neptune";       //Neptune Pack
            texturePaths[8] = szTextureFolderPath + "09-01_Vesta";      //Asteroid Belt Pack
            texturePaths[9] = szTextureFolderPath + "10-01_Pluto";      //Kuiper Belt Pack

            string[] packNames = new string[10];
            packNames[0] = "Inner System Texture Pack";
            packNames[1] = "Earth Texture Pack";
            packNames[2] = "Luna Texture Pack";
            packNames[3] = "Mars Texture Pack";
            packNames[4] = "Jupiter System Pack";
            packNames[5] = "Saturn System Pack";
            packNames[6] = "Uranus System Texture Pack";
            packNames[7] = "Neptune System Texture Pack";
            packNames[8] = "Asteroid Belt Texture Pack";
            packNames[9] = "Kuiper Belt Texture Pack";


            for (int i = 0; i < texturePaths.Length; i++)
            {
                string path = texturePaths[i];
                string pack = packNames[i];
                if (!Directory.Exists(path))
                {
                    errors += 1;

                    szUserMessage = $"The {pack} was not found in your installation.\n\n Please download it from the official RSS-Reborn texture pack repository.";
                    Debug.Log($"[RealSolarSystem | EXC] Path not found! {path}, is missing");


                    PopupDialog.SpawnPopupDialog
                    (
                        new Vector2(0.0f, 0.0f),
                        new Vector2(0.0f, 0.0f),
                        new MultiOptionDialog
                        (
                            "RSSInstallCheck",
                            szUserMessage,
                            "Missing RSS Texture Pack",
                            HighLogic.UISkin,
                            new Rect(0.25f, 0.75f, 320f, 80f),
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIButton
                            (
                                "Download",
                                delegate
                                {
                                    OpenRSSTexturesDownloadPage();
                                },
                                140.0f,
                                30.0f,
                                true
                            )
                        ),
                        false,
                        HighLogic.UISkin,
                        true,
                        string.Empty
                    );
                }
                else
                {
                    Debug.Log($"[RealSolarSystem] {pack} found!");
                }
            }

            if (errors == 0)
            {
                Debug.Log("[RealSolarSystem] No texture errors detected, safe flying CMDR.");
            }
            
        }

        private void CheckCompatibleKopInstalled()
        {
            string foxShaderPath = $"{KSPUtil.ApplicationRootPath}GameData{Path.AltDirectorySeparatorChar}Kopernicus{Path.AltDirectorySeparatorChar}Shaders{Path.AltDirectorySeparatorChar}scaledmesh2-windows.unity3d";
            string kopVer = Kopernicus.Constants.Version.VersionNumber;    // Is in format "Release-160"
            string sRelNum = kopVer.Split('-').Last();
            bool success = int.TryParse(sRelNum, out int relNum);

            const int minKopReleaseVer = 161;


            if (!success || relNum < minKopReleaseVer)
            {
                Debug.Log("[RealSolarSystem] Invalid Kopernicus version detected: " + kopVer);

                string errorMsg = $"The currently installed version of Kopernicus ({kopVer}) is missing features required for RSS to function properly.\n\nPlease install Kopernicus release {minKopReleaseVer} or newer.";

                PopupDialog.SpawnPopupDialog
                (
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 0.0f),
                    new MultiOptionDialog
                    (
                        "RSSKopInstallCheck",
                        errorMsg,
                        "Incompatible Kopernicus version found",
                        HighLogic.UISkin,
                        new Rect(0.35f, 0.65f, 320f, 80f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIButton
                        (
                            "Understood",
                            delegate { },
                            140.0f,
                            30.0f,
                            true
                        )
                    ),
                    false,
                    HighLogic.UISkin,
                    true,
                    string.Empty
                );
            }
            else if (!File.Exists(foxShaderPath))
            {
                Debug.Log("[RealSolarSystem | EXC] ScaledMesh2 Shader not found!");

                PopupDialog.SpawnPopupDialog
                (
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 0.0f),
                    new MultiOptionDialog
                    (
                        "RSSKopInstallCheck",
                        "The currently installed version of Kopernicus does not have the required shaders for RSS-Reborn.",
                        "Custom Kopernicus fork not installed.",
                        HighLogic.UISkin,
                        new Rect(0.25f, 0.75f, 320f, 80f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIButton
                        (
                            "Download",
                            delegate
                            {
                                OpenKopernicusDownloadPage();
                            },
                            140.0f,
                            30.0f,
                            true
                        )
                    ),
                    false,
                    HighLogic.UISkin,
                    true,
                    string.Empty
                );
            }
        }

        private void CheckTerrainInstalled()
        {
            string errorMsg = "RSS-Terrain was not found in your installation.\n\n Please download it from the official RSS-Reborn texture pack repository.";
            string szTerrainFolderPath = $"{KSPUtil.ApplicationRootPath}GameData{Path.AltDirectorySeparatorChar}RSS-Terrain";
            if (!Directory.Exists(szTerrainFolderPath))
            {
                Debug.Log($"[RealSolarSystem | EXC] Terrain Textures not found!");

                PopupDialog.SpawnPopupDialog
                (
                   new Vector2(0.0f, 0.0f),
                   new Vector2(0.0f, 0.0f),
                   new MultiOptionDialog
                   (
                       "RSSInstallCheck",
                       errorMsg,
                       "RSS-Terrain not found!",
                       HighLogic.UISkin,
                       new Rect(0.35f, 0.65f, 320f, 80f),
                       new DialogGUIFlexibleSpace(),
                       new DialogGUIButton
                       (
                           "Understood",
                            delegate
                            {
                                OpenRSSTexturesDownloadPage();
                            },
                           140.0f,
                           30.0f,
                           true
                       )
                   ),
                   false,
                   HighLogic.UISkin,
                   true,
                   string.Empty
               );
            }

        }

        private void OpenKopernicusDownloadPage()
        {
            Application.OpenURL("https://github.com/ballisticfox/Kopernicus/releases/latest");
        }
        private void OpenRSSTexturesDownloadPage()
        {
            Application.OpenURL("https://docs.google.com/document/d/1BfiIZ8_ZEh0lrx0nsIa27Lm7O2R4e2fKPli94XHga0Q/edit?usp=sharing");
        }
    }
}
