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
        public bool CheckErrors(Command cmd)
        {
            bool isError;
            isError = !cmd.CheckErrors();
            return isError;
        }
        /*        public void CheckArguments(Command cmd, Type[] types)
                {
                    if (types.Length < cmd.Args.Count || types.Length < cmd.needArgumentsLength)
                    {
                        cmd.AddDevelopError("Слишком маленький список типов");
                        return;
                    }
                    else if (types.Length > cmd.Args.Count || types.Length > cmd.needArgumentsLength)
                    {
                        cmd.AddDevelopError("Слишком большой список типов");
                        return;
                    }
                    for (int i = 0; i < cmd.Args.Count; i++)
                    {
                        if (types[i] == typeof(string))
                        {
                            if (cmd.Args[i].GetType() != typeof(string))
                            {
                                cmd.Args[i] = cmd.Args[i].ToString();
                            }
                        }
                        else
                        {
                            cmd.AddError("Не правильный тип аргумента по индексу " + i);
                        }
                    }*/
    }
}

