#if UNITY_IOS
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif

namespace AdvancedInputFieldPlugin.Editor
{
	public class PostProcessBuild
	{
#if UNITY_IOS
		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
		{
			if (buildTarget == BuildTarget.iOS)
			{
				UnityEngine.Debug.Log("Adding -force_load flag to Other Linker Flags");

				string projectPath = PBXProject.GetPBXProjectPath(path);
				PBXProject project = new PBXProject();
				project.ReadFromFile(projectPath);

				string target = project.TargetGuidByName(PBXProject.GetUnityTargetName());
				project.AddBuildProperty(target, "OTHER_LDFLAGS", "-force_load $(PROJECT_DIR)/Libraries/Plugins/iOS/NativeKeyboard.a");
				project.WriteToFile(projectPath);
			}
		}
#endif
	}
}

