using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CommandCore
{
    public class Command
    {
        //Values
        //Переменные

        public List<string> stringArgs = new List<string>();

        public List<bool> booleanArgs = new List<bool>();

        public List<decimal> decimalArgs = new List<decimal>();

        public List<object> Args = new List<object>();

        public int argumentsLength = 0;

        public int commandLength = 0;

        public string cmdStart = "";

        public int needArgumentsLength = 1;

        private List<Error> AllErrors = new List<Error>();
        private List<Error> NewErrors = new List<Error>();

        public Action<Error> onError = OnError;
        public Action<Error> onDevelopError = OnDevelopError;

        private List<Type> types = new List<Type>();
        public bool end;

        private static void OnDevelopError(Error error)
        {
            Console.WriteLine("onDevelopError is null");
        }

        private static void OnError(Error error)
        {
            Console.WriteLine("onError is null");
        }

        //Constuctor 
        //Конструктор
        void DefaultInit(string cmd, List<Type> types)
        {
            //Split the command 
            //Разделяем комманду
            string[] cmdSplitted = cmd.Split(" ");

            //Set start command part
            //Задаем начало комманды
            cmdStart = cmdSplitted[0];

            //Set public lengths (command's length, arguments'es length)
            //Назначает публичные переменные длинны команды и длинны аргументов
            commandLength = cmdSplitted.Length;
            argumentsLength = cmdSplitted.Length - 1;

            if (argumentsLength < needArgumentsLength)
            {
                AddError($"Слишком мало аргументов, добавьте {needArgumentsLength - argumentsLength} аргументов");
                return;
            }
            else if (argumentsLength > needArgumentsLength)
            {
                AddError($"Слишком много аргументов, уберите {argumentsLength - needArgumentsLength} аргументов");
                return;
            }

            //Try parse to int and add to need list
            //Пытаемся ковертировать строку в int и записываем в нужный список 
            for (int i = 1; i < cmdSplitted.Length; i++)
            {
                decimal parseDecimal;

                for (int j = 0; j < types.Count; j++)
                {
                    if (types[j] == typeof(int) || types[j] == typeof(float) || types[j] == typeof(long))
                    {
                        types[j] = typeof(decimal);

                    }
                    else
                    {
                        continue;
                    }
                }
                if (this.types != types)
                {
                    this.types = types;
                }

                for (int j = 0; j < cmdSplitted[i].Length; j++)
                {

                    if (cmdSplitted[i][j] == '.')
                    {
                        cmdSplitted[i] = cmdSplitted[i].Replace(".", ",");
                    }
                }

                if (decimal.TryParse(cmdSplitted[i], out parseDecimal))
                {
                    if (types[i - 1] == typeof(bool))
                    {
                        bool parseBool = false;
                        if (parseDecimal == 1)
                        {
                            parseBool = true;
                        }
                        else if (parseDecimal == 0)
                        {
                            parseBool = false;
                        }

                        if (parseDecimal >= 0)
                        {
                            Args.Add(parseBool);
                        }
                    }
                    else if (types[i - 1] == typeof(string))
                    {
                        Args.Add(parseDecimal.ToString());
                    }
                    else if (types[i - 1] == typeof(decimal))
                    {
                        Args.Add(parseDecimal);
                    }
                }
                else if (types[i - 1] == typeof(string))
                {
                    Args.Add(cmdSplitted[i]);
                }
                else if (types[i - 1] == typeof(bool))
                {
                    bool parseBool = false;

                    if (bool.TryParse(cmdSplitted[i], out parseBool))
                    {
                        Args.Add(parseBool);
                    }

                }
                else
                {
                    AddError("Не известный тип данных передан в списке по индексу " + i);
                }
            }




            for (int i = 0; i < Args.Count; i++)
            {
                if (Args[i].GetType() == typeof(bool))
                {
                    booleanArgs.Add((bool)Args[i]);
                }

                if (Args[i].GetType() == typeof(decimal))
                {
                    decimalArgs.Add((decimal)Args[i]);
                }

                if (Args[i].GetType() == typeof(string))
                {
                    stringArgs.Add(Args[i].ToString());
                }
            }

            for (int i = 0; i < types.Count && Args.Count == needArgumentsLength; i++)
            {
                if (Args[i].GetType() != types[i])
                {
                    AddDevelopError($"Не тот тип данных передан в аргументе по индексу {i}");
                }
            }
            end = true;
        }

        //Function for check errors
        //Функция для проверки оишбок
        public bool CheckErrors()
        {
            bool isError = false;

            if (GetNewErrors(true).Length > 0)
            {
                isError = true;
            }
            else
            {
                isError = false;
            }

            return isError;
        }

        //Add new error into lists of errors
        //Добавить новую ошибку в списки с ошибками
        public void AddError(string text)
        {
            Error error = new Error(text);
            AllErrors.Add(error);
            NewErrors.Add(error);
        }
        public void AddDevelopError(string text)
        {
            Error error = new DevelopError(text);
            AllErrors.Add(error);
            NewErrors.Add(error);
        }
        public void OnInitializeOnErrors()
        {
            foreach (Error error in AllErrors)
            {
                if (error is DevelopError)
                {
                    onDevelopError(error);
                }
                else if (error is Error)
                {
                    onError(error);
                }
            }

        }
        public Command(string cmd, List<Type> types)
        {
            this.types = types;
            DefaultInit(cmd, this.types);
        }

        //Constructor 2
        //Конструктор 2
        public Command(string cmd, int needArgumentsLength, List<Type> types)
        {
            this.needArgumentsLength = needArgumentsLength;
            this.types = types;

            DefaultInit(cmd, this.types);
        }

        //Get value from "stringArgs"
        //Получаем значени из "stringArgs"
        public string GetValueFromStringArgs(int index)
        {
            if (stringArgs.Count - 1 >= index)
            {
                return stringArgs[index];
            }
            else
            {
                return " ";
            }
        }
        //Get value from "decimalArgs"
        //Получаем значени из "decimalArgs"
        public decimal GetValueFromDecimalArgs(int index)
        {
            if (stringArgs.Count - 1 >= index)
            {
                return decimalArgs[index];
            }
            else
            {
                return 0;
            }
        }
        //Get value from "Args"
        public object GetValueFromArgs(int index)
        {
            if (Args.Count - 1 >= index)
            {
                return Args[index];
            }
            else
            {
                return null;
            }
        }

        //Get all errors from "NewErrors"
        //Получает все ошибки из "NewErrors"
        public Error[] GetNewErrors()
        {
            Error[] errors = this.NewErrors.ToArray();

            ClearNewErrors();

            return errors;
        }

        public Error[] GetNewErrors(bool IsClean)
        {
            Error[] errors = this.NewErrors.ToArray();

            if (IsClean == true)
            {
                ClearNewErrors();
            }
            return errors;
        }

        public Error GetErrorFromNewErrors(int index)
        {
            if (NewErrors.Count == index)
            {
                return NewErrors[index];
            }
            else
            {
                return null;
            }
        }

        //Get all errors from "NewErrors" of index and clean list "NewErrors" if "IsClean" == true
        //Получает все ошибки из "NewErrors" по индексу и чистит список "NewErrors" если "IsClean" == true
        public Error GetErrorFromNewErrors(int index, bool IsClean)
        {
            Error error;
            if (NewErrors.Count == index)
            {
                error = NewErrors[index];
            }
            else
            {
                error = null;
            }
            if (IsClean == true)
            {
                NewErrors.Clear();
            }
            return error;

        }
        //Get error from "AllErrors" of index
        //Получем ошибку из "AllErrors" по индексу
        public Error GetErrorFromAllErrors(int index)
        {
            if (AllErrors.Count - 1 == index)
            {
                return AllErrors[index];
            }
            else
            {
                return null;
            }
        }


        //Get all errors from "AllErrors"
        //Получает все ошибки из "AllErrors"
        public Error[] GetAllErrors()
        {
            Error[] errors = this.AllErrors.ToArray();

            return errors;
        }

        //Clear all errors in "NewErrors" 
        //Удаляет все ошибки в "NewErrors"
        public void ClearNewErrors()
        {
            this.NewErrors.Clear();
        }

    }

    //Class "Error" for seek and handling errors 
    //Класс "Error" для поиска и обработки ошибок
    public class Error
    {
        //Error's text
        //Текст ошибки
        public string errorText;

        //Constructor
        //Конструктор
        public Error(string error)
        {
            this.errorText = error;
        }

        //Write this Error in console
        //Пишет эту ошибку в консоли
        public void WriteError()
        {
            Console.WriteLine(errorText);
        }
        public void WriteError(string addStart)
        {
            Console.WriteLine(addStart + " " + errorText);
        }
        public void WriteError(string addStart, string addEnd)
        {
            Console.WriteLine(addStart + " " + errorText + " " + addEnd);
        }
        public async Task SendBotMessage(TelegramBotClient client, long chatId)
        {
            await client.SendTextMessageAsync(chatId, errorText);
        }
        public async Task SendBotMessage(TelegramBotClient client, long chatId, string addStart, string addEnd)
        {
            await client.SendTextMessageAsync(chatId, addStart + " " + errorText + addEnd);
        }
        public async Task SendBotMessage(TelegramBotClient client, long chatId, string addStart)
        {
            await client.SendTextMessageAsync(chatId, addStart + " " + errorText);
        }
    }
    public class DevelopError : Error
    {
        public DevelopError(string error) : base(error)
        {

        }
    }
}