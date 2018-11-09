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


        private void pictureBox1_Click_1(object sender, EventArgs e) // Xử lí click vào đĩa
        {
            PictureBox clickedDisk = (PictureBox)sender;             // Gán giá trị đĩa click
            if (disksA.Contains(clickedDisk))                        // Kiểm tra đĩa có thuộc cột A
                picRod_Click(picRodA, new EventArgs());              // Gọi lại hàm click cột A
            else if (disksB.Contains(clickedDisk))                   // Kiểm tra đĩa có thuộc cột B
                picRod_Click(picRodB, new EventArgs());              // Gọi lại hàm click cột B
            else picRod_Click(picRodC, new EventArgs());             // Gọi lại hàm click cột C 
        }
        //Hàm thể hiện nút luật chơi
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Luật chơi:\n- Mỗi lần chỉ được di chuyển 1 đĩa trên cùng của cọc.\n- Đĩa nằm trên phải nhỏ hơn đĩa nằm dưới. \n- Di chuyển hết đĩa từ cột A sang C để chiến thắng.", 
                "Luật Chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tmCoutTime_Tick(object sender, EventArgs e)
        {
            time = time.Add(new TimeSpan(0, 0, 1));
            lblTime.Text = string.Format("Thời gian: {0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Reset all
            tmCoutTime.Stop();                                                                  // Dừng đếm thời gian
            foreach(PictureBox disk in disks)                                                   // Ẩn hết các đĩa
            {
                disk.Visible = false;
            }
            time = new TimeSpan(0);                                                             // Trả thời gian về 0
            moveCount = 0;                                                                      // Trả số lần di chuyển về 0
            lblTime.Text = "Thời Gian: 00:00:00";
            lbMoveCout.Text = "Số Lần Di Chuyển: 0 lần";
            disksA.Clear();disksB.Clear();disksC.Clear();                                       // Xóa hết các đĩa trong các cột
            picRodA.BorderStyle = picRodB.BorderStyle = picRodC.BorderStyle = BorderStyle.None; // Ẩn viền cột đã chọn ở trước
            firstClickedDisks = secondClickedDisks = null;
            //Initialze
            nudLevel.Enabled = false;                                                           // Không cho chọn level
            btnGivenIn.Enabled = true;                                                          // Hiển thị nút chơi lại
            btnPlay.Text = "Chơi Lại";                                                          // Thay đổi Text hiển thị
            int x = picRodA.Location.X + DISTXFROMRODTODISK, y = FIRSTY;
            for (int i = (int)nudLevel.Value - 1; i >= 0; i--, y -= DISKHEIGHT)                 // Hiển thị các đĩa
            {
                disks[i].Location=new Point(x,y);                                               // Tạo tọa độ mới của đĩa
                disks[i].Visible = true;                                                        // Hiển thị đĩa
                disksA.Push(disks[i]);                                                          // Thêm đĩa vào cột A
            }
            tmCoutTime.Start();                                                                 // Bắt đầu đềm thời gian
        }

        private void btnGivenIn_Click(object sender, EventArgs e)
        {
            tmCoutTime.Stop();                                                                  // Dừng đếm thời gian
            nudLevel.Enabled = true;                                                            // Cho phép chọn lại số đĩa
            btnGivenIn.Enabled = false;                                                         // Tắt nút chịu thua
            btnPlay.Text = "Chơi";                                                              // Text nút play trả về "Chơi"
        }
        private void picRod_Click(object sender, EventArgs e)
        {
            if (nudLevel.Enabled) return; // Lúc này nút không được điều chỉnh đồng nghĩa với việc hiện tại không chơi nên không click
            PictureBox clickedRod = (PictureBox)sender;     //Lấy thông tin cột được click
            Stack<PictureBox> disksOfClickedRod = (Stack<PictureBox>)clickedRod.Tag; //Lấy ra stack ứng với cột click
            if (firstClickedDisks == null)
            {
                if (disksOfClickedRod.Count == 0) return;                   // Cột không có đĩa nào
                firstClickedDisks = disksOfClickedRod;                      // Lưu giá trị đĩa lần click 1
                clickedRod.BorderStyle = BorderStyle.FixedSingle;           // Hiển thị viền
            }
            else if (secondClickedDisks == null)
            {
                if (disksOfClickedRod == firstClickedDisks)                 // Chọn lại lần nữa
                {
                    firstClickedDisks = null;                               // reset giá trị lần click thứ nhất
                    clickedRod.BorderStyle = BorderStyle.None;              // Ẩn viền
                    return;
                }
                secondClickedDisks = disksOfClickedRod;                     // Lưu giá trị đĩa lần click 2
                ProcessMovingDisk(clickedRod);                              // Di chuyển đĩa

            }
        }
        private void ProcessMovingDisk(PictureBox clickedRod)
        {
            if (secondClickedDisks.Count == 0)                                          // Ở cột 2 đang không có đĩa
            {
                MoveDisk(new Point(clickedRod.Location.X + DISTXFROMRODTODISK, FIRSTY));    // Di chuyển đĩa
            }
            else
            {
                PictureBox firstTopDisk = firstClickedDisks.Peek();         //Lấy thông tin đĩa đầu của cột 1
                PictureBox secondTopDisk = secondClickedDisks.Peek();       //Lấy thông tin đĩa đầu của cột 2
                if (int.Parse(firstTopDisk.Tag.ToString()) < int.Parse(secondTopDisk.Tag.ToString()))  //So sánh 2 đĩa thông qua thuộc tính tag
                    MoveDisk(new Point(secondTopDisk.Location.X, secondTopDisk.Location.Y - DISKHEIGHT)); //Di chuyển đĩa tới vị trí mới có tọa độ mới
                else
                    secondClickedDisks = null;    // Không hợp lệ nên bỏ đánh dấu lần chọn thứ 2
            }
        }
        private void MoveDisk(Point point)
        {
            PictureBox firstTopDisk = firstClickedDisks.Pop();  // Lấy ra và xóa đĩa đầu của cột chọn trước
            firstTopDisk.Location = point;                      // Cập nhật tọa độ,tọa dộ mới được truyền vào
            secondClickedDisks.Push(firstTopDisk);              // Bỏ đĩa đã lấy ra vào đầu của cột đĩa chọn sau
            ++moveCount;                                        // Tăng số lần di chuyển
            lbMoveCout.Text = string.Format("Số lần di chuyển: {0} lần.", moveCount);           // Hiển thị số lần di chuyển
            firstClickedDisks = secondClickedDisks = null;                                      // Trả 2 lần click về null
            picRodA.BorderStyle = picRodB.BorderStyle = picRodC.BorderStyle = BorderStyle.None; // Xóa các đường biên
            if (disksC.Count == nudLevel.Value)                 // Kiểm tra cột C nếu bằng số đĩa đã chọn thì chiến thắng
            {
                btnGivenIn.PerformClick();                      // Sử dụng lại nút Chịu thua để chơi lại
                MessageBox.Show("Chúc mừng bạn đã hoàn thành trò chơi!","Chúc mừng");
            }
        }
        private void ThapHaNoi_Load(object sender, EventArgs e)
        {
            return;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void lbMoveCout_Click(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void lblTime_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
