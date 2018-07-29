using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using beRemote.VendorPlugins.SpellHelper.UI.Commands;

namespace beRemote.VendorPlugins.SpellHelper.UI
{
    public class ViewModel : INotifyPropertyChanged
    {
        //Key = Langauge; Value = The Letter
        private Dictionary<string, List<SpellLetter>> _Dictionaries = new Dictionary<string, List<SpellLetter>>();
        private List<SpellLetter> _SelectedLanguage; //The selected Language
        private KeyValuePair<string, List<SpellLetter>> _SelectedLanguageItem; //The selected Languageitem

        public CmdLoadedImpl CmdLoaded {get;set;}

        #region Constructor
        public ViewModel()
        {
            CmdLoaded = new CmdLoadedImpl();
            CmdLoaded.Loaded += CmdLoaded_Loaded;
        }

        #endregion
        
        void CmdLoaded_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDictionary();
            
            //Set the first Language as default language
            if (Dictionaries.Count > 0)
                SelectedLanguageItem = Dictionaries.First();
        }

        private void LoadDictionary()
        {
            var listReader = new System.IO.StreamReader("plugins\\ui\\beRemote.VendorPlugins.SpellHelper\\spelling\\list.ini", Encoding.Default);
            while (listReader.Peek() >= 0)
            {
                //Read the CSValue (i.e. International (ICAO);icao.spell)
                var line = listReader.ReadLine().Split(';');

                //Read the Spellfile
                var spellReader = new System.IO.StreamReader("plugins\\ui\\beRemote.VendorPlugins.SpellHelper\\spelling\\" + line[1], Encoding.Default);
                while (spellReader.Peek() >= 0)
                {
                    var spellLine = spellReader.ReadLine().Split(';');
                    var aLetter = new SpellLetter();
                    aLetter.Letter = spellLine[0];
                    aLetter.Word = spellLine[1];
                    if (spellLine.Length > 2) //if there is a Phonetic
                        aLetter.Phonetic = spellLine[2];

                    //Add Language if it not exists in the Dictionary
                    if (!_Dictionaries.ContainsKey(line[0]))
                        _Dictionaries.Add(line[0], new List<SpellLetter>());

                    //Add the letter
                    _Dictionaries[line[0]].Add(aLetter);
                }
                spellReader.Close();
            }
            listReader.Close();

            RaisePropertyChanged("Dictionaries");
        }

        #region Properties
        /// <summary>
        /// All declared Dictionaries
        /// </summary>
        public Dictionary<string, List<SpellLetter>> Dictionaries
        {
            get { return _Dictionaries; }
            set 
            {
                _Dictionaries = value;
                RaisePropertyChanged("Dictionaries");
            }
        }

        /// <summary>
        /// The currently selected Language
        /// </summary>
        public List<SpellLetter> SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                _SelectedLanguage = value;
                RaisePropertyChanged("SelectedLanguage");
            }
        }

        /// <summary>
        /// The currently selected Languageitem
        /// </summary>
        public KeyValuePair<string, List<SpellLetter>> SelectedLanguageItem
        {
            get { return _SelectedLanguageItem; }
            set
            {
                _SelectedLanguageItem = value;
                RaisePropertyChanged("SelectedLanguageItem");
            }
        }
        #endregion
        
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion

        public void Dispose()
        {
            CmdLoaded.Loaded -= CmdLoaded_Loaded;

            _Dictionaries.Clear();
            _SelectedLanguage.Clear();
        }
    }
}
