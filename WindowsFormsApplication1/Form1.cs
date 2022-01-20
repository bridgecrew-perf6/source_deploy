using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;

            this.DragOver += new DragEventHandler(Form1_DragOver);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            comboBox1.SelectedIndex = 0;
            FirstSetXML();

        }
        public string Tb_Drive = "";
        public string Cb_Drive_Num1 = "";
        public string Cb_Drive_Num2 = "";
        public string Cb_Drive_Num3 = "";
        public string Cb_File_Drive = "";
        public string Cb_Img_Drive = "";

        static string dir_rt = @":\Mega_source\";
        static string[] ds = { "megamd_web", "megals_web", "megalawyers_web", "mdnp_web", "megaPSAT", "megaLand", "megaSsam", "megaExpert" };



        //서버경로 그룹
        static public string[] Front_Num1 = { dir_rt + ds[0] + @"\megamd\", dir_rt + ds[1] + @"\Megals2014\", dir_rt + ds[2] + @"\www\", dir_rt + ds[3] + @"\mdnp\", dir_rt + ds[4] + @"\www\", dir_rt + ds[5] + @"\www\", dir_rt + ds[6] + @"\www\", dir_rt + ds[7] + @"\www\" };
        static public string[] Front_Num2 = { dir_rt + ds[0] + @"\megamd\", dir_rt + ds[1] + @"\Megals2014\", dir_rt + ds[2] + @"\www\", dir_rt + ds[3] + @"\mdnp\", dir_rt + ds[4] + @"\www\", dir_rt + ds[5] + @"\www\", dir_rt + ds[6] + @"\www\", dir_rt + ds[7] + @"\www\" };
        static public string[] Admin = { dir_rt + ds[0] + @"\megaAdmin\", dir_rt + ds[1] + @"\megaAdmin\", dir_rt + ds[2] + @"\admin\", dir_rt + ds[3] + @"\megaAdmin\", dir_rt + ds[4] + @"\admin\", dir_rt + ds[5] + @"\admin\", dir_rt + ds[6] + @"\admin\", dir_rt + ds[7] + @"\admin\" };
        static public string[] Cs = { dir_rt + ds[0] + @"\megacs\", dir_rt + ds[1] + @"\Megals_cs\", dir_rt + ds[2] + @"\cs\", dir_rt + ds[3] + @"\megacs\", dir_rt + ds[4] + @"\cs\", dir_rt + ds[5] + @"\cs\", dir_rt + ds[6] + @"\cs\", dir_rt + ds[7] + @"\cs\" };
        static public string[] campus_Num1 = { dir_rt + ds[0] + @"\megamd_campus\", "", "", "", "", "", "", "" };
        static public string[] campus_Num2 = { dir_rt + ds[0] + @"\megamd_campus\", "", "", "", "", "", "", "" };
        static public string[] Mobile_Num1 = { dir_rt + ds[0] + @"\megamd_m_v2\", dir_rt + ds[1] + @"\Megals_mobile_v2\", dir_rt + ds[2] + @"\m_v2\", dir_rt + ds[3] + @"\mdnp_m\", dir_rt + ds[4] + @"\m\", dir_rt + ds[5] + @"\m\", dir_rt + ds[6] + @"\m\", dir_rt + ds[7] + @"\m\" };
        static public string[] Mobile_Num2 = { dir_rt + ds[0] + @"\megamd_m_v2\", dir_rt + ds[1] + @"\Megals_mobile_v2\", dir_rt + ds[2] + @"\m_v2\", dir_rt + ds[3] + @"\mdnp_m\", dir_rt + ds[4] + @"\m\", dir_rt + ds[5] + @"\m\", dir_rt + ds[6] + @"\m\", dir_rt + ds[7] + @"\m\" };
        static public string[] Schams_Num1 = { dir_rt + ds[0] + @"\megamd_schams\", dir_rt + ds[1] + @"\Megals_schams\", "", "", "", "", "", "" };
        static public string[] Schams_Num2 = { dir_rt + ds[0] + @"\megamd_schams\", dir_rt + ds[1] + @"\Megals_schams\", "", "", "", "", "", "" };
        static public string[] Pay = { dir_rt + ds[0] + @"\megapay\", dir_rt + ds[1] + @"\Megapay\", dir_rt + ds[2] + @"\pay\", dir_rt + ds[3] + @"\megapay\", dir_rt + ds[4] + @"\pay\", dir_rt + ds[5] + @"\pay\", dir_rt + ds[6] + @"\pay\", dir_rt + ds[7] + @"\pay\" };
        static public string[] Tzone = { dir_rt + ds[0] + @"\megatzone\", dir_rt + ds[1] + @"\Megatzone\", dir_rt + ds[2] + @"\tz\", dir_rt + ds[3] + @"\megatzone\", dir_rt + ds[4] + @"\tz\", dir_rt + ds[5] + @"\tz\", dir_rt + ds[6] + @"\tz\", dir_rt + ds[7] + @"\tz\" };

        //Diff 툴 
        public string Diff_tool_dir = "";

        //CB 소스 백업

        public string Back_dir = "";

        //파일 복사 flg
        public Boolean file_copy_flg = true;

        //읽기 전용 flg
        public bool isReadonly = false;

        // 선택된 사이트 이름(라디오버튼)
        public string radio_text;
        
        // 선택된 사이트 이름(라디오버튼-영문)
        public string site_name;

        // 선택된 사이트 반영용 서버명
        public string Server_Num1;
        public string Server_Num2;

        // 배포할 사이트 구분 ( 1,2번 서버에 모두 할지 , 1번 서버만 할지 1,2번 : both , 1번 : only )
        public string dist_mode;

        public void radioButton(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                radio_text = ((RadioButton)sender).Text;
            string[] tempstr = radio_text.Split('-');
            radio_text = tempstr[0].Trim();
        }

        public static void SetFileReadAccess(string FileName, bool SetReadOnly)
        {
            // Create a new FileInfo object.
            FileInfo fInfo = new FileInfo(FileName);

            // Set the IsReadOnly property.
            fInfo.IsReadOnly = SetReadOnly;

        }

        // Returns wether a file is read-only.
        public static bool IsFileReadOnly(string FileName)
        {
            // Create a new FileInfo object.
            FileInfo fInfo = new FileInfo(FileName);

            // Return the IsReadOnly property value.
            return fInfo.IsReadOnly;

        }


        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            //dataGridView1.Rows.Clear();

            if (files != null)
            {
                foreach (string file in files)
                {
                    string[] file_t = file.Split('\\');
                    dataGridView1.Rows.Add(file.Substring(file.IndexOf(file_t[2]), (file.Length - (file.IndexOf(file_t[2])))));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) // Clear 버튼
        {
            dataGridView1.Rows.Clear();
        }
        public String Radio_Btn_Swt(string y, int x)
        {
            site_name = y;
            string temp_str = "";
            if (radio_text == "프론트")
            {
                temp_str = y + "_WEB1 , " + y + "_WEB2";
                dist_mode = "both";
                Server_Num1 = Front_Num1[x];
                Server_Num2 = Front_Num2[x];
            }
            else if (radio_text == "관리자")
            {
                temp_str = y + "_ADMIN";
                dist_mode = "only";
                Server_Num1 = Admin[x];
            }
            else if (radio_text == "상담툴")
            {
                temp_str = y + "_CS";
                dist_mode = "only";
                Server_Num1 = Cs[x];
            }
            else if (radio_text == "캠퍼스(양지,서초")
            {
                temp_str = y + "_CAMPUS1 , " + y + "_CAMPUS2";
                dist_mode = "both";
                Server_Num1 = campus_Num1[x];
                Server_Num2 = campus_Num2[x];
            }
            else if (radio_text == "모바일")
            {
                temp_str = y + "_M1 , " + y + "_M2";
                dist_mode = "both";
                Server_Num1 = Mobile_Num1[x];
                Server_Num2 = Mobile_Num2[x];
            }
            else if (radio_text == "수납툴")
            {
                temp_str = y + "_PAY";
                dist_mode = "only";
                Server_Num1 = Pay[x];
            }
            else if (radio_text == "티존")
            {
                temp_str = y + "_TZONE";
                dist_mode = "only";
                Server_Num1 = Tzone[x];
            }
            else if (radio_text == "풀서비스")
            {
                temp_str = y + "_SCHAMS1 , " + y + "_SCHAMS2";
                dist_mode = "both";
                Server_Num1 = Schams_Num1[x];
                Server_Num2 = Schams_Num2[x];
            }
            return temp_str;
        }
        private void Radio_Btn_Clk(object sender, MouseEventArgs e) // 프론트 사이트 RADIO 선택 시
        {
            int datagrid_count = dataGridView1.Rows.Count;
            if (datagrid_count > 0)
            {
                int counter = 0;

                for (var i = 0; i < datagrid_count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Selected == true)
                    {
                        switch (comboBox1.SelectedIndex)
                        {
                            case 0: //MD
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("MD",0);
                                DataGridViewButtonCell bCell = new DataGridViewButtonCell();
                                bCell.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell;

                                break;
                            case 1: //LS
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("LS", 1);
                                DataGridViewButtonCell bCell1 = new DataGridViewButtonCell();
                                bCell1.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell1;
                                break;
                            case 2: //LW
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("LW", 2);
                                DataGridViewButtonCell bCell2 = new DataGridViewButtonCell();
                                bCell2.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell2;
                                break;
                            case 3: //MDNP
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("MDNP", 3);
                                DataGridViewButtonCell bCell3 = new DataGridViewButtonCell();
                                bCell3.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell3;
                                break;
                            case 4: //PSAT
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("PSAT", 4);
                                DataGridViewButtonCell bCell4 = new DataGridViewButtonCell();
                                bCell4.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell4;
                                break;
                            case 5: //LAND
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("LAND", 5);
                                DataGridViewButtonCell bCell5 = new DataGridViewButtonCell();
                                bCell5.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell5;
                                break;
                            case 6: //SSAM
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("SSAM", 6);
                                DataGridViewButtonCell bCell6 = new DataGridViewButtonCell();
                                bCell6.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell6;
                                break;
                            case 7: //EXPERT
                                dataGridView1.Rows[i].Cells[1].Value = Radio_Btn_Swt("EXPERT", 7);
                                DataGridViewButtonCell bCell7 = new DataGridViewButtonCell();
                                bCell7.Value = "비교";
                                dataGridView1.Rows[i].Cells[2] = bCell7;
                                break;
                        }
                        counter = counter + 1;
                    }
                }
                if (counter == 0)
                {
                    MessageBox.Show("작업파일을 선택해 주세요.");
                }
            }
            else
            {
                MessageBox.Show("작업파일을 추가해 주세요.");
            }
        }

        private void button2_Click(object sender, EventArgs e) // 소스 배포 버튼
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int datagrid_count = dataGridView1.Rows.Count;
                int cell_cnt = 0;
                for (int counter = 0; counter < datagrid_count; counter++)
                {
                    try
                    {
                        if (dataGridView1.Rows[counter].Cells[1].Value.ToString() != "")
                        {
                            cell_cnt = cell_cnt + 1;
                        }
                    }
                    catch { }
                }
                if (cell_cnt > 0)
                {
                    Source_distribute();
                }
                else
                {
                    MessageBox.Show("배포할 파일이 없습니다.");
                }
            }
        }
        private void Source_distribute() // 배포 함수
        {
            int datagrid_cnt = dataGridView1.Rows.Count;
            string copy_source = "";
            string copy_dest = "";
            string copy_dest2 = "";
            string back_dest = "";
            string back_dest_txt = "";
            string back_dest_file = "";
            string Back_dir_name = "";
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                Back_dir_name = DateTime.Now.Year.ToString() + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                Back_dir_name = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
            }
            if (DateTime.Now.Day.ToString().Length == 1)
            {
                Back_dir_name += "0" + DateTime.Now.Day.ToString();
            }
            else
            {
                Back_dir_name += DateTime.Now.Day.ToString();

            }
            if (DateTime.Now.Hour.ToString().Length == 1)
            {
                Back_dir_name += "_0" + DateTime.Now.Hour.ToString();
            }
            else
            {
                Back_dir_name += "_" + DateTime.Now.Hour.ToString();
            }
            if (DateTime.Now.Minute.ToString().Length == 1)
            {
                Back_dir_name += "0" + DateTime.Now.Minute.ToString();
            }
            else
            {
                Back_dir_name += DateTime.Now.Minute.ToString();
            }

            back_dest_txt = @Back_dir + "\\" + Back_dir_name + "\\";
            for (int count = 0; count < datagrid_cnt - 1; count++)
            {
                string full_file = dataGridView1.Rows[count].Cells[0].Value.ToString();
                string[] file_t = full_file.Split('\\');
                copy_source = Tb_Drive + @":\mega_source\" + full_file;

                int file_t_cc = file_t.Count() - 1;
                int file_t_c = int.Parse(full_file.IndexOf(file_t[2]).ToString());
                int file_t_c_1 = int.Parse(full_file.IndexOf(file_t[file_t_cc]).ToString());

                back_dest = @Back_dir + "\\" + Back_dir_name + "\\" + full_file.Substring(0, file_t_c_1);
                back_dest_file = @Back_dir + "\\" + Back_dir_name + "\\" + full_file;
                DirectoryInfo di = new DirectoryInfo(back_dest);
                if (di.Exists == false)
                {
                    di.Create();
                }
                if (dist_mode == "both") // 1,2번 서버 배포 (프론트 , 모바일 , 캠퍼스 , 풀서비스 ) 
                {
                    try
                    {
                        copy_dest = Cb_Drive_Num1 + Server_Num1 + (full_file.Substring(file_t_c, (full_file.Length - file_t_c)));
                        copy_dest2 = Cb_Drive_Num2 + Server_Num2 + (full_file.Substring(file_t_c, (full_file.Length - file_t_c)));
                        FileInfo fi = new FileInfo(@copy_dest);
                        //FileInfo fi2 = new FileInfo(@copy_dest + "\\" + full_file);
                        //if (fi.Exists != false && fi2.Exists != false)
                        if (fi.Exists != false)
                        {
                            File.Copy(@copy_dest, @back_dest_file, true);

                        }
                        if (IsFileReadOnly(@copy_dest) == true && fi.Exists != false)
                        {
                            SetFileReadAccess(@copy_dest, false);
                        }
                        if (IsFileReadOnly(@copy_dest2) == true && fi.Exists != false)
                        {
                            SetFileReadAccess(@copy_dest2, false);
                        }

                        File.Copy(@copy_source, @copy_dest, true);
                        File.Copy(@copy_source, @copy_dest2, true);
                    }
                    catch (Exception ex)
                    {
                        file_copy_flg = false;
                        System.IO.File.WriteAllText(@back_dest_txt + "error_list.txt", ex.ToString() + "\r\n");
                    }
                }
                else if (dist_mode == "only") // 1번 서버 배포 (관리자, 상담툴, 수납툴, 티존) 
                {
                    try
                    {
                        if (dataGridView1.Rows[count].Cells[1].Value.ToString().Contains("_TZONE") == true)
                        {
                            copy_dest = Cb_Drive_Num1 + Server_Num1 + (full_file.Substring(file_t_c, (full_file.Length - file_t_c)));
                        }
                        else
                        {
                            copy_dest = Cb_Drive_Num3 + Server_Num1 + (full_file.Substring(file_t_c, (full_file.Length - file_t_c)));
                        }

                        FileInfo fi = new FileInfo(@copy_dest);
                        if (fi.Exists != false)
                        {
                            File.Copy(@copy_dest, @back_dest_file, true);
                        }
                        if (IsFileReadOnly(@copy_dest) == true && fi.Exists != false)
                        {
                            SetFileReadAccess(@copy_dest, false);
                        }
                        File.Copy(@copy_source, @copy_dest, true);
                    }
                    catch (Exception ex)
                    {
                        file_copy_flg = false;
                        System.IO.File.WriteAllText(@back_dest_txt + "error_list.txt", ex.ToString() + "\r\n");
                    }
                }
            }
            try
            {
                string dataview_text = "";
                for (int count = 0; count < datagrid_cnt - 1; count++)
                {
                    dataview_text += dataGridView1.Rows[count].Cells[0].Value.ToString() + "|" + dataGridView1.Rows[count].Cells[1].Value.ToString()+"\r\n";
                }
                System.IO.File.WriteAllText(@back_dest_txt + "file_list.txt", dataview_text.ToString());
            }
            catch 
            { 
            }
            if (file_copy_flg == false)
            {
                MessageBox.Show("배포중 오류 발생");
                Process.Start(@back_dest_txt + "error_list.txt");
            }
            else
            {
                MessageBox.Show("배포가 완료되었습니다.");
            }
            

        }
        private void Diff_tool(string sender1, string sender2)
        {
             if (sender1 != "" && sender2 != "")
             {
                FileInfo fi1 = new FileInfo(sender1);
                FileInfo fi2 = new FileInfo(sender2);
                if (fi1.Exists && fi2.Exists)
                {
                    Process.Start(@Diff_tool_dir, sender1 + " " + sender2);
                }
                else
                {
                    MessageBox.Show("비교 대상이 없습니다(파일 존재하지 않음).");
                }
            }
            else
            {
                MessageBox.Show("비교 대상이 없습니다(입력대상 없음).");
            }                        
        }
        private void datagridview1_CellContentclick(object sender, DataGridViewCellEventArgs e)
        {
            var sendderGrid = (DataGridView)sender;
            int datagrid_count = dataGridView1.Rows.Count;
            string copy_source = "";
            string copy_dest = "";
            var sendergrid = (DataGridView)sender;
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "비교" && e.ColumnIndex == 2)
                {
                    string full_file = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string[] file_t = full_file.Split('\\');
                    copy_source = Tb_Drive + @":\mega_source\" + full_file;
                    int file_t_cc = file_t.Count() - 1;
                    int file_t_c = int.Parse(full_file.IndexOf(file_t[2]).ToString());

                    try
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Contains("_ADMIN") == true || dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Contains("_CS") == true || dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Contains("_PAY") == true)
                        {
                            copy_dest = Cb_Drive_Num3 + Server_Num1 + (full_file.Substring(file_t_c, (full_file.Length - file_t_c)));
                            Diff_tool(copy_source, copy_dest);
                        }
                        else
                        {
                            copy_dest = Cb_Drive_Num1 + Server_Num1 + (full_file.Substring(file_t_c, (full_file.Length - file_t_c)));
                            Diff_tool(copy_source, copy_dest);
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        public string GetIniValue(String Section, String Key)
        {
            string path = "c:\\config.ini";
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileString(Section, Key, "", temp, 1024, path);
            return temp.ToString();
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: //MD
                    radioButton5.Show();
                    radioButton7.Show();
                    break;
                case 1: //LS
                    radioButton5.Hide();
                    radioButton7.Show();
                    break;
                default:  //기타
                    radioButton5.Hide();
                    radioButton7.Hide();
                    break;                
            }                 
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {           
                dataGridView1.Rows.RemoveAt(row.Index);                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 showForm = new Form2();
            showForm.Show();
        }
        private void FirstSetXML()
        {
            string updrivename = "";
            string cbdrivename1 = "";
            string cbdrivename2 = "";
            string cbdrivename3 = "";
            
            string difftool = "";
            string backupdirectory = "";

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(@"XMLFile1.xml");
            XmlNode FirstNode = XmlDoc.DocumentElement;
            XmlNodeList SubNode = XmlDoc.SelectNodes("configvalue");


            updrivename = SubNode[0].ChildNodes[0].InnerXml.ToString();
            cbdrivename1 = SubNode[0].ChildNodes[1].InnerXml.ToString();
            cbdrivename2 = SubNode[0].ChildNodes[2].InnerXml.ToString();
            cbdrivename3 = SubNode[0].ChildNodes[5].InnerXml.ToString();
            
            difftool = SubNode[0].ChildNodes[3].InnerXml.ToString();
            backupdirectory = SubNode[0].ChildNodes[4].InnerXml.ToString();

            Tb_Drive = updrivename.Substring(0, 1);
            Cb_Drive_Num1 = cbdrivename1.Substring(0,1);
            Cb_Drive_Num2 = cbdrivename2.Substring(0,1);
            Cb_Drive_Num3 = cbdrivename3.Substring(0,1);
            
            Diff_tool_dir = difftool;
            Back_dir = backupdirectory;

        }
    }           
}
    
