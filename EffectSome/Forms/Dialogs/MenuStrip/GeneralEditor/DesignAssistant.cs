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
    public partial class DesignAssistant : Form
    {
        public static int page = 0;
        public static int selectedObjectIndex = 0;
        public static bool isSelectedObjectToFind = false;
        public static List<LevelObject[]> objectsToFind = new List<LevelObject[]>();
        public static List<LevelObject[]> objectsToReplaceWith = new List<LevelObject[]>();
        public static List<bool> emptyPages = new List<bool>();

        public DesignAssistant()
        {
            InitializeComponent();
            objectsToFind.Add(new LevelObject[15]);
            objectsToReplaceWith.Add(new LevelObject[15]);
            emptyPages.Add(true);
        }

        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            page++;
            if (page > objectsToFind.Count)
            {
                objectsToFind.Add(new LevelObject[15]);
                objectsToReplaceWith.Add(new LevelObject[15]);
                emptyPages.Add(true);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (page > 0)
            {
                page--;
                if (emptyPages[page + 1])
                {
                    objectsToFind.RemoveAt(page + 1);
                    objectsToReplaceWith.RemoveAt(page + 1);
                    emptyPages.RemoveAt(page + 1);
                }
            }
            button2.Enabled = page > 0;
        }
        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            groupIDs.Items.Remove(numericUpDown47.Value);
            int[] groups = new int[groupIDs.Items.Count];
            for (int i = 0; i < groups.Length; i++)
                groups[i] = (int)groupIDs.Items[i];
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupIDs] = groups;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupIDs] = groups;
        }
        private void button6_Click(object sender, EventArgs e)
        {

        }
        private void button10_Click(object sender, EventArgs e)
        {

        }
        private void button11_Click(object sender, EventArgs e)
        {

        }
        private void button12_Click(object sender, EventArgs e)
        {
            AddItem(groupIDs, numericUpDown47.Value);
            int[] groups = new int[groupIDs.Items.Count];
            for (int i = 0; i < groups.Length; i++)
                groups[i] = (int)groupIDs.Items[i];
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupIDs] = groups;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupIDs] = groups;
        }
        private void button24_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region RadioButtons
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 0;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 0;
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = radioButton4.Checked;
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = numericUpDown4.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = numericUpDown4.Value;
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 9;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 9;
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 7;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 7;
        }
        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = -3;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = -3;
        }
        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = -1;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = -1;
        }
        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 1;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 1;
        }
        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 3;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 3;
        }
        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 5;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = 5;
        }
        #endregion
        #region CheckBoxes
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = checkBox1.Checked;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown7.Value, (double)numericUpDown5.Value, (double)numericUpDown3.Value, checkBox3.Checked, checkBox2.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown7.Value, (double)numericUpDown5.Value, (double)numericUpDown3.Value, checkBox3.Checked, checkBox2.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown8.Enabled = checkBox4.Checked;
        }
        private void checkBox92_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.DontFade] = checkBox92.Checked;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.DontFade] = checkBox92.Checked;
        }
        private void checkBox93_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupParent] = checkBox93.Checked;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupParent] = checkBox93.Checked;
        }
        private void checkBox192_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.DisableGlow] = !checkBox192.Checked;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.DisableGlow] = !checkBox192.Checked;
        }
        private void checkBox193_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.DontEnter] = checkBox193.Checked;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.DontEnter] = checkBox193.Checked;
        }
        private void checkBox194_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.HighDetail] = checkBox194.Checked;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.HighDetail] = checkBox194.Checked;
        }
        private void checkBox216_CheckedChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown138.Value, (double)numericUpDown137.Value, (double)numericUpDown136.Value, checkBox217.Checked, checkBox216.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
        }
        private void checkBox217_CheckedChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown138.Value, (double)numericUpDown137.Value, (double)numericUpDown136.Value, checkBox217.Checked, checkBox216.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
        }
        #endregion
        #region NumericUpDowns
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2] = (int)numericUpDown2.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2] = (int)numericUpDown2.Value;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown7.Value, (double)numericUpDown5.Value, (double)numericUpDown3.Value, checkBox3.Checked, checkBox2.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = (int)numericUpDown4.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer] = (int)numericUpDown4.Value;
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown7.Value, (double)numericUpDown5.Value, (double)numericUpDown3.Value, checkBox3.Checked, checkBox2.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Scaling] = (double)numericUpDown6.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Scaling] = (double)numericUpDown6.Value;
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown7.Value, (double)numericUpDown5.Value, (double)numericUpDown3.Value, checkBox3.Checked, checkBox2.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] = HSVValues;
        }
        private void numericUpDown48_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZOrder] = (int)numericUpDown48.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZOrder] = (int)numericUpDown48.Value;
        }
        private void numericUpDown49_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.EL1] = (double)numericUpDown49.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.EL1] = (double)numericUpDown49.Value;
        }
        private void numericUpDown50_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.EL2] = (double)numericUpDown50.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.EL2] = (double)numericUpDown50.Value;
        }
        private void numericUpDown51_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1] = (int)numericUpDown51.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1] = (int)numericUpDown51.Value;
        }
        private void numericUpDown135_ValueChanged(object sender, EventArgs e)
        {
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Rotation] = (double)numericUpDown135.Value;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Rotation] = (double)numericUpDown135.Value;
        }
        private void numericUpDown136_ValueChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown138.Value, (double)numericUpDown137.Value, (double)numericUpDown136.Value, checkBox217.Checked, checkBox216.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
        }
        private void numericUpDown137_ValueChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown138.Value, (double)numericUpDown137.Value, (double)numericUpDown136.Value, checkBox217.Checked, checkBox216.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
        }
        private void numericUpDown138_ValueChanged(object sender, EventArgs e)
        {
            object[] HSVValues = { (int)numericUpDown138.Value, (double)numericUpDown137.Value, (double)numericUpDown136.Value, checkBox217.Checked, checkBox216.Checked };
            if (isSelectedObjectToFind) objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
            else objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] = HSVValues;
        }
        #endregion
        #region PictureBoxes
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 0;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 0;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 1;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 1;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 2;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 2;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 3;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 3;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 4;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 4;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 5;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox12_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 5;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 6;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 6;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox15_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 7;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox16_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 7;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox17_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 8;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox18_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 8;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox19_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 9;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox20_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 9;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox21_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 10;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox22_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 10;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox23_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 11;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox24_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 11;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox25_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 12;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox26_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 12;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox27_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 13;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox28_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 13;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox29_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 14;
            isSelectedObjectToFind = true;
            ShowSelectedObjectParameters();
        }
        private void pictureBox30_Click(object sender, EventArgs e)
        {
            selectedObjectIndex = 14;
            isSelectedObjectToFind = false;
            ShowSelectedObjectParameters();
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox5_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox7_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox8_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox9_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox10_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox11_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox12_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox13_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox14_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox15_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox16_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox17_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox18_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox19_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox20_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox21_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox22_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox23_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox24_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox25_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox26_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox27_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox28_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox29_DoubleClick(object sender, EventArgs e)
        {

        }
        private void pictureBox30_DoubleClick(object sender, EventArgs e)
        {

        }
        #endregion
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
        void ShowSelectedObjectParameters()
        {
            groupIDs.Items.Clear();
            if (isSelectedObjectToFind)
            {
                object[] groups = (objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupIDs] as object[]);
                groupIDs.Items.AddRange(groups);
                int zLayer = (int)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer];
                radioButton19.Checked = zLayer == -3;
                radioButton20.Checked = zLayer == -1;
                radioButton21.Checked = zLayer == 1;
                radioButton22.Checked = zLayer == 3;
                radioButton23.Checked = zLayer == 5;
                radioButton6.Checked = zLayer == 7;
                radioButton5.Checked = zLayer == 9;
                radioButton3.Checked = zLayer == 0;
                radioButton4.Checked = !(radioButton19.Checked || radioButton20.Checked || radioButton21.Checked || radioButton22.Checked || radioButton23.Checked || radioButton6.Checked || radioButton5.Checked);
                numericUpDown4.Value = zLayer;
                numericUpDown48.Value = (int)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.ZOrder];
                checkBox92.Checked = (bool)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.DontFade];
                checkBox193.Checked = (bool)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.DontEnter];
                checkBox193.Checked = (bool)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.HighDetail];
                checkBox93.Checked = (bool)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupParent];
                checkBox192.Checked = !(bool)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.DisableGlow];
                numericUpDown51.Value = (int)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1];
                numericUpDown2.Value = (int)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2];
                numericUpDown138.Value = (int)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[0];
                numericUpDown137.Value = (decimal)(double)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[1];
                numericUpDown136.Value = (decimal)(double)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[2];
                checkBox217.Checked = (bool)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[4];
                checkBox216.Checked = (bool)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[5];
                numericUpDown7.Value = (int)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[0];
                numericUpDown5.Value = (decimal)(double)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[1];
                numericUpDown3.Value = (decimal)(double)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[2];
                checkBox3.Checked = (bool)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[4];
                checkBox2.Checked = (bool)(objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[5];
                numericUpDown6.Value = (decimal)(double)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Scaling];
                numericUpDown135.Value = (decimal)(double)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.Rotation];
                numericUpDown49.Value = (decimal)(double)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.EL1];
                numericUpDown50.Value = (decimal)(double)objectsToFind[page][selectedObjectIndex][LevelObject.ObjectParameter.EL2];
            }
            else
            {
                object[] groups = (objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupIDs] as object[]);
                groupIDs.Items.AddRange(groups);
                int zLayer = (int)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZLayer];
                radioButton19.Checked = zLayer == -3;
                radioButton20.Checked = zLayer == -1;
                radioButton21.Checked = zLayer == 1;
                radioButton22.Checked = zLayer == 3;
                radioButton23.Checked = zLayer == 5;
                radioButton6.Checked = zLayer == 7;
                radioButton5.Checked = zLayer == 9;
                radioButton3.Checked = zLayer == 0;
                radioButton4.Checked = !(radioButton19.Checked || radioButton20.Checked || radioButton21.Checked || radioButton22.Checked || radioButton23.Checked || radioButton6.Checked || radioButton5.Checked);
                numericUpDown4.Value = zLayer;
                numericUpDown48.Value = (int)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.ZOrder];
                checkBox92.Checked = (bool)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.DontFade];
                checkBox193.Checked = (bool)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.DontEnter];
                checkBox193.Checked = (bool)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.HighDetail];
                checkBox93.Checked = (bool)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.GroupParent];
                checkBox192.Checked = !(bool)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.DisableGlow];
                numericUpDown51.Value = (int)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1];
                numericUpDown2.Value = (int)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2];
                numericUpDown138.Value = (int)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[0];
                numericUpDown137.Value = (decimal)(double)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[1];
                numericUpDown136.Value = (decimal)(double)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[2];
                checkBox217.Checked = (bool)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[4];
                checkBox216.Checked = (bool)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color1HSVValues] as object[])[5];
                numericUpDown7.Value = (int)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[0];
                numericUpDown5.Value = (decimal)(double)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[1];
                numericUpDown3.Value = (decimal)(double)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[2];
                checkBox3.Checked = (bool)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[4];
                checkBox2.Checked = (bool)(objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Color2HSVValues] as object[])[5];
                numericUpDown6.Value = (decimal)(double)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Scaling];
                numericUpDown135.Value = (decimal)(double)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.Rotation];
                numericUpDown49.Value = (decimal)(double)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.EL1];
                numericUpDown50.Value = (decimal)(double)objectsToReplaceWith[page][selectedObjectIndex][LevelObject.ObjectParameter.EL2];
            }
        }
    }
}