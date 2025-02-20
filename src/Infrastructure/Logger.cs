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
using System.Runtime.CompilerServices;

using drz.PdfVpMod.Enum;
using drz.PdfVpMod.Interfaces;

namespace drz.PdfVpMod.Infrastructure
{
    /// <summary>
    /// Сервис сохранения сообщений
    /// </summary>
    [Serializable]
    public class Logger : ILogger
    {
        /// <summary>
        /// Gets the name of the caller.
        /// </summary>
        /// <value>
        /// The name of the caller.
        /// </value>
        public string CallerName => _callerName;

        /// <summary>
        /// Gets the date time stamp.
        /// </summary>
        /// <value>
        /// The date time stamp.
        /// </value>
        public DateTime DateTimeStamp => _dateTimeStamp;

        /// <summary>
        /// Gets the messag.
        /// </summary>
        /// <value>
        /// The messag.
        /// </value>
        public string Messag => _messag;

        /// <summary>
        /// Gets the type of the mesag.
        /// </summary>
        /// <value>
        /// The type of the mesag.
        /// </value>
        public MesagType MesagType => _mesagType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="messag">The messag.</param>
        /// <param name="mesagType">Type of the mesag.</param>
        /// <param name="CallerName">Name of the caller.</param>
        public Logger(string messag,
                      MesagType mesagType = MesagType.None,
                      [CallerMemberName] string CallerName = null)
        {
            _dateTimeStamp = DateTime.Now;

            _callerName = CallerName;

            _messag = messag;

            _mesagType = mesagType;
        }



        DateTime _dateTimeStamp;
        string _callerName;
        string _messag;
        MesagType _mesagType;
    }
}
