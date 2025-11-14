using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data container for a Part
/// </summary>
public class Part 
{
    public string designID;
    public string uuID;
    public string partType;
    public string materials;

    public Bone bone;

    public Part(){ }
    public Part(string designID, string uuID, string partType, string materials)
    {
        this.designID = designID;
        this.uuID = uuID;
        this.partType = partType;
        this.materials = materials;
    }

    public void AddBone(Bone connectingBone)
    {
        this.bone = connectingBone;
    }

}
