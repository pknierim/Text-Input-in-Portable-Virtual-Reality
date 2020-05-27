#if !UNITY_EDITOR && UNITY_WSA
using System;
using UnityEngine;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Class that has helper methods for scheduling actions on the main thread</summary>
	public class UWPThreadHelper: MonoBehaviour
	{
		/// <summary>Interface to make classes invokable</summary>
		private interface IInvokable
		{
			void Invoke();
		}

		private class SavedAction: IInvokable
		{
			private Action action;

			public SavedAction(Action action)
			{
				this.action = action;
			}

			public void Invoke()
			{
				action();
			}
		}

		private class SavedAction1<T>: IInvokable
		{
			private Action<T> action;
			private T parameter;

			public SavedAction1(Action<T> action, T parameter)
			{
				this.action = action;
				this.parameter = parameter;
			}

			public void Invoke()
			{
				action(parameter);
			}
		}

		private class SavedAction2<T, T2>: IInvokable
		{
			private System.Action<T, T2> action;
			private T parameter1;
			private T2 parameter2;

			public SavedAction2(System.Action<T, T2> action, T parameter1, T2 parameter2)
			{
				this.action = action;
				this.parameter1 = parameter1;
				this.parameter2 = parameter2;
			}

			public void Invoke()
			{
				action(parameter1, parameter2);
			}
		}

		/// <summary>List to store actions until they can be invoked on the main thread</summary>
		private ThreadsafeQueue<IInvokable> actionsOnMainThread;

		public static UWPThreadHelper Instance { get; private set; }

		public static void CreateInstance()
		{
			GameObject gameObject = new GameObject("UWPThreadHelper");
			Instance = gameObject.AddComponent<UWPThreadHelper>();
		}

		#region UNITY
		private void Awake()
		{
			actionsOnMainThread = new ThreadsafeQueue<IInvokable>();
		}

		private void Update()
		{
			ExecuteActionsOnMainThread();
		}

		private void OnDestroy()
		{
			if(actionsOnMainThread != null)
			{
				actionsOnMainThread.Clear();
			}
		}
		#endregion

		#region PUBLIC_METHODS
		public void ScheduleActionOnUWPThread(DispatchedHandler action)
		{
			CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
		}

		/// <summary>Schedules given action on the main thread</summary>
		public void ScheduleActionOnUnityThread(System.Action action)
		{
			if(actionsOnMainThread != null)
			{
				actionsOnMainThread.Enqueue(new SavedAction(action));
			}
		}

		/// <summary>Schedules given action on the main thread</summary>
		public void ScheduleActionOnUnityThread<T>(System.Action<T> action, T parameter)
		{
			if(actionsOnMainThread != null)
			{
				actionsOnMainThread.Enqueue(new SavedAction1<T>(action, parameter));
			}
		}

		/// <summary>Schedules given action on the main thread</summary>
		public void ScheduleActionOnUnityThread<T, T2>(System.Action<T, T2> action, T parameter1, T2 parameter2)
		{
			if(actionsOnMainThread != null)
			{
				actionsOnMainThread.Enqueue(new SavedAction2<T, T2>(action, parameter1, parameter2));
			}
		}
		#endregion

		#region PRIVATE_METHODS
		/// <summary>Executes stored actions on the main thread</summary>
		private void ExecuteActionsOnMainThread()
		{
			if(actionsOnMainThread != null)
			{
				while(actionsOnMainThread.Count > 0)
				{
					IInvokable savedAction = actionsOnMainThread.Dequeue();
					savedAction.Invoke();
					savedAction = null;
				}
			}
		}
		#endregion
	}
}
#endif