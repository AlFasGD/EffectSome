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
    public partial class StatusBox : Form
    {
        public static bool IsOpen = false;

        int currentlySelectedObjects = 0;
        int[] currentlySelectedObjectIDs = new int[0];
        int buildObjectID = Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x248);
        int[] availableGroupIDs = new int[0];
        int[] availableColorIDs = new int[0];
        int[] availableItemIDs = new int[0];
        int[] availableBlockIDs = new int[0];

        public StatusBox()
        {
            IsOpen = true;
            InitializeComponent();
            label4.Text = currentlySelectedObjectIDs.ToString();
            label5.Text = currentlySelectedObjects.ToString();
            label6.Text = buildObjectID.ToString();
            if (buildObjectID < 0)
                label6.Text += " (Custom Object)";
        }
        
        private void StatusBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }

        private void TimeToUpdate_Tick(object sender, EventArgs e)
        {
            currentlySelectedObjects = 0;
            currentlySelectedObjectIDs = new int[0];
            buildObjectID = Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x248);

            label4.Text = GenerateText(currentlySelectedObjectIDs);
            label5.Text = currentlySelectedObjects.ToString();
            label6.Text = buildObjectID.ToString();
        }

        string GenerateText(int[] contents)
        {
            if (contents.Length > 0)
                return "None";
            else
            {
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < contents.Length; i++)
                    result.Append(contents[i] + ", ");
                return result.Remove(result.Length - 2, 2).ToString();
            }
        }
    }
}
