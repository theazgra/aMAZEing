using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aMaze_ingSolver
{
    public partial class MazeForm : Form
    {
        private Image _image;
        Maze maze;
        public MazeForm()
        {
            InitializeComponent();

            //_image = Image.FromFile("../maze/tiny.png");
            _image = Image.FromFile("../maze/normal.png");
            imgBox.Image = _image;

            //Bitmap b = new Bitmap(_image);
            maze = new Maze(_image);
            maze.SetCanvas(imgBox);

            imgBox.Image = maze.GetImage();
            
        }

        private void miLoadMaze_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Images|*.png;*.bmp;*.jpg;*.jpeg"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _image = Image.FromFile(ofd.FileName);
                imgBox.Image = _image;
            }
        }
    }
}
