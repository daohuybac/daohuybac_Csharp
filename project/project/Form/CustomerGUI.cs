using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class CustomerGUI : Form
    {
        CustomerBAL cusBAL = new CustomerBAL();
        DepartmentDAL departBAL = new DepartmentDAL();
        AreaDAL areaBAL = new AreaDAL();
        GenderDAL genderBAL = new GenderDAL();
       




        public CustomerGUI()
        {
            InitializeComponent();
        }

        private void CustomerGUI_Load(object sender, EventArgs e)
        {
            tbImageUrl.Visible = false;
            List<CustomerBEL> lstCus = cusBAL.ReadCustomer();
            foreach (CustomerBEL cus in lstCus)
            {
                dgvCustomer.Rows.Add(cus.Id, cus.Codestu, cus.Name, cus.DateOfBirth.ToString("dd/MM/yyyy"), 
                    cus.GenderName, cus.Class, cus.DepartmentName, cus.AreaName, cus.Score, cus.Phone, cus.Address, cus.ImageUrl);
            }
            List<DepartmentBEL> lstDepart = departBAL.ReadDepartmentList();
            foreach (DepartmentBEL depart in lstDepart)
            {
                cbDepartment.Items.Add(depart);
            }
            cbDepartment.DisplayMember = "name";
            List<AreaBEL> lstArea = areaBAL.ReadAreaList();
            foreach (AreaBEL area in lstArea)
            {
                cbArea.Items.Add(area);
            }
            cbArea.DisplayMember = "name";
            List<GenderBEL> lsGender = genderBAL.ReadGenderList();
            foreach (GenderBEL gender in lsGender)
            {
                cbGender.Items.Add(gender);
            }
            cbGender.DisplayMember = "name";
            if (lstCus.Count > 0 && !string.IsNullOrWhiteSpace(lstCus[0].ImageUrl))
            {
                pictureBox1.Image = Image.FromFile(lstCus[0].ImageUrl);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            DataGridViewRow row = dgvCustomer.Rows[idx];
            if (row.Cells[0].Value != null)
            {
                tbId.Text = row.Cells[0].Value.ToString();
                tbCode.Text = row.Cells[1].Value.ToString();
                tbName.Text = row.Cells[2].Value.ToString();
                dtpDateOfBirth.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                cbGender.Text = row.Cells[4].Value.ToString();
                tbClass.Text = row.Cells[5].Value.ToString();
                cbDepartment.Text = row.Cells[6].Value.ToString();
                cbArea.Text = row.Cells[7].Value.ToString();
                tbScore.Text = row.Cells[8].Value.ToString();
                tbPhone.Text = row.Cells[9].Value.ToString();
                tbAddress.Text = row.Cells[10].Value.ToString();
                string imageUrl = row.Cells[11].Value.ToString();
                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    pictureBox1.Image = Image.FromFile(imageUrl);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                



            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(tbId.Text))
            {
                MessageBox.Show("Please enter an ID.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbCode.Text))
            {
                MessageBox.Show("Please enter the student code.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Please enter the name.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbClass.Text))
            {
                MessageBox.Show("Please enter the class.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbDepartment.SelectedItem == null)
            {
                MessageBox.Show("Please select a department.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbArea.SelectedItem == null)
            {
                MessageBox.Show("Please select an area.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbGender.SelectedItem == null)
            {
                MessageBox.Show("Please select a gender.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbScore.Text))
            {
                MessageBox.Show("Please enter the score.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbPhone.Text))
            {
                MessageBox.Show("Please enter the phone number.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbAddress.Text))
            {
                MessageBox.Show("Please enter the address.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbImageUrl.Text))
            {
                MessageBox.Show("Please choose Image.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (tbCode.Text.Length != 10)
            {
                MessageBox.Show("Student code must be 10 characters long.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tbPhone.Text.Length != 10)
            {
                MessageBox.Show("Phone number must be 10 characters long.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem ID đã tồn tại hay chưa
            int id = int.Parse(tbId.Text);
            List<CustomerBEL> existingCustomers = cusBAL.ReadCustomer();
            if (existingCustomers.Any(c => c.Id == id))
            {
                MessageBox.Show("ID already exists.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            if (existingCustomers.Any(c => c.Codestu == tbCode.Text))
            {
                MessageBox.Show("Student code already exists. Please enter a different code.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CustomerBEL cus = new CustomerBEL();
            cus.Id = int.Parse(tbId.Text);
            cus.Codestu = tbCode.Text;
            cus.Name = tbName.Text;
            cus.DateOfBirth = dtpDateOfBirth.Value;
            cus.Gender = (GenderBEL)cbGender.SelectedItem;
            cus.Class = tbClass.Text;
            cus.Department = (DepartmentBEL)cbDepartment.SelectedItem;
            cus.Area = (AreaBEL)cbArea.SelectedItem;
            cus.Score = decimal.Parse(tbScore.Text);
            cus.Phone = tbPhone.Text;
            cus.Address = tbAddress.Text;
            if (!string.IsNullOrWhiteSpace(tbImageUrl.Text))
            {
                cus.ImageUrl = tbImageUrl.Text;  
            }
            


            cusBAL.NewCustomer(cus);
            dgvCustomer.Rows.Add(cus.Id, cus.Codestu, cus.Name, cus.DateOfBirth.ToString("dd/MM/yyyy"), 
                cus.Gender.Name, cus.Class, cus.Department.Name, cus.Area.Name, cus.Score, cus.Phone, cus.Address, cus.ImageUrl);
        }

        
        

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tbPhone.Text.Length != 10)
            {
                MessageBox.Show("Phone number must have 10 digits.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewRow row = dgvCustomer.CurrentRow;
            if (row != null)
            {
                CustomerBEL cus = new CustomerBEL();
                cus.Id = int.Parse(tbId.Text);
                cus.Codestu = tbCode.Text;
                cus.Name = tbName.Text;
                cus.DateOfBirth = dtpDateOfBirth.Value;
                cus.Gender = (GenderBEL)cbGender.SelectedItem;
                cus.Class = tbClass.Text;
                cus.Department = (DepartmentBEL)cbDepartment.SelectedItem;
                cus.Area = (AreaBEL)cbArea.SelectedItem;
                cus.Score = decimal.Parse(tbScore.Text);
                cus.Phone = tbPhone.Text;
                cus.Address = tbAddress.Text;
                if (!string.IsNullOrWhiteSpace(tbImageUrl.Text))
                {
                    cus.ImageUrl = tbImageUrl.Text;  
                }
                


                cusBAL.EditCustomer(cus);
                row.Cells[0].Value = cus.Id;
                row.Cells[1].Value = cus.Codestu;
                row.Cells[2].Value = cus.Name;
                row.Cells[3].Value = cus.DateOfBirth.ToString("dd/MM/yyyy");
                row.Cells[4].Value = cus.GenderName;
                row.Cells[5].Value = cus.Class;
                row.Cells[6].Value = cus.DepartmentName;
                row.Cells[7].Value = cus.AreaName;
                row.Cells[8].Value = cus.Score;
                row.Cells[9].Value = cus.Phone;
                row.Cells[10].Value = cus.Address;
                row.Cells[11].Value = cus.ImageUrl;
                




            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbId.Text))
            {
                MessageBox.Show("Please enter an ID to delete.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CustomerBEL cus = new CustomerBEL();
            cus.Id = int.Parse(tbId.Text); 
           

            cusBAL.DeleteCustomer(cus);
            int idx = dgvCustomer.CurrentCell.RowIndex;
            dgvCustomer.Rows.RemoveAt(idx);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1 fr01 = new Form1();
            fr01.Show();
            this.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = tbName.Text;
            string searchCodestu = tbCode.Text;

            if (!string.IsNullOrWhiteSpace(searchCodestu) && searchCodestu.Length != 10)
            {
                MessageBox.Show("Code Student must be exactly 10 characters.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<CustomerBEL> searchResults;

            if (!string.IsNullOrWhiteSpace(searchCodestu))
            {
                searchResults = cusBAL.SearchCustomersByCodestu(searchCodestu);
            }
            else if (!string.IsNullOrWhiteSpace(searchName))
            {
                searchResults = cusBAL.SearchCustomers(searchName);
            }
            else
            {
                MessageBox.Show("Please enter Code Student or Name.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvCustomer.Rows.Clear();
            foreach (CustomerBEL cus in searchResults)
            {
                dgvCustomer.Rows.Add(cus.Id, cus.Codestu, cus.Name, cus.DateOfBirth.ToString("dd/MM/yyyy"), cus.GenderName, 
                    cus.Class, cus.DepartmentName, cus.AreaName, cus.Score, cus.Phone, cus.Address, cus.ImageUrl);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            dgvCustomer.Rows.Clear();
            List<CustomerBEL> lstCus = cusBAL.ReadCustomer();
            foreach (CustomerBEL cus in lstCus)
            {
                dgvCustomer.Rows.Add(cus.Id, cus.Codestu, cus.Name, cus.DateOfBirth.ToString("dd/MM/yyyy"), cus.GenderName, cus.Class,
                    cus.DepartmentName, cus.AreaName, cus.Score, cus.Phone, cus.Address, cus.ImageUrl);
            }
        }



        private void btnImageUrl_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
               
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                
                tbImageUrl.Text = openFileDialog.FileName;  
            }
        }

        private void tbImageUrl_TextChanged(object sender, EventArgs e)
        {
            tbImageUrl.ReadOnly = true;
        }
    }
}
