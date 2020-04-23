using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrblCNC.Glutils;
using System.Drawing.Text;

namespace GrblCNC.Controls
{
    public partial class ManualControl : UserControl
    {
        public enum AxisAction
        {
            Home = 0,
            CoordTouchOff,
            ToolTouchOff,
        }

        public enum Sliders
        {
            JogSpeed = 0,
            SpindleSpeed,
        }



        public delegate void AxisStepJogPressedDelegate(object sender, int axis, float amount, float speed);
        public event AxisStepJogPressedDelegate AxisStepJogPressed;
        public delegate void AxisContinuesJogPressedDelegate(object sender, int axis, int direction, float speed);
        public event AxisContinuesJogPressedDelegate AxisContinuesJogPressed;
        public delegate void AxisActionPressedDelegate(object sender, int axis, AxisAction action);
        public event AxisActionPressedDelegate AxisActionPressed;
        public delegate void SpindleActionDelegate(object sender, float speed, GrblComm.SpindleAction action);
        public event SpindleActionDelegate SpindleAction;

        float fastJog = 50;
        float slowJog = 50;
        bool isJogFast = true;
        public ManualControl()
        {
            InitializeComponent();
            comboJogStep.SelectedIndex = 0;
            Enabled = Global.GrblConnected;
            Global.GrblConnectionChanged += Global_GrblConnectionChanged;
            valueSlideSpinSpeed.ValueChange += valueSlideSpinSpeed_ValueChange;
            valueSlideJogSpeedXYZ.ValueChange += valueSlideJogSpeedXYZ_ValueChange;
        }

        public float GetJogSpeed()
        {
            return valueSlideJogSpeedXYZ.Value;
        }

        void valueSlideJogSpeedXYZ_ValueChange(object sender, float value)
        {
            float minval = valueSlideJogSpeedXYZ.MinValue;
            float maxval = valueSlideJogSpeedXYZ.MaxValue;
            float val = (valueSlideJogSpeedXYZ.Value - minval) * 100 / (maxval - minval);
            if (isJogFast)
                fastJog = val;
            else
                slowJog = val;
        }

        void valueSlideSpinSpeed_ValueChange(object sender, float value)
        {
            if (SpindleAction != null)
                SpindleAction(this, value, GrblComm.SpindleAction.Speed);
        }

        void Global_GrblConnectionChanged(bool isConnected)
        {
            Enabled = isConnected;
        }

        public float GetSelectedJogStep()
        {
            if (comboJogStep.SelectedIndex == 0)
                return 0;
            // parse the distance value from the distance string in the combo box
            string[] vars = comboJogStep.SelectedItem.ToString().Split(' ');
            return Utils.ParseFloatInvariant(vars[0]);
        }

        public void SetCurrentAxis(int axis)
        {
            if (axis < 0 || axis >= Global.NUM_AXIS)
                return;
            multiSelAxis.SelectedValue= axis;
        }

        private void AxisPos_click(object sender, EventArgs e)
        {
            if (comboJogStep.SelectedIndex <= 0)
                return;
            if (AxisStepJogPressed == null)
                return;
            JogButton b = (JogButton)sender;
            SetCurrentAxis(b.Id);
            AxisStepJogPressed(this, b.Id, GetSelectedJogStep(), valueSlideJogSpeedXYZ.Value);
        }

        private void AxisNeg_click(object sender, EventArgs e)
        {
            if (comboJogStep.SelectedIndex == 0)
                return;
            if (AxisStepJogPressed == null)
                return;
            JogButton b = (JogButton)sender;
            SetCurrentAxis(b.Id);
            AxisStepJogPressed(this, b.Id, -GetSelectedJogStep(), valueSlideJogSpeedXYZ.Value);
        }

        private void AxisHome_click(object sender, EventArgs e)
        {
            if (AxisActionPressed == null)
                return;
            AxisActionPressed(this, multiSelAxis.SelectedValue, AxisAction.Home);
        }

        private void jogButtTouchOff_Click(object sender, EventArgs e)
        {
            if (AxisActionPressed == null)
                return;
            AxisActionPressed(this, multiSelAxis.SelectedValue, AxisAction.CoordTouchOff);
        }

        private void jogButtToolTouchOff_Click(object sender, EventArgs e)
        {
            if (AxisActionPressed == null)
                return;
            AxisActionPressed(this, multiSelAxis.SelectedValue, AxisAction.ToolTouchOff);
        }

        private void jogButtSpindleStop_Click(object sender, EventArgs e)
        {
            if (SpindleAction == null)
                return;
            SpindleAction(this, 0, GrblComm.SpindleAction.Stop);
        }

        private void jogButtSpindleCCW_Click(object sender, EventArgs e)
        {
            if (SpindleAction == null)
                return;
            SpindleAction(this, valueSlideSpinSpeed.Value, GrblComm.SpindleAction.StartCCW);
        }

        private void jogButtSpindleCW_Click(object sender, EventArgs e)
        {
            if (SpindleAction == null)
                return;
            SpindleAction(this, valueSlideSpinSpeed.Value, GrblComm.SpindleAction.StartCW);
        }

        private void AxisPos_down(object sender, MouseEventArgs e)
        {
            if (comboJogStep.SelectedIndex != 0)
                return;
            if (AxisContinuesJogPressed == null)
                return;
            JogButton b = (JogButton)sender;
            SetCurrentAxis(b.Id);
            AxisContinuesJogPressed(this, b.Id, 1, valueSlideJogSpeedXYZ.Value);
        }

        private void AxisNeg_down(object sender, MouseEventArgs e)
        {
            if (comboJogStep.SelectedIndex != 0)
                return;
            if (AxisContinuesJogPressed == null)
                return;
            JogButton b = (JogButton)sender;
            SetCurrentAxis(b.Id);
            AxisContinuesJogPressed(this, b.Id, -1, valueSlideJogSpeedXYZ.Value);
        }

        private void Axis_up(object sender, MouseEventArgs e)
        {
            if (comboJogStep.SelectedIndex != 0)
                return;
            if (AxisContinuesJogPressed == null)
                return;
            JogButton b = (JogButton)sender;
            AxisContinuesJogPressed(this, b.Id, 0, 0);
        }

        void UpdateJogSlider()
        {
            float val = valueSlideJogSpeedXYZ.MaxValue - valueSlideJogSpeedXYZ.MinValue;
            if (isJogFast)
                val = val * fastJog / 100;
            else
                val = val * slowJog / 100;
            valueSlideJogSpeedXYZ.Value = valueSlideJogSpeedXYZ.MinValue + val;    
        }

        public void SetSliderMinMax(Sliders sld, float min, float max)
        {
            switch (sld)
            {
                case Sliders.JogSpeed: 
                    valueSlideJogSpeedXYZ.MinValue = min; 
                    valueSlideJogSpeedXYZ.MaxValue = max; 
                    UpdateJogSlider();
                    break;

                case Sliders.SpindleSpeed: 
                    valueSlideSpinSpeed.MinValue = min; 
                    valueSlideSpinSpeed.MaxValue = max; 
                    break;
            }
        }
    }
}
