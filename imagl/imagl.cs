// etar125
using System;
using System.Collections.Generic;

namespace imagl
{
	class Command
	{
		public Command() { }
		
		public Command(CMS Cmnd, string[] Arguments)
		{
			this.Cmnd = Cmnd;
			args = Arguments;
		}
		
		public enum CMS
		{
			Print,
			Pause,
			Set,
			Input, 
			Clear,
			Title,
			Label,
			Goto,
			Empty,
		}
		
		public CMS Cmnd;
		
		public string[] args;
	}
	class Program
	{
		public static Command[] app = { };
		public static Dictionary<string, string> vars = new Dictionary<string, string> { { "Version", "v0.15" } };
		public static string ConvertString(string str)
		{
			string result = "";
			for(int i = 0; i < str.Length; i++)
			{
				try
				{
					if(str[i] == '\\' && str[i + 1] == '%')
					{
						result += "%";
						i++;
						continue;
					}
					if(str[i] == '\\' && str[i + 1] == 'n')
					{
						result += "\n";
						i++;
						continue;
					}
					if(str[i] == '%')
					{
						string name = "";
						for(int a = i + 1; a < str.Length; a++)
						{
							if(str[a] != '%')
							{
								name += str[a];
								continue;
							}
							name = str.Substring(i + 1, a - i - 1);
							str = str.Remove(i, a + 1 - i);
							if(!vars.ContainsKey(name))
							{
								Console.WriteLine("Не найдена переменная " + name + "\nВ строке: \"" + str + "\"");
								throw new Exception();
							}
							str = str.Insert(i, vars[name]);
							break;
						}
					}
					result += str[i];
				}
				catch(Exception ex) { return result; }
			}
			return result;
		}
		
		public static void Main(string[] ne)
		{
			for(int i = 0; i < app.Length; i++)
			{
				Command a = app[i];
				if(a.Cmnd != Command.CMS.Empty)
				{
					if(a.Cmnd == Command.CMS.Print)
						Console.Write(ConvertString(a.args[0]));
					else if(a.Cmnd == Command.CMS.Pause)
						Console.ReadKey(true);
					else if(a.Cmnd == Command.CMS.Set)
					{
						if(vars.ContainsKey(a.args[0]))
						   vars[a.args[0]] = a.args[1];
						else
							vars.Add(a.args[0], a.args[1]);
					}
					else if(a.Cmnd == Command.CMS.Input)
					{
						string value = Console.ReadLine();
						if(vars.ContainsKey(a.args[0]))
						   vars[a.args[0]] = value;
						else
							vars.Add(a.args[0], value);
					}
					else if(a.Cmnd == Command.CMS.Clear)
						Console.Clear();
					else if(a.Cmnd == Command.CMS.Title)
						Console.Title = ConvertString(a.args[0]);
					else if(a.Cmnd == Command.CMS.Goto)
					{
						for(int j = 0; j < app.Length; j++)
						{
							if(app[j].Cmnd == Command.CMS.Label && app[j].args[0] == a.args[0])
							{
								i = j;
								break;
							}
						}
					}
				}
			}
		}
	}
}
/*
goto 25 113 124
title 23 111 113
clear 22 109 111
input 21 101 109
set 20 94 101
pause 19 92 94
print 18 90 92

100 35
 */