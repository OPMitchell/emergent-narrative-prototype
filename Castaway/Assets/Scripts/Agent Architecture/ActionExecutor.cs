using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExecutor : MonoBehaviour 
{
    public bool Executing{get;set;}
    private Character character;
    private GameManager manager;
    private ItemManager items;
    private ResourceManager resources;
    private BuildManager buildManager;
	private System.Random rnd;

    void Start()
    {
        rnd = new System.Random();
        character = GetComponent<Character>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        items = GameObject.Find("GameManager").GetComponent<ItemManager>();
        resources = GameObject.Find("GameManager").GetComponent<ResourceManager>();
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
    }

    public IEnumerator ExecuteAction(Action action)
    {
        Executing = true;
        if(action.Type == "Cut_Tree")
        {
            Tile tile = action.TargetGameObject.GetComponent<Tile>();
            if(tile != null)
            {
                character.WalkToCoordinates(tile.X, tile.Y);
                yield return new WaitUntil(() => (character.cX == tile.X && character.cY == tile.Y));
                tile.DisabledTagged();
                yield return new WaitForSeconds(1.0f);
                Destroy(tile.item);
                tile.item = null;
                GameObject logs = Instantiate(items.logs, action.TargetGameObject.GetComponent<Renderer>().bounds.center, Quaternion.identity);
                logs.name = "Logs" + " ("+ tile.X +"," + tile.Y + ")";
                tile.AddItem(logs);
                if(tile.zone == Zone.Stockpile)
                    resources.AddResource(Resource.logs, 1);
                action.Status = Status.Successful;
            }
            else
            {
                action.Status = Status.Failed;
            }
            tile.Free(gameObject);
        }
        else if(action.Type == "HaulItem")
        {
            Item item = action.TargetGameObject.GetComponent<Item>();
            if(item != null)
            {
                List<GameObject> emptyTiles = manager.GetEmptyStockpileTiles();
                if(emptyTiles.Count > 0)
                {
                    bool a = false;
                    Tile target = null;
                    for(int i = 0; i < emptyTiles.Count; i++)
                    {
                        target = emptyTiles[i].GetComponent<Tile>();
                        a = target.GetLock(gameObject);
                        if(a)
                            break;
                    }
                    if(a)
                    {
                        target.GetLock(gameObject);
                        Tile parent = item.Parent.GetComponent<Tile>();
                        character.WalkToCoordinates(parent.X, parent.Y);
                        yield return new WaitUntil(() => (character.cX == parent.X && character.cY == parent.Y));
                        character.PickUpItem(parent);
                        character.WalkToCoordinates(target.X, target.Y);
                        yield return new WaitUntil(() => (character.cX == target.X && character.cY == target.Y));
                        character.DropItem(target);
                        resources.AddResource(item.resource, 1);
                        action.Status = Status.Successful;
                        target.Free(gameObject);
                    }
                    else
                    {
                         action.Status = Status.Failed;
                         Debug.Log(transform.name + " couldn't find a free stockpile tile to haul item: " + item.name);
                    }
                }
            }
            else
            {
                action.Status = Status.Failed;
            }
            item.Free(gameObject);
        }
        else if(action.Type == "Build")
        {
            Tile tile = action.TargetGameObject.GetComponent<Tile>();
            GameObject passableTile = manager.GetPassableNeighbourTile(tile);
            List<GameObject> resources = manager.GetItemsOfResource(tile.toBuild.GetComponent<BuildableItem>().requiredResource);
            Item resource = resources[0].GetComponent<Item>();
            if(passableTile != null && resource.GetLock(gameObject))
            {
                Tile resourceTile = resource.Parent.GetComponent<Tile>();
                character.WalkToCoordinates(resourceTile.X, resourceTile.Y);
                yield return new WaitUntil(() => (character.AtPosition(resourceTile.X, resourceTile.Y)));
                character.PickUpItem(resourceTile);
                Tile destTile = passableTile.GetComponent<Tile>();
                character.WalkToCoordinates(destTile.X, destTile.Y);
                yield return new WaitUntil(() => (character.cX == destTile.X && character.cY == destTile.Y));
                character.DestroyItem();
                buildManager.Build(action.TargetGameObject, tile.toBuild);
                action.Status = Status.Successful;
                tile.Free(gameObject);
                tile.ToggleTagged(tile.tag);
            }
            else
            {
                action.Status = Status.Failed;
            }
            tile.Free(gameObject);
        }
        Executing = false;
    }
}