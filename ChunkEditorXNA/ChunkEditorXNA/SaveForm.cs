using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace ChunkEditorXNA
{
    public partial class SaveForm : Form
    {
        private ChunkEditorManager chunkEditorManager;
        public SaveForm( ChunkEditorManager manager )
        {
            chunkEditorManager = manager;
            InitializeComponent();
        }
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Error Saving Chunk");
                return;
            }
            this.Close();
        }
    }
}
