using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents a model made out of various bricks.
/// All data about the model are contained within. 
/// TODO: Turn into a struct instead?
/// </summary>
public class Model : MonoBehaviour
{
    public int TotalBricks;

    public List<Brick> bricks = new List<Brick>(); // A list of the bricks that this model contains

    public void PrintModelDataToConsole()
    {

        Debug.Log("--- Model information --- ");

        foreach (var brick in bricks)
        {
            Debug.Log($"Brick-UUID = {brick.uuid}");
            Debug.Log($"Brick-DesignID = {brick.designID}");

            Debug.Log($"Part-UUID = {brick.partuuID}");
            Debug.Log($"Part-DesignID = {brick.partDesignID}");
            Debug.Log($"Part-Type = {brick.partType}");
            Debug.Log($"Part-Materials = {brick.partMaterials}");

            Debug.Log($"Bone-UUID = {brick.uuid}");
            Debug.Log($"Bone-Transformation = {brick.boneTransformation}");

        }

        Debug.Log("--- --- --- --- ");
    }


}
