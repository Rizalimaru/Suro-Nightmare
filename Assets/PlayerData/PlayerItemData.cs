using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerItemData", order = 1)]
public class PlayerItemData : ScriptableObject
{
    public bool dapetKafan;
    public bool dapetKeris;
    public bool dapetKaca;
    public bool isTutorialDone;
}
