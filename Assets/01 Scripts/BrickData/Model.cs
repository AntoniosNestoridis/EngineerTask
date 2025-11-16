using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a design model made out of various bricks and sub-parts.
/// </summary>
public class Model
{
    public int TotalBricks;

    public List<Brick> bricks = new List<Brick>(); // A list of the bricks that this model contains


    public int GetTotalPartsCount()
    {

        int totalParts = 0;

        foreach (var brick in bricks)
        {
            totalParts += brick.parts.Count;
        }

        return totalParts;
    }

    public int GetUniquePartsCount()
    {

        int totalUniqueParts = 0;
        List<string> seenParts = new List<string>();

        foreach (var brick in bricks)
        {
            foreach (var part in brick.parts)
            {
                // New unique part found
                if (!seenParts.Contains(part.designID))
                {
                    seenParts.Add(part.designID);
                    totalUniqueParts++;
                }
            }
        }

        return totalUniqueParts;
    }


    /// <summary>
    /// Prints all current data of the model and its bricks.
    /// Mostly an editor testing function.
    /// </summary>
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
