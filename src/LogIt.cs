using System;
using System.IO;
using System.Text;

namespace FileManager
{
    /// <summary>
    /// Класс отвечающий за лог
    /// </summary>
    internal class LogIt
    {
        //Файлы для записи лога
        private static string _logFile = Directory.GetCurrentDirectory() + "\\log.txt";

        private static string _logCopyFile = Directory.GetCurrentDirectory() + "\\logFiles.txt";


        private static StringBuilder _currentLogText = new StringBuilder();

        /// <summary>
        /// Запись информации в лог файл и кеш
        /// </summary>
        public static string CurrentLogText
        {
            set
            {
                File.AppendAllText(_logFile, Helpers.GetTime() + " " + value + "\n");
                _currentLogText.AppendLine(value);
            }
        }


        private static StringBuilder _currentLogError = new StringBuilder();

        /// <summary>
        /// Запись ошибок в лог файл и кеш
        /// </summary>
        public static string CurrentLogError
        {
            set
            {
                File.AppendAllText(_logFile , Helpers.GetTime() + " " + value + "\n");
                _currentLogError.AppendLine(value);
            }
        }


        private static StringBuilder _currentLogFiles = new StringBuilder();


        /// <summary>
        /// Запись лога операций с файлами и директориями
        /// </summary>
        public static string CurrentLogFiles
        {
            set
            {
                File.AppendAllText(_logCopyFile, Helpers.GetTime() + " " + value + "\n");
                _currentLogFiles.AppendLine(value);
            }
        }

        
        /// <summary>
        /// Вывести текущий лог и обнулить кеш
        /// </summary>
        public static void PrintCurrentLog()
        {

            Console.WriteLine(_currentLogText);
            Helpers.WriteColorBox(_currentLogError.ToString(), ConsoleColor.White, ConsoleColor.Red); //Ошибки из лога выводятся цветом
            
            _currentLogText.Clear();
            _currentLogError.Clear();
        }

    }
}