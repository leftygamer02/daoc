using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.Language
{
    public class LanguageDataObject
    {
        public enum eTranslationIdentifier
            : byte
        {
            eArea = 0,
            eDoor = 1,
            eItem = 2,
            eNPC = 3,
            eObject = 4,
            eSystem = 5,
            eZone = 6
        }

        public LanguageDataObject() { }

        public eTranslationIdentifier TranslationIdentifier { get { return eTranslationIdentifier.eSystem; } }

        /// <summary>
        /// Gets or sets the translation id.
        /// </summary>
        public string TranslationId { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets costum data of / for the database row. Can be used to sort rows.
        /// </summary>
        public string Tag { get; set; }

        public string Text { get; set; }
    }
}
