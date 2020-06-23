using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GDMenu2MODE
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Please enter the path to the game folders:");
			Console.WriteLine(@"(Example: c:\users\gamer\gdemu");
			string filePath = Console.ReadLine();
			GetTheFiles(filePath);
			Console.WriteLine("Job's Finished. Hit enter to exit.");
			Console.ReadLine();
		}

		private static void GetTheFiles(string path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			List<DirectoryInfo> directories = di.EnumerateDirectories().ToList();
			if (directories.Count >= 1)
			{
				WalkDirectories(directories);
			}
			else
			{
				IEnumerable<FileInfo> files = di.EnumerateFiles();
				string newFolderName = WalkFiles(files);
				if (!string.IsNullOrEmpty(newFolderName))
				{
					UpdateDirectory(di, newFolderName);
				}
			}
		}

		private static void WalkDirectories(IEnumerable<DirectoryInfo> directories)
		{
			foreach (DirectoryInfo directory in directories)
			{
				IEnumerable<FileInfo> files = directory.EnumerateFiles();
				string newFolderName = WalkFiles(files);
				if (!string.IsNullOrEmpty(newFolderName))
				{
					UpdateDirectory(directory, newFolderName);
				}
			}
		}

		private static string WalkFiles(IEnumerable<FileInfo> files)
		{
			foreach (FileInfo file in files)
			{
				if (file.Extension.ToUpper().Equals(".TXT"))
				{
					string[] data = File.ReadAllLines(file.FullName);
					return data[0];
				}
			}

			return string.Empty;
		}

		private static void UpdateDirectory(DirectoryInfo directory, string newFolderName)
		{
			string currentFolderName = directory.Name;
			char[] invalidFile = Path.GetInvalidFileNameChars();

			foreach (char c in invalidFile)
			{
				newFolderName = newFolderName.Replace(c.ToString(), "");
			}

			string originalCompleteName = directory.FullName;
			string newCompleteName = directory.FullName.Replace(currentFolderName, newFolderName);
			directory.MoveTo(newCompleteName);
			if (Directory.Exists(originalCompleteName))
			{
				Directory.Delete(originalCompleteName);
			}
		}
	}
}
