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
    private DialogueManager dialogueManager;

    void Awake()
    {
        character = GetComponent<Character>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        items = GameObject.Find("GameManager").GetComponent<ItemManager>();
        resources = GameObject.Find("GameManager").GetComponent<ResourceManager>();
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        dialogueManager = GameObject.Find("GameManager").GetComponent<DialogueManager>();
    }

    public IEnumerator ExecuteAction(Action action)
    {
        Executing = true;
        if(action.Type == ActionType.Cut)
        {
            Tile tile = action.TargetObject.GetComponent<Tile>();
            if(tile != null)
            {
                character.WalkToCoordinates(tile.X, tile.Y);
                yield return new WaitUntil(() => (character.cX == tile.X && character.cY == tile.Y));
                yield return new WaitForSeconds(1.0f);
                GameObject droppedResource = tile.item.GetComponent<DestructableItem>().droppedResource;
                Destroy(tile.item);
                tile.item = null;
                tile.ClearTag();
                GameObject resource = Instantiate(droppedResource, action.TargetObject.GetComponent<Renderer>().bounds.center, Quaternion.identity);
                resource.name = droppedResource.name + " ("+ tile.X +"," + tile.Y + ")";
                tile.AddItem(resource);
                if(tile.zone == Zone.Stockpile)
                    resources.AddResource(resource.GetComponent<Item>().resource, 1);
                action.SetStatus(Status.Successful);
            }
            else
            {
                action.SetStatus(Status.Failed);
            }
            tile.Free(gameObject);
        }
        else if(action.Type == ActionType.Haul)
        {
            Item item = action.TargetObject.GetComponent<Item>();
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
                        if(character.PickUpItem(parent))
                        {
                            character.WalkToCoordinates(target.X, target.Y);
                            yield return new WaitUntil(() => (character.cX == target.X && character.cY == target.Y));
                            character.DropItem(target);
                            resources.AddResource(item.resource, 1);
                            action.SetStatus(Status.Successful);
                            target.Free(gameObject);
                        }
                        else
                        {
                            action.SetStatus(Status.Failed);
                        }
                    }
                    else
                    {
                        action.SetStatus(Status.Failed);
                        Debug.Log(transform.name + " couldn't find a free stockpile tile to haul item: " + item.name);
                    }
                }
            }
            else
            {
                action.SetStatus(Status.Failed);
            }
        }
        else if(action.Type == ActionType.Build)
        {
            Tile tile = action.TargetObject.GetComponent<Tile>();
            GameObject passableTile = manager.GetPassableNeighbourTile(tile);
            int resourceCost = tile.toBuild.GetComponent<BuildableItem>().resourceCost;
            int count = 0;
            if(passableTile != null)
            {
                for(int i = 0; i < resourceCost; i++)
                {
                    List<GameObject> resources = manager.GetItemsOfResourceInStockpile(tile.toBuild.GetComponent<BuildableItem>().requiredResource);
                    Item resource = resources[0].GetComponent<Item>();
                    if(resource.GetLock(gameObject))
                    {
                        Tile resourceTile = resource.Parent.GetComponent<Tile>();
                        character.WalkToCoordinates(resourceTile.X, resourceTile.Y);
                        yield return new WaitUntil(() => (character.AtPosition(resourceTile.X, resourceTile.Y)));
                        character.PickUpItem(resourceTile);
                        Tile destTile = passableTile.GetComponent<Tile>();
                        character.WalkToCoordinates(destTile.X, destTile.Y);
                        yield return new WaitUntil(() => (character.cX == destTile.X && character.cY == destTile.Y));
                        character.DestroyItem();
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                if(count == resourceCost)
                {
                    buildManager.Build(action.TargetObject, tile.toBuild);
                    action.SetStatus(Status.Successful);
                    tile.Free(gameObject);
                    tile.ClearTag();
                }
                else
                {
                    action.SetStatus(Status.Failed);
                }
            }
            else
            {
                action.SetStatus(Status.Failed);
            }
            tile.Free(gameObject);
        }
        else if(action.Type == ActionType.PickUpItem)
        {
            List<GameObject> items = manager.GetItemsOfResource(action.TargetObject.GetComponent<Item>().resource);
            GameObject g = items[manager.GetRandomInt(0, items.Count)];
            Item item = g.GetComponent<Item>();
            Tile parent = item.Parent.GetComponent<Tile>();
            if(item.GetLock(gameObject) && character.WalkToCoordinates(parent.X, parent.Y))
            {
                yield return new WaitUntil(() => (character.AtPosition(parent.X, parent.Y)));
                if(character.PickUpItem(parent))
                {
                    dialogueManager.Speak(gameObject, action.Dialog);
                    yield return new WaitForSeconds(5.0f);
                    action.SetStatus(Status.Successful);
                }
                else
                    action.SetStatus(Status.Failed);
            }
            else
            {
                action.SetStatus(Status.notSent);
            }
        }
        else if(action.Type == ActionType.TalkToTarget)
        {
            GameObject targetCharacter = GameObject.Find(action.TargetObject.name);
            Character c = targetCharacter.GetComponent<Character>();
            if(c.GetLock(gameObject))
            {
                yield return new WaitUntil(() => c.currentAction == null);
                Tile tile = manager.GetPassableNeighbourTile(manager.Map[c.cX,c.cY].GetComponent<Tile>()).GetComponent<Tile>();
                character.WalkToCoordinates(tile.X, tile.Y);
                yield return new WaitUntil(() => (character.AtPosition(tile.X, tile.Y)));
                dialogueManager.Speak(gameObject, action.Dialog);
                yield return new WaitForSeconds(5.0f);
                action.SetStatus(Status.Successful);
            }
            else
            {
                action.SetStatus(Status.notSent);
            }
        }
        else if(action.Type == ActionType.GiveItemToTarget)
        {
            if(character.heldItem.GetComponent<Item>().resource == action.Precondition.HoldingItem.GetComponent<Item>().resource)
            {
                GameObject targetCharacter = GameObject.Find(action.TargetObject.name);
                Character c = targetCharacter.GetComponent<Character>();
                if(c.GetLock(gameObject))
                {
                    Tile tile = manager.GetPassableNeighbourTile(manager.Map[c.cX,c.cY].GetComponent<Tile>()).GetComponent<Tile>();
                    character.WalkToCoordinates(tile.X, tile.Y);
                    yield return new WaitUntil(() => (character.AtPosition(tile.X, tile.Y)));
                    character.DestroyItem();
                    dialogueManager.Speak(gameObject, action.Dialog);
                    yield return new WaitForSeconds(5.0f);
                    action.SetStatus(Status.Successful);
                }
                else
                {
                    action.SetStatus(Status.notSent);
                }
            }
            else
            {
                action.SetStatus(Status.Failed);
            }
        }
        else
        {
            Debug.Log("Action not found!");
            action.SetStatus(Status.Failed);
        }
        StartCoroutine(SendToTarget(action));
        Executing = false;
    }

    private IEnumerator SendToTarget(Action action)
    {
        //if action was successful, send a receipt to the target
        if(action.Status == Status.Successful)
        {
            GameObject target = GameObject.Find(action.TargetObject.name);
            if(target != null && target.tag == "Character")
            {
                target.GetComponent<ReceivingQueue>().QueueAction(action);
                yield return new WaitForSeconds(1.0f);
                target.GetComponent<Character>().Free(gameObject);
            }
        }
    }
}