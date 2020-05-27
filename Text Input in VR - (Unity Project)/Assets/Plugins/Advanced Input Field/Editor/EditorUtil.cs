using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace AdvancedInputFieldPlugin.Editor
{
	public class EditorUtil
	{
		public static List<T> FindObjectsOfTypeAll<T>(bool includeInactive = false)
		{
			List<T> results = new List<T>();
			SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => results.AddRange(g.GetComponentsInChildren<T>(includeInactive)));
			return results;
		}

		public static string GetCurrentDirectoryPath()
		{
			string path = "Assets";
			foreach(UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), UnityEditor.SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if(File.Exists(path))
				{
					path = Path.GetDirectoryName(path);
				}
				break;
			}

			return path;
		}
	}
}
