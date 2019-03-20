using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
    public class Node : MonoBehaviour
	{
        public int F
        {
            get
            {
                return G + H;
            }
        }
		public int H{get;set;}
		public int G{get;set;}
		public int X {get;set;}
		public int Y {get;set;}
		[SerializeField] private bool Passable;
		public Node Parent {get; set;}

		public Node(int x, int y, bool passable)
		{
            G = 0;
            H = 0;
			X = x;
			Y = y;
			Passable = passable;
		}

		public bool IsPassable()
		{
			return Passable;
		}

		public void SetPassable(bool a)
		{
			Passable = a;
		}
	}