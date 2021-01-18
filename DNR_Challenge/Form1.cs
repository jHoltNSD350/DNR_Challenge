using System;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DNR_Challenge
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Once clicked, the submit button takes the entered text and filters the DNA string
        /// 
        /// Once filtered, the string is formatted.
        /// The formatted string is displayed as well as the count of the specified pattern by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //Retrieve the entered DNA sequence and search sequence
            string corruptedText = txtEntry.Text;
            string searchSequence = txtSearch.Text.Trim().ToUpper();

            bool searching = searchSequence.Length > 0;

            //filter the search sequence
            if(searching && UncorruptSequence(searchSequence).Trim().Length != 4)
            {
                MessageBox.Show("This program is only designed to search for sets of 4\r\n" +
                    "valid nucleotide symbols at a time.\r\n\r\nTry again.");
                return;
            }

            //Filter and format the DNA sequence
            string cleanDNA = UncorruptSequence(corruptedText);

            StringBuilder sb = new StringBuilder();

            //First, display the filtered and formatted DNA sequence
            sb.AppendLine("The clean DNA sequence is: ");
            sb.AppendLine(Regex.Replace(cleanDNA, ".{30}", "$0\r\n"));
            sb.AppendLine();

            if (searching)
            {
                sb.Append("The occurances of sequence '" + searchSequence + "': ");

                /*
                 * The uncorrupted DNA sequence is here searched for a given sequence
                 * 
                 * ASSUMPTION: 
                 * the assumption is that only in a pregrouped sequence is the pattern
                 * to be counted. 
                 * 
                 * For example, 'ATCC TGCC' contains the given pattern of 'CCTG', but it is split
                 * between two sequences of four and therefore not counted. 
                 */
                int count = Regex.Matches(cleanDNA, searchSequence).Count;
                sb.AppendLine(count.ToString());
            }

            //The result is displayed in a message box
            MessageBox.Show(sb.ToString());
        }

        /// <summary>
        /// This method uncorrupts the DNA text sequence via regular expressions. 
        /// </summary>
        /// <param name="input">The possibly corrupted DNA text</param>
        /// <returns>The uncorrupted and reformatted version of the original DNA sequence</returns>
        private static string UncorruptSequence(string input)
        {
            //This statement removes all characters that are not A, C, G, or T(case insensitive).
            string uncorrupted = Regex.Replace(input, "[^aAcCgGtT]", "").ToUpper();

            //This statement then segments all of the text into sequences of 4 characters with a space following. 
            string reformatted = Regex.Replace(uncorrupted, ".{4}", "$0 ");

            return reformatted;
        }
    }
}
