using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class Voting
    {
        private string[] options = new string[0];

        public string[] Options
        {
            get
            {
                if (options.Length <= 0 && !String.IsNullOrEmpty(this.OptionStr))
                {
                    options = this.OptionStr.Split(";");
                }

                return options;
            }
        }

        public void AddOption(string choice)
        {
            if (string.IsNullOrEmpty(choice))
                return;

            string[] array = new string[options.Length + 1];
            options.CopyTo(array, 0);
            array[options.Length] = choice;
            options = array;

            OptionStr = string.Join(";", options);
        }

        public void AddDescription(string line)
        {
            if (String.IsNullOrEmpty(Description))
            {
                Description = line;
            }
            else
            {
                Description = Description + "\n" + line;
            }
        }

    }
}
