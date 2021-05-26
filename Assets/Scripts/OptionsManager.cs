using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    
    [SerializeField] GameObject menu;
    SolverHandler solverHandler;
    HandConstraintPalmUp handConstraintPalmUp;
    [SerializeField] TextMeshPro pathButtonText;
    [SerializeField] TextMeshPro handButtonText;
    [SerializeField] TextMeshPro zoneButtonText;


    Handedness[] handednesses = { Handedness.Both, Handedness.Left, Handedness.Right };
    string[] handednessText = { "Ambas", "Izquierda", "Derecha" };
    HandConstraint.SolverSafeZone[] solverSafeZones = {HandConstraint.SolverSafeZone.AboveFingerTips,
        HandConstraint.SolverSafeZone.AtopPalm,
        HandConstraint.SolverSafeZone.BelowWrist,
        HandConstraint.SolverSafeZone.RadialSide,
        HandConstraint.SolverSafeZone.UlnarSide};
    string[] safeZoneText = { "Encima de los dedos", "Palma", "Muñeca","Lateral externo","Lateral interno"};

    int currentHand;
    int currentZone;

    private void Start()
    {
        solverHandler = menu.GetComponent<SolverHandler>();
        handConstraintPalmUp = menu.GetComponent<HandConstraintPalmUp>();

        LoadPreferences();
    }

    public void LoadPreferences()
    {
        LoadHandOption();
        LoadMenuPostition();
        LoadDownloadPath();
    }

    public void SaveHandOption()
    {
        PlayerPrefs.SetInt("handOption", currentHand);
    }

    public void LoadHandOption()
    {
        currentHand = PlayerPrefs.GetInt("handOption");
        solverHandler.TrackedHandness = handednesses[currentHand];
        handButtonText.text = handednessText[currentHand];
        Debug.Log("LoadHand: " +currentHand);
    }

    public void SwitchHandOption()
    {
        if (currentHand == handednesses.Length - 1)
        {
            currentHand = 0;
        }
        else
        {
            currentHand++;
        }
        SaveHandOption();
        solverHandler.TrackedHandness = handednesses[currentHand];
        handButtonText.text = handednessText[currentHand];
        Debug.Log("LoadHand: " + currentHand);
    }

    public void SaveMenuPosition()
    {
        PlayerPrefs.SetInt("zoneOption", currentZone);
    }

    public void LoadMenuPostition()
    {
        currentZone = PlayerPrefs.GetInt("zoneOption");
        handConstraintPalmUp.SafeZone = solverSafeZones[currentZone];
        zoneButtonText.text = safeZoneText[currentZone];
        Debug.Log("LoadZone: " + currentZone);
    }

    public void SwitchMenuPosition()
    {
        if (currentZone == solverSafeZones.Length - 1)
        {
            currentZone = 0;
        }
        else
        {
            currentZone++;
        }
        SaveMenuPosition();
        handConstraintPalmUp.SafeZone = solverSafeZones[currentZone];
        zoneButtonText.text = safeZoneText[currentZone];
        Debug.Log("LoadZone: " + currentZone);
    }

    public void SaveDownloadPath()
    {
        string downloadPath = GameManager.Instance.downloadPath;
        PlayerPrefs.SetString("downloadPath", downloadPath);
    }

    public void LoadDownloadPath()
    {
        string downloadPath;
        downloadPath=PlayerPrefs.GetString("downloadPath");
        GameManager.Instance.downloadPath = downloadPath;
        if(downloadPath==null || downloadPath.Length == 0)
        {
            downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            downloadPath = Path.Combine(downloadPath, "Downloads");
        }
        pathButtonText.text = downloadPath;
        Debug.Log("LoadPath: " + downloadPath);
    }

    public void SwitchDownloadPath()
    {
        FilePickerManager fp = new FilePickerManager();
        fp.onFolderPicked += UpdateDownloadPath;
        fp.PickFolder();
    }

    public void UpdateDownloadPath(string downloadPath)
    {
        GameManager.Instance.downloadPath = downloadPath;
        SaveDownloadPath();
        pathButtonText.text = downloadPath;
        Debug.Log("LoadPath: " + downloadPath);
    }
}
