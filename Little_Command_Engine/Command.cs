using System;
using System.Collections.Generic;

namespace CommandCore
{
    public class Command
    { 
        //Values
        //Переменные

        public List<int> intArgs = new List<int>();

        public List<string> stringArgs = new List<string>();

        public List<object> Args = new List<object>();

        public int argumentsLength = 0;

        public int commandLength = 0;

        public string cmdStart = "";

        public int needArgumentsLength = 1;

        private List<Error> AllErrors = new List<Error>();
        private List<Error> NewErrors = new List<Error>();

        public Action<Error> onError = OnError;

        private static void OnError(Error error)
        {
            Console.WriteLine("onError is null");
        }

        //Constuctor 
        //Конструктор
        void DefaultInit(string cmd, Type[] types)
        {

            //Split the command 
            //Разделяем комманду
            string[] cmdSplitted = cmd.Split(" ");

            //Set start command part
            //Задаем начало комманды
            cmdStart = cmdSplitted[0];

            //Lists of arguments
            //Списки аргументов
            List<string> stringArgs = new List<string>();
            List<int> intArgs = new List<int>();

            //Try parse to int and add to need list
            //Пытаемся ковертировать строку в int и записываем в нужный список 
            for (int i = 1; i < cmdSplitted.Length; i++)
            {
                int parseInt;
                if (int.TryParse(cmdSplitted[i], out parseInt))
                {
                    intArgs.Add(parseInt);
                }
                else if (!int.TryParse(cmdSplitted[i], out parseInt))
                {
                    stringArgs.Add(cmdSplitted[i]);
                }
            }
            if (types.Length < needArgumentsLength)
            {
                AddError($"Длина типов меньше необходимого {needArgumentsLength}");
            }

            //Set public lengths (command's length, arguments'es length)
            //Назначает публичные переменные длинны команды и длинны аргументов
            commandLength = cmdSplitted.Length;
            argumentsLength = cmdSplitted.Length - 1;

            this.stringArgs = stringArgs;
            this.intArgs = intArgs;

            //Full list arguments which have "intArgs" and "stringArgs"
            //Полный список аргументов включающий в себя "intArgs" и "stringArgs"
            for (int i = 0; i < intArgs.Count + stringArgs.Count; i++)
            {
                if (i < intArgs.Count)
                {
                    if (types[i] == 0.GetType()) { 
                        Args.Add(intArgs[i]);
                    }
                    else
                    {
                        AddError("Тип аргумента не совпадает с нужным типом");
                    }
                }
                if (i < stringArgs.Count)
                {
                    if (types[i] == "".GetType())
                    {
                        Args.Add(stringArgs[i]);
                    }
                    else
                    {
                        AddError("Тип аргумента не совпадает с нужным типом");
                    }
                }
            }

        }

        //Check errors
        public bool CheckErrors()
        {
            //Check args length
            //Проверяем длинну аргументов
            if (argumentsLength < needArgumentsLength)
            {
                AddError($"Добавьте {needArgumentsLength - argumentsLength} аргументов");

            }
            else if (argumentsLength > needArgumentsLength)
            {
                AddError($"Удалите {argumentsLength - needArgumentsLength} аргументов");

            }
            else
            {
                return false;
            }
            return true;
        }

        //Add new error into lists of errors
        //Добавить новую ошибку в списки с ошибками
        private void AddError(string text)
        {
            Error error = new Error(text);
            AllErrors.Add(error);
            NewErrors.Add(error);
            onError(error);
        }
        private void AddFatalError(string text)
        {
            Error error = new FatalError(text);
            AllErrors.Add(error);
            NewErrors.Add(error);
            onError(error);
        }
        public Command(string cmd, Type[] types)
        {
            DefaultInit(cmd, types);
        }

        //Constructor 3
        //Конструктор 3
        public Command(string cmd, int needArgumentsLength, Type[] types)
        {
            this.needArgumentsLength = needArgumentsLength;

            DefaultInit(cmd, types);
        }
        public Command(string cmd, List<Type> types)
        {
            DefaultInit(cmd, types.ToArray());

        }

        //Constructor 4
        //Конструктор 4
        public Command(string cmd, int needArgumentsLength, List<Type> types)
        {
            this.needArgumentsLength = needArgumentsLength;

            DefaultInit(cmd, types.ToArray());
        }

        //Get value from "intArgs"
        //Получаем значени из "intArgs"
        public int GetValueFromIntArgs(int index)
        {
            if (intArgs.Count - 1 >= index)
            {
                return intArgs[index];
            }
            else
            {
                return 0;
            }
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
        //Get value from "Args"
        public object GetValueFromArgs(int index)
        {
            if (Args.Count - 1 >= index)
            {
                return Args[index];
            }
            else
            {
                return " ";
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
        public Error GetErrorFromNewErrors(int index,bool IsClean)
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
    }
    public class FatalError : Error
    {
        public FatalError(string errorText) : base(errorText)
        {
            ThrowError();
        }
        public void ThrowError()
        {
            throw new ApplicationException(errorText);
        }
    }
}