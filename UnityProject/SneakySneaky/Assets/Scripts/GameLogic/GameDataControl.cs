using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataControl: MonoBehaviour
{
	private int currentMaxLevel;
	private bool[] currentlyAvailableHats;
	
	private void Awake()
    {
		LoadGameData();
    }
    
	public void SaveGameData()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");
		SaveData data = new SaveData();
		data.currentMaxLevel = currentMaxLevel;
		data.currentlyAvailableHats = currentlyAvailableHats;
		bf.Serialize(file, data);
		file.Close();
		Debug.Log("Game data saved!");
	}

	public void LoadGameData()
	{
		if (File.Exists(Application.persistentDataPath
					   + "/SaveData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =
					   File.Open(Application.persistentDataPath
					   + "/SaveData.dat", FileMode.Open);

			file.Position = 0;
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();
			currentMaxLevel = data.currentMaxLevel;
			currentlyAvailableHats = data.currentlyAvailableHats;
			Debug.Log("Game data loaded!");
		}
		else
        {
			Debug.Log("There is no save data!");
			//Scene 2 = Level 1
			currentMaxLevel = 2;
			currentlyAvailableHats =new bool[]{ true,true,false,false,false};
		}
			
	}
	public void LevelWon(int level)
    {
		if(currentMaxLevel < level+1)
			currentMaxLevel=level+1;
		SaveGameData();
    }
	public int GetCurrentMaxLevel()
    {
		return currentMaxLevel;
    }
	public bool IsHatCurrentlyAvailable(int index)
    {
		return currentlyAvailableHats[index];
    }
	public void FoundHat(int index)
    {
		currentlyAvailableHats[index] = true;
    }
}

[Serializable]
class SaveData
{
    public int currentMaxLevel;
	public bool[] currentlyAvailableHats;
}
