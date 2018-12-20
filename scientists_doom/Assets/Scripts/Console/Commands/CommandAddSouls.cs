using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
	public class CommandAddSouls : ConsoleCommand{
		public override string Name { get; protected set; }
		public override string Command { get; protected set; }
		public override string Description { get; protected set; }
		public override string Help { get; protected set; }

		public CommandAddSouls (){
			Name = "AddSouls";
			Command = "addsouls";
			Description = "Add given amount of souls to player inventory.";
			Help = "Call with int argument to add this amount of souls to current character's inventory.";
			
			AddCommandToConsole();
		}

		public override void RunCommandInt(int souls)
		{
			Inventory inventory = CharacterManager.activeCharacterObject.GetComponent<Inventory>();
			inventory.AddSouls(souls);
			DeveloperConsole.AddStaticMessageToConsole("Current souls balance: "+inventory.souls);
		}

		public static CommandAddSouls CreateCommand(){
			return new CommandAddSouls();
		}

        public override void RunCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}

