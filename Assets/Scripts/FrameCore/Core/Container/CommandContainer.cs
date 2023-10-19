using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class CommandContainer : IContainer
    {
        private IGameStructure structure;

        private Dictionary<string, ICommand> _Commands = new();

        public CommandContainer(IGameStructure structure)
        {
            this.structure = structure;
        }

        public void Init()
        {

        }

        //添加无参数的Command
        public void AddCommand<T>() where T : ICommand, new()
        {
            T command = new T();
            command.GameStructure = structure;
            string commandName = typeof(T).Name;
            if (!_Commands.ContainsKey(commandName))
                _Commands.Add(commandName, command);
        }

        //移除Command
        public void RemoveCommand<T>() where T : ICommand
        {
            string commandName = typeof(T).Name;
            if (_Commands.ContainsKey(commandName))
                _Commands.Remove(commandName);
            else
                Debug.LogWarning("容器中未找到" + commandName + ", 请检查容器中是否添加了该Command");
        }

        //执行无参Command
        public void ExecuteCommand<T>() where T : class, ICommand
        {
            string commandName = typeof(T).Name;
            if (_Commands.ContainsKey(commandName))
                (_Commands[commandName] as T).Execute();
            else
                Debug.LogWarning("容器中未找到" + commandName + ", 请检查容器中是否添加了该Command");
        }

        //清空Command
        public void ClearCommand()
        {
            _Commands.Clear();
        }
    }
}


