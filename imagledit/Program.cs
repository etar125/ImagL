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
		
		public string Path = "imagl.cs";
		public Dictionary<string, bool> del = 
		{
			{ "print", false },
			{ "pause", false },
			{ "set", false },
		};
		
		public static void Main(string[] args)
		{
			Console.WriteLine("ImageL Code Edit v0.01\nBy Etar125\n\nChecking arguments...");
			var handle = GetConsoleWindow();
			foreach(string s in args)
			{
				if(s == "/s" || s == "/silent") {
					ShowWindow(handle, SW_HIDE); Console.WriteLine("...Silent!"); }
				else if(s[0] == '/' && del.ContainsKey(s.Remove(0, 1)))
				        del[s.Remove(0, 1)] = true;
				else if(File.Exists(s)) {
					Path = s; Console.WriteLine("...File found!: \\'" + Path + "\\'"); }
				else
					Console.WriteLine("!!! Unkown argument/file not exists/unkown command: \\'" + s + "\\'");
			}
			if(File.Exists(Path))
				Console.WriteLine("DONE! File: \\'" + Path + "\\'");
			else
			{
				Console.WriteLine("FAIL! Not found file: \\'" + Path + "\\'");
				Thread.Sleep(5000);
				Environment.Exit(0);
			}
			Console.WriteLine("Read file...");
			string[] file = File.ReadAllLines(Path);
		}
	}
}