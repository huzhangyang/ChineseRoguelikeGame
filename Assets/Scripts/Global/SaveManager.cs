using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class SaveManager : MonoBehaviour {

/*
存档管理器
*/
	
	private static SaveManager s_Instance;
	public SaveManager() { s_Instance = this; }
	public static SaveManager Instance { get { return s_Instance; } }

	string PATH_SAVE;
	string PATH_CONFIG;
	string ENCRYPT_KEY;
	bool ENCRYPT_ENABLED = true;

	void Start()
	{
		//PATH_SAVE = Application.dataPath + "/../GameData/Player.sav";//local
		//PATH_CONFIG = Application.dataPath + "/../GameData/Config.sav";//local
		PATH_SAVE = Application.persistentDataPath + "/Player.sav";
		PATH_CONFIG = Application.persistentDataPath + "/Config.sav";
		ENCRYPT_KEY = "CRGProject";
		if(Application.platform == RuntimePlatform.Android)
		{
			PATH_SAVE = Application.persistentDataPath + "/Player.sav";
			PATH_CONFIG = Application.persistentDataPath + "/Config.sav";
			ENCRYPT_KEY = "CRGProject_Android";
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			PATH_SAVE = Application.persistentDataPath + "/Player.sav";
			PATH_CONFIG = Application.persistentDataPath + "/Config.sav";
			ENCRYPT_KEY = "CRGProject_IOS";
		}

		if (!Directory.Exists (Path.GetDirectoryName (PATH_SAVE)))
			Directory.CreateDirectory (Path.GetDirectoryName (PATH_SAVE));
	}

	public void SaveGame()
	{
		PlayerData[] data = DataManager.Instance.GetPlayerDatas ().datas.ToArray ();
		string playerDatastring = SerializeObject (data, typeof(PlayerData[]));
		WriteXML (PATH_SAVE, playerDatastring);
	}

	public void LoadGame()
	{
		if (!File.Exists (PATH_SAVE))
			return;

		string playerDatastring = ReadXML (PATH_SAVE);
		PlayerData[] data = DeserializeObject(playerDatastring, typeof(PlayerData[])) as PlayerData[];
		DataManager.Instance.SetPlayerDatas (data);
	}

	public void SaveConfig()
	{

	}
	
	public void LoadConfig()
	{
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
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(data));
		new XmlTextWriter(memoryStream, Encoding.UTF8);
		memoryStream.Close ();
		return xs.Deserialize(memoryStream);
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
		
		if( ENCRYPT_ENABLED )
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
		
		if( ENCRYPT_ENABLED )
			data = Decrypt(data);

		return data;
	} 
	
	string Encrypt(string toEncrypt)
	{
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
		
		MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
		byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ENCRYPT_KEY));
		
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
		byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ENCRYPT_KEY));
		
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
