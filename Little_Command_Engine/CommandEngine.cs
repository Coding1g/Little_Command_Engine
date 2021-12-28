//Engine

namespace CommandCore
{
    //Engine
    public class CommandEngine
    {
        public static CommandEngine engine;
        
        //Init static value "engine"
        private void Init()
        {
            engine = this;
        }

        //Constructor
        public CommandEngine()
        {
            Init();
        }

        //function "CheckCommand" return boolean value (true if value "commandStart" == text and false if value "commandStart" != text)
        public bool CheckCommand(string text, Command command)
        {
            if (command.cmdStart == text)
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