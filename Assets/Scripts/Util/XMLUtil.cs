using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class XMLUtil 
{
	/// <summary>
	/// 获取该节点下指定子节点
	/// </summary>
	/// <param name="node"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public static XmlNode GetSelectedXmlNodeFromRoot(XmlNode node, string name)
	{
		return node.SelectSingleNode(name);
	}

	/// <summary>
	/// 抽取指定xml文件的指定节点
	/// </summary>
	/// <param name="path"></param>
	/// <param name="name">默认为root</param>
	/// <returns>XmlNode</returns>
	public static XmlNode LoadXml(string path, string name = "root")
	{
		if (path == null) return null;
		string s = ((Resources.Load(path) as TextAsset)).text;
		//读取路径下的文件
		XmlDocument document = new XmlDocument();
		document.LoadXml(s);
		if (document == null) return null;
		//得到指定节点内的内容
		XmlNode node = document.SelectSingleNode(name);
		return node;
	}

	/// <summary>
	/// 获取该节点的名字和值
	/// </summary>
	/// <param name="node"></param>
	/// <returns>item[0]=name item[1]=value</returns>
	public static string[] GetNameValueOfNode(XmlNode node)
	{
		string[] item = new string[2];
		item.SetValue(node.Name, 0);
		item.SetValue(node.InnerText, 1);
		return item;
	}
	public static List<XmlNode> GetChildNodes(XmlNode node)
    {
		XmlNodeList xmlNodeList = node.ChildNodes;
		List< XmlNode> list = new List<XmlNode>();
		foreach (XmlNode e in xmlNodeList)
		{
			list.Add(e);
		}
		return list;
	}
	/// <summary>
	/// 获取该节点下特定标签的子节点
	/// </summary>
	/// <param name="node"></param>
	/// <param name="attribute">标签名称</param>
	/// <param name="value">标签的内容</param>
	/// <returns></returns>
	public static XmlElement GetNodeByAttributeFromNode(XmlNode node, string attribute, string value)
	{
		if (node == null) return null;
		XmlNodeList xmlNodeList = node.ChildNodes;

		//Debug.Log(xmlNodeList);
		foreach (XmlElement e in xmlNodeList)
		{
			if (e.Attributes.GetNamedItem(attribute).Value == value) return e;
		}
		return null;
	}

}
