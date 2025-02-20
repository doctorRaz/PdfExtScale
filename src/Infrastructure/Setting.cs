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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

using drz.PdfVpMod.Enum;

namespace drz.PdfVpMod.Infrastructure
{
    //https://www.google.com/url?q=https://translated.turbopages.org/proxy_u/en-ru.ru.6eb8cb99-67b179cb-0385a42a-74722d776562/https/stackoverflow.com/questions/7385921/how-to-write-a-comment-to-an-xml-file-when-using-the-xmlserializer&source=gmail&ust=1739815037916000&usg=AOvVaw1ct3e-CIQ1wZEzPd6FIseL


    /// <summary>
    /// Настройки программы
    /// </summary>
    [Serializable]
    public class Setting
    {
        WinGraphicsUnit _unit = WinGraphicsUnit.Millimeter;

        /// <summary>
        /// Gets or sets the unit XML comment.
        /// </summary>
        /// <value>
        /// The unit XML comment.
        /// </value>
        [XmlAnyElement("UnitXmlComment")]
        public XmlComment UnitXmlComment { get { return GetType().GetXmlComment(); } set { } }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [XmlComment("<Unit> Опции единиц измерения видового экрана:\n" +
            "\t\t[Millimeter] - миллиметры (по умолчанию)\n" +
            "\t\t[Point] - точки/пойнты\n" +
            "\t\t[Inch] - дюймы\n" +
            "\t\t[Centimeter] - сантиметры\n" +
            "\t\t[Presentation] - единицы презентации (1/96 дюйма)")]
        public WinGraphicsUnit Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }

        /// <summary>
        /// Режим default
        /// </summary>
        ModeChangVp _mode = ModeChangVp.Add;

        /// <summary>
        /// Gets or sets the mode XML comment.
        /// </summary>
        /// <value>
        /// The mode XML comment.
        /// </value>
        [XmlAnyElement("ModeXmlComment")]
        public XmlComment ModeXmlComment { get { return GetType().GetXmlComment(); } set { } }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        [XmlComment("<Mode> Способ изменения видового экрана (VP):\n" +
            "\t\t[Add] - добавить VP (по умолчанию)\n" +
            "\t\t[Delete] - удалить VP\n" +
            "\t\t[AddOrModify] - изменить VP")]
        public ModeChangVp Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }

        /// <summary>
        /// The exit confirmation default
        /// </summary>
        bool _exitConfirmation = true;

        /// <summary>
        /// Gets or sets the exit confirmation XML comment.
        /// </summary>
        /// <value>
        /// The exit confirmation XML comment.
        /// </value>
        [XmlAnyElement("ExitConfirmationXmlComment")]
        public XmlComment ExitConfirmationXmlComment { get { return GetType().GetXmlComment(); } set { } }


        /// <summary>
        /// Gets or sets a value indicating whether [exit confirmation].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [exit confirmation]; otherwise, <c>false</c>.
        /// </value>
        [XmlComment("<ExitConfirmation> Выход из программы:\n" +
            "\t\t[true] - с подтверждением (по умолчанию)\n" +
            "\t\t[false] - без подтверждения, если нет ошибок")]
        public bool ExitConfirmation
        {
            get
            {
                return _exitConfirmation;
            }
            set
            {
                _exitConfirmation = value;
            }
        }

        /// <summary>
        /// The add backup default
        /// </summary>
        bool _addBak = true;

        /// <summary>
        /// Gets or sets the add backup XML comment.
        /// </summary>
        /// <value>
        /// The add backup XML comment.
        /// </value>
        [XmlAnyElement("AddBakXmlComment")]
        public XmlComment AddBakXmlComment { get { return GetType().GetXmlComment(); } set { } }

        /// <summary>
        /// Gets or sets a value indicating whether [add backup].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add backup]; otherwise, <c>false</c>.
        /// </value>
        [XmlComment("<AddBak> Резервные копии файлов PDF:\n" +
            "\t\t[true] - сохранять *.bak (по умолчанию)\n" +
            "\t\t[false] - не сохранять перезаписывать оригинальный файл")]
        public bool AddBak
        {
            get
            {
                return _addBak;
            }
            set
            {
                _addBak = value;
            }
        }

        //static Setting()
        //{

        //}

    }

    #region ExtensionXML   

    /// <summary>
    /// Xml Comment Attribute
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class XmlCommentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlCommentAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public XmlCommentAttribute(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }

    /// <summary>
    /// Xml Comment Extensions
    /// </summary>
    public static class XmlCommentExtensions
    {
        const string XmlCommentPropertyPostfix = "XmlComment";

        static XmlCommentAttribute GetXmlCommentAttribute(this Type type, string memberName)
        {
            var member = type.GetProperty(memberName);
            if (member == null)
                return null;
            var attr = member.GetCustomAttribute<XmlCommentAttribute>();
            return attr;
        }

        /// <summary>
        /// Gets the XML comment.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <returns></returns>
        public static XmlComment GetXmlComment(this Type type, [CallerMemberName] string memberName = "")
        {
            var attr = GetXmlCommentAttribute(type, memberName);
            if (attr == null)
            {
                if (memberName.EndsWith(XmlCommentPropertyPostfix))
                    attr = GetXmlCommentAttribute(type, memberName.Substring(0, memberName.Length - XmlCommentPropertyPostfix.Length));
            }
            if (attr == null || string.IsNullOrEmpty(attr.Value))
                return null;
            return new XmlDocument().CreateComment(attr.Value);
        }
    }
    #endregion
}
