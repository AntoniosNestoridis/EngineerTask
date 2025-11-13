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

    public List<Brick> bricks; // A list of the bricks that this model contains

}
