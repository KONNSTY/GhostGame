using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialougManager", menuName = "Scriptable Objects/DialougManager")]
public class DialougManager : ScriptableObject
{
    public CutSceneData[] CutScene;
}


[System.Serializable]
public class CutSceneData
{
    public GameMode.MissionType mission;
    public Sprite cutSceneImage;
    public string cutSceneDialog;
}
