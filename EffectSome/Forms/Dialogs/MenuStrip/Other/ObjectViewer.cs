using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EffectSome
{
    public partial class ObjectViewer : Form
    {
        public static bool IsOpen = false;

        public ObjectViewer()
        {
            IsOpen = true;
            InitializeComponent();
        }

        private void ObjectViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
    }
}
