using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// µÿÕº≈‰÷√
/// </summary>
[CreateAssetMenu(menuName = "MapConfig ", fileName = "Map")]
public class MapConfig : ScriptableObject
{
    public string mapName;
    public int elementsNum { get { return elementsPos.Count; } }
    public List<Vector2> elementsPos;

	

}
