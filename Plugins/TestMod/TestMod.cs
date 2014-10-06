using System;
using Magnetite;

namespace TestMod
{
    public class TestMod : Module
	{
		public override string Name {
			get {
				return "TestMod";
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
			Hooks.OnClientAuth += Hooks_OnClientAuth;
		}

		void Hooks_OnClientAuth(Magnetite.Events.AuthEvent ae)
		{
			
		}

		public override void DeInitialize()
		{
			
		}
    }
}
