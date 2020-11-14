using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class ProgressTrackerComponent : MonoBehaviour
	{
		[Serializable]
		public class TaskProgress
		{
			public string taskName;
			public bool isCompleted;

			[SerializeField] 
			private float progress;

			public TaskProgress(string taskName, float progress = 0f) {
				this.taskName = taskName;
				this.progress = progress;
				isCompleted = progress == 1f;
			}

			public float Progress {
				get => progress;
				set {
					progress = Mathf.Clamp01(value);
					isCompleted = progress == 1f;
				}
			}
		}

		[SerializeField] private List<TaskProgress> taskList = new List<TaskProgress>();

		public event Action Completed;
		public event Action<float> Changed;

		public TaskProgress this[string taskName] {
			get => taskList.Find((task) => task.taskName == taskName);
		}

		public void AddTask(string newTaskName, float progress = 0f) {
			taskList.Add(new TaskProgress(newTaskName, progress));
			Changed?.Invoke(GetProgress());
			if (progress == 1f) CheckCompleted();
		}

		public void CompleteTask(string taskName) {
			this[taskName].Progress = 1f;
			Changed?.Invoke(GetProgress());
			CheckCompleted();
		}

		public void ChangeTaskProgress(string taskName, float progress) {
			TaskProgress task = this[taskName];
			if (task == null) return;
			task.Progress = progress;
			Changed?.Invoke(GetProgress());
			if (progress == 1) CheckCompleted();
		}

		public void RemoveTask(string taskName) {
			taskList.Remove(this[taskName]);
			Changed?.Invoke(GetProgress());
		}

		public void CompleteAllTasks() => taskList.ForEach((task) => CompleteTask(task.taskName));

		public void RemoveAllTasks() => taskList.ForEach((task) => RemoveTask(task.taskName));

		public IEnumerator TrackTaskProgress(string taskName, AsyncOperation operation) {
			while (!operation.isDone) {
				ChangeTaskProgress(taskName, operation.progress);
				yield return null;
			}
		}

		public List<TaskProgress> GetTasks() => new List<TaskProgress>(taskList);
		public int GetTaskCount() => taskList.Count;

		public float GetProgress() {
			if (taskList.Count == 0f) return 1f;

			float progress = 0f;
			foreach (TaskProgress task in taskList)
				progress += task.Progress;
			return progress / taskList.Count;
		}


		private void CheckCompleted() {
			if (Completed == null) return;
			foreach (TaskProgress task in taskList)
				if (!task.isCompleted) return;
			Completed.Invoke();
		}
	}
}
