using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class IXMLFileParser : MonoBehaviour
{

    public string FileDirectory; // TODO: Eventually we want this to be the Application.Filepath
    public string filename;

    public bool ReadFile;

    void Start()
    {
       
    }


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
            newModel.PrintModelDataToConsole();

        } 
    }


    /// <summary>
    /// Parses an IXFML file and creates and returns a new instance of a model or null.
    /// </summary>
    public Model ParseFile(string fileContents)
    {
        Model newImportedModel = new Model();

        // We assume a valid string format here
        Debug.Log("Attempting to Parse text file into data...");

        string text = fileContents;
        string brickListPattern = "<Brick(?:.*?|\n)*</Brick>"; 

        // Instantiate the regular expression object.
        Regex r = new Regex(brickListPattern, RegexOptions.Multiline);

        // Match the regular expression pattern against a text string.
        var brickMatches = r.Matches(text);
        int matchCount = brickMatches.Count;

        Debug.Log($"Found {matchCount} bricks in model");
        // Assign later?
        newImportedModel.TotalBricks = matchCount;

        // Caching temp variables to reuse during data creation below.
        string newBrickDesignID;
        string newBrickUUID;

        string newPartUUID;
        string newPartDesignID;
        string newPartType;
        string newPartMaterials;

        string newBoneUUID;
        string newBoneTransformation;

        List<Brick> bricks = new List<Brick>();

        // Extra brick, part and bone data from each match
        foreach (Match brickMatch in brickMatches)
        {
            // Debug.Log(brickMatch);
            
            //1. Brick data
            string brickInfoLine = brickMatch.Value;

            string brickDataPattern = "(designID=\"(.*)\" uuid=\"(.*)\">)";
            Regex brickR = new Regex(brickDataPattern, RegexOptions.None);
            Match brickInfoMatches = brickR.Match(brickInfoLine);

            newBrickDesignID = brickInfoMatches.Groups[2].Value;
            newBrickUUID = brickInfoMatches.Groups[3].Value;
            
            Brick newBrick = new Brick(newBrickUUID, newBrickDesignID);

            //2. - Parts and bones
            string partInfoLine = brickMatch.Value;

            // Admittedly not the most beautiful regex of all time
            string partAndBoneDataPattern = "<Part uuid=\"(.*)\" designID=\"(.*)\" partType=\"(.*)\" materials=\"(.*)\">(?:\\s|\n)*<Bone uuid=\"(.*)\" transformation=\"(.*)\" />"; 
            Regex partR = new Regex(partAndBoneDataPattern, RegexOptions.Multiline);
            var partMatches = partR.Matches(partInfoLine);


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
               
                newPart.AddBone(newBone);

                // Clean this up a bit perhaps
                newBrick.AddPart(newPart);
            }

            bricks.Add(newBrick);
        }


        // Add bricks to the model list
        newImportedModel.bricks = bricks;

        return newImportedModel;
    }

    /// <summary>
    /// Reads a text file from the disk and returns its contents as a string or an empty string if there is an error in the process.
    /// </summary>
    /// <param name="directory">The directory path </param>
    /// <param name="filename"> Name of the file to read </param>
    /// <returns></returns>
    public string ReadFileFromDisk(string directory, string filename)
    {
        string fileContents = "";

        Debug.Log("Attempting to read file contents...");

        try
        {
            // TODO: Check if directory and/or filename exist first before attempting to read them. But I guess the try catch does the same job.
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
            Debug.LogError("The file could not be read:");
            Debug.LogError(e.Message);
        }

        return fileContents;     
    }

    
}
