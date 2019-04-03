using System.Collections;
using System.Collections.Generic;

public class Connection
{
    public MemoryNode partner {get; private set;}
    public int Strength {get; private set;}

    public Connection(MemoryNode n)
    {
        partner = n;
        Strength = 0;
    }

    public void IncreaseStrength()
    {
        Strength++;
    }
}
