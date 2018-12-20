using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
	public class CommandSpawnWave : ConsoleCommand{
		public override string Name { get; protected set; }
		public override string Command { get; protected set; }
		public override string Description { get; protected set; }
		public override string Help { get; protected set; }

		public CommandSpawnWave (){
			Name = "SpawnWave";
			Command = "spawnwave";
			Description = "Quits the game.";
			Help = "Use with no arguments to force the game to quit.";
			
			AddCommandToConsole();
		}

		public override void RunCommand()
		{
			GameObject.FindObjectOfType<EnemySpawner>().StartSpawnWaveIfInactive();
			DeveloperConsole.AddStaticMessageToConsole("Spawning Enemy Wave");
		}

		public static CommandSpawnWave CreateCommand(){
			return new CommandSpawnWave();
		}

        public override void RunCommandInt(int k)
        {
            DeveloperConsole.AddStaticMessageToConsole("Wrong usage, this method does not take any parameter!");
        }
    }
}

