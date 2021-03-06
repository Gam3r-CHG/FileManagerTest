# Консольный файл менеджер.

Здесь представлен простой консольный файл менеджер. Интерфейс пока не доделан до конца, упор был на функциональность. Все команды вводятся в консоль, по примеру terminal. Реализованы почти все необходимые команды для работы с файлами и директориями (копирование, удаление, переименование, чтение и т.д.). При выходе из приложения настройки сохраняются (открытая папка и режим отображения). Все действия логируются и записываются в файл (с указанием класса).

![Main Window](https://github.com/Gam3r-CHG/FileManagerTest/blob/main/Pics/main2.png "Главное окно приложения")

## Список команд и примеры

Все команды регистр независимы. У почти всех команд есть краткие версии (перечислены через запятую). Все команды, которые предполагают вывод текста в главное окно позволяют указать номер страницы через команду ```-p № страницы```

- **help, ?** - вывести список команд
- **cd, chdir** - сменить директорию или диск
    - ```Например: cd t: |  cd t:\tests\```
- **showfiles** - переключатель выводить файлы в командах ls, tree, cd или нет. По умолчанию выключено 
- **ls, list** - вывести список директорий (и файлов). Также меняет текущую директорию.
    - ```Например: ls (в текущей папке)  |  ls c: | ls c:\tests -p 2 (номер страницы)```
- **tr, tree** - вывести дерево директорий (и файлов). Также меняет текущую директорию.
    - ```Например: tr (в текущей папке)  |  tree c: | tree c:\tests -p 2 (номер страницы)```
- **fl, files** - вывести список файлов. Также меняет текущую директорию.
    - ```Например: fl (в текущей папке)  |  files c: | fl c:\tests -p 2 (номер страницы)```
- **finfo, fileinfo** - информация о файле
    - ```Например: finfo test.txt  |  fileinfo c:\test.txt```
- **read** - чтение текстового файла
    - ```Например: read test.txt  |  read c:\test.txt -p 5```
- **cp, copy** - копировать файл или директорию
    - ```Например: cp c:\source d:\target  |  copy c:\source.txt d:\target.txt (cp test.txt >> создать копию файла с именем test_n.txt	- пока не реализовано)```
- **rm, del** - удалить файл или директорию
    - ```Например: rm c:\source  |  rm c:\source.txt```
- **ren, rename** - переименовать файл (или директорию - пока не реализовано)
    - ```Например: ren c:\source c:\source2  |  rename c:\source.txt c:\source2.txt```
- **md, mkdir** - создать директорию
    - ```Например: md c:\source |  md source```
- **run, start** - запустить файл
    - ```Например: run notepad.exe  |  start explorer.exe```
- **exit, quit** - выход и сохранение состояния приложения

![Help](https://github.com/Gam3r-CHG/FileManagerTest/blob/main/Pics/main4.png "Пример help файла")

## Интерфейс

Интерфейс состоит из 6 элементов:
1. **Главное окно** - выводится информации о структуре директорий и вывод текстовых файлов
2. **Инфо** - информация о текущей директории или файле
3. **Флаги** - отображение включённых флагов (тип главного окна и показ файлов)
4. **Help** - краткая справка о командах
5. **Консоль** - ввод команд
6. **Лог** - вывод отчета о всех действиях программы и ошибках

![Windows](https://github.com/Gam3r-CHG/FileManagerTest/blob/main/Pics/windows.png "Расположение окон")


## Структура проекта

Проект состоит из нескольких классов. Каждый их них выполняет свою функцию.
- ***Program*** - точка входа
- ***Helpers*** - методы для вывода текста цветом и работы со строками
- ***LogIt*** - логирование всех действий и ошибок
- ***CommandLine*** - Работа командной строки и передача параметров (в планах усовершенствовать, поэтому выделен в отдельный класс)
- ***ParseCommand*** - Обработка командной строки и запуск действий
- ***Actions*** - Действия с файлами и директориями. Выполнение команд.
- ***GetData*** - Обработка и передача данных о файлах и директориях.
- ***DrawWindows*** - Интерфейс приложения. Вся работа идет из него. При желании можно заменить другим интерфейсом.

Сначала отрисовываются окна, заполняются информацией полученной из класса GetData.
Затем программа ждет ввода команд, идет обработка ввода и выполнение запрошенных действий.
Потом опять отрисовка окон и так по кругу. На всех этапах идет логирование с указанием класса проекта (чтобы отлавливать ошибки) и запись лога в файл. При копировании файлов создается  отдельный файл с логом действий для каждого файла и директории

![Log copy file](https://github.com/Gam3r-CHG/FileManagerTest/blob/main/Pics/log2.png "Пример лог файла при копировании")

```Init > DrawWindows (GetData) > CommandLine > ParseCommand > Actions > DrawWindows (GetData) ...```

## Что в планах...

- [ ] Переделать командную строку, сделать более универсальную с вводом команд с помощью функциональных клавиш (F1-F12) 
- [ ] Расширить количество команд и их параметров, добавить возможность использовать длинные имена файлов и директорий
- [ ] Добавить возможность перемещения по списку файлов и директорий клавишами
- [ ] Добавить другие интерфейсы, не только консольные

![Log file](https://github.com/Gam3r-CHG/FileManagerTest/blob/main/Pics/log1.png "Пример лог файла")
