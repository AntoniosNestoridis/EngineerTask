using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data holder for a brick of the model
/// </summary>
public class Brick 
{
    // Brick
    public string uuid;
    public string designID;

    // Parts
    public List<Part> parts = new List<Part>();

    public Brick(){ }
    public Brick(string uuid , string designID)
    {
        this.uuid = uuid;
        this.designID = designID;
    }

    public void AddPart(Part newPart)
    {
        if (!parts.Contains(newPart))
        {
            parts.Add(newPart);
        }
    }

    public void AddParts(List<Part> newParts)
    {
        parts = newParts;
    }
}
