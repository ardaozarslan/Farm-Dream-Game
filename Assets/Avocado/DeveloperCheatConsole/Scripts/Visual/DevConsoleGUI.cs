using Avocado.DeveloperCheatConsole.Scripts.Core;
using UnityEngine;

namespace Avocado.DeveloperCheatConsole.Scripts.Visual {
    public abstract class DevConsoleGUI : MonoBehaviour {
        [SerializeField]
        [Range(0.05f, 1f)]
        protected float Scale = 0.1f;
        protected DeveloperConsole _console;
        
        protected string _input;
        protected Vector2 _scroll;
        protected bool _inputFocus;
        
        private void Awake() {
#if UNITY_EDITOR
            _console = DeveloperConsole.Instance;
            GenerateCommands();
#endif
        }
        
        private void GenerateCommands() {
            
        }
        
        private void OnGUI() {
#if UNITY_EDITOR
            DrawGUI();
#endif
        }
        
        protected void OnReturn() {
            if (_console.ShowConsole) {
                _console.InvokeCommand(_input);
                _input = "";
            }
        }

        protected void HandleEscape() {
            if (!_console.ShowHelp) {
                _console.ShowConsole = false;
                _inputFocus = false;
            } else {
                _console.ShowHelp = false;
                GUI.FocusControl("inputField");
            }
        }

        protected virtual void HandleShowConsole() {
        }
        
        protected virtual void HadnleKeyboardInGUI() {
        }
        
        private void DrawGUI() {
            if (!_console.ShowConsole) {
                HandleShowConsole();
                return;
            }
            
            HadnleKeyboardInGUI();

            var inputHeight = Screen.height * Scale;
			// Debug.Log("inputHeight: " + inputHeight);
            var y = Screen.height - inputHeight;

            if (_console.ShowHelp) {
                ShowHelp(y);
            }

            GUI.Box(new Rect(0, y, Screen.width, inputHeight), "");
            GUI.backgroundColor = Color.black;
            
            var labelStyle = new GUIStyle("TextField");
            labelStyle.fontStyle = FontStyle.Normal;
            var fontSize = inputHeight * 0.4f;
            labelStyle.fontSize = (int)fontSize;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            GUI.SetNextControlName("inputField");
            _input = GUI.TextField(new Rect(Screen.width * 0.005f, y + Screen.height * 0.0093f, Screen.width * 0.99f, inputHeight * 0.7f), _input, labelStyle);

            SetFocusTextField();
        }

        protected virtual void SetFocusTextField() {
            if (!_inputFocus) {
                _inputFocus = true;
                GUI.FocusControl("inputField");
                _input = string.Empty;
            }
        }

        private void ShowHelp(float y) {
			var inputHeight = Screen.height * Scale; // scale --> inputHeight * 0.01f
            GUI.Box(new Rect(0, y - 500 * inputHeight * 0.01f, Screen.width, 500* inputHeight * 0.01f), "");
            var viewPort = new Rect(0, 0, Screen.width - 30, 80 * _console.Commands.Count * inputHeight * 0.01f);
            _scroll = GUI.BeginScrollView(new Rect(0, y - 480f * inputHeight * 0.01f, Screen.width, 480 * inputHeight * 0.01f), _scroll, viewPort);

            int i = 0;
            foreach (var command in _console.Commands) {
                var label = $"{command.Id} - {command.Description}";
                var labelRect = new Rect(10, 50 * i * inputHeight * 0.01f, viewPort.width-100, 50 * inputHeight * 0.01f);
                    
                var labelStyleHelp = new GUIStyle("label");
                labelStyleHelp.fontStyle = FontStyle.Normal;
                var fontSize = 30 * inputHeight * 0.01f;
                labelStyleHelp.fontSize = (int)fontSize;
                    
                GUI.Label(labelRect, label, labelStyleHelp);
                
                i++;
            }
                
            GUI.EndScrollView();
        }
    }
}
