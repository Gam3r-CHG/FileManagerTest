namespace FileManager
{


    /// <summary>
    /// Парсинг команд и выбор действия
    /// </summary>
    internal class ParseCommand
    {


        //Все команды записываются в лог (или ошибки)
        //Дальше вызываются методы из класса Actions или обновляются окна

        /// <summary>
        /// Парсинг команды List
        /// </summary>
        /// <param name="option1"></param>
        /// <param name="option2"></param>
        /// <param name="option3"></param>
        public static void ParseList(string option1, string option2, string option3)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogText = $"Parser: Введена команда List без опций.>";
                Actions.ActionList();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда List, опции: <{option1}> <{option2}> <{option3}>";
                Actions.ActionList(option1, option2, option3);
            }
        }


        /// <summary>
        /// Парсинг команды Tree
        /// </summary>
        /// <param name="option1"></param>
        /// <param name="option2"></param>
        /// <param name="option3"></param>
        public static void ParseTree(string option1, string option2, string option3)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogText = $"Parser: Введена команда Tree без опций.>";
                Actions.ActionTree();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда Tree, опции: <{option1}> <{option2}> <{option3}>";
                Actions.ActionTree(option1, option2, option3);
            }
        }

        /// <summary>
        /// Парсинг команды FileList
        /// </summary>
        /// <param name="option1"></param>
        /// <param name="option2"></param>
        public static void ParseFileList(string option1, string option2)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogText = $"Parser: Введена команда FileList без опций";
                Actions.ActionFileList();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда FileList, опции: <{option1}> <{option2}>";
                Actions.ActionFileList(option1, option2);
            }

        }



        /// <summary>
        /// Парсинг команды Copy
        /// </summary>
        /// <param name="option1"></param>
        /// <param name="option2"></param>
        public static void ParseCopy(string option1, string option2)
        {
            if (string.IsNullOrEmpty(option2))
            {
                LogIt.CurrentLogError = $"Parser: Введена команда Copy без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда Copy, опции: <{option1}> <{option2}>";
                Actions.ActionCopy(option1, option2);
                
            }
        }


        /// <summary>
        /// Парсинг команды Del
        /// </summary>
        /// <param name="option1"></param>
        public static void ParseDel(string option1)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogError = $"Parser: Введена команда Del без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда Del, опции: <{option1}>";
                Actions.ActionDel(option1);
            }
        }

        /// <summary>
        /// Парсинг команды FileInfo
        /// </summary>
        /// <param name="option1"></param>
        public static void ParseFileInfo(string option1)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogError = $"Parser: Введена команда FileInfo без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда FileInfo, опции: <{option1}>";
                Actions.ActionFileInfo(option1);

            }
        }

        /// <summary>
        /// Парсинг команды CD
        /// </summary>
        /// <param name="option1"></param>
        public static void ParseChdir(string option1)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogError = "Parser: Введена команда ChDir без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда ChDir, опции: <{option1}>";
                Actions.ActionChdir(option1);
            }
        }

        /// <summary>
        /// Парсинг команды Help
        /// </summary>
        /// <param name="option1"></param>
        public static void ParseHelp(string option1)
        {
            LogIt.CurrentLogText = $"Parser: Введена команда Help.";
            Actions.ActionHelp();
        }


        /// <summary>
        /// Парсинг команды Mkdir
        /// </summary>
        /// <param name="option1"></param>
        public static void ParseMkdir(string option1)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogError = $"Parser: Введена команда MkDir без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда MkDir, опции: <{option1}>";
                Actions.ActionMkdir(option1);
            }
        }


        /// <summary>
        /// Парсинг команды Run
        /// </summary>
        /// <param name="option1"></param>
        public static void ParseRun(string option1)
        {
            if (string.IsNullOrEmpty(option1))
            { 
                LogIt.CurrentLogError = $"Parser: Введена команда Run без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
               LogIt.CurrentLogText = $"Parser: Введена команда Run, опции: <{option1}>";
                Actions.ActionRun(option1);
            }
        }


        /// <summary>
        /// Парсинг команды Exit
        /// </summary>
        public static void ParseExit()
        {
            LogIt.CurrentLogText = $"Parser: Введена команда Exit";
            Actions.ActionExit();
        }


        /// <summary>
        /// Парсинг команды ShowFiles
        /// </summary>
        public static void ParseShowFiles()
        {
            LogIt.CurrentLogText = $"Parser: Включить показ файлов <{!GetData.ShowFiles}>.";
            Actions.ShowFiles();
        }



        /// <summary>
        /// Парсинг команды Ren
        /// </summary>
        /// <param name="option1"></param>
        /// <param name="option2"></param>
        public static void ParseRen(string option1, string option2)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogError = $"Parser: Введена команда Rename без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда Rename, опции: <{option1}> <{option2}>";
                Actions.ActionRen(option1, option2);
            }
        }


        /// <summary>
        /// Парсинг команды Read
        /// </summary>
        /// <param name="option1"></param>
        public static void ParseReadFile(string option1)
        {
            if (string.IsNullOrEmpty(option1))
            {
                LogIt.CurrentLogError = $"Parser: Введена команда ReadFile без опций";
                DrawWindows.RefreshLog();
            }
            else
            {
                LogIt.CurrentLogText = $"Parser: Введена команда ReadFile, опции: <{option1}>";
                Actions.ActionReadFile(option1);

            }
        }
    }
}