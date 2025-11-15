using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// A simple parser for IXML file types. 
/// Has the ability to load a file from the disk and convert it into a Model data structure
/// that can be stored and processed by the application.
/// </summary>
public class IXMLFileParser : MonoBehaviour
{

    public string FileDirectory; // TODO: Eventually we want this to be the Application.Filepath
    public string filename;

    // Quick/Debug editor testing only
    public bool ReadFile;

    void Update()
    {
        if (ReadFile)
        {
            ReadFile = false;
            CreateBrickData();
        }
    }



    public void CreateBrickData()
    {
        // 1. Read text file from disk
        string fileContents = ReadFileFromDisk(FileDirectory,filename);

        // 2. Parse file into a data form
        if (!string.IsNullOrEmpty(fileContents))
        {
            Model newModel = ParseFile(fileContents);

            // 3. Print the data to the console / Update UI
            if (newModel != null)
            {
                newModel.PrintModelDataToConsole();
            }         
        } 
    }


    /// <summary>
    /// Attempts to parse an IXFML file type into a Model structure.
    /// Returns a new Model instance upon success or null if the parsing the file was invalid.
    /// </summary>
    /// <param name="fileContents"> The contents of an IXMFL file type as a string. </param>    
    public Model ParseFile(string fileContents)
    {
        Model newImportedModel = new Model();

        Debug.Log("Attempting to [arse text file into data...");

        string brickListPattern = "<Brick(?:.*?|\n)*</Brick>"; 
        Regex r = new Regex(brickListPattern, RegexOptions.Multiline);

        var brickMatches = r.Matches(fileContents);
        int brickMatchCount = brickMatches.Count;

        //No brick matches, invalid file format or simply empty.
        if (brickMatchCount == 0)
        {
            Debug.Log("No matches found for brick pattern. File format is incorrect.");
            return null;
        }

        Debug.Log($"-- Found {brickMatchCount} bricks in model description-- ");
        newImportedModel.TotalBricks = brickMatchCount;

        // Caching temporary variables for reuse during data creation below.
        string newBrickDesignID;
        string newBrickUUID;

        string newPartUUID;
        string newPartDesignID;
        string newPartType;
        string newPartMaterials;

        string newBoneUUID;
        string newBoneTransformation;

        List<Brick> bricks = new List<Brick>();

        foreach (Match brickMatch in brickMatches)
        {  
            //1. Extract brick specific data
            string brickInfoLine = brickMatch.Value;

            string brickDataPattern = "(designID=\"(.*)\" uuid=\"(.*)\">)";
            Regex brickR = new Regex(brickDataPattern, RegexOptions.None);
            Match brickInfoMatches = brickR.Match(brickInfoLine);

            newBrickDesignID = brickInfoMatches.Groups[2].Value;
            newBrickUUID = brickInfoMatches.Groups[3].Value;
            

            Brick newBrick = new Brick(newBrickUUID, newBrickDesignID);


            //2. - Extract parts and bones data
            string partInfoLine = brickMatch.Value;

            // Admittedly not the most beautiful regex of all time
            string partAndBoneDataPattern = "<Part uuid=\"(.*)\" designID=\"(.*)\" partType=\"(.*)\" materials=\"(.*)\">(?:\\s|\n)*<Bone uuid=\"(.*)\" transformation=\"(.*)\" />"; 
            Regex partR = new Regex(partAndBoneDataPattern, RegexOptions.Multiline);
            var partMatches = partR.Matches(partInfoLine);

            // Each brick may contain multiple parts
            foreach (Match partMatch in partMatches)
            {
                // Part
                newPartUUID = partMatch.Groups[1].Value;
                newPartDesignID = partMatch.Groups[2].Value;            
                newPartType = partMatch.Groups[3].Value;
                newPartMaterials = partMatch.Groups[4].Value;
                Part newPart = new Part(newPartDesignID, newPartUUID, newPartType, newPartMaterials);

                // Bone
                newBoneUUID = partMatch.Groups[5].Value;
                newBoneTransformation = partMatch.Groups[6].Value;
                Bone newBone = new Bone(newBoneUUID,newBoneTransformation);
               
                // 
                newPart.ConnectBone(newBone);
                newBrick.AddPart(newPart);
            }

            bricks.Add(newBrick);
        }

        //3. Finalize brick list into the model
        newImportedModel.bricks = bricks;
        return newImportedModel;
    }


    /// <summary>
    /// Loads and reads a text file from the disk. 
    /// Returns its contents as a singular string if they are valid, otherwise an empty string.
    /// </summary>
    /// <param name="directory">The directory path to load from </param>
    /// <param name="filename"> Name of the file to read </param>
    /// <returns></returns>
    public string ReadFileFromDisk(string directory, string filename)
    {
        string fileContents = "";

        Debug.Log($"-- Attempting to load {filename} from path:{directory} --");

        try
        {
            // Create an instance of StreamReader to read from a file.
            using (StreamReader sr = new StreamReader( directory + "/" + filename))
            {
                fileContents = sr.ReadToEnd();
                
                if (!string.IsNullOrEmpty(fileContents))
                {
                    Debug.Log("Success!");
                }
            }
        }
        catch (Exception e)
        {
            // Error output
            Debug.LogError("Failure. The file could not be read:");
            Debug.LogError(e.Message);
        }

        return fileContents;     
    }

    
}
