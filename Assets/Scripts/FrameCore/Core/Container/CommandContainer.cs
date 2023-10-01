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

        public void AddCommand()
        {

        }

        public void RemoveCommand()
        {

        }

        public void ExecuteCommand()
        {

        }

    }
}


