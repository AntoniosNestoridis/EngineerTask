/// <summary>
/// A data container for a part of a brick.
/// </summary>
public class Part 
{
    // Part data
    public string designID;
    public string uuID;
    public string partType;
    public string materials;


    // Bone - We assume for the moment that only one bone can be connected to each part
    public Bone bone;



    public Part(){ }
    public Part(string designID, string uuID, string partType, string materials)
    {
        this.designID = designID;
        this.uuID = uuID;
        this.partType = partType;
        this.materials = materials;
    }

    /// <summary>
    /// Connects a new bone to this part. Will replace the existing one should there be one.
    /// </summary>
    /// <param name="connectingBone">The new bone to connect with this part </param>
    public void ConnectBone(Bone connectingBone)
    {
        this.bone = connectingBone;
    }

}
