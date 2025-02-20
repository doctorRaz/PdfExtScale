/*
*        Copyright doctorRAZ 2014-2025 by Разыграев Андрей
*
*        Licensed under the Apache License, Version 2.0 (the "License");
*        you may not use this file except in compliance with the License.
*        You may obtain a copy of the License at
*
*            http://www.apache.org/licenses/LICENSE-2.0
*
*        Unless required by applicable law or agreed to in writing, software
*        distributed under the License is distributed on an "AS IS" BASIS,
*        WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*        See the License for the specific language governing permissions and
*        limitations under the License.
*/

using System;
using System.IO;

namespace drz.PdfVpMod.Servise
{
    /// <summary>
    /// утилиты для работы с файлами
    /// </summary>
    internal class UtilitesWorkFil
    {

        /// <summary>
        /// Копирует файл в назначенную директорию
        /// </summary>
        /// <param name="sFulPathFileCopiedFil">Полный путь копируемого файла</param>
        /// <param name="sPathDestination">Каталог куда копировать</param>
        /// <param name="sOut">Успех - Полный путь куда скопирован файл
        /// <br>Неудача - Описание ошибки</br>
        /// </param>
        /// <returns>Успех</returns>
        public static bool TryCopyTo(string sFulPathFileCopiedFil, string sPathDestination, out string sOut)
        {
            try
            {
                string sFileName = Path.GetFileName(sFulPathFileCopiedFil);
                string sPathFullDestination = Path.Combine(sPathDestination, sFileName);

                //если юзер подключает файл из папки лицензии
                //сравним пути
                if (string.Compare(sFulPathFileCopiedFil, sPathFullDestination, true) == 0)
                {//пути равны
                    sOut = sPathFullDestination;//вернем путь
                    return true;

                }

                File.Copy(sFulPathFileCopiedFil, sPathFullDestination, true);

                sOut = sPathFullDestination;
                return true;
            }
            catch (Exception ex)
            {
                sOut = ex.Message;
                sOut += "\n";
                sOut += ex.StackTrace;

                return false;
            }
        }

        /// <summary>
        /// По полному пути с расширением подбор уникального имени
        /// </summary>
        /// <param name="sFUlName">Полное имя путь+имя+расширение</param>
        /// <returns>Полный путь+уникальное имя+расширение</returns>
        public static string GetFileNameUniqu(string sFUlName)
        {            //путь
            string sPathTmp = Path.GetDirectoryName(sFUlName);
            //имя без расширения
            string sFilTmp = Path.GetFileNameWithoutExtension(sFUlName);
            //расширение
            string sFilExt = Path.GetExtension(sFUlName).Replace(".", "");
            //string sFilExt = sFilExtDot.Replace( ".", "");
            //новое уникальное имя
            // перегрузка для имени файла
            sFilTmp = GetFileNameUniqu(sPathTmp, sFilTmp, sFilExt);

            return Path.Combine(sPathTmp, sFilTmp + "." + sFilExt);
        }

        /// <summary> Подбор уникального имени файла для сохранения в файл</summary>
        /// <param name="sPlotPath">путь к файлу</param>
        /// <param name="sFilName">имя файла</param>
        /// <param name="sFilExt">расширение без точки!!!</param>
        /// <returns>Имя файла без расширения</returns>
        public static string GetFileNameUniqu(string sPlotPath, string sFilName, string sFilExt)
        {
            string filename_initial = Path.Combine(sPlotPath, sFilName + "." + sFilExt);
            // sPlotPath
            //sPlotFilName
            String filename_current = filename_initial;
            int count = 0;
            while (File.Exists(filename_current))
            {
                count++;
                filename_current = Path.GetDirectoryName(filename_initial)
                                 + Path.DirectorySeparatorChar
                                 + Path.GetFileNameWithoutExtension(filename_initial)
                                 + "("
                                 + count.ToString()
                                 + ")"
                                 + Path.GetExtension(filename_initial);
            }

            return Path.GetFileNameWithoutExtension(filename_current);
        }

        /// <summary>Получить список путей фалов в директории</summary>
        /// <param name="sPath">Директория с файлами</param>
        /// <param name="WithSubfolders">Учитывать поддиректории</param>
        /// <param name="sSerchPatern">Маска поиска</param>
        /// <returns>Пути к файлам</returns>
        static string[] GetFilesOfDir(string sPath, bool WithSubfolders, string sSerchPatern = "*.pdf")
        {
            try
            {
                return Directory.GetFiles(sPath,
                                            sSerchPatern,
                                            (WithSubfolders
                                            ? SearchOption.AllDirectories
                                            : SearchOption.TopDirectoryOnly));
            }
            catch
            {
                //Console.WriteLine(ex.Message);
                return new string[0];
            }
        }
    }

}
