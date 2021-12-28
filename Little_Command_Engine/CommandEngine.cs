using System;

namespace CommandCore
{
    public class CommandEngine
    {
        public static CommandEngine engine;


        private void Init()
        {
            engine = this;
        }
        public CommandEngine()
        {
            Init();
        }

        public bool CheckCommand(string commandStart, Command cmd)
        {
            if (cmd.cmdStart == commandStart)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ChecErrors(Command cmd)
        {
            return cmd.CheckErrors();
        }
        public bool CheckArguments(Command cmd, Type[] types)
        {
            if (types.Length < cmd.Args.Count || types.Length < cmd.needArgumentsLength)
            {
                cmd.AddDevelopError("Слишком маленький список типов");
                return false;
            }
            else if (types.Length > cmd.Args.Count || types.Length > cmd.needArgumentsLength)
            {
                cmd.AddDevelopError("Слишком большой список типов");
                return false;
            }
            for (int i = 0; i < cmd.Args.Count; i++)
            {
                if (cmd.Args[i].GetType() != types[i])
                {
                    if (types[i] == typeof(bool))
                    {
                        if (cmd.Args[i].GetType() == typeof(int))
                        {
                            if ((int)cmd.Args[i] == 0)
                            {
                                cmd.Args[i] = false;
                                cmd.intArgs.Add(0);
                                continue;
                            }
                            else if((int)cmd.Args[i] == 1)
                            {
                                cmd.Args[i] = true;
                                cmd.booleanArgs.Add(true);
                                continue;
                            }
                        }
                    }
                    cmd.AddError("Не правильный тип аргумента");
                    return false;
                }
            }
            return true;
        }
    }

}