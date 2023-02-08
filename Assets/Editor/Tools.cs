using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class Tools
{
	[MenuItem("Tools/Map2Config", false, 1001)]
	public static void convet()
	{
		

		if (Selection.activeGameObject == null)
			throw new UnityException("must select Map");

		Transform map = Selection.activeGameObject.transform;
		string assetPath = @"Assets/Resources/MapConfig/"+map.name+".asset";

		MapConfig info = AssetDatabase.LoadAssetAtPath(assetPath, typeof(MapConfig)) as MapConfig;
		bool isExist = (info != null);

		if (info == null)
		{
			Debug.Log("AssetDatabase not exist MapConfig");
			info = ScriptableObject.CreateInstance<MapConfig>() as MapConfig;
		}

		info.elementsPos = new List<Vector2>(new Vector2[map.childCount]);
		for (int i = 0, L = map.childCount; i < L; ++i)
		{
			Transform t = map.GetChild(i);
			info.elementsPos[i]=t.localPosition;
		}

		if (!isExist)
		{
			AssetDatabase.CreateAsset(info, assetPath);
			return;
		}

		// 这里很重要，如果没有告诉unity已经被改变。它只写入内存，没有写入磁盘
		EditorUtility.SetDirty(info);
		AssetDatabase.SaveAssets();
	}
	[MenuItem("Tools/AngleTest", false, 1001)]
	public static void Angle()
    {
		//(3.26, 0.77)←(2.94, 1.01, 0.00)
		Vector2 from = new Vector2(2.94f, 1.01f), to = new Vector2(3.26f, 0.77f),vertical;
		vertical = GetVerticalVectorFrom(from, to);
		Debug.Log("vertical:"+vertical);
		var angle = Vector2.Angle(Vector2.up, vertical);
		Debug.Log(angle);//-45
		Debug.Log(Quaternion.Euler(0, 0, angle));
		Selection.activeGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

	}
	private static Vector2 GetVerticalVectorFrom(Vector2 from, Vector2 to)
	{
		//from.x * -from.y + from.y * from.x
		// return new Vector2(from.y,-from.x);
		return new Vector2(-(to.y - from.y), (to.x - from.x));
	}
	[MenuItem("Tools/Texture2DTest", false, 1001)]
	public static void LoadTexture()
    {
		var waitingBalls = new List<Texture2D>();
		for (int i = 1; i <= 36; i++)
		{
           
            waitingBalls.Add(Resources.Load("Sprite/Balls/"+ i) as Texture2D);

		}
		Texture2D texture = waitingBalls[Random.Range(0, waitingBalls.Count - 1)];
		SpriteRenderer renderer = Selection.activeGameObject.GetComponent<SpriteRenderer>();
		Sprite sp= Sprite.Create(texture, new Rect(renderer.sprite.textureRect.x, renderer.sprite.textureRect.y, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		renderer.sprite = sp;
		Debug.Log(renderer.sprite.textureRect.x+" "+ renderer.sprite.textureRect.y);
	}


}
