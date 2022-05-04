using System;
using System.IO;
using System.Text;

namespace FileManager
{
    /// <summary>
    /// Получение и передача данных о файлах и директориях. Save/Load состояния
    /// </summary>
    static class GetData
    {

        //Файлы необходимые для работы приложения
        private static readonly string _helpFile = Directory.GetCurrentDirectory() + "\\help_short.txt";

        private static readonly string _stateFileName = Directory.GetCurrentDirectory() + "\\state.cfg";

        private static readonly string _helpFileName = Directory.GetCurrentDirectory() + "\\help.txt";

        //Переключатель отображения файлов
        public static bool ShowFiles;

        //Директория для отображения
        private static string _directoryName = Directory.GetCurrentDirectory();
        
        public static string DirectoryName
        {
            get { return _directoryName; }
            set
            {
                if (Directory.Exists(Path.GetFullPath(_directoryName + "\\" + value)))
                {
                    _directoryName = Path.GetFullPath(_directoryName + "\\" + value);
                    LogIt.CurrentLogText = $"GetData: Директория успешно установлена <{_directoryName}>";

                }
                else if (Directory.Exists(Path.GetFullPath(value + "\\")))
                {
                    _directoryName = Path.GetFullPath(value + "\\");
                    LogIt.CurrentLogText = $"GetData: Директория успешно сменена на <{_directoryName}>";
                }
                else
                {
                    LogIt.CurrentLogError = $"GetData: Директорию не удалось сменить на <{value}>";
                }

                Directory.SetCurrentDirectory(_directoryName);
            }
        }

        //Файл для чтения или для информации о файле
        private static string _fileName;
        public static string FileName
        {
            get { return _fileName; }
            set
            {
                if (File.Exists(value))
                {
                    _fileName = value;
                    LogIt.CurrentLogText = $"GetData: Запрошенный файл  <{_fileName}> существует";
                }
                else
                {
                    LogIt.CurrentLogError = $"GetData: Запрошенный файл  <{_fileName}> не существует";
                }
            }
        }


        /// <summary>
        /// Запросить список директорий
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetDirs()
        {
            LogIt.CurrentLogText = "GetData: Список директорий получен.";
            return DirectoriesList(new DirectoryInfo(_directoryName));
        }

        /// <summary>
        /// Запросить список директорий в виде дерева
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetTree()
        {
            LogIt.CurrentLogText = "GetData: Список директорий в виде дерева получен.";
            return DirectoriesTree(new StringBuilder(), new DirectoryInfo(_directoryName), "", false);
        }

        /// <summary>
        /// Запросить список файлов в директории
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetFilesInDir()
        {
            LogIt.CurrentLogText = "GetData: Список файлов получен.";
            return FilesInDirectory(new DirectoryInfo(_directoryName));
        }



        /// <summary>
        /// Получить список папок и файлов
        /// </summary>
        /// <param name="directory">Начальная директория</param>
        private static StringBuilder DirectoriesList(DirectoryInfo directory)
        {
            StringBuilder dirList = new StringBuilder();

            DirectoryInfo[] dirs = directory.GetDirectories();

            for (int i = 0; i < dirs.Length; i++)
            {
                try //Чтобы избежать ошибок для системных папок
                {
                    dirList.AppendLine("► " + dirs[i].Name);
                }
                catch { }
            }
            if (GetData.ShowFiles) dirList.Append(FilesInDirectory(directory));   //Если переключатель ShowFiles = true, получить список файлов

            return dirList;
        }


        /// <summary>
        /// Получить дерево папок, вложенных папок и файлов рекурсивным способом
        /// </summary>
        /// <param name="directory">Начальная директория</param>
        /// <param name="indent">Отступ (для оформления)</param>
        /// <param name="isLastDirectory">Последняя директория?</param>
        private static StringBuilder DirectoriesTree(StringBuilder treeList, DirectoryInfo directory, string indent, bool isLastDirectory)
        {
            treeList.Append(indent);                //добавляем накопившийся отступ

            if (isLastDirectory)                    //если последняя директория в списке
            {
                treeList.AppendFormat("└─");        //добавляем символ
                indent += "  ";                     //и увеличиваем отступ

            }
            else
            {
                treeList.Append("├─");              //если не последняя в списке - добавляем символ
                indent += "│ ";                     //и увеличиваем отступ
            }

            treeList.Append(directory.Name + "\n"); //добавляем название директории и перевод строки


            if (ShowFiles) treeList.Append(FilesForDirectoriesTree(directory, indent));   //Если переключатель ShowFiles = true, получить список файлов

            DirectoryInfo[] subDirs = directory.GetDirectories();

            for (int i = 0; i < subDirs.Length; i++)
            {
                try //Чтобы избежать ошибок для системных папок
                {
                    DirectoriesTree(treeList, subDirs[i], indent, i == subDirs.Length - 1);
                }
                catch { }
            }

            return treeList;
        }


