using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Tools
{
    /// <summary>
    /// Contains useful methods used when testing features that use Unity Input System.
    /// Also contains input devices that should be used to pass ButtonControls to the methods.
    /// </summary>
    public class InputTestTools : InputTestFixture
    {
        public Keyboard Keyboard { get; } = InputSystem.AddDevice<Keyboard>();
        public Mouse Mouse { get; } = InputSystem.AddDevice<Mouse>();

        private EditorWindow _gameViewWindow;

        private EditorWindow GameViewWindow
        {
            get
            {
                if (_gameViewWindow != null)
                {
                    return _gameViewWindow;
                }

                System.Reflection.Assembly assembly = typeof(EditorWindow).Assembly;
                Type type = assembly.GetType("UnityEditor.GameView");
                _gameViewWindow = EditorWindow.GetWindow(type);
                return _gameViewWindow;
            }
        }

        /// <summary>
        /// Waits for the editor "GameView"-tab to repaint
        /// </summary>
        public IEnumerator WaitForRepaint()
        {
            GameViewWindow.Repaint();
            yield return null;
        }

        /// <summary>
        /// Start this coroutine to press a specified key for one frame.
        /// </summary>
        /// <param name="control">The key to press.</param>
        /// <param name="repeats">The number of times the key should be pressed.</param>
        public IEnumerator PressForFrame(ButtonControl control, int repeats = 1)
        {
            for (int i = 0; i < repeats; i++)
            {
                Press(control);
                GameViewWindow.Repaint();
                yield return null;
                Release(control);
                GameViewWindow.Repaint();
                yield return null;
            }
        }

        /// <summary>
        /// Start this coroutine to press a specified key for a specified number of seconds.
        /// </summary>
        /// <param name="control">The key to press.</param>
        /// <param name="seconds">The number of seconds to press the key for.</param>
        /// <param name="repeats">The number of times the key should be pressed.</param>
        public IEnumerator PressForSeconds(ButtonControl control, float seconds, int repeats = 1)
        {
            for (int i = 0; i < repeats; i++)
            {
                Press(control);
                yield return new WaitForSeconds(seconds);
                Release(control);
                yield return null;
            }
        }

        /// <summary>
        /// Sets the position of the mouse in the scene.
        /// </summary>
        /// <param name="position">The position to set the mouse to.</param>
        public IEnumerator SetMousePosition(Vector2 position)
        {
            Set(Mouse.position, position);
            yield return null;
        }
        
        /// <summary>
        /// Sets the position of the mouse in the scene.
        /// </summary>
        /// <param name="position">The position to set the mouse to.</param>
        public IEnumerator SetMousePositionWorldSpace(Vector2 position)
        {
            Set(Mouse.position, Camera.main.WorldToScreenPoint(position));
            yield return null;
        }

        /// <summary>
        /// Spams a button until a particular behaviour is active and enabled.
        /// </summary>
        /// <param name="behaviour">The behaviour to wait for.</param>
        /// <param name="key">The key to press.</param>
        /// <returns></returns>
        public IEnumerator WaitForBehaviourActiveAndEnabled(Behaviour behaviour, ButtonControl key)
        {
            while (!behaviour.isActiveAndEnabled)
            {
                yield return PressForSeconds(key, 0.2f);
            }
        }

        /// <summary>
        /// Sets the mouse to a position and clicks.
        /// </summary>
        /// <param name="position">The position to click at.</param>
        public IEnumerator ClickAtPositionWorldSpace(Vector2 position)
        {
            yield return SetMousePositionWorldSpace(position);
            yield return PressForFrame(Mouse.leftButton);
        }

        /// <summary>
        /// Holds the X Key until a DialogueController is not busy
        /// </summary>
        /// <param name="dialogueController">The DialogueController to wait for</param>
        public IEnumerator ProgressStory(DialogueController dialogueController)
        {
            Press(Keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
            Release(Keyboard.xKey);
        }
    }
}
