using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

public class DialogueDataConvertor : MonoBehaviour
{
	const string PATH_EXCEL = "/../Documents/ExcelData/DialogueData.xlsx";
	const string PATH_ASSET = "Assets/Resources/GameData/DialogueData.asset";
	static DialogueDataSet dialogueDataSet;
	static DataTable ExcelData;

	[MenuItem("CRGTools/DialogueDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		ExcelData = excelReader.AsDataSet ().Tables["Dialogue"];
		excelReader.Dispose ();
		dialogueDataSet = ScriptableObject.CreateInstance<DialogueDataSet>();
		
		if (ExcelData != null)
		{
			LoadData();
			AssetDatabase.CreateAsset(dialogueDataSet, PATH_ASSET);
		}
	}

	static void LoadData()
	{		
		for (int i = 1; i < ExcelData.Rows.Count;)
		{
			DialogueData data = new DialogueData();
			dialogueDataSet.dataSet.Add(data);

			data.dialogueID = ExcelLoader.GetIntCell(ExcelData, i, 0);
			SentenceData sentence;
			sentence.tellerName = ExcelLoader.GetCell(ExcelData, i, 1);
			sentence.avatarID = ExcelLoader.GetIntCell(ExcelData, i, 2);
			sentence.content = ExcelLoader.GetCell(ExcelData, i, 3);
			data.sentences.Add(sentence);

			while(++i < ExcelData.Rows.Count && ExcelLoader.GetIntCell(ExcelData, i, 0, true, 0) == 0)
			{
				sentence.tellerName = ExcelLoader.GetCell(ExcelData, i, 1);
				sentence.avatarID = ExcelLoader.GetIntCell(ExcelData, i, 2);
				sentence.content = ExcelLoader.GetCell(ExcelData, i, 3);
				data.sentences.Add(sentence);
			}
		}
	}
	
}



