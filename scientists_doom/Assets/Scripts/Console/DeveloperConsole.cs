using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console
{

    public abstract class ConsoleCommand{
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }

        public void AddCommandToConsole(){
            string addMessage = " command has been added to the console.";

            DeveloperConsole.AddCommandsToConsole(Command, this);
            DeveloperConsole.AddStaticMessageToConsole(this.Name + addMessage);
        }

        public abstract void RunCommand();
    }
    public class DeveloperConsole : MonoBehaviour{

        public static DeveloperConsole Instance { get; private set;}
        public static Dictionary<string, ConsoleCommand> Commands {get; private set;}

        [Header("UI Components")]
        public Canvas consoleCanvas;
        public ScrollRect scrollRect;
        public TextMeshProUGUI consoleText;
        public TMP_InputField inputText;

        void Awake()
        {
            if (Instance != null){
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
        }
        void Start()
        {
            consoleCanvas.gameObject.SetActive(false);
            consoleText.text = "Doom Console 0.01\n";
            CreateCommands();
        }
        void Update()
            {
                if(Input.GetKeyDown(KeyCode.F1)){
                    consoleCanvas.gameObject.SetActive(!consoleCanvas.gameObject.activeInHierarchy);
                    scrollRect.verticalNormalizedPosition = 0f;
                }
                if(consoleCanvas.gameObject.activeInHierarchy){
                    if(Input.GetKeyDown(KeyCode.Return)){
                        if(inputText.text != ""){
                            AddMessageToConsole(inputText.text);
                            ParseInput(inputText.text);
                        }
                    }
                }
            }
        private void CreateCommands(){
            CommandQuit commandQuit = CommandQuit.CreateCommand();
            CommandSpawnWave commandSpawnWave = CommandSpawnWave.CreateCommand();
        }

        public static void AddCommandsToConsole(string _name, ConsoleCommand _command){
            if(!Commands.ContainsKey(_name)){
                Commands.Add(_name, _command);
            }

        }

        public void AddMessageToConsole(string msg){
            consoleText.text += "\n" + msg;
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public static void AddStaticMessageToConsole(string msg){
            DeveloperConsole.Instance.consoleText.text += msg + "\n";
            DeveloperConsole.Instance.scrollRect.verticalNormalizedPosition = 0f;
        }

        public void ParseInput(string input){
            string[] _input = input.Split(null);

            if(_input.Length == 0 || _input == null){
                AddMessageToConsole("Command not recognized!");
                return;
            }

            if(!Commands.ContainsKey(_input[0])){
                AddMessageToConsole("Command not recognized!");
            }else{
                Commands[_input[0]].RunCommand();
            }
        }
    }
}