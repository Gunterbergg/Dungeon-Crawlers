using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers
{
	public class ComponentCollection : MonoBehaviour, IList<MonoBehaviour>
	{
		[SerializeField]
		private List<MonoBehaviour> components = new List<MonoBehaviour>();

		public event Action<ComponentCollection> CollectionChanged;
		public event Action<MonoBehaviour> Added;
		public event Action<MonoBehaviour> Removed;

		public MonoBehaviour this[int index] {
			get {
				if (index >= Count) return components[index % Count];
				return components[index];
			}
			set {
				if (Count <= index) components.Add(value);
				else components.Insert(index, value);
				CollectionChanged?.Invoke(this);
			}
		}
		public int Count { get => components.Count; }
		public bool IsReadOnly { get => ((IList<MonoBehaviour>)components).IsReadOnly; }

		IEnumerator IEnumerable.GetEnumerator() => GetGenericEnumerator();
		private IEnumerator GetGenericEnumerator() => GetEnumerator();

		public IEnumerator<MonoBehaviour> GetEnumerator() {
			List<MonoBehaviour> notNull = new List<MonoBehaviour>();
			foreach (MonoBehaviour component in components) {
				if (component == null) continue;
				notNull.Add(component);
				yield return component;
			}
			components = notNull;
		}

		public void ForEach(Action<MonoBehaviour> action) {
			foreach (MonoBehaviour component in this) action.Invoke(component);
		}

		public void ForEach<Type>(Action<Type> action) where Type : MonoBehaviour {
			foreach (MonoBehaviour component in this) {
				Type castComponent = component as Type;
				if (castComponent == null) continue;
				action.Invoke(castComponent);
			}
		}

		public void SetComponentList(IList<MonoBehaviour> newComponentList) {
			if (newComponentList == null) return;	
			components = (List<MonoBehaviour>)newComponentList;
			CollectionChanged?.Invoke(this);
		}

		public void Add(IEnumerable<MonoBehaviour> nodes) {
			foreach (MonoBehaviour node in nodes) Add(node);
		}
		
		public void Add(IEnumerator<MonoBehaviour> nodes) {
			while (nodes.MoveNext()) Add(nodes.Current);
			nodes.Reset();
		}

		public void Add(MonoBehaviour component) {
			components.Add(component);
			Added?.Invoke(component);
			CollectionChanged?.Invoke(this);
		}

		public void AddNodeComponents<C>(GameObject node) where C : MonoBehaviour {
			foreach (C nodeComponent in node.GetComponentsInChildren<C>()) Add(nodeComponent);
		}

		public bool Remove(MonoBehaviour component) {
			bool result = components.Remove(component);
			Removed?.Invoke(component);
			CollectionChanged?.Invoke(this);
			return result;
		}

		public void RemoveAt(int index) {
			Remove(components[index]);
		}

		public void Clear() {
			components.Clear();
			CollectionChanged?.Invoke(this);
		}

		public void SwapOrder(int originIndex, int targetIndex) {
			if (originIndex >= Count || targetIndex >= Count) return;
			MonoBehaviour originMonoBehaviour = components[originIndex];
			components[originIndex] = components[targetIndex];
			components[targetIndex] = originMonoBehaviour;
			CollectionChanged?.Invoke(this);
		}

		public MonoBehaviour Find(Predicate<MonoBehaviour> evaluation) {
			return components.Find(evaluation);
		}

		public Type Find<Type>(Predicate<Type> evaluation) where Type : MonoBehaviour {
			foreach (MonoBehaviour component in components) {
				Type derivedComponent = component as Type;
				if (component == null) continue;
				if (evaluation.Invoke(derivedComponent)) return derivedComponent;
			}
			return null;
		}

		public int FindIndex(Predicate<MonoBehaviour> evaluation) {
			return IndexOf(components.Find(evaluation));
		}

		public int IndexOf(MonoBehaviour item) {
			return components.IndexOf(item);
		}

		public void Insert(int index, MonoBehaviour item) {
			components.Insert(index, item);
			Added?.Invoke(item);
			CollectionChanged?.Invoke(this);
		}

		public bool Contains(MonoBehaviour item) {
			return ((IList<MonoBehaviour>)components).Contains(item);
		}

		public void CopyTo(MonoBehaviour[] array, int arrayIndex) {
			components.CopyTo(array, arrayIndex);
			CollectionChanged?.Invoke(this);
		}
	}
}
