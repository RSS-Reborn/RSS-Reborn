﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealSolarSystem
{
    // From Starwaster.
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class RealSolarSystemEditor : MonoBehaviour
    {
        static Rect windowPosition = new Rect(64, 64, 320, 640);
        static GUIStyle windowStyle = null;

        bool GUIOpen = false;

        double counter = 0;

        Vector2 scrollPos;

        // Camera parameters.

        List<CameraWrapper> cams = null;

        public class CameraWrapper : MonoBehaviour
        {
            public string depth;
            public string farClipPlane;
            public string nearClipPlane;
            public string camName;

            public CameraWrapper()
            {
                depth = farClipPlane = nearClipPlane = camName = "";
            }

            public void Apply()
            {
                Camera[] cameras = Camera.allCameras;

                try
                {
                    bool notFound = true;

                    foreach (Camera cam in cameras)
                    {
                        if (camName.Equals(cam.name))
                        {
                            if (float.TryParse(depth, out float ftmp))
                                cam.depth = ftmp;

                            if (float.TryParse(farClipPlane, out ftmp))
                                cam.farClipPlane = ftmp;

                            if (float.TryParse(nearClipPlane, out ftmp))
                                cam.nearClipPlane = ftmp;

                            depth = cam.depth.ToString();
                            nearClipPlane = cam.nearClipPlane.ToString();
                            farClipPlane = cam.farClipPlane.ToString();

                            notFound = false;
                        }
                    }

                    if (notFound)
                    {
                        Debug.Log($"[RealSolarSystem] Could not find camera {camName} when applying settings!");
                    }
                }
                catch (Exception exceptionStack)
                {
                    Debug.Log($"[RealSolarSystem] Error applying to camera {camName}: exception {exceptionStack.Message}");
                }
            }
        }

        public void Update()
        {
            if (counter < 5)
            {
                counter += TimeWarp.fixedDeltaTime;
                return;
            }

            if (cams == null)
            {
                cams = new List<CameraWrapper>();

                Camera[] cameras = Camera.allCameras;

                foreach (Camera cam in cameras)
                {
                    try
                    {
                        var thisCam = new CameraWrapper
                        {
                            camName = cam.name,

                            depth = cam.depth.ToString()
                        };

                        thisCam.farClipPlane += cam.farClipPlane.ToString();
                        thisCam.nearClipPlane += cam.nearClipPlane.ToString();

                        cams.Add(thisCam);
                    }
                    catch (Exception exceptionStack)
                    {
                        Debug.Log($"[RealSolarSystem] Exception getting camera {cam.name}\n{exceptionStack}");
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.G) && Input.GetKey(KeyCode.LeftAlt))
            {
                GUIOpen = !GUIOpen;
            }
        }

        public void OnGUI()
        {
            if (GUIOpen)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
                    windowPosition = GUILayout.Window(69105, windowPosition, ShowGUI, "RealSolarSystem Parameters", windowStyle);
            }
        }

        public void Start()
        {
            windowStyle = new GUIStyle(HighLogic.Skin.window);
            windowStyle.stretchHeight = true;
        }

        private void ShowGUI(int windowID)
        {
            GUILayout.BeginVertical();

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.Label("RSSRunwayFix");

            GUILayout.BeginHorizontal();
            GUILayout.Label("isOnRunway: ");
            GUILayout.Label(RSSRunwayFix.Instance.isOnRunway.ToString(), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("hold: ");
            GUILayout.Label(RSSRunwayFix.Instance.hold.ToString(), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("lastHitCollider: ");
            GUILayout.Label(RSSRunwayFix.Instance.lastHitColliderName, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            if (cams != null)
            {
                GUILayout.Label("--------------");
                GUILayout.BeginHorizontal();
                GUILayout.Label("CAMERA EDITOR");
                GUILayout.EndHorizontal();

                foreach (CameraWrapper cam in cams)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Camera: " + cam.camName);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Depth");
                    cam.depth = GUILayout.TextField(cam.depth, 10);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Far Clip");
                    cam.farClipPlane = GUILayout.TextField(cam.farClipPlane, 10);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Near Clip");
                    cam.nearClipPlane = GUILayout.TextField(cam.nearClipPlane, 10);
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Apply to " + cam.camName))
                    {
                        cam.Apply();
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}
