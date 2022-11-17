using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WallState
{
    public int index;
    public bool isVisible;
    public bool isDangerous;

    public WallState(int _index, bool _isVisible = true, bool _isDangerous = true)
    {
        index = _index;
        isVisible = _isVisible;
        isDangerous = _isDangerous;
    }
}

[CreateAssetMenu(menuName = "Level Design/Board Settings")]
public class BoardSettings : ScriptableObject
{
    public int mapWidth, mapHeight;
    public string mapName;

    public int entranceIndex;
    public int exitIndex;

    public List<WallState> horizontalDangerousWalls;
    public List<WallState> VerticalDangerousWalls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
