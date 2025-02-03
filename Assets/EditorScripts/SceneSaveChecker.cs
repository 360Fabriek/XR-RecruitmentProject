using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[InitializeOnLoad]
public static class SceneSaveChecker
{
    private static DateTime lastSaveTime;
    private static double checkInterval = 10.0; // Check every 10 seconds
    private static DateTime lastCheckTime;

    static SceneSaveChecker()
    {
        lastSaveTime = DateTime.Now;
        lastCheckTime = DateTime.Now;
        EditorApplication.update += CheckSceneSaveStatus;
        EditorSceneManager.sceneSaved += OnSceneSaved;
    }

    private static void OnSceneSaved(Scene scene)
    {
        lastSaveTime = DateTime.Now;
    }

    private static void CheckSceneSaveStatus()
    {
        if (!EditorApplication.isPlaying)
        {
            if ((DateTime.Now - lastCheckTime).TotalSeconds >= checkInterval)
            {
                lastCheckTime = DateTime.Now;
                //Log check for save time

                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    TimeSpan timeSinceLastSave = DateTime.Now - lastSaveTime;
                    if (timeSinceLastSave.TotalMinutes > 1)
                    {
                        bool saveScene = EditorUtility.DisplayDialog(
                            "Unsaved Scene Detected",
                            "The current scene has been modified and not beeb saved. Do you want to save it now?",
                            "Save", "Ignore"
                        );

                        if (saveScene)
                        {
                            EditorSceneManager.SaveOpenScenes();
                        }

                        lastSaveTime = DateTime.Now; // Prevent repeated popups
                    }
                }
            }
        }
    }
}
