using System.Collections;
using System.Collections.Generic;

public class MemoryNode
{
    public string Keyword {get; private set;}
    public float Activation {get; set;}
    public List<Connection> Connections {get; private set;}

    public MemoryNode(string keyword)
    {
        Keyword = keyword;
        Activation = 1.0f;
        Connections = new List<Connection>();
    }
}
