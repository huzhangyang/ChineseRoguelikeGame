using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

public class SaveManager : MonoBehaviour {

/*
存档管理器
*/
	
	private static SaveManager s_Instance;
	public SaveManager() { s_Instance = this; }
	public static SaveManager Instance { get { return s_Instance; } }



	void Start()
	{
		if (!Directory.Exists (Path.GetDirectoryName (GlobalDataStructure.PATH_SAVE)))
			Directory.CreateDirectory (Path.GetDirectoryName (GlobalDataStructure.PATH_SAVE));
		if (!Directory.Exists (Path.GetDirectoryName (GlobalDataStructure.PATH_CONFIG)))
			Directory.CreateDirectory (Path.GetDirectoryName (GlobalDataStructure.PATH_CONFIG));
	}

	public void SaveGame()
	{
		PlayerData[] datas = DataManager.Instance.GetPlayerDataSet ().dataSet.ToArray ();
		foreach (var data in datas)
		{
			data.itemKeys.Clear();
			data.itemValues.Clear();
			foreach (var itemData in data.items)
			{
				data.itemKeys.Add(itemData.Key);
				data.itemValues.Add(itemData.Value);
			}
		}
		string playerDatastring = SerializeObject (datas, typeof(PlayerData[]));
		WriteXML (GlobalDataStructure.PATH_SAVE, playerDatastring);
	}

	public PlayerDataSet LoadGame()
	{
		if (!File.Exists (GlobalDataStructure.PATH_SAVE))
			return null;

		string playerDatastring = ReadXML (GlobalDataStructure.PATH_SAVE);
		PlayerData[] datas = DeserializeObject(playerDatastring, typeof(PlayerData[])) as PlayerData[];

		foreach (var data in datas)
		{
			data.items.Clear();
			for (int i = 0; i != Math.Min(data.itemKeys.Count, data.itemValues.Count); i++)
			{
				data.items.Add(data.itemKeys[i], data.itemValues[i]);
			}
		}

		PlayerDataSet dataSet = new PlayerDataSet();
		dataSet.dataSet = new List<PlayerData>(datas);
		return dataSet;
	}

	public void SaveConfig()
	{
		ConfigData data = DataManager.Instance.GetConfigData ();
		string configDatastring = SerializeObject (data, typeof(ConfigData));
		WriteXML (GlobalDataStructure.PATH_CONFIG, configDatastring);
	}
	
	public ConfigData LoadConfig()
	{
		if (!File.Exists (GlobalDataStructure.PATH_CONFIG))
			return null;
		
		string configDatastring = ReadXML (GlobalDataStructure.PATH_CONFIG);
		return DeserializeObject(configDatastring, typeof(ConfigData)) as ConfigData;
	}

	public bool IsSaveExist()
	{
		return File.Exists (GlobalDataStructure.PATH_SAVE);
	}
	
	string SerializeObject(object pObject, System.Type userType)
	{
		string XmlizedString = null;
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xs = new XmlSerializer(userType);
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xs.Serialize(xmlTextWriter, pObject);
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
		memoryStream.Close ();
		return XmlizedString;
	}

	object DeserializeObject(string data, System.Type userType)
	{
		XmlSerializer xs = new XmlSerializer(userType);
		using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(data)))
		{
			new XmlTextWriter(memoryStream, Encoding.UTF8);
			return xs.Deserialize(memoryStream);
		}
	}

	void WriteXML(string path, string data)
	{			
		if(!Directory.Exists(Path.GetDirectoryName(path)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
		}
		
		StreamWriter writer;
		FileInfo t = new FileInfo(path);
		
		if(!t.Exists)
		{
			writer = t.CreateText();
		}
		else
		{
			t.Delete();
			writer = t.CreateText();
		}
		
		if( GlobalDataStructure.ENCRYPT_ENABLED )
			writer.Write(Encrypt(data));
		else
			writer.Write(data);
		
		writer.Close();
	}
	
	string ReadXML(string path)
	{
		StreamReader r = File.OpenText(path);
		string data = r.ReadToEnd();
		r.Close();
		
		if( GlobalDataStructure.ENCRYPT_ENABLED )
			data = Decrypt(data);

		return data;
	} 
	
	string Encrypt(string toEncrypt)
	{
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
		
		MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
		byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(GlobalDataStructure.ENCRYPT_KEY));
		
		hashmd5.Clear();
		
		TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
		tdes.Key = keyArray;
		tdes.Mode = CipherMode.ECB;
		tdes.Padding = PaddingMode.PKCS7;
		
		ICryptoTransform cTransform = tdes.CreateEncryptor();
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		tdes.Clear();
		
		return Convert.ToBase64String(resultArray, 0, resultArray.Length);
	}
	
	string Decrypt(string cipherString)
	{
		byte[] toEncryptArray = Convert.FromBase64String(cipherString);
		
		MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
		byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(GlobalDataStructure.ENCRYPT_KEY));
		
		hashmd5.Clear();
		
		TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
		tdes.Key = keyArray;
		tdes.Mode = CipherMode.ECB;
		tdes.Padding = PaddingMode.PKCS7;
		
		ICryptoTransform cTransform = tdes.CreateDecryptor();
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);           
		tdes.Clear();

		return UTF8Encoding.UTF8.GetString(resultArray);
	}

	string UTF8ByteArrayToString(byte[] characters)
	{      
		UTF8Encoding encoding = new UTF8Encoding();
		string constructedString = encoding.GetString(characters);
		return (constructedString);
	}
	
	byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding encoding = new UTF8Encoding();
		byte[] byteArray = encoding.GetBytes(pXmlString);
		return byteArray;
	}
}
