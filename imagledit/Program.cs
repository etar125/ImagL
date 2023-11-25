// etar125
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace imagledit
{
	class Program
	{
		const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int SW_Min = 2;
        const int SW_Max = 3;
        const int SW_Norm = 4;
 
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
 
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		
		public static string Path = "imaglmod.cs";
		public static Dictionary<string, bool> del = new Dictionary<string, bool>
		{
			{ "print", false },
			{ "pause", false },
			{ "set", false },
			{ "input", false },
			{ "goto", false },
			{ "clear", false },
		};
		
		public static List<string> Remove(List<string> ara, int strt, int end)
		{
			List<string> mod = ara;
			for(int i = end; i > strt; i--)
				mod.RemoveAt(i);
			return mod;
		}
		
		public static void Main(string[] args)
		{
			Console.WriteLine("ImagL Code Edit v0.05\nBy Etar125\n\nChecking arguments...");
			var handle = GetConsoleWindow();
			foreach(string s in args)
			{
				if(s == "/s" || s == "/silent") {
					ShowWindow(handle, SW_HIDE); Console.WriteLine("...Silent!"); }
				else if(s[0] == '/' && del.ContainsKey(s.Remove(0, 1)))
				{
			        del[s.Remove(0, 1)] = true;
				}
				else if(File.Exists(s)) {
					Path = s; Console.WriteLine("...File found!: '" + Path + "'"); }
				else
					Console.WriteLine("!!! Unkown argument/file not exists/unkown command: '" + s + "'");
			}
			if(File.Exists(Path))
				Console.WriteLine("DONE! File: '" + Path + "'");
			else
			{
				Console.WriteLine("FAIL! Not found file: '" + Path + "'");
				Thread.Sleep(5000);
				Environment.Exit(0);
			}
			Console.WriteLine("Read file...");
			List<string> file = new List<string> { };
			foreach(string s in File.ReadAllLines(Path))
				file.Add(s);
			Console.WriteLine("DONE!\nEdit file...\nStep 1");
			List<int> del2 = new List<int> { };
			for(int i = int.Parse(file[file.Count - 2].Split(' ')[0]); i < file.Count - 2; i++)
			{
				string[] splt = file[i].Split(' ');
				//Console.WriteLine(string.Join(" | ", splt));
				if(del.ContainsKey(splt[0]) && del[splt[0]])
				{
					//Console.WriteLine(del.ContainsKey(splt[0]) + "|" + del[splt[0]]);
					del2.Add(int.Parse(splt[1]));
					file = Remove(file, int.Parse(splt[2]), int.Parse(splt[3]));
				}
			}
			Console.WriteLine("DONE!\nStep 2");
			foreach(int s in del2)
				file.RemoveAt(s);
			Console.WriteLine("DONE!\nSave file...");
			File.WriteAllLines(Path, file.ToArray());
			Console.WriteLine("DONE!");
			Thread.Sleep(2000);
		}
	}
}