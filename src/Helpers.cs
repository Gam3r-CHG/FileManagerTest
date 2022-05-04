using System;
using System.ComponentModel;
using System.Text;

namespace FileManager
{
    public class Helpers
    {
        /// <summary>
        /// Поставить на паузу выполнение программы
        /// </summary>
        /// <param name="task">Варианты: 0 - выйти из программы; 1 - продолжить</param>
        public static void PressAnyKey(int task)
        {
            switch (task)
            {
                case 0:
                    Console.WriteLine();
                    Console.Write(" Для выхода из программы нажмите любую клавишу...");
                    Console.ReadKey(true);
                    break;

                case 1:
                    Console.WriteLine();
                    Console.Write(" Для продолжения нажмите любую клавишу...");
                    Console.ReadKey(true);
                    break;

                default:
                    Console.ReadKey(true);
                    break;
            }
        }

        /// <summary>
        /// Получить текущее время в виде строки
        /// </summary>
        /// <returns></returns>
        public static string GetTime()
        {
            return DateTime.Now.ToString();
        }


        /// <summary>
        /// Вывести цветной текст с фоном (без перевода строки)
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="colorBackground">(Необязательный параметр) Цвет фона. Например {ConsoleColor.Black}</param>
        /// <param name="colorForeground">(Необязательный параметр) Цвет текста. Например {ConsoleColor.Red}</param>
        public static void WriteColorBox(string text, ConsoleColor colorBackground = ConsoleColor.Black, ConsoleColor colorForeground = ConsoleColor.Gray)
        {
            ConsoleColor lastColorBackground = Console.BackgroundColor;
            ConsoleColor lastColorForeground = Console.ForegroundColor;
            Console.BackgroundColor = colorBackground;        //Установить цвет, если передан параметр
            Console.ForegroundColor = colorForeground;        //Установить цвет, если передан параметр
            Console.Write(text);
            Console.BackgroundColor = lastColorBackground;    //Сбросить настройки цвета
            Console.ForegroundColor = lastColorForeground;    //Сбросить настройки цвета
        }


        /// <summary>
        /// Вывести цветной текст (без перевода строки)
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="color">(Необязательный параметр) Цвет текста. Например {ConsoleColor.Red}</param>
        public static void WriteColor(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            ConsoleColor lastColor = Console.ForegroundColor;
            Console.ForegroundColor = color;        //Установить цвет, если передан параметр
            Console.Write(text);
            Console.ForegroundColor = lastColor;    //Сбросить настройки цвета
        }


        /// <summary>
        /// Вывести цветной текст (с переводом строки)
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="color">(Необязательный параметр) Цвет текста. Например {ConsoleColor.Red}</param>
        public static void WriteLineColor(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            ConsoleColor lastColor = Console.ForegroundColor;
            Console.ForegroundColor = color;    //Установить цвет, если передан параметр
            Console.WriteLine(text);
            Console.ForegroundColor = lastColor;    //Сбросить настройки цвета
        }


        /// <summary>
        /// Разделить StrinBuilder на строки по {X} символов. С учетов уже имеющихся переносов. Можно вызывать в методе.
        /// </summary>
        /// <param name="sb">StringBuilder для изменения</param>
        /// <param name="number">Количество символов в строке</param>
        /// <returns></returns>
        public static StringBuilder SplitString(StringBuilder sb, int number)
        {
            for (int i = 0, isNumber = 0; i <= sb.Length; i++, isNumber++)
            {
                if (sb[i] == '\n')
                {
                    i++;
                    isNumber = 0;
                }

                if (isNumber == number)
                {
                    sb.Insert(i, '\n');
                    isNumber = -1;
                }
            }
            return sb;
        }

        /// <summary>
        /// Разделить строку на строки по {X} символов. С учетов уже имеющихся переносов. Можно вызывать в методе.
        /// </summary>
        /// <param name="sb">Строка для изменения</param>
        /// <param name="number">Количество символов в строке</param>
        /// <returns></returns>
        public static string SplitString(string str, int number)
        {
            StringBuilder sb = new StringBuilder(str);

            for (int i = 0, isNumber = 0; i < sb.Length; i++, isNumber++)
            {
                if (sb[i] == '\n')
                {
                    i++;
                    isNumber = 0;
                }

                if (isNumber == number)
                {
                    sb.Insert(i, '\n');
                    isNumber = -1;
                }
            }
            str = sb.ToString();

            return str;
        }


        /// <summary>
        /// Разделить строку {ref} на строки по {X} символов. С учетов уже имеющихся переносов. Можно вызывать в методе.
        /// </summary>
        /// <param name="sb">Строка для изменения {ref]</param>
        /// <param name="number">Количество символов в строке</param>
        /// <returns></returns>
        public static string SplitString(ref string str, int number)
        {
            StringBuilder sb = new StringBuilder(str);

            for (int i = 0, isNumber = 0; i < sb.Length; i++, isNumber++)
            {
                if (sb[i] == '\n')
                {
                    i++;
                    isNumber = 0;
                }

                if (isNumber == number)
                {
                    sb.Insert(i, '\n');
                    isNumber = -1;
                }
            }
            str = sb.ToString();

            return str;
        }


        /// <summary>
        /// Получить описание элемента Enum (Быстро, без проверок на null и другие ошибки)
        /// </summary>
        /// <param name="enumValue">Объект Enum: (Enum.Element) или (enum)</param>
        /// <returns>Описание</returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return ((DescriptionAttribute)attrs[0]).Description;
        }


    }
}