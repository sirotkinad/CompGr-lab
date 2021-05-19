

using System;
using System.Windows.Forms;

namespace RayTracing
{
    public partial class MainForm : Form
    {
        private OpenTK.GLControl GLViewer;
        private float TrackBarStepX = 0.1F;
        private float TrackBarStepY = 0.1F;
        private float TrackBarStepZ = 0.1F;
        private ShaderView SV;
        public MainForm()
        {
            InitializeComponent();
            GLViewer = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));
            GLViewer.Paint += GLPaint;
            GLViewer.MouseWheel += PosZ;
            Controls.Add(GLViewer);
            GLViewer.Top = 12;
            GLViewer.Left = 12;
            GLViewer.Width = 500;
            GLViewer.Height = 500;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SV = new ShaderView(GLViewer.Width, GLViewer.Height, GLViewer.Width, GLViewer.Height);
            TrackBarStepX = ShaderView.TOTAL_VIEW_WIDTH / (tbPosX.Maximum - tbPosX.Minimum);
            TrackBarStepY = ShaderView.TOTAL_VIEW_WIDTH / (tbPosY.Maximum - tbPosY.Minimum);
            TrackBarStepZ = ShaderView.TOTAL_VIEW_WIDTH / 100.0F;
            GLViewer.MakeCurrent();
        }
        private void GLPaint(object sender, PaintEventArgs e)
        {
            GLViewer.MakeCurrent();
            SV.DrawQuads();
            GLViewer.SwapBuffers();
        }
        private void buttonProcess_Click(object sender, EventArgs e)
        {
            GLViewer.Invalidate();
        }
        private void PosZ(object sender, MouseEventArgs e)
        {
            int DeltaNormalized = e.Delta * SystemInformation.MouseWheelScrollLines / 120;            
            if (radioLight.Checked)
            {
                SV.LightSourcePosition.Z += TrackBarStepZ * DeltaNormalized;
            }
            else if (radioCamera.Checked)
            {
                SV.CameraPosition.Z += TrackBarStepZ * DeltaNormalized;
            }
            GLViewer.Invalidate();
        }
        private void tbPosX_Scroll(object sender, EventArgs e)
        {
            if (radioLight.Checked)
            {
                SV.LightSourcePosition.X = TrackBarStepX * tbPosX.Value;
            }
            else if (radioCamera.Checked)
            {
                SV.CameraPosition.X = TrackBarStepX * tbPosX.Value;
            }
            GLViewer.Invalidate();
        }
        private void tbPosY_Scroll(object sender, EventArgs e)
        {
            if (radioLight.Checked)
            {
                SV.LightSourcePosition.Y = TrackBarStepY * tbPosY.Value;
            }
            else if (radioCamera.Checked)
            {
                SV.CameraPosition.Y = TrackBarStepY * tbPosY.Value;
            }
            GLViewer.Invalidate();
        }
        private void radioPositionChanged(object sender, EventArgs e)
        {
            if (radioLight.Checked)
            {
                tbPosX.Value = (int)(SV.LightSourcePosition.X / TrackBarStepX);
                tbPosY.Value = (int)(SV.LightSourcePosition.Y / TrackBarStepY);
            }
            else if (radioCamera.Checked)
            {
                tbPosX.Value = (int)(SV.CameraPosition.X / TrackBarStepX);
                tbPosY.Value = (int)(SV.CameraPosition.Y / TrackBarStepY);
            }
        }
        private void buttonAddCube_Click(object sender, EventArgs e)
        {
            float x = (float)Convert.ToDouble(textCubePosX.Text.Replace('.', ',').Trim());
            float y = (float)Convert.ToDouble(textCubePosY.Text.Replace('.', ',').Trim());
            float z = (float)Convert.ToDouble(textCubePosZ.Text.Replace('.', ',').Trim());
            int colorIndex = comboColor.SelectedIndex;
            string sizeStr = comboSize.SelectedItem as string;
            if(sizeStr != null)
            {
                float sizeFlt = (float)Convert.ToDouble(sizeStr.Replace('.', ',').Trim());
                SV.AddCube(x, y, z, colorIndex, sizeFlt);
                GLViewer.Invalidate();
            }
        }
        private void SetRayTracingDepth(object sender, EventArgs e)
        {
            SV.RayTracingDepth = Convert.ToInt32(trackRayTrDepth.Value);
            GLViewer.Invalidate();
        }
        private void ComboColor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ComboSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
