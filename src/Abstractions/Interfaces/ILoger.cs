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

using drz.PdfVpMod.Enum;



namespace drz.PdfVpMod.Interfaces
{
    /// <summary>
    /// Logger
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the date time stamp.
        /// </summary>
        /// <value>
        /// The date time stamp.
        /// </value>
        DateTime DateTimeStamp { get; }

        /// <summary>
        /// Gets the name of the caller.
        /// </summary>
        /// <value>
        /// The name of the caller.
        /// </value>
        string CallerName { get; }

        /// <summary>
        /// Gets the type of the mesag.
        /// </summary>
        /// <value>
        /// The type of the mesag.
        /// </value>
        MesagType MesagType { get; }

        /// <summary>
        /// Gets the messag.
        /// </summary>
        /// <value>
        /// The messag.
        /// </value>
        string Messag { get; }

    }
}