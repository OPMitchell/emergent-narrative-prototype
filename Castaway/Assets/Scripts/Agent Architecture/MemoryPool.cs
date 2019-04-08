using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MemoryPool
{
    private List<MemoryNode> nodes;
    private List<MemoryPattern> memories;
    private const int n = 3;
    private List<MemoryNode> hashmap;

    public MemoryPool()
    {
        nodes = new List<MemoryNode>();
        memories = new List<MemoryPattern>();
        hashmap = new List<MemoryNode>();
    }

    public void AddMemoryPattern(MemoryPattern mp)
    {
        memories.Add(mp);
        foreach(string keyword in mp.Keywords)
        {
            AddMemoryNode(keyword);
        }
        foreach (string keyword in mp.Keywords)
        {
            List<string> keywords = new List<string>(mp.Keywords);
            keywords.Remove(keyword);
            AddConnections(keyword, keywords);
        }
    }

    public MemoryPattern RetrieveMemoryPattern(int id)
    {
        foreach(MemoryPattern mp in memories)
        {
            if(mp.ID == id)
            {
                return mp;
            }
        }
        return null;
    }

    public MemoryPattern RetrieveMemoryPattern(string keyword)
    {
        hashmap.Clear();
        MemoryNode k = GetMemoryNodeByKeyword(keyword);
        if(k != null)
        {
            SpreadActivation(1, k, 0.5f);
            hashmap = hashmap.OrderByDescending(x => x.Activation).ToList();
            float highestActivation = hashmap[0].Activation;
            List<MemoryNode> topN = new List<MemoryNode>();
            int count = 1;
            foreach(MemoryNode node in hashmap)
            {
                if(node.Activation == highestActivation)
                    topN.Add(node);
                else if(node.Activation < highestActivation && count < n)
                {
                    count++;
                    highestActivation = node.Activation;
                    topN.Add(node);
                }
                else if (node.Activation < highestActivation && count >= n)
                    break;
            }
            return GetMatch(topN, keyword);
        }
        return null;
    }

    private MemoryPattern GetMatch(List<MemoryNode> topN, string keyword)
    {
        Dictionary<MemoryPattern, float> scores = new Dictionary<MemoryPattern, float>();
        foreach(MemoryPattern mp in memories)
        {
            if(!mp.Keywords.Contains(keyword))
                continue;
            float score = 0.0f;
            foreach(string keyword1 in mp.Keywords)
            {
                foreach(MemoryNode node in topN)
                {
                    if(keyword1 == node.Keyword)
                    {
                        score += node.Activation;
                    }
                }
            }
            scores.Add(mp, score);
        }

        var scoreList = scores.ToList();
        scoreList.Sort((pair1,pair2) => pair1.Value.CompareTo(pair2.Value));
        if(scoreList.Count > 0)
            return scoreList[scoreList.Count-1].Key;
        return null;
    }

    private void SpreadActivation(int rank, MemoryNode n, float activationAmount)
    {
        if(rank == 1)
        {
            IncreaseActivation(rank, n, activationAmount);
            hashmap.Add(n);
        }
        rank++;
        foreach(Connection c in n.Connections)
        {
        if(!hashmap.Contains(c.partner))
        {
                IncreaseActivation(rank, c.partner, activationAmount/(float)n.Connections.Count);
                hashmap.Add(c.partner);
            }
        }
        foreach(Connection c in n.Connections)
        {
            if(rank == 2 || !hashmap.Contains(c.partner))
                SpreadActivation(rank, c.partner, activationAmount);
        }
    }

    private void IncreaseActivation(int rank, MemoryNode n, float activationAmount)
    {
        if(activationAmount < n.Activation)
            n.Activation = ((float)(rank*n.Activation) + (n.Activation+activationAmount))/(float)(rank+1.0f);  
    }

    private void AddMemoryNode(string keyword)
    {
        if(GetMemoryNodeByKeyword(keyword) == null)
        {
            nodes.Add(new MemoryNode(keyword));
        }
    }

    private void AddConnections(string keyword, List<string> keywords)
    {
        MemoryNode a = GetMemoryNodeByKeyword(keyword);
        if (a != null)
        {
            foreach(string k in keywords)
            {
                MemoryNode b = GetMemoryNodeByKeyword(k);
                if(b != null)
                {
                    Connection newConnection = new Connection(b);
                    Connection c = SearchForConnection(a, newConnection);
                    if(c == null)
                        a.Connections.Add(newConnection); 
                    else
                        c.IncreaseStrength();  
                }
            }
        }
    }

    private MemoryNode GetMemoryNodeByKeyword(string keyword)
    {
        foreach (MemoryNode n in nodes)
        {
            if(n.Keyword == keyword)
            {
                return n;
            }
        }
        return null;
    }

    private Connection SearchForConnection(MemoryNode n, Connection newConnection)
    {
        foreach (Connection c in n.Connections)
        {
            if(c.partner == newConnection.partner)
            {
                return c;
            }
         }
        return null;
    }

    public List<MemoryNode> GetMemoryNodes()
    {
        return nodes;
    }
}
