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
    public partial class ObjectsSelectionMenu : Form
    {
        public static bool IsOpen = false;
        enum Parameters
        {
            ObjectID,
            GroupID,
            MainColorID,
            DetailColorID,
            ZOrder,
            ZLayer,
            EL1,
            EL2,
            DontEnterFadeGroupParentHighDetail
        }
        public ObjectsSelectionMenu()
        {
            IsOpen = true;
            InitializeComponent();
            // button4.Enabled = ;
            // button5.Enabled = ;
            // button7.Enabled = ;
            // button10.Enabled = ;
            // button16.Enabled = ;
            // button19.Enabled = ;
            // button22.Enabled = ;
            // button25.Enabled = ;
        }

        #region Basic functions
        private void ObjectsSelectionMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox2);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            AddItem(listBox1, numericUpDown1.Value);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            AddItem(listBox2, numericUpDown2.Value);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.ObjectID);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.GroupID);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox1);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.MainColorID);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox3);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            AddItem(listBox3, numericUpDown3.Value);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.DetailColorID);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox4);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            AddItem(listBox4, numericUpDown4.Value);
        }
        private void button13_Click(object sender, EventArgs e)
        {

        }
        private void button14_Click(object sender, EventArgs e)
        {

        }
        private void button15_Click(object sender, EventArgs e)
        {

        }
        private void button16_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.ZOrder);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox6);
        }
        private void button18_Click(object sender, EventArgs e)
        {
            AddItem(listBox6, numericUpDown4.Value);
        }
        private void button19_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.ZLayer);
        }
        private void button20_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox7);
        }
        private void button21_Click(object sender, EventArgs e)
        {
            AddItem(listBox7, numericUpDown7.Value);
        }
        private void button22_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.EL1);
        }
        private void button23_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox8);
        }
        private void button24_Click(object sender, EventArgs e)
        {
            AddItem(listBox8, numericUpDown8.Value);
        }
        private void button25_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.EL2);
        }
        private void button26_Click(object sender, EventArgs e)
        {
            RemoveItems(listBox9);
        }
        private void button27_Click(object sender, EventArgs e)
        {
            AddItem(listBox9, numericUpDown9.Value);
        }
        private void button28_Click(object sender, EventArgs e)
        {
            // Apply filters
        }
        private void button29_Click(object sender, EventArgs e)
        {
            // Select now!

        }
        private void button30_Click(object sender, EventArgs e)
        {
            // Deselect objects that do not meet the filtering criteria
        }
        private void button31_Click(object sender, EventArgs e)
        {
            // Select all objects of the level!
        }
        private void button32_Click(object sender, EventArgs e)
        {
            AddParametersFromSelectedObjects(Parameters.DontEnterFadeGroupParentHighDetail);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked && listBox2.SelectedItems.Count != 0;
            numericUpDown2.Enabled = checkBox1.Checked;
            button3.Enabled = checkBox1.Checked;
            listBox2.Enabled = checkBox1.Checked;
            // button5.Enabled = ;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button6.Enabled = checkBox2.Checked && listBox1.SelectedItems.Count != 0;
            numericUpDown1.Enabled = checkBox2.Checked;
            button2.Enabled = checkBox2.Checked;
            listBox1.Enabled = checkBox2.Checked;
            // button4.Enabled = ;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            button8.Enabled = checkBox3.Checked && listBox3.SelectedItems.Count != 0;
            numericUpDown3.Enabled = checkBox3.Checked;
            button9.Enabled = checkBox3.Checked;
            listBox3.Enabled = checkBox3.Checked;
            // button7.Enabled = ;
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            button11.Enabled = checkBox4.Checked && listBox4.SelectedItems.Count != 0;
            numericUpDown4.Enabled = checkBox4.Checked;
            button12.Enabled = checkBox4.Checked;
            listBox4.Enabled = checkBox4.Checked;
            // button10.Enabled = ;
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown5.Enabled = checkBox7.Checked;
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            button17.Enabled = checkBox8.Checked && listBox6.SelectedItems.Count != 0;
            numericUpDown6.Enabled = checkBox8.Checked;
            button18.Enabled = checkBox8.Checked;
            listBox6.Enabled = checkBox8.Checked;
            // button16.Enabled = ;
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            button20.Enabled = checkBox9.Checked && listBox7.SelectedItems.Count != 0;
            numericUpDown7.Enabled = checkBox9.Checked;
            button21.Enabled = checkBox9.Checked;
            listBox7.Enabled = checkBox9.Checked;
            // button19.Enabled = ;
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            button23.Enabled = checkBox10.Checked && listBox8.SelectedItems.Count != 0;
            numericUpDown8.Enabled = checkBox10.Checked;
            button24.Enabled = checkBox10.Checked;
            listBox8.Enabled = checkBox10.Checked;
            // button22.Enabled = ;
        }
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            button26.Enabled = checkBox11.Checked && listBox9.SelectedItems.Count != 0;
            numericUpDown9.Enabled = checkBox11.Checked;
            button27.Enabled = checkBox11.Checked;
            listBox9.Enabled = checkBox11.Checked;
            // button25.Enabled = ;
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            checkBox16.Enabled = checkBox12.Checked;
        }
        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            checkBox17.Enabled = checkBox12.Checked;
        }
        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            checkBox19.Enabled = checkBox12.Checked;
        }
        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            checkBox18.Enabled = checkBox12.Checked;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button6.Enabled = listBox1.SelectedItems.Count != 0;
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = listBox2.SelectedItems.Count != 0;
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button8.Enabled = listBox3.SelectedItems.Count != 0;
        }
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            button11.Enabled = listBox4.SelectedItems.Count != 0;
        }
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            button14.Enabled = listBox5.SelectedItems.Count != 0;
        }
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            button17.Enabled = listBox6.SelectedItems.Count != 0;
        }
        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            button20.Enabled = listBox7.SelectedItems.Count != 0;
        }
        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            button23.Enabled = listBox8.SelectedItems.Count != 0;
        }
        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            button26.Enabled = listBox9.SelectedItems.Count != 0;
        }
        #endregion

        public void CloseForm()
        {
            Close();
        }
        void AddItem(ListBox listBox, decimal item)
        {
            if (item != 0)
            {
                if (listBox.Items.Contains(item) == false)
                {
                    if (listBox.Items.Count != 0)
                    {
                        if (item < (decimal)listBox.Items[listBox.Items.Count - 1])
                        {
                            for (int i = 0; i < listBox.Items.Count; i++)
                                if (item < (decimal)listBox.Items[i])
                                {
                                    listBox.Items.Insert(i, item);
                                    break;
                                }
                        }
                        else
                            listBox.Items.Add(item);
                    }
                    else
                        listBox.Items.Add(item);
                }
            }
        }
        void RemoveItems(ListBox listBox)
        {
            while (listBox.SelectedItems.Count != 0)
                listBox.Items.Remove(listBox.SelectedItems[listBox.SelectedItems.Count - 1]);
        }
        void AddParametersFromSelectedObjects(Parameters param)
        {
            try
            {
                if (param == Parameters.DetailColorID)
                    AddItem(listBox4, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x354, 0xe8));
                else if (param == Parameters.DontEnterFadeGroupParentHighDetail)
                {
                    checkBox16.Checked = Memory.GetBoolFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x31b);
                    checkBox17.Checked = Memory.GetBoolFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x31b);
                    checkBox18.Checked = Memory.GetBoolFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x31b);
                    checkBox19.Checked = Memory.GetBoolFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x31b);
                }
                else if (param == Parameters.EL1)
                    AddItem(listBox8, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x3a4));
                else if (param == Parameters.EL2)
                    AddItem(listBox9, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x3a8));
                else if (param == Parameters.GroupID)
                    for (int i = 0; i < 10; i++)
                        AddItem(listBox2, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x39c, 0x4 * i));
                else if (param == Parameters.MainColorID)
                    AddItem(listBox3, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x350, 0xe8));
                else if (param == Parameters.ObjectID)
                    AddItem(listBox1, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x310));
                else if (param == Parameters.ZLayer)
                    AddItem(listBox7, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0x360));
                else if (param == Parameters.ZOrder)
                    AddItem(listBox6, Memory.GetIntFromPointers(EffectSome.baseAddress, 0x150, 0x208, 0x284, 0xa4));
            }
            catch (NullReferenceException)
            {

            }
        }
    }
}
