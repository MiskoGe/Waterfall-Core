using System.Collections.Generic;
using System.IO;
using Waterfall.System.Core.ProcessManager;

namespace Waterfall.System.Security.FS
{
	public static class FileManagment
	{
		static Dictionary<string, int> CantDeleteFiles = new Dictionary<string, int>();
		static Dictionary<string, int> ReadOnlyFiles = new Dictionary<string, int>();
		static Dictionary<string, int> CantDeleteDirectories = new Dictionary<string, int>();

		static Dictionary<string, int> ProtectedDirectories = new Dictionary<string, int>();

		public static void Setup()//0 - admin and 1 - root
		{

		}
		public static bool DeleteFile(string path, Process proc)
		{
			if (path.EndsWith(@"\"))
				path = path.Substring(0, path.Length - 2);
			foreach (var item in ProtectedDirectories)
			{
				if (path.StartsWith(item.Key))
				{
					if (proc.UserControl.PermissionLevel < item.Value + 1)
						return false;
				}
			}
			if (CantDeleteFiles.ContainsKey(path))
			{
				if (proc.UserControl.PermissionLevel >= CantDeleteFiles[path] + 1)
				{
					File.Delete(path);
					return true;
				}
				return false;
			}
			else
			{
				File.Delete(path);
				return true;
			}
		}
		public static bool DeleteDirectory(string path, Process proc)
		{
			if (path.EndsWith(@"\"))
				path = path.Substring(0, path.Length - 2);
			if (path == @"0:\RadianceOS")
				return false;
			foreach (var item in ProtectedDirectories)
			{
				if (path.StartsWith(item.Key))
				{
					if (proc.UserControl.PermissionLevel < item.Value + 1)
						return false;
				}
			}
			if (CantDeleteDirectories.ContainsKey(path))
			{
				if (proc.UserControl.PermissionLevel >= CantDeleteDirectories[path] + 1)
				{
					Directory.Delete(path, true);
					return true;
				}
				return false;
			}
			else
			{
				Directory.Delete(path, true);
				return true;
			}
		}
		public static bool CanEdit(string path, Process proc)
		{
			if (path.EndsWith(@"\"))
				path = path.Substring(0, path.Length - 2);
			foreach (var item in ProtectedDirectories)
			{
				if (path.StartsWith(item.Key))
				{
					if (proc.UserControl.PermissionLevel < item.Value + 1)
						return false;
				}
			}
			if (CantDeleteFiles.ContainsKey(path))
			{
				if (proc.UserControl.PermissionLevel >= CantDeleteFiles[path] + 1)
				{
					return true;
				}
				return false;
			}
			else
			{
				return true;
			}
		}
		public static bool CanCreate(string path, Process proc)
		{
			if (path.EndsWith(@"\"))
				path = path.Substring(0, path.Length - 2);
			foreach (var item in ProtectedDirectories)
			{
				if (path.StartsWith(item.Key))
				{
					if (proc.UserControl.PermissionLevel < item.Value + 1)
						return false;
				}
			}
			return true;
		}
	}
}
