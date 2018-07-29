using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.VendorPlugins.SpellHelper
{
    public class SpellLetter
    {
        public string Letter { get; set; } //Must be stirng, because some "letters" have more than just one sign
        public string Word { get; set; }
        public string Phonetic { get; set; }
    }
}
