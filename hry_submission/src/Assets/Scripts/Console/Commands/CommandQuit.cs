using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
	public class CommandQuit : ConsoleCommand{
		public override string Name { get; protected set; }
		public override string Command { get; protected set; }
		public override string Description { get; protected set; }
		public override string Help { get; protected set; }

		public CommandQuit (){
			Name = "Quit";
			Command = "quit";
			Description = "Quits the game.";
			Help = "Use with no arguments to force the game to quit.";
			
			AddCommandToConsole();
		}

		public override void RunCommand()
		{
			if(Application.isEditor){
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#endif
			}else{
				Application.Quit();
			}
		}

		public static CommandQuit CreateCommand(){
			return new CommandQuit();
		}

        public override void RunCommandInt(int k)
        {
            DeveloperConsole.AddStaticMessageToConsole("Wrong usage, this method does not take any parameter!");
        }
    }
}

