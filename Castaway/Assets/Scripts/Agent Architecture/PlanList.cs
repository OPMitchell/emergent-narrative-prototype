using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

public class PlanList
{
    private List<Plan> plans = new List<Plan>();

    public void Add(Plan plan)
    {
        if(!plans.Any(x => x.isEqual(plan)))
            plans.Add(plan);
    }

    public void Remove(Plan plan)
    {
        plans.Remove(plans.Single(x => x == plan));
    }

    public Plan GetBestPlan(GameManager manager, ActionQueue actionQueue, MemoryManager memoryManager, Transform t)
    {
        foreach(Plan p in plans)
        {
            p.Evaluate(memoryManager, actionQueue);
            Testing.WriteToLog(t.name, Testing.GetPlanInfo(p));
        }
        var sorted = plans.OrderByDescending(x => x.Score);
        List<Plan> best = new List<Plan>();
        float bestScore = sorted.FirstOrDefault().Score;
        foreach(Plan p in sorted)
        {
            if(p.Score == bestScore)
                best.Add(p);
            else if(p.Score < bestScore)
                break;
        }
        return best[manager.GetRandomInt(0, best.Count)];
    }

    public int Count()
    {
        return plans.Count;
    }
}
