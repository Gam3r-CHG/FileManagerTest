using System;
using System.Diagnostics;
using System.IO;

namespace FileManager
{

    /// <summary>
    /// Действия, в зависимости от класса ParseCommand. Запись выполненных действий в лог
    /// </summary>
    internal class Actions
    {
        
        /// <summary>
        /// Установить номер страницы для вывода в классе DrawWindows, в зависимости от параметров
        /// </summary>
        /// <param name="option">Параметр</param>
        /// <param name="page">Номер страницы</param>
        private static void SetPageNumber(string dir = "", string option = "", string page = "")
        {
            if (option == "-p" && !string.IsNullOrEmpty(page))
            {
                if (int.TryParse(page, out int pageNumber))
                {
                    DrawWindows.pageForMainWindow = pageNumber;
                }
                else { LogIt.CurrentLogError = "Actions: Введен некорректный номер страницы, установлено на 1"; }
            }
            else if (dir == "-p" && !string.IsNullOrEmpty(option))
            {
                if (int.TryParse(option, out int pageNumber))
                {
                    DrawWindows.pageForMainWindow = pageNumber;
                }
                else
                {
                    LogIt.CurrentLogError = "Actions: Введен некорректный номер страницы, установлено на 1";
                }
            }
            else { DrawWindows.pageForMainWindow = 1;} //Номер страницы по умолчанию - 1
        }

        /// <summary>
        /// Вывести список директорий и установить вид главного окна - List
        /// </summary>
        /// <param name="dir">Директория</param>
        /// <param name="option">Опции</param>
        /// <param name="page">Номер страницы</param>
        public static void ActionList(string dir = "", string option = "", string page = "")
        {
            GetData.DirectoryName = dir;    //Передать имя директории в класс GetData

            SetPageNumber(dir, option, page);  //Установить номер страницы

            DrawWindows.typeOfData = TypeOfData.List; //Установить вид главного окна

            DrawWindows.RefreshMainWindows();  //Обновить окна
        }

        /// <summary>
        /// Вывести дерево директорий и файлов и установить вид главного окна - Tree
        /// </summary>
        /// <param name="dir">Директория</param>
        /// <param name="option">Опции</param>
        /// <param name="page">Номер страницы</param>
        public static void ActionTree(string dir = "", string option = "", string page = "")
        {
            GetData.DirectoryName = dir;  //Передать имя директории в класс GetData

            SetPageNumber(dir, option, page);  //Установить номер страницы

            DrawWindows.typeOfData = TypeOfData.Tree; //Установить вид главного окна

            DrawWindows.RefreshMainWindows();  //Обновить окна
        }

        /// <summary>
        /// Вывести файлы в директории и установить вид главного окна - Files
        /// </summary>
        /// <param name="dir">Директория</param>
        /// <param name="page">Номер страницы</param>
        public static void ActionFileList(string dir = "", string option = "", string page = "")
        {
            GetData.DirectoryName = dir;  //Передать имя директории в класс GetData

            SetPageNumber(dir, option, page);  //Установить номер страницы

            DrawWindows.typeOfData = TypeOfData.Files; //Установить вид главного окна

            DrawWindows.RefreshMainWindows();  //Обновить окна
        }


        /// <summary>
        /// Переключить выводить файлы или нет в списках директорий (list, tree)
        /// </summary>
        public static void ShowFiles()
        {
            GetData.ShowFiles = !GetData.ShowFiles; //Переключить параметр в GetData
            DrawWindows.RefreshMainWindows();  //Обновить окна
        }



        /// <summary>
        /// Копировать файл или директорию (вместе со всем содержимым). Метод выбирает - файл или директория
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        public static void ActionCopy(string path1, string path2)
        {
            if (Directory.Exists(path1))
            {
                CopyDir(path1, path2);
                LogIt.CurrentLogText = $"Actions: Директория <{path1}> скопирована в <{path1}>. Лог записан в файл logFiles.txt"; //Запись всех действий в отдельный лог файл
            }

            else if (File.Exists(path1) && Directory.Exists(path2))
            {
                CopyFileToDir(path1, path2);
            }
            else if (File.Exists(path1) && !Directory.Exists(path2))
            {
                CopyFile(path1, path2);
            }
            else
            {
                LogIt.CurrentLogError = $"Actions: Ошибка! Директория или файл с таким именем <{path1}> не найдены.";
            }
            DrawWindows.RefreshMainWindows();  //Обновить окна
        }



