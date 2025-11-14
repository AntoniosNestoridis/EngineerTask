using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents a model made out of various bricks.
/// All data about the model are contained within. 
/// </summary>
public class Model
{
    public int TotalBricks;

    public List<Brick> bricks = new List<Brick>(); // A list of the bricks that this model contains

    public void PrintModelDataToConsole()
    {

        Debug.Log("--- Model information --- ");
        Brick tempBrick;
        Part tempPart;

        for (short i = 0; i < bricks.Count; i++) 
        {
            tempBrick = bricks[i];

            Debug.Log($"--- Brick {i+1} --- ");
            Debug.Log($"Brick-UUID = {tempBrick.uuid}");
            Debug.Log($"Brick-DesignID = {tempBrick.designID}");

            Debug.Log("--- Parts and bones --- ");
            for (short j = 0; j < tempBrick.parts.Count; j++)        
            {
                tempPart = tempBrick.parts[j];

                Debug.Log($"Part {j+1} :: UUID = {tempPart.uuID} - DesignID = {tempPart.designID} - Type = {tempPart.partType} - Materials = {tempPart.materials}");
                Debug.Log($"Bone {j+1} :: UUID = {tempPart.bone.uuID} - Transformation = {tempPart.bone.transformation}");
            }
        }

        Debug.Log("--- --- --- --- --- ");
    }


}
