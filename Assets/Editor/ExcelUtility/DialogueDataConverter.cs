using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

public class DialogueDataConvertor : MonoBehaviour
{
	const string PATH_EXCEL = "/../Documents/ExcelData/DialogueData.xls";
	const string PATH_ASSET = "Assets/Resources/GameData/DialogueData.asset";
	static DialogueDataSet dialogueDatas;
	static DataTable ExcelData;

	[MenuItem("CRGTools/DialogueDataConverter")]
	public static void ProcessConvert()
	{		
		FileStream stream = File.Open(Application.dataPath + PATH_EXCEL, FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		ExcelData = excelReader.AsDataSet ().Tables["Dialogue"];
		excelReader.Dispose ();
		dialogueDatas = ScriptableObject.CreateInstance<DialogueDataSet>();
		dialogueDatas.dataSet = new List<DialogueData>();
		
		if (ExcelData != null)
		{
			LoadData();
			AssetDatabase.CreateAsset(dialogueDatas, PATH_ASSET);
		}
	}

	static void LoadData()
	{
		DialogueData data = null;
		
		for (int i = 1; i < ExcelData.Rows.Count;)
		{
			int dialogueID = ExcelUtility.GetIntCell(ExcelData, i, 0);

			data = new DialogueData(dialogueID);
			dialogueDatas.dataSet.Add(data);

			SentenceData sentence;
			sentence.tellerName = ExcelUtility.GetCell(ExcelData, i, 1);
			sentence.avatarID = ExcelUtility.GetIntCell(ExcelData, i, 2);
			sentence.content = ExcelUtility.GetCell(ExcelData, i, 3);
			data.sentences.Add(sentence);

			while(++i < ExcelData.Rows.Count && ExcelUtility.GetIntCell(ExcelData, i, 0, true, 0) == 0)
			{
				sentence.tellerName = ExcelUtility.GetCell(ExcelData, i, 1);
				sentence.avatarID = ExcelUtility.GetIntCell(ExcelData, i, 2);
				sentence.content = ExcelUtility.GetCell(ExcelData, i, 3);
				data.sentences.Add(sentence);
			}
		}
	}
	
}



