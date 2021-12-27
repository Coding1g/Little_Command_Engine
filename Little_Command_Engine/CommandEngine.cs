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
    }
}