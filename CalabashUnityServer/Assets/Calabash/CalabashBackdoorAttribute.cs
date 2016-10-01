using System;

namespace Calabash {

	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class Backdoor : System.Attribute
	{
		private string name;

		public Backdoor(string name)
		{
			this.name = name;
		}
	}
}