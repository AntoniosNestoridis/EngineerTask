using System.Collections.Generic;

/// <summary>
/// A brick of the model containing one or more parts and equivalent data
/// </summary>
public class Brick 
{
    // Brick data
    public string uuid;
    public string designID;

    // Parts data
    public List<Part> parts = new List<Part>();



    public Brick(){ }
    public Brick(string uuid , string designID)
    {
        this.uuid = uuid;
        this.designID = designID;
    }

    /// <summary>
    /// Adds a new part to the brick. 
    /// </summary>
    /// <param name="newPart"> The new part to be added </param>
    public void AddPart(Part newPart)
    {
        if (parts != null && !parts.Contains(newPart))
        {
            parts.Add(newPart);
        }
       
    }

    /// <summary>
    /// Sets/replaces the current list of parts for this brick with the provided one.
    /// </summary>
    /// <param name="newParts">The list of new parts to set</param>
    public void SetParts(List<Part> newParts)
    {
        if (newParts != null)
        {
            parts = newParts;
        }     
    }
}
