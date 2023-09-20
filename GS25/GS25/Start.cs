using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS25
{
    public partial class Start : MetroFramework.Forms.MetroForm
    {
        public Start()
        {
            InitializeComponent();
        }
        //이어하기
        private void btn_continue_Click(object sender, EventArgs e)
        {
            if (File.Exists("DBkind.txt"))
            {
                string DB_="";
                using (StreamReader sw = new StreamReader(new FileStream("DBkind.txt", FileMode.Open)))
                {
                    DB_=sw.ReadLine();

                    System.Diagnostics.Debug.WriteLine(DB_);
                }

                using (Main mainform = new Main(DB_))
                {
                    this.Hide();

                    mainform.ShowDialog();

                    this.Show();

                }
            }
            else
            {
                MessageBox.Show("저장된 데이터가 없습니다. 새로하기를 선택해주세요");
            }
        }
        //새로하기
        private void btn_new_Click(object sender, EventArgs e)
        {
            string DB_="";

            if(radio_lite.Checked || radio_sql.Checked)
            {
                //DB연결, 테이블 생성
                if (radio_lite.Checked)
                {
                    //sqlite 버튼을 선택했을시
                    DB_=radio_lite.Text;

                    DBsqlite db_lite = new DBsqlite();
                    db_lite.DbTable();

                }
                else if(radio_sql.Checked)
                {
                    //mysql을 선택했을시
                    DB_=radio_sql.Text;

                    DBmysql db_lite = new DBmysql();
                    db_lite.DbTable();

        
                }
                //선택한 종류 파일에 쓰기.
                using (StreamWriter sw = new StreamWriter(new FileStream("DBkind.txt", FileMode.Create)))
                {
                    sw.WriteLine(DB_);
                }
        
               
                
                using (Main mainform = new Main(DB_))
                {
                    this.Hide();

                    mainform.ShowDialog();

                    this.Show();
                }

            }
            else
            {
                MessageBox.Show("사용하실 DB를 선택해주세요.");
            }

        }
    }
}
