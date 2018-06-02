using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EffectSome
{
    public partial class About : Form
    {
        public static bool IsOpen = false;
        public About()
        {
            IsOpen = true;
            InitializeComponent();
            textBox3.Lines = new string[]
                {
                    "Program Layout:\tAlFas",
                    "Program Code:\tAbsolute, AlFas and skyvlan",
                    "Pointers:\t\tAbsolute and AlFas",
                    "RE:\t\tAbsolute",
                    "Object Images:\tAbsolute and AlFas",
                    "Website Setup:\tAbsolute, AlFas and Cos8o",
                    "Website Host:\tAlterVista",
                };
            textBox4.Lines = new string[]
                {
                    "Beta Testers:",
                    "Creator Flake, albertalberto, Hydro0, Tygrysek, KplusH2O, G4lvatron",
                    "",
                    "Special Thanks:",
                    "- To skyvlan for his gamesave decryption code.",
                    "- To albertalberto and Psychomaniac14 for their ideas.",
                    "",
                    "Also special thanks to our friends and you for using the software."
                };
        }

        private void About_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
    }
}
