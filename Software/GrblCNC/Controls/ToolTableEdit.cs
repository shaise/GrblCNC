using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrblCNC.Controls
{
    public partial class ToolTableEdit : UserControl
    {
        public ToolTableEdit()
        {
            InitializeComponent();
            MinimumSize = new Size(600, 200);
            Dock = System.Windows.Forms.DockStyle.Fill;
            Name = "ToolTableEdit";
            Size = new System.Drawing.Size(600, 300);
            TabIndex = 0;
        }

        protected override void OnResize(EventArgs e)
        {
            int bh = Height - buttSave.Height - 3;
            buttSave.Location = new Point(3, bh);
            buttExport.Location = new Point(buttSave.Location.X + buttSave.Width + 6, bh);
            buttImport.Location = new Point(buttExport.Location.X + buttExport.Width + 6, bh);
            buttRemove.Location = new Point(Width - buttRemove.Width - 3, bh);
            buttAdd.Location = new Point(buttRemove.Location.X - buttAdd.Width - 6, bh);
            dataGridTools.Width = Width - 6;
            dataGridTools.Height = bh - 6;
            base.OnResize(e);
        }
    }
}
