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

		private static Dictionary<string, List<CommandHandler>> Events = new Dictionary<string, List<CommandHandler>>();

		public delegate void CommandHandler(Command command);

		public static void Add(string cmd, CommandHandler fn)
		{
			lock (Events)
			{
				if (!Events.ContainsKey(cmd))
				{
					Events.Add(cmd, new List<CommandHandler>());
				}
				Events[cmd].Add(fn);
			}
		}

		public static void Remove(string cmd, CommandHandler fn)
		{
			lock (Events)
			{
				if (!Events.ContainsKey(cmd))
				{
					return;
				}
				foreach (CommandHandler current in Events[cmd])
				{
					if (current == fn)
					{
						Events[cmd].Remove(current);
						break;
					}
				}
			}
		}

		private static bool ConditionsTest(Command command, Condition[] conditions, out string error)
		{
			foreach (Condition condition in conditions)
			{
				if (!condition.Test(command))
				{
					error = condition.Error;
					return false;
				}
			}
			error = null;
			return true;
		}

		private static void OnCommand(Command command)
		{
			if (Events.ContainsKey(command.cmd))
			{
				List<CommandHandler> commands = Events[command.cmd];
				List<string> errors = new List<string>();

				foreach (CommandHandler cmd in commands)
				{
					string error;
					Condition[] conditions = (Condition[])cmd.Method.GetCustomAttributes(typeof(Condition), true);
					if (ConditionsTest(command, conditions, out error))
					{
						cmd(command);
						return;
					}
					else
					{
						errors.Add(error);
					}
				}
				if (errors.Count > 0)
				{
					foreach (string error in errors)
					{
						//Localization.GetInstance().Message(command.User, error);
					}
				}
				else
				{
					//Localization.GetInstance().Notice(command.User, "Command not found!");
				}
			}
			else
			{
				//Localization.GetInstance().Notice(command.User, "Command not found!");
			}
		}

		public abstract class Condition : Attribute
		{
			public virtual string Error { get; set; }

			public virtual bool Test(Command command)
			{
				return true;
			}
		}

		public sealed class Admin : Condition
		{
			public string Error = "You don't have permission to use this command!";

			public Admin() {}

			public override bool Test(Command command)
			{
				return command.User.Admin;
			}
		}

		public sealed class Moderator : Condition
		{
			public string Error = "You don't have permission to use this command!";

			public Moderator() { }

			public override bool Test(Command command)
			{
				return command.User.Admin || command.User.Owner || command.User.Moderator;
			}
		}

		public sealed class Var : Condition
		{
			public string Error = "Command has been disabled!";

			public string var;

			public Var(string var)
			{
				this.var = var;
			}

			public override bool Test(Command command)
			{
				return true;
			}
		}

		public sealed class VarEqual : Condition
		{
			public Type type;

			public string var;

			public object value;

			public VarEqual(Type type, string var, object value, string error)
			{
				this.type = type;
				this.var = var;
				this.value = value;
				this.Error = error;
			}

			public override bool Test(Command command)
			{
				return Util.GetTypeField(type, var) == value;
			}
		}

		public class VarInt : Condition
		{
			public Type type;

			public string var;

			public int value;

			public VarInt(Type type, string var, int value, string error)
			{
				this.type = type;
				this.var = var;
				this.value = value;
				this.Error = error;
			}

			public override bool Test(Command command)
			{
				return (int)Util.GetTypeField(type, var) == value;
			}
		}

		public sealed class VarUpper : VarInt
		{
			public VarUpper(Type type, string var, int value, string error) : base(type, var, value, error) { }

			public override bool Test(Command command)
			{
				return (int)Util.GetTypeField(type, var) > value;
			}
		}

		public sealed class VarLower : VarInt
		{
			public VarLower(Type type, string var, int value, string error) : base(type, var, value, error) { }

			public override bool Test(Command command)
			{
				return (int)Util.GetTypeField(type, var) < value;
			}
		}

		public sealed class Usage : Attribute
		{
			public string usage;

			public Usage(string usage)
			{
				this.usage = usage;
			}
		}

		public class Arguments : Condition
		{
			public string Error = "Wrong number of arguments.";

			public int length;

			public Arguments(int length)
			{
				this.length = length;
			}

			public override bool Test(Command command)
			{
				return length == command.quotedArgs.Length;
			}
		}

		public sealed class MinArguments : Arguments
		{
			public MinArguments(int length)
				: base(length)
			{ }

			public override bool Test(Command command)
			{
				return command.quotedArgs.Length >= length;
			}
		}

		public sealed class MaxArguments : Arguments
		{
			public MaxArguments(int length) : base(length) { }

			public override bool Test(Command command)
			{
				return command.quotedArgs.Length <= length;
			}
		}
	}
}

