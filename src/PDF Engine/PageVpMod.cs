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
using System.Linq;

using drz.PdfVpMod.Enum;
using drz.PdfVpMod.Infrastructure;
using drz.PdfVpMod.Interfaces;

using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace drz.PdfVpMod.PdfSharp
{


    /// <summary>
    /// Добавляет удаляет VP с масштабом вида
    /// </summary>
    internal class PageVpMod
    {
        List<ILogger> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageVpMod"/> class.
        /// </summary>
        public PageVpMod(List<ILogger> logger)
        {
            Logger = logger;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="PageVpMod"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="_pageNum"></param>
        /// <param name="convertUnit">The convert unit.</param>
        /// <param name="isMod">true - Add or Modify<br>false - only Add</br> </param>           
        public bool PageVpModAdd(PdfPage page,
                         int _pageNum,
                        XGraphicsUnit convertUnit = XGraphicsUnit.Millimeter,
                        bool isMod = false)
        {
            pageNum = _pageNum;
            _page = page;

            XUnit unit = new XUnit(1, convertUnit);

            _scalefactor = 1 / unit.Point;//пересчет единиц в точку

            try
            {
                #region Check VP Page
                _arrVP = Page.Elements.GetObject("/VP") as PdfArray;

                if (ArrVP == null)//если VP нет
                {
                    #region VP NEW
                    _arrVP = new PdfArray();
                    Page.Elements.Add("/VP", ArrVP);//добавить VP
                    #endregion

                    if (AddArrVP())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (isMod)//заказано изменить VP scale factor
                {
                    if (ModArrVP())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Logger.Add(new Logger($"\tVP существует в Page:{pageNum}", MesagType.Idle));
                    return false;
                }
                #endregion 
            }
            catch (Exception ex)
            {
                Logger.Add(new Logger($"\tVP сбой в Page:{pageNum} {ex.Message}", MesagType.Error));
                return false;
            }
        }

        /// <summary>
        /// Modifies the scale VP.
        /// </summary>
        /// <returns>Успех</returns>
        bool ModArrVP()
        {
            bool isMod = false;
            try
            {
                PdfDictionary dicMeasure = null;
                foreach (PdfDictionary item in ArrVP.Elements.Cast<PdfDictionary>())
                {
                    dicMeasure = item.Elements.GetObject("/Measure") as PdfDictionary;
                    if (dicMeasure is PdfDictionary)
                    {
                        break;
                    }
                }

                PdfArray arrX = dicMeasure.Elements.GetValue("/X") as PdfArray;

                foreach (PdfDictionary item in arrX.Elements.Cast<PdfDictionary>())
                {
                    PdfReal ConversionFactor = item.Elements.GetValue("/C") as PdfReal;
                    if (ConversionFactor is PdfReal)
                    {
                        var dd = ConversionFactor.Value;
                        var dr = Math.Round(dd, 5);
                        var sr = Math.Round(Scalefactor, 5);
                        if (dr != sr)
                        {
                            item.Elements.SetValue("/C", new PdfReal(Scalefactor));
                            isMod = true;
                            Logger.Add(new Logger($"\tVP изменен в Page:{pageNum}", MesagType.Ok));
                            break;
                        }
                        else
                        {
                            isMod = false;
                            Logger.Add(new Logger($"\tVP не изменен в Page:{pageNum}", MesagType.Idle));
                            break;
                        }
                    }
                }
                return isMod;
            }
            catch (Exception ex)
            {
                Logger.Add(new Logger($"\tVP сбой при изменении Page:{pageNum} {ex.Message}", MesagType.Error));
                return false;
            }
        }

        /// <summary>
        /// Adds the scale vp.
        /// </summary>
        /// <returns>Успех</returns>
        bool AddArrVP()
        {
            try
            {
                #region VP

                PdfDictionary dicVPitem = new PdfDictionary();
                ArrVP.Elements.Add(dicVPitem);

                #region BBox

                PdfArray dicBBox = new PdfArray();
                dicVPitem.Elements.Add("/BBox", dicBBox);

                dicBBox.Elements.Add(new PdfInteger((int)Page.MediaBox.X1));
                dicBBox.Elements.Add(new PdfInteger((int)Page.MediaBox.Y1));
                dicBBox.Elements.Add(new PdfInteger((int)Page.MediaBox.X2));
                dicBBox.Elements.Add(new PdfInteger((int)Page.MediaBox.Y2));

                #endregion

                #region Measure

                PdfDictionary dicMeasure = new PdfDictionary();
                dicVPitem.Elements.Add("/Measure", dicMeasure);

                #region /A
                PdfArray arrA = new PdfArray();
                dicMeasure.Elements.Add("/A", arrA);

                PdfDictionary arrAitem = new PdfDictionary();
                arrA.Elements.Add(arrAitem);

                arrAitem.Elements.Add("/C", new PdfInteger(1));
                arrAitem.Elements.Add("/U", new PdfString(" "));
                #endregion

                #region /D
                PdfArray arrD = new PdfArray();
                dicMeasure.Elements.Add("/D", arrD);

                PdfDictionary arrDitem = new PdfDictionary();
                arrD.Elements.Add(arrDitem);

                arrDitem.Elements.Add("/C", new PdfInteger(1));
                arrDitem.Elements.Add("/U", new PdfString(" "));

                #endregion

                #region /R
                PdfString strR = new PdfString(" ");
                dicMeasure.Elements.Add("/R", strR);

                #endregion

                #region /Subtype
                PdfName nameSubtype = new PdfName("/RL");
                dicMeasure.Elements.Add("/Subtype", nameSubtype);

                #endregion

                #region  /Type
                PdfName nameType = new PdfName("/Measure");
                dicMeasure.Elements.Add("/Type", nameType);

                #endregion

                #region  /X
                PdfArray arrX = new PdfArray();
                dicMeasure.Elements.Add("/X", arrX);

                PdfDictionary dicXitem = new PdfDictionary();
                arrX.Elements.Add(dicXitem);

                dicXitem.Elements.Add("/C", new PdfReal(Scalefactor));//.35278
                dicXitem.Elements.Add("/U", new PdfString(" "));

                #endregion

                #endregion

                #region  Type
                PdfName dicType = new PdfName("/Viewport");
                dicVPitem.Elements.Add("/Type", dicType);
                #endregion

                #endregion
                Logger.Add(new Logger($"\tVP добавлен в Page:{pageNum}", MesagType.Ok));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Add(new Logger($"\tVP сбой при добавлении Page:{pageNum} {ex.Message}", MesagType.Error));
                return false;
            }
        }

        /// <summary>
        /// Pages the VP mod delete.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageNum"></param>
        /// <returns>Успех</returns>
        public bool PageVpModDel(PdfPage page, int pageNum)
        {
            _page = page;
            _arrVP = Page.Elements.GetObject("/VP") as PdfArray;

            if (ArrVP != null)//VP yes
            {
                try
                {
                    Page.Elements.Remove("/VP");

                    Logger.Add(new Logger($"\tVP удален из Page:{pageNum}", MesagType.Ok));
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Add(new Logger($"\tVP сбой удаления в Page:{pageNum} {ex.Message}", MesagType.Error));
                    return false;
                }
            }
            else
            {
                Logger.Add(new Logger($"\tVP отсутствует в Page:{pageNum}", MesagType.Idle));
                return false;
            }

        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public PdfPage Page => _page;

        /// <summary>
        /// Gets the arr view port.
        /// </summary>
        /// <value>
        /// The arr view port.
        /// </value>
        public PdfArray ArrVP => _arrVP;

        /// <summary>
        /// Gets the scale factor.
        /// </summary>
        /// <value>
        /// The scale factor.
        /// </value>
        public double Scalefactor => _scalefactor;

        int pageNum;

        PdfPage _page;
        PdfArray _arrVP;


        double _scalefactor;

    }


}
