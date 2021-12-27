using Telegram.Bot.Types.ReplyMarkups;


//Namespace for bot's keyboards
//Пространство имен для клавиутр бота

namespace Example.KeyBoards
{
    public class KeyBoard
    {
        public static ReplyKeyboardMarkup testKeyboard = new(new[]
        {
            new KeyboardButton[] {"Музыка", "Сообщение", "Стикер"},
        })
        {
            ResizeKeyboard = true
        };
    }
}
