// etar125
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.Linq;

namespace imaglc
{
	class Program
	{
		public string MSB = @"msb\";
		
		
		const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int SW_Min = 2;
        const int SW_Max = 3;
        const int SW_Norm = 4;
 
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
 
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		
        public static char get(int num)
        {
            if (num == 0)
                return ' ';
            if (num == 1)
                return '\n';
            return "**qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNMйцукенгшщзхъфывапролджэячсмитьбюёЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁ1234567890!@#$%^&*()_+-={}[];:'\"\\|/,.<>?`~"[num];
        }
        
        public static void Main(string[] args)
		{
        	Console.WriteLine("ImageL Compiler v0.12\nBy Etar125\n\nChecking arguments...");
        	Dictionary<string, bool> use = new Dictionary<string, bool> { { "print", true }, { "set", true }, { "pause", true } };
        	string path = "main.png";
        	bool debug = true;
			var handle = GetConsoleWindow();
			foreach(string s in args)
			{
				if(s == "/s" || s == "/silent") {
					ShowWindow(handle, SW_HIDE); Console.WriteLine("...Silent!"); }
				else if(s == "/r" || s == "/release") {
					debug = false; Console.WriteLine("...Release!"); }
				else if(File.Exists(s)) {
					path = s; Console.WriteLine("...File found!: '" + path + "'"); }
				else
					Console.WriteLine("!!! Unkown argument(or file not exists): '" + s + "'");
			}
			if(File.Exists(path))
				Console.WriteLine("DONE! File: '" + path + "'");
			else
			{
				Console.WriteLine("FAIL! Not found file: '" + path + "'");
				Thread.Sleep(5000);
				Environment.Exit(0);
			}
			Console.WriteLine("Converting code...");
			Bitmap bmp = new Bitmap(Image.FromFile(path));
			List<string> cms = new List<string> { };
			for(int y = 0; y < bmp.Height; y++)
			{
				for(int x = 0; x < bmp.Width; x++)
				{
					if(bmp.GetPixel(x, y) == Color.FromArgb(255, 255, 0, 0)) // Print
					{
						if(use["print"])
							use["print"] = false;
						string result = "";
						for(int x2 = x + 1; x2 < bmp.Width; x2++)
						{
							Color clr = bmp.GetPixel(x2, y);
							if(clr == Color.FromArgb(255, 0, 0, 0))
								break;
							if(clr.R == 64)
								result += get(clr.G);
						}
						cms.Add("new Command(Command.CMS.Print, new string[] { \"" + result + "\" })");
					}
					else if(bmp.GetPixel(x, y) == Color.FromArgb(255, 0, 255, 0)) // Set
					{
						if(use["set"])
							use["set"] = false;
						string name = "";
						string value = "";
						for(int x2 = x + 1; x2 < bmp.Width; x2++)
						{
							Color clr = bmp.GetPixel(x2, y);
							if(clr == Color.FromArgb(255, 255, 255, 255))
							{
								for(int x3 = x2 + 1; x3 < bmp.Width; x3++)
								{
									Color clr2 = bmp.GetPixel(x3, y);
									if(clr2 == Color.FromArgb(255, 0, 0, 0))
										break;
									if(clr2.R == 64)
										value += get(clr2.G);
								}
								break;
							}
							if(clr.R == 64)
								name += get(clr.G);
						}
						cms.Add("new Command(Command.CMS.Set, new string[] { \"" + name + "\", \"" + value + "\" })");
					}
					else if(bmp.GetPixel(x, y) == Color.FromArgb(255, 255, 255, 0)) // Pause
					{
						if(use["pause"])
							use["pause"] = false;
						cms.Add("new Command(Command.CMS.Pause, null)");
					}
				}
			}
			Console.WriteLine("DONE!");
			Console.WriteLine("Edit ImagL code...");
			string[] file = File.ReadAllLines("imagl.cs");
			file[33] = "\t\tpublic static Command[] app = { " + string.Join(", ", cms.ToArray()) + " };";
			File.WriteAllLines("imaglmod.cs", file);
			Console.WriteLine("DONE!");
			Process a = new Process();
			Console.WriteLine("Edit file...");
			string gen = "";
			for(int i = 0; i < use.Count; i++)
				if(use.ElementAt(i).Value == true)
					gen += " /" + use.ElementAt(i).Key;
			Console.WriteLine("...Arguments done!");
			a.StartInfo.FileName = @"imagledit";
			a.StartInfo.Arguments = "imaglmod.cs" + gen;
			Console.WriteLine("Start ImaglL Code Editor...");
			a.Start();
			Console.WriteLine("...");
			a.WaitForExit();
			Console.WriteLine("DONE!");
			Console.WriteLine("Start MSBuild...");
			a.StartInfo.FileName = @"msb\msbuild";
			a.StartInfo.Arguments = "compile.csproj /noconlog /nologo";
			if(debug) a.StartInfo.Arguments += " /p:Configuration=Debug";
			else a.StartInfo.Arguments += " /p:Configuration=Release";
			a.Start();
			Console.WriteLine("Compiling...");
			a.WaitForExit();
			Console.WriteLine("DONE!");
			Thread.Sleep(5000);
		}
	}
}