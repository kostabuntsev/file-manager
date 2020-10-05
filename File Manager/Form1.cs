using System;
using System.Windows.Forms;
using System.IO;


namespace File_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog ofd = new OpenFileDialog();


        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void buttonDeleteFile_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {

            }
            else
            {
                var newForm = new AreYouSureDelete();
                newForm.ShowDialog();


                string filePath = textBox1.Text;
                if (File.Exists(filePath) && newForm.DeleteConfirmed)
                {
                    File.Delete(filePath);
                    MessageBox.Show("File Deleted!", "Removed File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox2.Text = "C:";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {

            }
            else
            {
                string filePath = textBox1.Text;
                if (File.Exists(filePath))
                {
                    string source = this.textBox1.Text;
                    //string[] sourceFilePathParts = this.textBox1.Text.Split('.');
                    int extIndex = source.LastIndexOf('.');
                    string destination = source.Substring(0, extIndex) + " - Copy" + source.Substring(extIndex);
                    while (File.Exists(destination))
                    {
                        extIndex = destination.LastIndexOf('.');
                        destination = destination.Substring(0, extIndex) + " - Another One" + destination.Substring(extIndex);
                    }

                    //string nextToLastPart = sourceFilePathParts[sourceFilePathParts.Length - 2] + " - Copy";

                    File.Copy(source, destination);
                    MessageBox.Show("Copied File", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                FillFolderContent(fbd.SelectedPath);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as string;
            if (selectedItem.StartsWith("📁"))
            {
                string selectedFolderName = selectedItem.Substring(4);
                string slash = this.textBox2.Text.EndsWith("\\") ? "" : "\\";
                string newFolderPath = this.textBox2.Text + slash + selectedFolderName;
                FillFolderContent(newFolderPath);
                
            }
            else
            {
                string selectedFileame = selectedItem.Substring(4);
                string fullFilePath = this.textBox2.Text + "\\" + selectedFileame;
                System.Diagnostics.Process.Start(fullFilePath);
            }
        }

        private void FillFolderContent(string folderPath)
        {
            this.listBox1.Items.Clear();

            if (!folderPath.EndsWith("\\"))
            {
                folderPath = folderPath + "\\";
            }

            string[] dirs = Directory.GetDirectories(folderPath);
            string[] files = Directory.GetFiles(folderPath);

            foreach (string dir in dirs)
            {
                this.listBox1.Items.Add("📁  " + Path.GetFileName(dir));
            }
            foreach (string file in files)
            {
                this.listBox1.Items.Add("📄  " + Path.GetFileName(file));
            }

            this.textBox2.Text = folderPath;
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if(this.textBox2.Text.Length == 3)
            {
                MessageBox.Show("Can't move up anymore", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int lastSlashIndex = this.textBox2.Text.LastIndexOf('\\');
                string newPath = this.textBox2.Text.Substring(0, lastSlashIndex);

                if (this.textBox2.Text.EndsWith("\\"))
                {
                    lastSlashIndex = newPath.LastIndexOf('\\');
                    newPath = newPath.Substring(0, lastSlashIndex);
                }
                                
                FillFolderContent(newPath.Substring(0, lastSlashIndex));
            }
        }
    }
}
