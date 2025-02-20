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


using System.Collections.Generic;

using drz.PdfVpMod.Enum;
using drz.PdfVpMod.Infrastructure;
using drz.PdfVpMod.Interfaces;

using PdfSharp.Pdf;

namespace drz.PdfVpMod.PdfSharp
{
    /// <summary>
    /// Движок
    /// </summary>
    public class PdfManager
    {
        #region INIT

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfManager" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sets"></param>
        public PdfManager(List<ILogger> logger,
                         Setting sets)
        {
            Logger = logger;
            _sets = sets;
        }
        #endregion

        /// <summary>
        /// PDFs обработка.
        /// </summary>
        /// <param name="pdfFiles">The PDF files.</param>
        public void PdfRun(List<string> pdfFiles)
        {
            //новый филер документов
            PdfFiler Filer = new PdfFiler(Logger);

            //новый конвертер
            PdfConversion Conv = new PdfConversion(Logger, Sets);

            //по списку файлов
            foreach (string pdffile in pdfFiles)
            {
                //получаем документ
                if (!Filer.PdfOpen(pdffile))//документа нет пропуск
                {
                    continue;
                }

                PdfDocument pdfDoc = Filer.PdfDoc;

                if (Conv.ConversionRun(Filer.PdfDoc))//режим только добавление VP
                {
                    Filer.PdfSave(pdfDoc, Sets.AddBak);//сохраняем                   
                }
                else//ни один VP не добавлен, сохранять не надо
                {
                    Logger.Add(new Logger($"Изменений нет. Файл не сохранен: {pdffile}", MesagType.Idle));
                }
            }
        }

        #region Environ        

        Setting _sets;

        Setting Sets => _sets;

        List<ILogger> Logger;


        //ModeChangVp ChangeVpPage => Sets.Mode;

        //XGraphicsUnit _convertUnit;
        //XGraphicsUnit ConvertUnit => _convertUnit;

        //WinGraphicsUnit WinConvertUnit
        //{
        //    set
        //    {
        //        switch (value)
        //        {
        //            case WinGraphicsUnit.Centimeter: _convertUnit = XGraphicsUnit.Centimeter; break;
        //            case WinGraphicsUnit.Inch: _convertUnit = XGraphicsUnit.Inch; break;
        //            case WinGraphicsUnit.Millimeter: _convertUnit = XGraphicsUnit.Millimeter; break;
        //            case WinGraphicsUnit.Point: _convertUnit = XGraphicsUnit.Point; break;
        //            case WinGraphicsUnit.Presentation: _convertUnit = XGraphicsUnit.Presentation; break;
        //            default: throw new InvalidEnumArgumentException();
        //        }
        //    }
        //}

        #endregion
    }
}


