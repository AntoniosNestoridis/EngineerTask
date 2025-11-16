/// <summary>
/// A data container for a Bone.
/// </summary>
public class Bone 
{
    public string uuID;
    public string transformation;


    public Bone(){ }

    public Bone(string uuID, string transformation)
    {
        this.uuID = uuID;
        this.transformation = transformation;
    }
}
