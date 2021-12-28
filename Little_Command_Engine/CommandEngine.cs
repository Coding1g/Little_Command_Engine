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
        public bool CheckErrors(Command cmd, Type[] types)
        {
            bool isError;
            CheckArguments(cmd, types);
            isError = cmd.CheckErrors();
            return isError;
        }
        public void CheckArguments(Command cmd, Type[] types)
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
                if (cmd.Args[i].GetType() != types[i])
                {
                    if (types[i] == typeof(bool))
                    {
                        if (cmd.Args[i].GetType() == typeof(int))
                        {
                            if (cmd.Args[i].ToString() == "0")
                            {
                                cmd.Args[i] = false;

                                if (cmd.booleanArgs.Count - 1 >= i)
                                {
                                    cmd.booleanArgs[i] = false;
                                }
                                else
                                {
                                    cmd.booleanArgs.Add(false);
                                }
                                cmd.intArgs.RemoveAt(i);

                            }
                            if (cmd.Args[i].ToString() == "1")
                            {
                                cmd.Args[i] = true;
                                if (cmd.booleanArgs.Count - 1 >= i)
                                {
                                    cmd.booleanArgs[i] = true;
                                }
                                else
                                {
                                    cmd.booleanArgs.Add(true);
                                }
                                cmd.intArgs.RemoveAt(i);

                            }
                        }
                    }
                    else
                    {
                        cmd.AddError("Не правильный тип аргумента");
                    }
                }
            }
        }
    }

}