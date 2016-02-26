using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace beRemote.Core.Common.SimpleSettings {

    public class IniFile
    {
        private List<String> lines = new List<string>();

        private FileInfo _file = null;

        private String CommentCharacters = "#;";

        private String regCommentStr = "";

        private Regex regKey = null;

        private Regex regSection = null;

        private Dictionary<String, Dictionary<String,String>> _settingsCacheDictionary = new Dictionary<string, Dictionary<string, string>>();

        public IniFile()
        {
            
            regCommentStr = @"(\s*[" + CommentCharacters + "](?<comment>.*))?";
            regKey = new Regex(@"^[ \t]*(?<entry>([^=])+)=(?<value>([^=" + CommentCharacters + "])+)" + regCommentStr + "$");
            regSection = new Regex(@"^[ \t]*(\[(?<caption>([^\]])+)\]){1}" + regCommentStr + "$");
        }

        public IniFile(StreamReader stream)
            :this()
        {
            while (!stream.EndOfStream) lines.Add(stream.ReadLine().TrimEnd());
        }

        public IniFile(FileInfo fileinfo)
            : this()
        {
            if (!fileinfo.Exists) throw new IOException("File " + fileinfo.Name + "  not found");
            _file = fileinfo;
            using (StreamReader sr = new StreamReader(_file.FullName))
                while (!sr.EndOfStream) lines.Add(sr.ReadLine().TrimEnd());
        }

        public IniFile(String filename)
            : this(new FileInfo(filename))
        {

        }
            
        public Boolean Save()
        {
           //if (_file == null) return false;
           // try
           // {
           //     using (StreamWriter sw = new StreamWriter(_file.FullName))
           //         foreach (String line in lines)
           //             sw.WriteLine(line);
           // }
           // catch (IOException ex)
           // {
           //     throw new IOException("Fehler beim Schreiben der Datei " + fileName, ex);
           // }
           // catch
           // {
           //     throw new IOException("Fehler beim Schreiben der Datei " + fileName);
           // }
           // return true;
            return false;
        }
               
        public String fileName
        {
            get { return _file.Name; }
        }

        public FileInfo GetFilePath()
        {
            return _file;
        }

        public String GetDirectory()
        {
            return Path.GetDirectoryName(_file.FullName);
        }

      
        private int SearchCaptionLine(String section, Boolean CaseSensitive)
        {
            if (!CaseSensitive) section = section.ToLower();
            for (int i = 0; i < lines.Count; i++)
            {
                String line = lines[i].Trim();
                if (line == "") continue;
                if (!CaseSensitive) line = line.ToLower();
                // Erst den gewünschten Abschnitt suchen
                if (line == "[" + section + "]")
                    return i;
            }
            return -1;// Bereich nicht gefunden
        }

      
        private int SearchEntryLine(String section, String key, Boolean CaseSensitive)
        {
            section = section.ToLower();
            if (!CaseSensitive) key = key.ToLower();
            int CaptionStart = SearchCaptionLine(section, false);
            if (CaptionStart < 0) return -1;
            for (int i = CaptionStart + 1; i < lines.Count; i++)
            {
                String line = lines[i].Trim();
                if (line == "") continue;
                if (!CaseSensitive) line = line.ToLower();
                if (line.StartsWith("["))
                    return -1;// Ende, wenn der nächste Abschnitt beginnt
                if (Regex.IsMatch(line, @"^[ \t]*[" + CommentCharacters + "]"))
                    continue; // Kommentar
                if (line.StartsWith(key))
                    return i;// Eintrag gefunden
            }
            return -1;// Eintrag nicht gefunden
        }

        /// <summary>
        /// delets a value
        /// </summary>       
        public Boolean DeleteValue(String section, String key, Boolean CaseSensitive)
        {
            int line = SearchEntryLine(section, key, CaseSensitive);
            if (line < 0) return false;
            lines.RemoveAt(line);
            return true;
        }

        /// <summary>
        /// Gets a value
        /// </summary>       
        public String GetValue(String section, String key, Boolean CaseSensitive)
        {
            if (_settingsCacheDictionary.ContainsKey(section))
            {
                if (_settingsCacheDictionary[section].ContainsKey(key))
                {
                    return _settingsCacheDictionary[section][key];
                }
            }
            else
            {
                _settingsCacheDictionary.Add(section, new Dictionary<string, string>());
            }

            int line = SearchEntryLine(section, key, CaseSensitive);
            if (line < 0) return "";
            int pos = lines[line].IndexOf("=");
            if (pos < 0) return "";
            var result = lines[line].Substring(pos + 1).Trim();

            if (result.Contains("{") && result.Contains("}"))
            {
                // This may contains placeholder
                MatchCollection mc_full = Regex.Matches(result, "{.*?}.{.*?}");
                foreach (Match match in mc_full)
                {
                    MatchCollection mc = Regex.Matches(match.Value, "{.*?}");

                    // match have to be 2 values. otherwise we will ignore this
                    if (mc.Count == 2)
                    {
                        String m_section = mc[0].Value;
                        String m_key = mc[1].Value;

                        var m_result = GetValue(m_section.Replace("{", "").Replace("}", ""), m_key.Replace("{", "").Replace("}", ""), CaseSensitive);

                        result = result.Replace(match.Value, m_result);
                    }
                }
                
                
            }

            if (_settingsCacheDictionary[section].ContainsKey(key))
            {
                _settingsCacheDictionary[section][key] = result;
            }
            else
            {
                _settingsCacheDictionary[section].Add(key, result);
            }

            return result;
        }

        /// <summary>
        /// Gets a value (CaseSensitive!)
        /// </summary>       
        public String GetValue(String section, String key)
        {
            return GetValue(section, key, true);
        }

        /// <summary>
        /// Sets a key value pair even if the sectiond does not exist
        /// </summary>
        public Boolean SetValue(String section, String key, String value, Boolean CaseSensitive)
        {
            section = section.ToLower();
            if (!CaseSensitive) key = key.ToLower();
            int lastCommentedFound = -1;
            int CaptionStart = SearchCaptionLine(section, false);
            if (CaptionStart < 0)
            {
                lines.Add("[" + section + "]");
                lines.Add(key + "=" + value);
                return true;
            }
            int EntryLine = SearchEntryLine(section, key, CaseSensitive);
            for (int i = CaptionStart + 1; i < lines.Count; i++)
            {
                String line = lines[i].Trim();
                if (!CaseSensitive) line = line.ToLower();
                if (line == "") continue;
                // Ende, wenn der nächste Abschnitt beginnt
                if (line.StartsWith("["))
                {
                    lines.Insert(i, key + "=" + value);
                    return true;
                }
                // Suche aukommentierte, aber gesuchte Einträge
                // (evtl. per Parameter bestimmen können?), falls
                // der Eintrag noch nicht existiert.
                if (EntryLine < 0)
                    if (Regex.IsMatch(line, @"^[ \t]*[" + CommentCharacters + "]"))
                    {
                        String tmpLine = line.Substring(1).Trim();
                        if (tmpLine.StartsWith(key))
                        {
                            // Werte vergleichen, wenn gleich,
                            // nur Kommentarzeichen löschen
                            int pos = tmpLine.IndexOf("=");
                            if (pos > 0)
                            {
                                if (value == tmpLine.Substring(pos + 1).Trim())
                                {
                                    lines[i] = tmpLine;
                                    return true;
                                }
                            }
                            lastCommentedFound = i;
                        }
                        continue;// Kommentar
                    }
                if (line.StartsWith(key))
                {
                    lines[i] = key + "=" + value;
                    return true;
                }
            }
            if (lastCommentedFound > 0)
                lines.Insert(lastCommentedFound + 1, key + "=" + value);
            else
                lines.Insert(CaptionStart + 1, key + "=" + value);
            return true;
        }

        /// <summary>
        /// Returns all keys and values of a given section
        /// </summary>
        /// <param name="section">name of section</param>
        public SortedList<String, String> GetSection(String section)
        {
            SortedList<String, String> result = new SortedList<string, string>();
            Boolean CaptionFound = false;
            for (int i = 0; i < lines.Count; i++)
            {
                String line = lines[i].Trim();
                if (line == "") continue;
                // Erst den gewünschten Abschnitt suchen
                if (!CaptionFound)
                    if (line.ToLower() != "[" + section + "]") continue;
                    else
                    {
                        CaptionFound = true;
                        continue;
                    }
                // Ende, wenn der nächste Abschnitt beginnt
                if (line.StartsWith("[")) break;
                if (Regex.IsMatch(line, @"^[ \t]*[" + CommentCharacters + "]")) continue; // Kommentar
                int pos = line.IndexOf("=");
                if (pos < 0)
                    result.Add(line, "");
                else
                    result.Add(line.Substring(0, pos).Trim(), line.Substring(pos + 1).Trim());
            }
            return result;
        }

        /// <summary>
        /// Generates a list of all available Sections
        /// </summary>
        public List<string> GetAllSections()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                String line = lines[i];
                Match mCaption = regSection.Match(lines[i]);
                if (mCaption.Success)
                    result.Add(mCaption.Groups["caption"].Value.Trim());
            }
            return result;
        }

    }


}