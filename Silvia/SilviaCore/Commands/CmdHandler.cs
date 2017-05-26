using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilviaCore.Commands
{
    public static class CmdHandler
    {
        static List<Command> cmds = new List<Command>();
        public static List<Command> Cmds
        {
            get
            {
                return new List<Command>(cmds);
            }
        }

        public static void AddCmd(Command cmd)
        {
            if (!cmds.Contains(cmd))
            {
                cmds.Add(cmd);
            }
        }
    }
}
