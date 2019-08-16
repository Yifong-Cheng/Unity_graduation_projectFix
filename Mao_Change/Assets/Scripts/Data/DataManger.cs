using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class DataManger {

    private static DataManger instance;
    public static DataManger Instance
    {
        get
        {
            if (instance == null)
                instance = new DataManger();
            return instance;
        }
    }

    public void CreatData()
    {

    }
    public string LoadData(string path, string name)
    {
        if(File.Exists(path+"//"+name))
        {
            string filepath = (path + "//" + name);
            string JsondataInfo = File.ReadAllText(filepath);
            //Debug.Log("DataManger : "+JsondataInfo);
            return JsondataInfo;
        }
        else
        {
            Debug.LogWarning("Had No File");
            return null;
        }
        

    }
    public void SaveData(string path, string name, string JsondataInfo)
    {
        string FileName = (path + "//" + name);

        File.WriteAllText(FileName, JsondataInfo);
        Debug.Log("Save File Success!!");

    }

    public void DeleteData(string path, string name)
    {
        string FileName = (path + "//" + name);
        File.Delete(FileName);
        
    }
}
