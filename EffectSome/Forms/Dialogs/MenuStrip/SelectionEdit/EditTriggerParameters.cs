using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EffectSome.EffectSome;
using static EffectSome.Memory;

namespace EffectSome
{
    public partial class EditTriggerParameters : Form
    {
        public EditTriggerParameters()
        {
            InitializeComponent();
        }

        private void EditObjectParameters_Load(object sender, EventArgs e)
        {

        }
        private void EditTriggerParameters_Activated(object sender, EventArgs e)
        {

        }
        void CheckSelectedTriggers(LevelObject.Trigger t)
        {
            #region Control Arrays
            Label[] MoveLs = { label9, label10, label11, label12, label21 };
            NumericUpDown[] MoveNUDs = { numericUpDown33, numericUpDown32, numericUpDown31, numericUpDown40 };
            CheckBox[] MoveCBs = { checkBox63, checkBox62 };

            Label[] PulseLs = { label13, label14, label15, label16, label17, label18, label22, label32/*, label33*/, label23 };
            NumericUpDown[] PulseNUDs = { numericUpDown36, numericUpDown35, numericUpDown34, numericUpDown41, numericUpDown54, /*numericUpDown57, numericUpDown56, numericUpDown55,*/ numericUpDown42 };
            CheckBox[] PulseCBs = { checkBox67, checkBox66/*, checkBox73, checkBox72 */};
            RadioButton[] PulseRBs = { radioButton13, radioButton14, radioButton16, radioButton18 };

            Label[] ColorLs = { label31, label34, label36, label35 };
            NumericUpDown[] ColorNUDs = { numericUpDown53, numericUpDown59, numericUpDown62, numericUpDown61, numericUpDown60, numericUpDown58, numericUpDown63 };
            CheckBox[] ColorCBs = { checkBox75, checkBox74, checkBox77, checkBox91 };

            Label[] AlphaLs = { label19, label20, label24 };
            NumericUpDown[] AlphaNUDs = { numericUpDown39, numericUpDown38, numericUpDown43 };
            #endregion
            #region Setting enabled properties depending on trigger type
            groupBox21.Enabled = t == LevelObject.Trigger.Move;
            for (int i = 0; i < MoveLs.Length; i++)
                MoveLs[i].Enabled = t == LevelObject.Trigger.Move;
            for (int i = 0; i < MoveNUDs.Length; i++)
            {
                MoveNUDs[i].Enabled = t == LevelObject.Trigger.Move;
                if (MoveNUDs[i].Enabled == false)
                    MoveNUDs[i].Value = 0;
            }
            for (int i = 0; i < MoveCBs.Length; i++)
                MoveCBs[i].Enabled = t == LevelObject.Trigger.Move;
            comboBox2.Enabled = t == LevelObject.Trigger.Move;
            checkBox65.Enabled = t == LevelObject.Trigger.Move && comboBox2.Text != "None";
            checkBox64.Enabled = t == LevelObject.Trigger.Move && comboBox2.Text != "None";

            groupBox22.Enabled = t == LevelObject.Trigger.Pulse;
            for (int i = 0; i < PulseLs.Length; i++)
                PulseLs[i].Enabled = t == LevelObject.Trigger.Pulse;
            for (int i = 0; i < PulseNUDs.Length; i++)
            {
                PulseNUDs[i].Enabled = t == LevelObject.Trigger.Pulse;
                if (PulseNUDs[i].Enabled == false)
                    PulseNUDs[i].Value = 0;
            }
            for (int i = 0; i < PulseCBs.Length; i++)
                PulseCBs[i].Enabled = t == LevelObject.Trigger.Pulse;
            for (int i = 0; i < PulseRBs.Length; i++)
                PulseRBs[i].Enabled = t == LevelObject.Trigger.Pulse;

            groupBox27.Enabled = t == LevelObject.Trigger.Color || t == LevelObject.Trigger.BG || t == LevelObject.Trigger.GRND || t == LevelObject.Trigger.GRND2 || t == LevelObject.Trigger.Line || t == LevelObject.Trigger.Obj || t == LevelObject.Trigger.ThreeDL;
            for (int i = 0; i < ColorLs.Length; i++)
                ColorLs[i].Enabled = t == LevelObject.Trigger.Color || t == LevelObject.Trigger.BG || t == LevelObject.Trigger.GRND || t == LevelObject.Trigger.GRND2 || t == LevelObject.Trigger.Line || t == LevelObject.Trigger.Obj || t == LevelObject.Trigger.ThreeDL;
            for (int i = 0; i < ColorNUDs.Length; i++)
            {
                ColorNUDs[i].Enabled = t == LevelObject.Trigger.Color || t == LevelObject.Trigger.BG || t == LevelObject.Trigger.GRND || t == LevelObject.Trigger.GRND2 || t == LevelObject.Trigger.Line || t == LevelObject.Trigger.Obj || t == LevelObject.Trigger.ThreeDL;
                if (ColorNUDs[i].Enabled == false)
                    ColorNUDs[i].Value = 0;
            }
            for (int i = 0; i < ColorCBs.Length; i++)
                ColorCBs[i].Enabled = t == LevelObject.Trigger.Color || t == LevelObject.Trigger.BG || t == LevelObject.Trigger.GRND || t == LevelObject.Trigger.GRND2 || t == LevelObject.Trigger.Line || t == LevelObject.Trigger.Obj || t == LevelObject.Trigger.ThreeDL;

            groupBox23.Enabled = t == LevelObject.Trigger.Alpha;
            for (int i = 0; i < AlphaLs.Length; i++)
                AlphaLs[i].Enabled = t == LevelObject.Trigger.Alpha;
            for (int i = 0; i < AlphaNUDs.Length; i++)
            {
                AlphaNUDs[i].Enabled = t == LevelObject.Trigger.Alpha;
                if (AlphaNUDs[i].Enabled == false)
                    AlphaNUDs[i].Value = 0;
            }

            groupBox24.Enabled = t == LevelObject.Trigger.Toggle;
            label25.Enabled = t == LevelObject.Trigger.Toggle;
            numericUpDown44.Enabled = t == LevelObject.Trigger.Toggle;
            if (numericUpDown44.Enabled == false)
                numericUpDown44.Value = 0;
            checkBox69.Enabled = t == LevelObject.Trigger.Toggle;

            groupBox25.Enabled = t == LevelObject.Trigger.Spawn;
            label26.Enabled = t == LevelObject.Trigger.Spawn;
            numericUpDown45.Enabled = t == LevelObject.Trigger.Spawn;
            if (numericUpDown45.Enabled == false)
                numericUpDown45.Value = 0;
            #endregion
            #region Showing the currently selected triggers' values
            if (t != 0)
            {
                if (t == LevelObject.Trigger.Move)
                    numericUpDown33.Value = (decimal)GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3E4);
                numericUpDown32.Value = (decimal)(GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3F4)) / 3;
                numericUpDown31.Value = (decimal)(GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3F8)) / 3;
                checkBox63.Checked = GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x400);
                checkBox62.Checked = GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x401);
                if (GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3FC) > 18 || GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3FC) <= 0)
                    comboBox2.Text = comboBox2.Items[0].ToString();
                else
                    comboBox2.Text = comboBox2.Items[((GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3FC) - 1) / 3) + 1].ToString();
                checkBox65.Checked = (GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3FC) % 3 == 2 || GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3FC) % 3 == 1) && comboBox2.Text != "None";
                checkBox64.Checked = (GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3FC) % 3 == 0 || GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3FC) % 3 == 1) && comboBox2.Text != "None";
                if (t == LevelObject.Trigger.Move)
                    numericUpDown40.Value = GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3EC);

                radioButton16.Checked = !GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x410);
                radioButton18.Checked = GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x410);
                radioButton13.Checked = !GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414);
                radioButton14.Checked = GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414);
                numericUpDown36.Value = (decimal)GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x404);
                numericUpDown35.Value = (decimal)GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x408);
                numericUpDown34.Value = (decimal)GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x40C);
                //checkBox67.Checked = (GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414) == true) || (GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414) == GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414));
                //checkBox66.Checked = (GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414) == true) || (GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414) == GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x414));
                numericUpDown54.Value = (decimal)GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x428);
                if ((t == LevelObject.Trigger.Pulse) && radioButton14.Checked)
                    numericUpDown41.Value = GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3EC);
                else if ((t == LevelObject.Trigger.Pulse) && radioButton13.Checked)
                    numericUpDown42.Value = GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3EC);
                CheckColorID(numericUpDown54);
                CheckColorID(numericUpDown42);

                checkBox69.Checked = GetBoolFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x42F);
                if (t == LevelObject.Trigger.Toggle)
                    numericUpDown44.Value = GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3EC);

                if (t == LevelObject.Trigger.Alpha)
                {
                    numericUpDown39.Value = (decimal)GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3E4);
                    numericUpDown38.Value = (decimal)GetFloatFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3E8);
                    numericUpDown43.Value = GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3EC);
                }

                if (t == LevelObject.Trigger.Spawn)
                    numericUpDown45.Value = GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x3EC);
            }
            else
            {

            }
            #endregion
        }
        void CheckColorID(NumericUpDown NUD)
        {
            if (NUD.Value == 1000)
                toolTip1.SetToolTip(NUD, "BG");
            else if (NUD.Value == 1001)
                toolTip1.SetToolTip(NUD, "GRND");
            else if (NUD.Value == 1002)
                toolTip1.SetToolTip(NUD, "Line");
            else if (NUD.Value == 1003)
                toolTip1.SetToolTip(NUD, "Obj");
            else if (NUD.Value == 1004)
                toolTip1.SetToolTip(NUD, "3DL");
            else if (NUD.Value == 1005)
                toolTip1.SetToolTip(NUD, "P1");
            else if (NUD.Value == 1006)
                toolTip1.SetToolTip(NUD, "P2");
            else if (NUD.Value == 1007)
                toolTip1.SetToolTip(NUD, "LGB");
            else if (NUD.Value == 1009)
                toolTip1.SetToolTip(NUD, "GRND2");
            else if (NUD.Value == 1010)
                toolTip1.SetToolTip(NUD, "Black");
            else if (NUD.Value == 1011)
                toolTip1.SetToolTip(NUD, "White");
            else if (NUD.Value == 1012)
                toolTip1.SetToolTip(NUD, "Lighter");
            else if (NUD.Value > 1000)
                toolTip1.SetToolTip(NUD, "N/A Special Color Channel");
            else if (NUD.Value == 0)
                toolTip1.SetToolTip(NUD, "Default");
            else if (NUD.Value < 0)
            {
                NUD.Value = 1011;
                toolTip1.SetToolTip(NUD, "White");
            }
            else
                toolTip1.SetToolTip(NUD, "Color " + ((int)NUD.Value).ToString());
        }
        void CheckTriggersStatus()
        {
            CheckSelectedTriggers((LevelObject.Trigger)GetIntFromPointers(baseAddress, 0x150, 0x208, 0x284, 0x310));

            //if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.BG) // BG Color trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.BG);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.GRND) // GRND Color trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.GRND);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Obj) // Obj Color trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Obj);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.ThreeDL) // 3DL Color trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.ThreeDL);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Color) // Color trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Color);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.GRND2) // GRND2 trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.GRND2);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Move) // Move trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Move);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Line) // Line Color trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Line);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Pulse) // Pulse trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Pulse);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Alpha) // Alpha trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Alpha);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Toggle) // Toggle trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Toggle);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Spawn) // Spawn trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Spawn);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Animate) // Animate trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Animate);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.BGEffectOff) // BG Effect Off trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.BGEffectOff);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.BGEffectOn) // BG Effect On trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.BGEffectOn);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Collision) // Collision trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Collision);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Count) // Count trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Count);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Follow) // Follow trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Follow);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.FollowPlayerY) // Follow Player Y trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.FollowPlayerY);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.HidePlayer) // Hide Player trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.HidePlayer);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.InstantCount) // Instant Count trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.InstantCount);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.OnDeath) // On Death trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.OnDeath);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Pickup) // Pickup trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Pickup);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Rotate) // Rotate trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Rotate);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Shake) // Shake trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Shake);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.ShowPlayer) // Show Player trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.ShowPlayer);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Stop) // Stop trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Stop);
            //else if (GetIntFromPointers(baseAddress, new IntPtr(0x150), new IntPtr(0x208), new IntPtr(0x284), new IntPtr(0x310)) == (int)LevelObject.Trigger.Touch) // Touch trigger
            //    CheckSelectedTriggers(LevelObject.Trigger.Touch);
            //else
            //    CheckSelectedTriggers(LevelObject.Trigger.None);
        }
    }
}