        /// <summary>
        /// Копировать файл в файл
        /// </summary>
        /// <param name="path1">Исходный файл</param>
        /// <param name="path2">Новый файл</param>
        private static void CopyFile(string path1, string path2)
        {
            if (File.Exists(path2))
            {
                LogIt.CurrentLogError = $"Actions: Ошибка! Файл с таким именем <{path2}> ужу существует.";
                return;
            }

            try
            {
                File.Copy(path1, path2);
                LogIt.CurrentLogText = $"Actions: Файл <{path1}> успешно скопирован.";
            }
            catch (Exception e)
            {
                LogIt.CurrentLogError = $"Actions: Невозможно скопировать файл <{path1}>.";
                LogIt.CurrentLogError = "Exception: " + e.Message;
            }
        }

        /// <summary>
        /// Копировать файл в директорию
        /// </summary>
        /// <param name="path1">Исходный файл</param>
        /// <param name="path2">Директория</param>
        private static void CopyFileToDir(string path1, string path2)
        {
            string newFile = Path.Combine(Path.GetFullPath(path2), Path.GetFileName(path1));
            if (File.Exists(newFile))
            {
                LogIt.CurrentLogError = $"Actions: Ошибка! Файл с таким именем <{newFile}> ужу существует.";
                return;
            }

            try
            {
                File.Copy(path1, newFile);
                LogIt.CurrentLogText = $"Actions: Файл <{path1}> успешно скопирован в директорию <{path2}>.";
            }
            catch (Exception e)
            {
                LogIt.CurrentLogError = $"Actions: Невозможно скопировать файл <{path1}>.";
                LogIt.CurrentLogError = "Exception: " + e.Message;
            }
        }


        /// <summary>
        /// Копировать директорию вместе со всем содержимым (файлы перезаписываются)
        /// </summary>
        /// <param name="path1">Исходная директория</param>
        /// <param name="path2">Куда копировать (создать если не существует)</param>
        private static void CopyDir(string path1, string path2)
        {
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
                LogIt.CurrentLogFiles = $"Создана директория <{path2}>";
            }
            else { LogIt.CurrentLogFiles = $"Директория <{path2}> уже существовала"; }

            string[] files = Directory.GetFiles(path1);

            foreach (var file in files)
            {
                try
                {
                    string file2 = path2 + "\\" + Path.GetFileName(file);
                    if (File.Exists(file2)) { LogIt.CurrentLogFiles = $"Файл <{file2}> уже существует и будет перезаписан!"; }
                    File.Copy(file, file2, true);
                    LogIt.CurrentLogFiles = $"Скопирован <{file}> в <{file2}>";
                }
                catch (Exception e)
                {
                    LogIt.CurrentLogFiles = $"Exception: {e.Message}";
                }

            }

            //Рекурсивный вызов, чтобы пройти по всем вложенным директориям
            string[] dirs = Directory.GetDirectories(path1);
            foreach (var dir in dirs)
            {
                CopyDir(dir, path2 + "\\" + Path.GetFileName(dir));
            }
        }


        /// <summary>
        /// Удалить файл или директорию (вместе с вложенными). Выбор файл или директория!
        /// </summary>
        /// <param name="path">Название файла или директории</param>
        public static void ActionDel(string path)
        {
            if (Directory.Exists(path))
            {
                DelDir(path);
            }

            else if (File.Exists(path))
            {
                DelFile(path);
            }

            else
            {
                LogIt.CurrentLogError = $"Actions: Ошибка! Директория или файл с таким именем <{path}> не найдены.";
            }

            DrawWindows.RefreshMainWindows();   //Обновить окна
        }

        
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="path">Название файла</param>
        private static void DelFile(string path)
        {
            try
            {
                File.Delete(path);
                LogIt.CurrentLogText = $"Actions: Файл <{path}> удален.";
            }
            catch (Exception e)
            {
                LogIt.CurrentLogError = $"Actions: Невозможно удалить файл <{path}>.";
                LogIt.CurrentLogError = "Exception: " + e.Message;

            }
        }


