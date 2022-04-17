using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : Node
{
    public Branch()
    {
        name = "Branch";
    }
    public Branch(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Debug.Log("Processing Branch: " + this.name);
        Status childStatus = children[currentChild].Process();

        
        if (childStatus == Status.FAILURE)
        {
            currentChild = 0;
            return Status.FAILURE;
        }
        
        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
        }
        return Status.RUNNING;
    }
}
