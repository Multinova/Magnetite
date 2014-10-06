using System;
using Magnetite;

namespace Help
{
    public class Help : Module
	{
		public override string Name {
			get {
				return "Help";
			}
		}

		public override string Author {
			get {
				return "Yohann Seris";
			}
		}

		public override Version Version {
			get {
				return new Version(0, 1, 0, 0);
			}
		}
		public override void Initialize()
		{
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			Hooks.OnCommand -= Hooks_OnCommand;
		}

		void Hooks_OnCommand(Command command)
		{
			if (command.cmd == "help")
			{
				foreach (ModuleContainer mc in ModuleManager.Modules)
				{
					string help = mc.Plugin.Help;
					if (help is string && help != String.Empty && help != null)
					{
						command.User.Message(help);
					}
				}
			}
		}
    }
}
