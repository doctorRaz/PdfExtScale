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
using System.ComponentModel;

using drz.PdfVpMod.Enum;
using drz.PdfVpMod.Infrastructure;
using drz.PdfVpMod.Interfaces;

using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace drz.PdfVpMod.PdfSharp
{
    internal class PdfConversion
    {
        #region INIT       

        /// <summary>
        /// Настройка конвертора VP
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sets"></param>
        public PdfConversion(List<ILogger> logger, Setting sets)
        {
            Logger = logger;
            _sets = sets;
            WinConvertUnit = Sets.Unit;
            //ChangeVpPage = sets.Mode;// changeVpPage;
            _isModified = false;
        }
        #endregion

        /// <summary>
        /// Обработка документа.
        /// </summary>
        /// <param name="PdfDoc">PDF document.</param>
        /// <returns>
        /// успех
        /// </returns>
        public bool ConversionRun(PdfDocument PdfDoc)
        {
            _isModified = false;

            int pageNum = 1;//номер стр для логгера

            PageVpMod PVM = new PageVpMod(Logger); 
            //перебор страниц
            foreach (PdfPage page in PdfDoc.Pages)
            {
                if (ChangeVpPage == ModeChangVp.Delete)//Delete VP
                {
                    if (PVM.PageVpModDel(page, pageNum))
                    {
                        _isModified = true;//если меняли хоть один лист
                    }
                    else
                    {
                        //_isModified = false;
                    }
                }
                else
                {
                    bool isMod = false;
                    if (ChangeVpPage == ModeChangVp.AddOrModify)
                    {
                        isMod = true;
                    }

                    bool IsChanged = PVM.PageVpModAdd(page, pageNum, ConvertUnit, isMod);

                    if (IsChanged)//Page chang
                    {
                        _isModified = true;//если меняли хоть один лист
                    }
                    //else//Page not chang
                    //{
                    //    //_isModified = false;
                    //}
                }
                //Logger.Add(_logItem);
                ++pageNum;
            }
            if (IsModified)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #region ENVIRON

        Setting _sets;

        Setting Sets => _sets;


        #region Servise

        //ILogger _logItem;

        /// <summary>
        /// The logger
        /// </summary>
        public List<ILogger> Logger { get; set; }

        //ModeChangVp ChangeVpPage;

        #endregion

        #region VP


        ModeChangVp ChangeVpPage => Sets.Mode;

        XGraphicsUnit _convertUnit;
        XGraphicsUnit ConvertUnit => _convertUnit;

        WinGraphicsUnit WinConvertUnit
        {
            set
            {
                switch (value)
                {
                    case WinGraphicsUnit.Centimeter: _convertUnit = XGraphicsUnit.Centimeter; break;
                    case WinGraphicsUnit.Inch: _convertUnit = XGraphicsUnit.Inch; break;
                    case WinGraphicsUnit.Millimeter: _convertUnit = XGraphicsUnit.Millimeter; break;
                    case WinGraphicsUnit.Point: _convertUnit = XGraphicsUnit.Point; break;
                    case WinGraphicsUnit.Presentation: _convertUnit = XGraphicsUnit.Presentation; break;
                    default: throw new InvalidEnumArgumentException();
                }
            }
        }




        #endregion
        #region PDF

        //PdfPage _page;
        //PdfPage Page => _page;

        //PdfDocument _pdfDoc;


        //public PdfDocument PdfDoc => _pdfDoc;

        #endregion

        /// <summary>
        /// The is PDF modified
        /// </summary>
        public bool IsModified => _isModified;

        bool _isModified;

        #endregion

    }

}
