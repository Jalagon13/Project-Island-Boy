using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public abstract class RuntimeSet<T> : ScriptableObject
	{
		protected List<T> _items = new();
		
		public int ListSize { get {return _items.Count; }}
		
		public void Initialize()
		{
			_items.Clear();
		}
		
		public T GetItemIndex(int index)
		{
			return _items[index];
		}
		
		public void AddToList(T thingToAdd)
		{
			if(!_items.Contains(thingToAdd))
			{
				_items.Add(thingToAdd);
			}
		}
		
		public void RemoveFromList(T thingToRemove)
		{
			if(_items.Contains(thingToRemove))
			{
				_items.Remove(thingToRemove);
			}
		}
	}
}
