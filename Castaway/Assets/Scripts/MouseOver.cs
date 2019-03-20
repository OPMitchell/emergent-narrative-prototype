using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour 
{
	public GameObject selected;
	public GameObject selected2;
	private GameObject selectedRef;

    private GameManager manager;
    private bool mouseOver = false;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnMouseDown()
    {
        manager.ClearTileSelections();
        manager.mouseDown = true;
        ClearSelection();
        selectedRef = Instantiate(selected2, transform.position, Quaternion.identity);
    }

    void OnMouseOver()
    {
        if(manager.mouseDown)
        {
            manager.SetSelected(gameObject);
            ClearSelection();
            selectedRef = Instantiate(selected2, transform.position, Quaternion.identity);
        }
    }

    void OnMouseUp()
    {
        manager.mouseDown = false;
    }

    public void ClearSelection()
    {
        Destroy(selectedRef);
    }
}
