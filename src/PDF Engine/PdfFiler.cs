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
using System.Collections.Generic;
using System.IO;

using drz.PdfVpMod.Enum;
using drz.PdfVpMod.Infrastructure;
using drz.PdfVpMod.Interfaces;
using drz.PdfVpMod.Servise;

using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace drz.PdfVpMod.PdfSharp
{
    /// <summary>
    /// Open Save PDF doc
    /// </summary>
    internal class PdfFiler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfFiler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PdfFiler(List<ILogger> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Opens the PDF.
        /// </summary>
        /// <param name="pdffile">The pdffile.</param>
        /// <returns></returns>
        public bool PdfOpen(string pdffile)
        {
            _pdfDoc = new PdfDocument();
            try
            {
                _pdfDoc = PdfReader.Open(pdffile, PdfDocumentOpenMode.Modify);
                logItem = new Logger($"Open: {pdffile}", MesagType.Info);
                Logger.Add(logItem);
                return true;
            }
            catch (Exception ex)
            {
                logItem = new Logger($"{ex.Message}: {pdffile}", MesagType.Error);
                Logger.Add(logItem);
                return false;
            }
        }

        /// <summary>
        /// Saves the PDF.
        /// </summary>
        /// <param name="pdfDoc">The PDF document.</param>
        /// <param name="addBak"></param>
        /// <returns></returns>
        public bool PdfSave(PdfDocument pdfDoc, bool addBak = true)
        {
            string sPDFfile = string.Empty;
            try
            {
                sPDFfile = pdfDoc.FullPath;

                if (addBak)//сохраняем копию
                {
                    //string sTempFile = Path.Combine(DataSetWpfOpt.sTemp, "temp.pdf");
                    string sTempFile = Path.Combine(Path.GetTempPath(), "temp.pdf");

                    //подбираем в темп уникальное имя
                    sTempFile = UtilitesWorkFil.GetFileNameUniqu(sTempFile);

                    //сохраняем в темп, если все Ок
                    pdfDoc.Save(sTempFile);

                    //подбираем сущ. файлу уникальное имя *.BAK
                    string sPDFfileBAK = UtilitesWorkFil.GetFileNameUniqu($"{sPDFfile}.BAK");

                    //переименовываем сущ. файл
                    File.Move(sPDFfile, sPDFfileBAK);

                    //перекидываем из темп новый файл на место существующего
                    File.Move(sTempFile, sPDFfile);
                }
                else//перезапись существующего
                {
                    //сразу сохраняем по месту
                    pdfDoc.Save(sPDFfile);
                }
                logItem = new Logger($"Файл сохранен: {sPDFfile}", MesagType.Ok);
                Logger.Add(logItem);
                return true;
            }
            catch (Exception ex)
            {
                logItem = new Logger($"{ex.Message} Не удалось сохранить: {sPDFfile}", MesagType.Error);
                Logger.Add(logItem);
                return false;
            }
        }

        /// <summary>
        /// Gets the PDF document.
        /// </summary>
        /// <value>
        /// The PDF document.
        /// </value>
        public PdfDocument PdfDoc => _pdfDoc;

        //public bool IsOpenedPdf => _isOpenedPdf;

        //public bool IsSavedPdf => _isSavedPdf;


        List<ILogger> Logger { get; set; }

        ILogger logItem;

        //bool _isOpenedPdf;
        PdfDocument _pdfDoc;
        //bool _isSavedPdf;



    }
}