        /// <summary>
        /// Удалить директорию
        /// </summary>
        /// <param name="path">Название директории</param>
        private static void DelDir(string path)
        {
            try
            {
                Directory.Delete(path, true);
                LogIt.CurrentLogText = $"Actions: Директория <{path}> удалена.";
            }
            catch (Exception e)
            {
                LogIt.CurrentLogError = $"Actions: Невозможно удалить директорию <{path}>.";
                LogIt.CurrentLogError = "Exception: " + e.Message;
            }
        }

        /// <summary>
        /// Показать информацию о файле
        /// </summary>
        /// <param name="fileName">Название файла</param>
        public static void ActionFileInfo(string fileName)
        {
            GetData.FileName = fileName; //Передать имя файла в класс GetData
            DrawWindows.isDrawFileInfo = true; //Установить вид окна в DrawWindows
            DrawWindows.RefreshInfo();   //Обновить окно с информацией
        }


        /// <summary>
        /// Сменить директорию
        /// </summary>
        /// <param name="path">Название директории</param>
        public static void ActionChdir(string path)
        {
            GetData.DirectoryName = path; //Передать имя директории в класс GetData
            DrawWindows.RefreshMainWindows();  //Обновить окна
        }


        /// <summary>
        /// Вывести текст с командами (Help)
        /// </summary>
        public static void ActionHelp()
        {
            DrawWindows.typeOfData = TypeOfData.Help; //Установить вид окна в DrawWindows

            DrawWindows.RefreshMainWindows();  //Обновить окна
        }

        /// <summary>
        /// Создать директорию
        /// </summary>
        /// <param name="dir">Название директории</param>
        public static void ActionMkdir(string dir)
        {
            if (Directory.Exists(dir))
            {
                LogIt.CurrentLogError = $"Actions: Директория с именем <{dir}> уже существует.";
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(dir);
                    LogIt.CurrentLogText = $"Actions: Директория с именем <{dir}> создана.";
                }
                catch (Exception e)
                {
                    LogIt.CurrentLogError = $"Actions: Не удалось создать директорию с именем <{dir}>.";
                    LogIt.CurrentLogError = "Exception: " + e.Message;
                }
            }

            DrawWindows.RefreshMainWindows();  //Обновить окна
        }

        /// <summary>
        /// Запуск приложения
        /// </summary>
        /// <param name="appName">Имя приложения</param>
        public static void ActionRun(string appName)
        {
            try
            {
                Process.Start(appName);
                LogIt.CurrentLogText = $"Actions: Процесс с именем <{appName}> успешно запущен.";
            }
            catch (Exception e)
            {
                LogIt.CurrentLogError = $"Actions: Процесс с именем <{appName}> не удалось запустить.";
                LogIt.CurrentLogError = "Exception: " + e.Message;
            }

            DrawWindows.RefreshLog();
        }

        /// <summary>
        /// Выйти из программы и сохранить состояние
        /// </summary>
        public static void ActionExit()
        {
            DrawWindows.ClearCommandLog(); //Очистить лог окно и установить курсор
            GetData.SaveState();  //Сохранить состояние в файл
            Helpers.PressAnyKey(0);
            Environment.Exit(0);
        }

        /// <summary>
        /// Переименовать или переместить файл
        /// </summary>
        /// <param name="path1">Исходный файл</param>
        /// <param name="path2">Новый файл</param>
        public static void ActionRen(string path1, string path2)
        {
            if (!File.Exists(path1))
            {
                LogIt.CurrentLogError = $"Action: Файл <{path1}> не существует";
            }
            else if (File.Exists(path2)){ LogIt.CurrentLogError = $"Action: Файл <{path2}> уже существует"; }
            else
            {
                try
                {
                    File.Move(path1, path2);
                    LogIt.CurrentLogText = $"Action: Файл <{path1}> успешно переименован в <{path2}>";
                }
                catch (Exception e)
                {
                    LogIt.CurrentLogError = $"Action: Файл <{path1}> не удалось переименовать в <{path2}>";
                    LogIt.CurrentLogError = "Exception: " + e.Message;
                }
            }

            DrawWindows.RefreshMainWindows();  //Обновить окна
        }

        /// <summary>
        /// Прочитать текстовый файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public static void ActionReadFile(string fileName)
        {
            DrawWindows.typeOfData = TypeOfData.Read; //Установить вид окна в DrawWindows
            GetData.FileName = fileName; //Передать имя файла в класс GetData

            DrawWindows.RefreshMainWindows();  //Обновить окна
        }



    }
}