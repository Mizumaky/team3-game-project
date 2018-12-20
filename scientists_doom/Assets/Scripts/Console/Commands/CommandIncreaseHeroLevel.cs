using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
	public class CommandIncreaseHeroLevel : ConsoleCommand{
		public override string Name { get; protected set; }
		public override string Command { get; protected set; }
		public override string Description { get; protected set; }
		public override string Help { get; protected set; }

		public CommandIncreaseHeroLevel (){
			Name = "IncreaseHeroLevel";
			Command = "levelup";
			Description = "Increases current character's level by given amount of levels.";
			Help = "Call with int argument to level up a character by that amount of levels.";
			
			AddCommandToConsole();
		}

		public override void RunCommandInt(int levels)
		{
			PlayerStats playerStats = CharacterManager.activeCharacterObject.GetComponent<PlayerStats>();
			playerStats.LevelUp(levels);
			DeveloperConsole.AddStaticMessageToConsole("Hero level increased to "+playerStats.GetCurrentHeroLevel());
		}

		public static CommandIncreaseHeroLevel CreateCommand(){
			return new CommandIncreaseHeroLevel();
		}

        public override void RunCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}

