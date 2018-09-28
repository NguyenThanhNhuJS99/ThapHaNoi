using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThapHaNoi
{
    public partial class ThapHaNoi : Form
    {
        TimeSpan time;
        int moveCount;
        PictureBox[] disks;
        Stack<PictureBox> disksA, disksB, disksC;
        const int FIRSTY = 501, DISKHEIGHT = 27, DISTXFROMRODTODISK = 11;

        public ThapHaNoi()
        {
            InitializeComponent();
            disks = new PictureBox[] { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8 };
            disksA = new Stack<PictureBox>();
            disksB = new Stack<PictureBox>();
            disksC = new Stack<PictureBox>();
        }

        private void ThapHaNoi_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Luật chơi:\n- Mỗi lần chỉ được di chuyển 1 đĩa trên cùng của cọc.\n- Đĩa nằm trên phải nhỏ hơn đĩa nằm dưới.", 
                "Luật Chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tmCoutTime_Tick(object sender, EventArgs e)
        {
            time = time.Add(new TimeSpan(0, 0, 1));
            lblTime.Text = string.Format("Thời gian: {0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
        }

        private void lbMoveCout_Click(object sender, EventArgs e)
        {

        }

        private void lblTime_Click(object sender, EventArgs e)
        {

        }


        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Reset all
            tmCoutTime.Stop();
            foreach(PictureBox disk in disks)
            {
                disk.Visible = false;
            }
            time = new TimeSpan(0);
            moveCount = 0;
            lblTime.Text = "Thời Gian: 00:00:00";
            lbMoveCout.Text = "Số Lần Di Chuyển: 0 lần";
            disksA.Clear();disksB.Clear();disksC.Clear();
            picRodA.BorderStyle = picRodB.BorderStyle = picRodC.BorderStyle = BorderStyle.None;

            //Initialze
            nudLevel.Enabled = false;
            btnGivenIn.Enabled = true;
            btnPlay.Text = "Chơi Lại";
            int x = picRodA.Location.X + DISTXFROMRODTODISK, y = FIRSTY;
            for (int i = (int)nudLevel.Value - 1; i >= 0; i--, y -= DISKHEIGHT)
            {
                disks[i].Location=new Point(x,y);
                disks[i].Visible = true;
                disksA.Push(disks[i]);
            }
            tmCoutTime.Start();
        }

        private void btnGivenIn_Click(object sender, EventArgs e)
        {
            tmCoutTime.Stop();
            nudLevel.Enabled = true;
            btnGivenIn.Enabled = false;
            btnPlay.Text = "Chơi";
        }
    }
}
