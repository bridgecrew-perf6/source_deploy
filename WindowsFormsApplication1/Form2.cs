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


namespace WindowsFormsApplication1
{    
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            comboBox1.DataSource = Environment.GetLogicalDrives();
            comboBox2.DataSource = Environment.GetLogicalDrives();
            comboBox3.DataSource = Environment.GetLogicalDrives();
            comboBox4.DataSource = Environment.GetLogicalDrives();
            FirstSetXML();
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
            
            
            if (updrivename != "")
            {                
                comboBox1.SelectedIndex = comboBox1.FindStringExact(updrivename);
            }
            if (cbdrivename1 != "")
            {
                comboBox2.SelectedIndex = comboBox2.FindStringExact(cbdrivename1);
            }
            if (cbdrivename2 != "")
            {
                comboBox3.SelectedIndex = comboBox3.FindStringExact(cbdrivename2);
            }
            if (cbdrivename3 != "")
            {
                comboBox4.SelectedIndex = comboBox4.FindStringExact(cbdrivename3);
            }
           
            textBox4.Text = difftool.ToString();
            textBox5.Text = backupdirectory.ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "EXE File(*.exe)|*.exe";
            openfile.InitialDirectory = @"C:\";
            openfile.Title = "소스비교툴 실행파일을 선택.";

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                this.textBox4.Text = openfile.FileName;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox5.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("설정을 저장 하시겠습니까?","" , MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ModifyXml();                
                this.Close();
            }
        }
        private void ModifyXml()
        {            
            try{
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                XmlWriter xmlWriter = XmlWriter.Create(@"XMLFile1.xml");
                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement("configvalue");
                xmlWriter.WriteElementString("updridevname", this.comboBox1.SelectedValue.ToString());
                xmlWriter.WriteElementString("cbdrivename1", this.comboBox2.SelectedValue.ToString());
                xmlWriter.WriteElementString("cbdrivename2", this.comboBox3.SelectedValue.ToString());
                xmlWriter.WriteElementString("cbdrivename3", this.comboBox4.SelectedValue.ToString());
                
                xmlWriter.WriteElementString("difftool", this.textBox4.Text.ToString());
                xmlWriter.WriteElementString("backupdirectory", this.textBox5.Text.ToString());

                xmlWriter.Flush();
                xmlWriter.Close();
                MessageBox.Show("저장완료");
            }catch(Exception ex){                
                MessageBox.Show("오류="+ex.Message);
                //return false;
            }
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
