using System;
using System.Collections.Generic;
using UnityEngine;

namespace Magnetite
{
	public class Command
	{
		public string ReplyWith;

		public readonly string cmd;

		public readonly string[] args;

		public readonly string[] quotedArgs;

		public readonly Player User;

		public Command(Player player, string[] command)
		{
			User = player;
			ReplyWith = String.Format("/{0} executed!", String.Join("", command));
			cmd = command[0];
			args = new string[command.Length - 1];
			Array.Copy(command, 1, args, 0, command.Length - 1);
			quotedArgs = GetQuotedStringArgs(args);

			Logger.Log("Command: " + cmd + ", " + quotedArgs.Length + " arguments, " + String.Join(" ", quotedArgs));
		}

		public string[] GetArguments(int start, int length)
		{
			string[] arguments = new string[length];
			Array.Copy(quotedArgs, start, arguments, 0, length);
			return arguments;
		}

		public static string[] GetQuotedStringArgs(string[] sArr)
		{
			bool inQuote = false;
			string current = "";
			List<string> final = new List<string>();

			foreach (string str in sArr)
			{
				if (str.StartsWith("\""))
				{
					inQuote = true;
				}

				if (str.EndsWith("\""))
				{
					inQuote = false;
				}

				if (inQuote)
				{
					if (current != "")
					{
						current += " " + str;
					}

					if (current == "")
					{
						current = str;
					}
				}

				if (!inQuote)
				{
					if (current != "")
					{
						final.Add((current + " " + str).Replace("\"", ""));
					}
					if (current == "")
					{
						final.Add((str).Replace("\"", ""));
					}
					current = "";
				}
			}
			return final.ToArray();
		}

		public static string[] GetQuotedStringArr(string[] sArr)
		{
			bool inQuote = false;
			string current = "";
			var final = new string[10]; // should be enough
			int Count = 0;

			foreach (string str in sArr) {
				if (str.StartsWith("\""))
					inQuote = true;

				if (str.EndsWith("\""))
					inQuote = false;

				if (inQuote) {
					if (current != "")
						current += " " + str;
					if (current == "")
						current = str;
				}

				if (!inQuote) {
					if (current != "")
						final[Count] = (current + " " + str).Replace("\"", "");
					if (current == "")
						final[Count] = (str).Replace("\"", "");
					Count += 1;
					current = "";
				}
			}
			return final;
		}
	}
}

