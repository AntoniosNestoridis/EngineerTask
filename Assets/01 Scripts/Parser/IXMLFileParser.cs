using UnityEngine;
using System.IO;
using UnityEditor;

public class IXMLFileParser : MonoBehaviour
{

    public string FileDirectory;
    public string filename;

    public bool ReadFile;

    void Start()
    {
        Debug.Log("Hello");
    }


    void Update()
    {
        if (ReadFile)
        {
            ReadFile = false;
            ReadFileFromDisk(filename);
        }
    }

    /// <summary>
    /// Parses an IXFML file and creates a new instance of brick data information
    /// </summary>
    public void ParseFile()
    {
            
    }

    /// <summary>
    /// 
    /// </summary>
    public void ReadFileFromDisk(string filename)
    {

        //AssetDatabase.ImportAsset(path); 
        //Application.dataPath
        // Example path Assets/04 Data/everyoneisawesome
        var sr = new StreamReader( FileDirectory + "/" + filename);
        var fileContents = sr.ReadToEnd();
        sr.Close();
     

        //Print the text from the file
        Debug.Log(fileContents);
    }

    
}
