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
        Stack<PictureBox> disksA, disksB, disksC, firstClickedDisks, secondClickedDisks;
        const int FIRSTY = 501, DISKHEIGHT = 27, DISTXFROMRODTODISK = 11;

        public ThapHaNoi()
        {
            InitializeComponent();
            disks = new PictureBox[] { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8 };
            picRodA.Tag = disksA = new Stack<PictureBox>();
            picRodB.Tag = disksB = new Stack<PictureBox>();
            picRodC.Tag = disksC = new Stack<PictureBox>();
        }

        private void ThapHaNoi_Load(object sender, EventArgs e)
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
            PictureBox clickedDisk = (PictureBox)sender;
            if (disksA.Contains(clickedDisk))
                picRod_Click(picRodA, new EventArgs());
            else if (disksB.Contains(clickedDisk))
                picRod_Click(picRodB, new EventArgs());
            else picRod_Click(picRodC, new EventArgs());
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
        private void picRod_Click(object sender, EventArgs e)
        {
            if (nudLevel.Enabled) return; // is not playing
            PictureBox clickedRod = (PictureBox)sender;
            Stack<PictureBox> disksOfClickedRod = (Stack<PictureBox>)clickedRod.Tag;
            if (firstClickedDisks == null)
            {
                if (disksOfClickedRod.Count == 0) return;
                firstClickedDisks = disksOfClickedRod;
                clickedRod.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (secondClickedDisks == null)
            {
                if (disksOfClickedRod == firstClickedDisks)
                {
                    firstClickedDisks = null;
                    clickedRod.BorderStyle = BorderStyle.None;
                    return;
                }
                secondClickedDisks = disksOfClickedRod;
                ProcessMovingDisk(clickedRod);

            }
        }
        private void ProcessMovingDisk(PictureBox clickedRod)
        {
            if (secondClickedDisks.Count == 0)
            {
                MoveDisk(new Point(clickedRod.Location.X + DISTXFROMRODTODISK, FIRSTY));
            }
            else
            {
                PictureBox firstTopDisk = firstClickedDisks.Peek();
                PictureBox secondTopDisk = secondClickedDisks.Peek();
                if (int.Parse(firstTopDisk.Tag.ToString()) < int.Parse(secondTopDisk.Tag.ToString()))
                    MoveDisk(new Point(secondTopDisk.Location.X, secondTopDisk.Location.Y - DISKHEIGHT));
                else
                    secondClickedDisks = null;
            }
        }
        private void MoveDisk(Point point)
        {
            PictureBox firstTopDisk = firstClickedDisks.Pop();
            firstTopDisk.Location = point;
            secondClickedDisks.Push(firstTopDisk);
            ++moveCount;
            lbMoveCout.Text = string.Format("Số lần di chuyển: {0} lần.", moveCount);
            firstClickedDisks = secondClickedDisks = null;
            picRodA.BorderStyle = picRodB.BorderStyle = picRodC.BorderStyle = BorderStyle.None;
            if (disksC.Count == nudLevel.Value)
            {
                btnGivenIn.PerformClick();
                MessageBox.Show("Chúc mừng bạn đã hoàn thành trò chơi!","Chúc mừng");
            }
        }
    }
}
