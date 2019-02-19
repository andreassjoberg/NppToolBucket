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
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace phdesign.NppToolBucket
{
    internal class Helpers
    {
        internal static void ExportWithHeadersToInsert()
        {
            var editor = Editor.GetActive();
            var text = editor.GetSelectedOrAllText();
            if (string.IsNullOrEmpty(text)) return;

            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return;

            var header = lines.First();
            var content = lines.Skip(1).ToList();

            var sb = new StringBuilder();

            var columns = header.Split('\t').Select(p => p.Trim()).ToList();
            foreach (var line in content)
            {
                var l = line.Split('\t').ToList();
                if (l.Count != columns.Count) return;

                sb.AppendLine($"insert into [Table] ({string.Join(", ", columns.Select(p => $"[{p}]"))}) values ({string.Join(", ", l.Select(p => $"'{p}'"))})");
            }

            var result = sb.ToString();

            editor.SetSelectedText(result);
        }
        internal static void CsvToJson()
        {
            var editor = Editor.GetActive();
            var text = editor.GetSelectedOrAllText();
            if (string.IsNullOrEmpty(text)) return;

            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 0) return;

            const char separator = '\t';
            var header = lines[0].Split(separator);
            if (header.Length <= 0) return;

            var columnTypes = new Type[header.Length];

            var sb = new StringBuilder("[\r\n");
            for (var i = 1; i < lines.Length; ++i)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var line = lines[i].Split(separator);
                if (line.Length != header.Length) break;

                var stringBuilder = new StringBuilder("\t{ ");
                for (var column = 0; column < line.Length; ++column)
                {
                    if (string.IsNullOrWhiteSpace(line[column]) ||
                        line[column].Equals("null", StringComparison.OrdinalIgnoreCase))
                    {
                        stringBuilder.Append($"\"{header[column].Trim()}\": null");
                    }
                    else if (decimal.TryParse(line[column], NumberStyles.Any, new NumberFormatInfo(), out _) &&
                             columnTypes[column] != typeof(string))
                    {
                        stringBuilder.Append($"\"{header[column].Trim()}\": {line[column].Trim()}");
                    }
                    else
                    {
                        columnTypes[column] = typeof(string);
                        stringBuilder.Append($"\"{header[column].Trim()}\": \"{line[column]}\"");
                    }

                    if (column + 1 < line.Length)
                    {
                        stringBuilder.Append(", ");
                    }
                }

                stringBuilder.AppendLine($@" }}{(i + 1 < lines.Length ? "," : "")}");
                sb.Append(stringBuilder);
            }

            sb.AppendLine("]");
            var result = sb.ToString();

            editor.SetSelectedText(result);
        }
    }
}