/*
 * Copyright 2011-2012 Paul Heasley
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using phdesign.NppToolBucket.PluginCore;

namespace phdesign.NppToolBucket
{
    class Main : PluginBase
    {
        private enum CmdIndex
        {
            EXPORT_WITH_HEADERS_TO_INSERT,
            CSV_TO_JSON
        }

        #region Fields

        internal const string PluginName = "andreassjoberg";

        #endregion

        #region StartUp/CleanUp

        internal static void CommandMenuInit()
        {
            SetCommand((int)CmdIndex.EXPORT_WITH_HEADERS_TO_INSERT, "Export with headers -> Insert", Helpers.ExportWithHeadersToInsert);
            SetCommand((int)CmdIndex.CSV_TO_JSON, "CSV -> Json", Helpers.CsvToJson);
        }

        internal static void SetToolBarIcon()
        {
        }

        internal static void PluginCleanUp()
        {
        }

        #endregion
    }
}