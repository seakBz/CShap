﻿using QuanLyHocSinhTHPT.Controller;
using QuanLyHocSinhTHPT.Helper;
using QuanLyHocSinhTHPT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyHocSinhTHPT.View.VHocSinh
{
    public partial class frmThaoTacHS : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string _ID = null;
        Controller.HocSinhController hsc;

        public frmThaoTacHS(string id)
        {
            InitializeComponent();
            _ID = id;
            hsc = new Controller.HocSinhController();
            HocSinh hs = new HocSinh();
            //Xử lý mã lớp
            DataTable dt = sqlHelper.Query("SELECT Malop FROM Lop");
            List<DataRow> list = dt.AsEnumerable().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = list[i].ItemArray[0].ToString();
                item.Value = list[i].ItemArray[0].ToString();
                cbClass.Items.Add(item);
            }
            if (id == null)
            {
                txtMaHS.Enabled = true;
                return;
            }
            hs = hsc.GetData(id);

            txtMaHS.Text = hs.MaHS;
            txtHoTen.Text = hs.HoTen;
            DateTime _NgaySinh = (Convert.ToDateTime(hs.NgaySinh));
            dtNgaySinh.Value = _NgaySinh;
            if (hs.GioiTinh == "Nam")
            {
                ckNam.Checked = true;
            }
            else if (hs.GioiTinh == "Nữ")
                ckNu.Checked = true;
            txtTenBo.Text = hs.TenBo;
            txtTenMe.Text = hs.TenMe;
            txtQueQuan.Text = hs.QueQuan;
            lblTen.Text = hs.HoTen;
            string gt = null;
            if (hs.GioiTinh == "Nam")
            {
                gt = "Nam";
            }
            else if (hs.GioiTinh == "Nữ")
                gt = "Nữ";
            lblGoiTinh.Text = gt;
            int tuoi = DateTime.Now.Year - _NgaySinh.Year;
            lblTuoi.Text = tuoi.ToString() + " Tuổi";
            for (int i = 0; i < list.Count; i++)
            {
                if (hs.MaLop == list[i].ItemArray[0].ToString())
                {
                    cbClass.SelectedIndex = i;
                }
            }
            if(gt == "Nam")
            {
                picAvta.Image = Image.FromFile(Application.StartupPath + "\\Resources\\user1600.png");
            }
            else
            {
                picAvta.Image = Image.FromFile(Application.StartupPath + "\\Resources\\nanny-icon.png");

            }
          
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            hsc = new Controller.HocSinhController();
            int gt = 0;
            if (hsc.CheckErr(txtMaHS.Text.Trim(), txtHoTen.Text.Trim(), txtQueQuan.Text.Trim(), gt, dtNgaySinh.Value.ToString(), txtTenBo.Text.Trim(), txtTenMe.Text.Trim()))
            {
                // xử lú giới tính
             
                if(ckNam.Checked&& ckNu.Checked)
                {
                    MessageBox.Show("Chỉ Chọn một Giới Tính", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }else if(!ckNam.Checked&& !ckNu.Checked)
                {
                    MessageBox.Show("Vui lòng Chọn một Giới Tính", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                if (ckNam.Checked) gt = 0;
                if (ckNu.Checked) gt = 1;
                //xckNu.Checkedử lý ngày sinh
                string _ngaysinh = dtNgaySinh.Value.Year + "/" + dtNgaySinh.Value.Month + "/" + dtNgaySinh.Value.Day;
                // xử lý mã lớp
                DataTable dt = sqlHelper.Query("SELECT Malop FROM Lop");
                List<DataRow> list = dt.AsEnumerable().ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = list[i].ItemArray[0].ToString();
                    item.Value = list[i].ItemArray[0].ToString();
                    cbClass.Items.Add(item);
                }

                string malop = cbClass.Text;
                if (_ID == null)
                {
                    if (hsc.ThemHS(txtMaHS.Text.Trim(), txtHoTen.Text.Trim(), txtQueQuan.Text.Trim(), gt, _ngaysinh, txtTenBo.Text.Trim(), txtTenMe.Text.Trim(), malop))
                    {
                        MessageBox.Show("Thành Công", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (hsc.SuaHS(txtMaHS.Text.Trim(), txtHoTen.Text.Trim(), txtQueQuan.Text.Trim(), gt, _ngaysinh, txtTenBo.Text.Trim(), txtTenMe.Text.Trim(), malop))
                    {
                        MessageBox.Show("Thành Công", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void bbiSaveAndClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bbiSave_ItemClick(sender, e);
            this.Close();
        }

        private void bbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


    }
}
