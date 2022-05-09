using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace FileManager
{
    /// <summary>
    /// Тип главного окна и его название
    /// </summary>
    public enum TypeOfData
    {
        [Description("List of Directories")]
        List = 0,
        [Description("Tree of Directories")]
        Tree = 1,
        [Description("List of Files")]
        Files = 2,
        [Description("Help Information")]
        Help = 3,
        [Description("Text in File")]
        Read = 4,
    }

    /// <summary>
    /// Класс отвечающий за отрисовку интерфейса
    /// </summary>
    static class DrawWindows
    {
        //Тип главного окна
        public static TypeOfData typeOfData;

        //Размеры консоли и окон
        const int WindowWidth = 140;
        const int WindowHeight = 50;

        const int DirWindowWidth = 80;
        const int DirWindowHeight = 35;
        const int InfoWindowWidth = 59;
        const int InfoWindowHeight = 12;
        const int FlagWindowWidth = 59;
        const int FlagWindowHeight = 3;
        const int HelpWindowWidth = 59;
        const int HelpWindowHeight = 20;
        const int ConsoleWindowWidth = 140;
        const int ConsoleWindowHeight = 3;


        public static bool isDrawFileInfo; //Вывести информацию о файле вместо директории
        public static int pageForMainWindow = 1; //Номер страницы главного окна

        /// <summary>
        /// Начальная отрисовка интерфейса
        /// </summary>
        public static void InitDraw()
        {
            //Установить название окна
            Console.Title = "FileManager";

            //Установить цвет окна и текста
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            //Установить размеры консоли
            Console.SetWindowSize(WindowWidth, WindowHeight);
            Console.SetBufferSize(WindowWidth, WindowHeight);

            //Записать в лог информацию
            LogIt.CurrentLogText = "InitDraw: Инициализация успешно завершена";

            //Загрузить состояние приложения
            GetData.LoadState();

            //Обновить все окна
            RefreshAll();
        }


        /// <summary>
        /// Заполнение данными главного окна в зависимости от его типа
        /// </summary>
        private static void DrawMainWindow()
        {
            StringBuilder sb = new StringBuilder();
            
            switch (typeOfData)
            {
                case TypeOfData.List:
                    sb = GetData.GetDirs();
                    break;
                case TypeOfData.Tree:
                    sb = GetData.GetTree();
                    break;
                case TypeOfData.Files:
                    sb = GetData.GetFilesInDir();
                    break;
                case TypeOfData.Help:
                    sb = GetData.ReadHelpFile();
                    break;
                case TypeOfData.Read:
                    sb = GetData.ReadFile(GetData.FileName);
                    break;
            }

            DrawMainWindowText(sb);
        }

        /// <summary>
        /// вывод текста в главное окно
        /// </summary>
        /// <param name="text"></param>
        private static void DrawMainWindowText(StringBuilder text)
        {
            if (pageForMainWindow == 0) pageForMainWindow = 1; //Если установлен не существующий номер страницы, вывести первую
            
            int pageLines = DirWindowHeight - 2;
            string[] lines = text.ToString().Split(new char[] { '\n' });
            int pageTotal = (lines.Length + pageLines - 1) / pageLines;
            if (pageForMainWindow > pageTotal) pageForMainWindow = pageTotal; //Если установлена страница больше чем есть, вывести последнюю

            for (int i = (pageForMainWindow - 1) * pageLines, counter = 0; i < pageForMainWindow * pageLines; i++, counter++)
            {
                if (lines.Length - 1 > i)
                {
                    Console.SetCursorPosition(1, 1 + counter);
                    
                    //Проверить длину строки
                    string cutLine;
                    if (lines[i].Length > DirWindowWidth-6)
                    {
                        cutLine = lines[i].Remove(DirWindowWidth-6) + "..."; //Если строка слишком длинная, обрезать
                    }
                    else {cutLine = lines[i];}

                    Console.WriteLine(cutLine);
                }
            }

            //Оформление нижней части главного окна (вывод номера страниц для наглядности)
            if (pageForMainWindow > 1)
            {
                string prevPage = $"<<< prev page {pageForMainWindow - 1}|";
                Console.SetCursorPosition(2, DirWindowHeight - 1);
                Console.WriteLine(prevPage);
            }

            if (pageTotal > 1)
            {
                string currentPage = $"< page #{pageForMainWindow} >";
                Console.SetCursorPosition(DirWindowWidth / 2 - currentPage.Length / 2, DirWindowHeight - 1);
                Console.WriteLine(currentPage);
            }

            if (pageTotal > 1 && pageForMainWindow != pageTotal)
            {
                string nextPage = $"|next page {pageForMainWindow + 1}>>>";
                Console.SetCursorPosition(DirWindowWidth - nextPage.Length - 2, DirWindowHeight - 1);
                Console.WriteLine(nextPage);
            }

        }
        
        /// <summary>
        /// Вывод информации о файле или директории
        /// </summary>
        private static void DrawInfo()
        {
            StringBuilder sb = new StringBuilder();
            
            //Выбрать какую информации выводить (файл или директория)
            if (isDrawFileInfo)
            {
                sb = GetData.GetFileInfo();
                isDrawFileInfo = false; //Сбросить параметр
            }
            else sb = GetData.GetDirectoryInfo();

            Helpers.SplitString(sb, InfoWindowWidth - 2); //Форматирование строк под ширину окна
            string[] lines = sb.ToString().Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(DirWindowWidth + 2, 1 + i);
                Console.WriteLine(lines[i]);
            }
        }

        /// <summary>
        /// Отрисовка "флагов"
        /// </summary>
        private static void DrawFlags()
        {
            Console.SetCursorPosition(DirWindowWidth + 2, InfoWindowHeight + 1);
            
            Console.Write("Основное окно: ");
            Helpers.WriteColorBox(typeOfData.ToString(), ConsoleColor.White, ConsoleColor.Black);

            Console.Write("    Показывать файлы: ");
            Helpers.WriteColorBox(GetData.ShowFiles.ToString(), ConsoleColor.White, ConsoleColor.Black);
        }

        /// <summary>
        /// Отрисовка окна Help
        /// </summary>
        private static void DrawHelp()
        {
            string[] lines = GetData.GetShortHelpText().Split("\n");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(DirWindowWidth + 2, InfoWindowHeight + FlagWindowHeight + i + 1);
                Console.WriteLine(lines[i]);
            }
        }


        /// <summary>
        /// Очистить окно с логами и установить курсор
        /// </summary>
        public static void ClearCommandLog()
        {
            Console.SetCursorPosition(0, DirWindowHeight + ConsoleWindowHeight);

            for (int i = DirWindowHeight + ConsoleWindowHeight; i < WindowHeight - 1; i++)
            {
                Console.WriteLine("".PadLeft(WindowWidth-1));
            }
            Console.SetCursorPosition(0, DirWindowHeight + ConsoleWindowHeight);
        }



        /// <summary>
        /// Нарисовать пустое окно в консоли
        /// </summary>
        /// <param name="x">Начальная позиция по оси X</param>
        /// <param name="y">Начальная позиция по оси Y</param>
        /// <param name="width">Ширина окна</param>
        /// <param name="height">Высота окна</param>
        /// <param name="windowTitle">Название окна</param>
        private static void DrawEmptyWindow(int x, int y, int width, int height, string windowTitle = "")
        {
            Console.SetCursorPosition(x, y);
            
            Console.Write("╔"); //Левый верхний угол
            for (int i = 0; i < width - 2; i++) 
            {
                if (windowTitle != "" && i == 3)
                {
                    Console.Write(windowTitle); //Название окна
                    i += windowTitle.Length - 1;
                    continue;
                }

                Console.Write("═"); //Верхняя граница
            }

            Console.Write("╗"); //Правый верхний угол

            Console.SetCursorPosition(x, ++y);
            for (int i = 0; i < height - 2; i++)
            {
                Console.Write("║");  //Левая граница
                for (int j = x + 1; j < x + width - 1; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║"); //Правая граница
                Console.SetCursorPosition(x, ++y);
            }

            Console.Write("╚");  //Левый нижний угол
            
            for (int i = 0; i < width - 2; i++) Console.Write("═"); //Нижняя граница
            
            Console.Write("╝");  //Правый нижний угол
        }


        /// <summary>
        /// Нарисовать окно для ввода команд (консоль)
        /// </summary>
        /// <param name="x">Начальная позиция по оси X</param>
        /// <param name="y">Начальная позиция по оси Y</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        private static void DrawConsole(int x, int y, int width, int height)
        {
            DrawEmptyWindow(x, y, width, height);
            Console.SetCursorPosition(x + 1, y + 1);
            Console.Write($"{Directory.GetCurrentDirectory()}>"); //Вывести название текущей директории
        }


        /// <summary>
        /// Обновить консоль и ожидать ввода команд
        /// </summary>
        private static void UpdateConsole()
        {
            DrawConsole(0, DirWindowHeight, WindowWidth, ConsoleWindowHeight);
            CommandLine.ReadCommandLine(); //Ожидать ввода команд
        }



        /// <summary>
        /// Обновить все окна
        /// </summary>
        public static void RefreshAll()
        {
            //Нарисовать пустые окна
            DrawEmptyWindow(0, 0, DirWindowWidth, DirWindowHeight, Helpers.GetEnumDescription(typeOfData));
            DrawEmptyWindow(DirWindowWidth + 1, 0, InfoWindowWidth, InfoWindowHeight, "Information");
            DrawEmptyWindow(DirWindowWidth + 1, InfoWindowHeight, FlagWindowWidth, FlagWindowHeight, "Flags");
            DrawEmptyWindow(DirWindowWidth + 1, InfoWindowHeight + FlagWindowHeight, HelpWindowWidth, HelpWindowHeight, "Help");

            //Заполнить информацией
            DrawMainWindow();

            DrawInfo();

            DrawHelp();

            RefreshLog();
        }

        /// <summary>
        /// Обновить все окна кроме Help
        /// </summary>
        public static void RefreshMainWindows()
        {
            //Нарисовать пустые окна
            DrawEmptyWindow(0, 0, DirWindowWidth, DirWindowHeight, Helpers.GetEnumDescription(typeOfData));
            DrawEmptyWindow(DirWindowWidth + 1, 0, InfoWindowWidth, InfoWindowHeight, "Information");
            DrawEmptyWindow(DirWindowWidth + 1, InfoWindowHeight, FlagWindowWidth, FlagWindowHeight, "Flags");

            //Заполнить информацией
            DrawMainWindow();

            DrawInfo();

            RefreshLog();
        }

        /// <summary>
        /// Обновить окно с информацией
        /// </summary>
        public static void RefreshInfo()
        {
            DrawEmptyWindow(DirWindowWidth + 1, 0, InfoWindowWidth, InfoWindowHeight, "Information");

            DrawInfo();

            RefreshLog();
        }

        /// <summary>
        /// Обновить лог окно и окно консоли
        /// </summary>
        public static void RefreshLog()
        {
            DrawFlags();
            ClearCommandLog();
            LogIt.PrintCurrentLog(); //Вывести кеш лога
            UpdateConsole();
        }

    }
}