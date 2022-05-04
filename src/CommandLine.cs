using System;
using System.Linq;

namespace FileManager
{
    internal class CommandLine
    {

        /// <summary>
        /// Ожидать строки с командами
        /// </summary>
        public static void ReadCommandLine()
        {
            do
            {
                ParseCommandLine(Console.ReadLine());

            } while (true);
        }

        /// <summary>
        /// Обработать строку ввода и выбрать варианты действия в зависимости от первой команды до пробела
        /// </summary>
        /// <param name="commandLine"></param>
        private static void ParseCommandLine(string commandLine)
        {
            /*
            Например: 
            ls C:\Source -p 2               Вывод дерева файловой системы с условием “пейджинга”              
            cp C:\Source D:\Target          Копирование каталога          
            cp C:\source.txt D:\target.txt  Копирование файла  
            rm C:\Source                    Удаление каталога рекурсивно                    
            rm C:\source.txt                Удаление файла                
            file C:\source.txt              Вывод информации
            */

            //Передать в лог
            LogIt.CurrentLogText = $"CommandLine: Пользователь ввел в командную строку <{commandLine}>";
            
            string[] commandLineArgs = commandLine.Split(" ");

            //Выбрать варианты действия
            switch (commandLineArgs[0].ToLower())
            {
                case "list":
                case "ls":
                    ParseCommand.ParseList(commandLineArgs.ElementAtOrDefault(1), commandLineArgs.ElementAtOrDefault(2), commandLineArgs.ElementAtOrDefault(3));
                    break;
                case "tree":
                case "tr":
                    ParseCommand.ParseTree(commandLineArgs.ElementAtOrDefault(1), commandLineArgs.ElementAtOrDefault(2), commandLineArgs.ElementAtOrDefault(3));
                    break;
                case "files":
                case "fl":
                    ParseCommand.ParseFileList(commandLineArgs.ElementAtOrDefault(1), commandLineArgs.ElementAtOrDefault(2));
                    break;
                case "copy":
                case "cp":
                    ParseCommand.ParseCopy(commandLineArgs.ElementAtOrDefault(1), commandLineArgs.ElementAtOrDefault(2));
                    break;
                case "del":
                case "rm":
                    ParseCommand.ParseDel(commandLineArgs.ElementAtOrDefault(1));
                    break;
                case "fileinfo":
                case "finfo":
                    ParseCommand.ParseFileInfo(commandLineArgs.ElementAtOrDefault(1));
                    break;
                case "chdir":
                case "cd":
                    ParseCommand.ParseChdir(commandLineArgs.ElementAtOrDefault(1));
                    break;
                case "help":
                case "?":
                    ParseCommand.ParseHelp(commandLineArgs.ElementAtOrDefault(1));
                    break;
                case "md":
                case "mkdir":
                    ParseCommand.ParseMkdir(commandLineArgs.ElementAtOrDefault(1));
                    break;
                case "ren":
                case "rename":
                    ParseCommand.ParseRen(commandLineArgs.ElementAtOrDefault(1), commandLineArgs.ElementAtOrDefault(2));
                    break;
                case "start":
                case "run":
                    ParseCommand.ParseRun(commandLineArgs.ElementAtOrDefault(1));
                    break;
                case "exit":
                case "quit":
                    ParseCommand.ParseExit();
                    break;
                case "showfiles":
                    ParseCommand.ParseShowFiles();
                    break;
                case "read":
                    ParseCommand.ParseReadFile(commandLineArgs.ElementAtOrDefault(1));
                    break;
                default:
                    LogIt.CurrentLogError = "CommandLine: Неизвестная команда"; //Если неизвестная команда или пустая строка
                    DrawWindows.RefreshLog();
                    break;
            }
        }
    }
}