        /// <summary>
        /// Получить список файлов в указанной директории для метода DirectoriesTree
        /// </summary>
        /// <param name="directory">Директория</param>
        /// <param name="indent">Отступ (для оформления)</param>
        private static StringBuilder FilesForDirectoriesTree(DirectoryInfo directory, string indent)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < directory.GetFiles().Length; i++)
            {
                sb.Append(indent);
                if (i < directory.GetFiles().Length - 1)
                {
                    sb.Append("├─" + directory.GetFiles()[i] + "\n");  //Добавить отступ и название файла + перевод строки
                }
                else
                {
                    sb.Append("└─" + directory.GetFiles()[i] + "\n");  //Добавить отступ и название файла + перевод строки
                }
            }

            return sb;

        }


        /// <summary>
        /// Получить список файлов в указанной директории
        /// </summary>
        /// <param name="directory">Директория</param>
        private static StringBuilder FilesInDirectory(DirectoryInfo directory)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < directory.GetFiles().Length; i++)
            {
                if (ShowFiles)
                {
                    sb.Append("  > " + directory.GetFiles()[i].Name + "\n");
                }
                else sb.Append(directory.GetFiles()[i].Name + "\n");
            }

            return sb;
        }




        /// <summary>
        /// Получить информацию о директории <_directoryName>
        /// </summary>
        public static StringBuilder GetDirectoryInfo()
        {
            StringBuilder sb = new StringBuilder();
            DirectoryInfo dirInfo = new DirectoryInfo(_directoryName);
            sb.AppendLine("Информация о выбранной директории:");
            sb.AppendLine($"Родительская папка: {dirInfo.Parent}");
            sb.AppendLine($"Полное имя: {dirInfo.FullName}");
            sb.AppendLine($"Краткое имя: {dirInfo.Name}");
            sb.AppendLine($"Дата создания: {dirInfo.CreationTime}");
            sb.AppendLine($"Дата изменения: {dirInfo.LastWriteTime}");
            sb.AppendLine($"Атрибуты: {dirInfo.Attributes}");
            LogIt.CurrentLogText = "GetData: Информация о директории получена.";
            return sb;
        }


        /// <summary>
        /// Получить информацию о файле <fileName>
        /// </summary>
        public static StringBuilder GetFileInfo()
        {
            StringBuilder sb = new StringBuilder();
            FileInfo fi = new FileInfo(_fileName);
            sb.AppendLine($"Информация о файле {fi.Name}:");
            sb.AppendLine($"Полное имя: {fi.FullName}");
            sb.AppendLine($"Размер файла: {fi.Length} байт");
            sb.AppendLine($"Дата создания: {fi.CreationTime}");
            sb.AppendLine($"Только для чтения?: {fi.IsReadOnly}");
            sb.AppendLine($"Last Access Time: {fi.LastAccessTime}");
            sb.AppendLine($"Last Write Time: {fi.LastWriteTime}");
            LogIt.CurrentLogText = $"GetData: Информация о файле {_fileName} получена.";
            return sb;
        }


        /// <summary>
        /// Получить информацию для окна Help
        /// </summary>
        /// <returns></returns>
        public static string GetShortHelpText()
        {
            return File.ReadAllText(_helpFile);
        }


        /// <summary>
        /// Получить данные из Help файла для главного окна
        /// </summary>
        /// <returns></returns>
        public static StringBuilder ReadHelpFile()
        {
            return ReadFile(_helpFileName);
        }


        /// <summary>
        /// Получить данные из файла
        /// </summary>
        /// <param name="readFile">Название файла (параметр !help считывает файл помощи)</param>
        /// <returns></returns>
        public static StringBuilder ReadFile(string readFile)
        {
            StringBuilder sb = new StringBuilder();

            if (File.Exists(readFile))
            {
                try
                {
                    sb.Append(File.ReadAllText(readFile));
                    LogIt.CurrentLogText = $"GetData: Данные из <{readFile}> файла успешно прочитаны.";
                }
                catch (Exception e)
                {
                    LogIt.CurrentLogError = $"GetData: Данные из <{readFile}> не удалось прочитать.";
                    LogIt.CurrentLogError = $"Exception: {e}";
                }
            }

            else
            {
                LogIt.CurrentLogError = $"GetData: Файл <{readFile}> не найден.";
            }

            return sb;
        }



        /// <summary>
        /// Загрузить данные о состоянии приложения
        /// </summary>
        public static void LoadState()
        {
            if (File.Exists(_stateFileName))
            {
                string[] lines = File.ReadAllLines(_stateFileName);

                DirectoryName = lines[0];
                ShowFiles = Convert.ToBoolean(lines[1]);
                Enum.TryParse(lines[2], out DrawWindows.typeOfData);
                LogIt.CurrentLogText = "GetData: Данные из конфигурационного файла успешно прочитаны.";
            }
            else
            {
                LogIt.CurrentLogError = $"GetData: Конфигурационный файл {_stateFileName} не найден.";
            }
        }

        /// <summary>
        /// Сохранить данные о состоянии приложения
        /// </summary>
        public static void SaveState()
        {
            if (DrawWindows.typeOfData == TypeOfData.Read) DrawWindows.typeOfData = TypeOfData.List; //Сбросить состояние окна
            string temp = _directoryName + "\n" + ShowFiles + "\n" + DrawWindows.typeOfData;
            File.WriteAllText(_stateFileName, temp);
            LogIt.CurrentLogText = "GetData: Данные успешно записаны в конфигурационный файл.";
        }


    }
}