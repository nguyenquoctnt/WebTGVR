using AutoSendMailMuti.TeampleateVe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDMMutiline.Core;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.Ultilities;

namespace AutoSendMailMuti
{
    public partial class Form1 : Form
    {
        private string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["Vdm_MutilineConnectionString"].ConnectionString ?? "";
        private string startDate = ConfigurationManager.AppSettings["startDate"];

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            txtmailhuydagui.Enabled = false;
            txtsomaildagui.Enabled = false;
            txtmailcamom.Enabled = false;
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        private bool checkgui(string userid)
        {
            //if (userid.Trim().ToUpper() == "KL")
            //{
            //    return true;
            //}
            //else
            //{
            //    var daouser = new AspNetUserDao();
            //    var objuser = daouser.GetInfoByLoginName(userid.Trim());
            //    if (objuser != null)
            //    {
            //        if (objuser.GroupId == 2)
            //            return true;
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return true;
            //    }

            //}
            return true;
        }
        int mailfail = 1;
        int mailnhac = 1;
        int mailcamon = 1;
        #region send mail hủy vé
        private void GuimailFail()
        {
            var dao = new BK_BookingDao();

            var list = dao.VeHetHanGiuChoGuiMail(Utility.ConvertToDate(startDate)).ToList();
            foreach (var danhSachVeInfo in list)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var obj = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == danhSachVeInfo.ID);
                    if (obj != null)
                    {
                        try
                        {
                            if (checkgui(obj.UserId) && !string.IsNullOrEmpty(obj.BookCode))
                            {
                                sendmail(obj.ID, "Hủy", Constant.TemPleateHTMLID.HuyVe);
                            }
                            obj.HetHan = true;
                            obj.Status = Constant.StatusVe.DaHuyVe;
                            dbContext.SubmitChanges();
                            txtmailhuydagui.Text = mailfail.ToString();
                            mailfail++;
                        }
                        catch (Exception ex)
                        {
                            txtnote.Text += (obj.BookCode + " - " + ex.Message);
                        }
                    }
                }

            }
        }
        #endregion
        #region send mail nhắc 
        private void Guimailnhac()
        {
            try
            {
                var dao = new BK_BookingDao();
                var list = dao.NhacLichbay(Utility.ConvertToDate(startDate)).Where(z => !string.IsNullOrEmpty(z.BookCode));
                foreach (var danhSachVeInfo in list)
                {
                    using (var dbContext = new ContextDataContext(ConnectionString))
                    {
                        var obj = dbContext.BK_Tickets.FirstOrDefault(z => z.ID == danhSachVeInfo.ticketid);
                        if (obj != null)
                        {
                            try
                            {
                                obj.DaGuiMaiNhac = true;
                                dbContext.SubmitChanges();
                                txtsomaildagui.Text = mailnhac.ToString();
                                mailnhac++;
                                sendmail(danhSachVeInfo.ID, "CHUYẾN BAY SẮP TỚI", Constant.TemPleateHTMLID.VeSapBay, (danhSachVeInfo.Startdate.HasValue ? danhSachVeInfo.Startdate.Value.ToString("dd/MM/yyyy HH:mm") : ""));
                            }
                            catch (Exception ex)
                            {
                                txtnote.Text += (obj.CodeBook + " - " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        #endregion
        #region send mail nhắc 
        private void Guimailcamon()
        {
            try
            {
                var dao = new BK_BookingDao();
                var list = dao.ChuyenBayKetthuc(Utility.ConvertToDate(startDate)).Where(z => !string.IsNullOrEmpty(z.BookCode));
                foreach (var danhSachVeInfo in list)
                {
                    using (var dbContext = new ContextDataContext(ConnectionString))
                    {
                        var obj = dbContext.BK_Tickets.FirstOrDefault(z => z.ID == danhSachVeInfo.ticketid);
                        if (obj != null)
                        {
                            try
                            {
                                obj.Daguimailcamon = true;
                                dbContext.SubmitChanges();
                                txtmailcamom.Text = mailcamon.ToString();
                                mailcamon++;
                                sendmail(danhSachVeInfo.ID, "HOÀN TẤT HÀNH TRÌNH", Constant.TemPleateHTMLID.HoanTatHanhTrinh, (danhSachVeInfo.Startdate.HasValue ? danhSachVeInfo.Startdate.Value.ToString("dd/MM/yyyy HH:mm") : ""));
                            }
                            catch (Exception ex)
                            {
                                txtnote.Text += (obj.CodeBook + " - " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        #endregion
        public void sendmail(int Bookid, string titel, int type)
        {
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(Bookid);
            if (inforbk != null)
            {
                var listseting = CommonUI.GetSettingByUser(inforbk.UserCreate, true);
                var tieude = titel + " | [" + inforbk.BookCode + "] | " +
                             inforbk.Name +
                             " | " + inforbk.FromCity + " " + inforbk.ToCity;
                var list = new List<MailAddress> { new MailAddress(inforbk.Email) };
                var userInfo = CommonUI.GetInforByUserOrUserParent(inforbk.UserCreate);
                if (userInfo != null)
                {
                    var html = RenderTeamPletave.Teampleateve(inforbk, listseting, type, userInfo.Id);
                    string filename = "";
                    filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                    filename += "_" + inforbk.BookCode;
                    filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                    filename += "_" + inforbk.FromCity + inforbk.ToCity;
                    filename += ".pdf";
                    var export = new ExportPDF();
                    string htmlPDF = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, UIUty.GetIdPdfTempleate(inforbk), userInfo.Id);
                    var file = export.ExportPdfReturnStream(htmlPDF);
                    var att = new Attachment(file, filename, "application/pdf");
                    var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                    var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                    var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                    var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");
                    if (!string.IsNullOrEmpty(Host) && !string.IsNullOrEmpty(port) && !string.IsNullOrEmpty(fromAddress) && !string.IsNullOrEmpty(fromPassword))
                        MailUtily.sendmail(listseting, tieude, html, list, att,
                                    fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                }
            }

        }
        public void sendmail(int Bookid, string titel, int type, string Ngaybay)
        {
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(Bookid);
            if (inforbk != null)
            {
                var listseting = CommonUI.GetSettingByUser(inforbk.UserCreate, true);
                var tieude = titel + " | [" + inforbk.BookCode + "] | " +
                             inforbk.Name +
                             " | " + inforbk.FromCity + " " + inforbk.ToCity;
                var list = new List<MailAddress> { new MailAddress(inforbk.Email) };
                var userInfo = CommonUI.GetInforByUserOrUserParent(inforbk.UserCreate);
                if (userInfo != null)
                {
                    var html = RenderTeamPletave.Teampleateve(inforbk, listseting, type, userInfo.Id, Ngaybay);
                    string filename = "";
                    filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                    filename += "_" + inforbk.BookCode;
                    filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                    filename += "_" + inforbk.FromCity + inforbk.ToCity;
                    filename += ".pdf";
                    var export = new ExportPDF();
                    string htmlPDF = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, UIUty.GetIdPdfTempleate(inforbk), userInfo.Id);
                    var file = export.ExportPdfReturnStream(htmlPDF);
                    var att = new Attachment(file, filename, "application/pdf");
                    var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                    var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                    var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                    var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");
                    if (!string.IsNullOrEmpty(Host) && !string.IsNullOrEmpty(port) && !string.IsNullOrEmpty(fromAddress) && !string.IsNullOrEmpty(fromPassword))
                        MailUtily.sendmail(listseting, tieude, html, list, att,
                                fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                }
            }

        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimeAuto.Stop();
        }
        private void backgroundWorkerFail_DoWork(object sender, DoWorkEventArgs e)
        {
            GuimailFail();
        }
        private void backgroundWorkerSapBay_DoWork(object sender, DoWorkEventArgs e)
        {
            Guimailnhac();
        }
        private void camonw_DoWork(object sender, DoWorkEventArgs e)
        {
            Guimailcamon();
        }
        private void btnbatdat_Click(object sender, EventArgs e)
        {
            if (txtthoigianquetlaplai.Text != "")
            {
                btnbatdat.Enabled = false;
                txtthoigianquetlaplai.Enabled = false;
                btnketthuc.Enabled = true;
                TimeAuto.Interval = int.Parse(txtthoigianquetlaplai.Text);
                TimeAuto.Start();
            }
        }
        private void btnketthuc_Click(object sender, EventArgs e)
        {
            if (txtthoigianquetlaplai.Text != "")
            {
                btnbatdat.Enabled = true;
                txtthoigianquetlaplai.Enabled = true;
                btnketthuc.Enabled = false;
                TimeAuto.Stop();
            }
        }
        private void TimeAuto_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorkerFail.IsBusy)
                backgroundWorkerFail.RunWorkerAsync();
            if (!backgroundWorkerSapBay.IsBusy)
                backgroundWorkerSapBay.RunWorkerAsync();
            if (!camonw.IsBusy)
                camonw.RunWorkerAsync();
        }
    }
}
