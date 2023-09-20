using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GS25
{
    
    public partial class Main : MetroFramework.Forms.MetroForm
    {
   
        IDB DBconnect=null;
        int selectedRowIndex;
        public Main(string DB_)
        {

            InitializeComponent();

            if(DB_=="MYSQL")
            {
                 DBconnect = new DBmysql();
            }
            else if(DB_=="SQLITE")
            {
                 DBconnect = new DBsqlite();
            }


            //날짜설정, 금액설정
            string today_day = DBconnect.DbgetDate();
            if (today_day == null)
            {
                label_date.Text = "2023-09-01";
            }
            else
            {
                label_date.Text = today_day;
            }

            string today_money = DBconnect.DbgetMoney();
            if (today_money == null)
            {
                label_money.Text = "100000";
            }
            else
            {
                label_money.Text = today_money;
            }
            //시작할때 보이는 재고페이지 탭.
            metroTabControl1.SelectedTab = metroTabPage1;


        }
        public int change_int(string str,string type)
        {
            int num;
            if (int.TryParse(str, out num))
            {
                if (num < 0)
                {
                    MessageBox.Show($"{type}:정수만 입력해주세요");
                    return -1;
                }
                return num;
            }
            else
            {
                MessageBox.Show($"{type}:숫자만 입력해주세요");
                return -1;
            }

        }

        private void btn_addproduct_Click(object sender, EventArgs e)
        {
            //상품정보 db에 저장
       
            if((txt_cost.Text=="")||(txt_expiry.Text=="")||(txt_name.Text=="")||(txt_price.Text==""))
            {
                MessageBox.Show("정보를 입력해주세요.");
            }
            else
            {
                string name = txt_name.Text;
                int cost = change_int(txt_cost.Text,"cost");
                if (cost == -1) return;
                int price = change_int(txt_price.Text, "price");
                if (price == -1) return;
                int expiry = change_int(txt_expiry.Text, "expiry");
                if (expiry == -1) return;

                DBconnect.DbAddproduct(name, cost, price, expiry);

                MessageBox.Show("상품이 등록되었습니다.");

                txt_cost.Clear();
                txt_expiry.Clear();
                txt_name.Clear();   
                txt_price.Clear();

                DataSet dataset = DBconnect.Dbgetproduct();

                dataGridView2.DataSource = dataset.Tables[0];


            }

    
        }


        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            // DataGridView에서 선택한 행의 인덱스 가져오기
            try
            {
                selectedRowIndex = dataGridView3.SelectedCells[0].RowIndex;

                // 선택한 행의 이름 열의 값을 가져와서 TextBox에 설정
                if (dataGridView3.Rows[selectedRowIndex].Cells[0].Value != null)
                {
                    txt_ordername.Text = dataGridView3.Rows[selectedRowIndex].Cells[0].Value.ToString();
                }
                else
                {
                    // 선택한 행의 값이 null인 경우 TextBox를 비웁니다.
                    txt_ordername.Text = string.Empty;
                }
            }
            catch
            {
                txt_ordername.Text = string.Empty;
            }
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = metroTabControl1.SelectedIndex;

            // 원하는 탭의 인덱스를 확인하여 특정 탭이 클릭되었을 때 동작
            if (selectedIndex == 3) // 네번째 탭 >> 발주
            {
                // 특정 탭이 클릭되었을 때 실행할 코드 추가
                DataSet dataset = DBconnect.Dbgetproduct();

                dataGridView3.DataSource = dataset.Tables[0];
            }
            else if(selectedIndex ==1) //등록
            {
                DataSet dataset = DBconnect.Dbgetproduct();

                dataGridView2.DataSource = dataset.Tables[0];
            }
            else if(selectedIndex ==0) //재고
            {
                DataSet dataset = DBconnect.Dbgetinven(label_date.Text);
              
                dataGridView1.DataSource = dataset.Tables[0];
                
            }

        }

        private void btn_order_Click(object sender, EventArgs e)
        {
            if(txt_ordername.Text=="")
            {
                MessageBox.Show("주문하실 상품을 선택해주세요");
            }
            else if(txt_ordernumber.Text=="")
            {
                MessageBox.Show("주문하실 상품의 개수를 입력해주세요.");
            }
            else
            {
                int number = change_int(txt_ordernumber.Text, "개수");
                if (number == -1) return;

                // 데이터베이스에 주문내역저장. 타입은 "재고"
                // 이름 txt_ordername.Text,갯수txt_ordernumber.Text,오늘날짜label_date.Text,type = 재고,폐기일?

                //같은날 중복주문 못하게 
                if (DBconnect.Dbexistinven(label_date.Text,txt_ordername.Text))
                {
                    MessageBox.Show("이미 주문하신 상품입니다. 다음 날 주문해주세요.");
                }
                else
                {
                    //물품 입고가격
                    int cost_product = Convert.ToInt32(dataGridView3.Rows[selectedRowIndex].Cells[1].Value);

                    if (Convert.ToInt32(label_money.Text) < cost_product * number)
                    {
                        MessageBox.Show("보유금액이 부족합니다.");
                    }
                    else
                    {
                        //금액 변동
                        label_money.Text=(Convert.ToInt32(label_money.Text)- cost_product * number).ToString();
                        DBconnect.DbsetMoney(Convert.ToInt32(label_money.Text),label_date.Text,"구매", (-1*cost_product * number));
                        //db에 저장
                        DBconnect.DbrecordSave(txt_ordername.Text, number, label_date.Text, "재고");
              

                        txt_ordername.Clear();
                        txt_ordernumber.Clear();
                    }
                }
            }
        }

        private void nextday_Click(object sender, EventArgs e)
        {
            string yester = label_date.Text;
            
            DataSet dataset = DBconnect.Dbgetinven(label_date.Text);


            dataGridView1.DataSource = dataset.Tables[0];
            
            if (dataGridView1.RowCount>1)
            {
                //판매처리     
                DBconnect.DbrecordSell(label_date.Text);

                //+1일해주기
                DateTime nextday = DateTime.ParseExact(label_date.Text, "yyyy-MM-dd", null);
                nextday = nextday.AddDays(1);
                string next_ = nextday.ToString("yyyy-MM-dd");

                label_date.Text = next_;
                //폐기,재고처리

                DBconnect.Dbdiscard_save(label_date.Text, yester);

                string sell_money = DBconnect.Dbsellmoney(yester);
                label_money.Text = (Convert.ToInt32(label_money.Text) + Convert.ToInt32(sell_money)).ToString();

                DBconnect.DbsetMoney(Convert.ToInt32(label_money.Text), yester, "판매", Convert.ToInt32(sell_money));

                MessageBox.Show("하루가 지나갔습니다.");
            }
            else
            {
                MessageBox.Show("판매할 물품이 없습니다.");
            }
        }
        //재고 갱신 view
        private void renew(object sender, EventArgs e)
        {
            DataSet dataset = DBconnect.Dbgetinven(label_date.Text);

            dataGridView1.DataSource = dataset.Tables[0];
   

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string year_p=combo_year.SelectedItem.ToString();
            string month_p=combo_month.SelectedItem.ToString();

            if(year_p!=null && month_p!=null)
            {
                DataSet dt=DBconnect.Dbgetsellinven(year_p, month_p,"판매");
                dataGridView4.DataSource = dt.Tables[0];

                int total = 0;
                foreach(DataGridViewRow row in dataGridView4.Rows)
                {
                    if (row.Cells["개수"].Value!=null && row.Cells["가격"]!=null)
                    {
                        total=total+Convert.ToInt32(row.Cells["개수"].Value) * Convert.ToInt32(row.Cells["가격"].Value);
                    }
                }

                txt_sellmoney.Text = total.ToString();

                DataSet dt_t = DBconnect.Dbgetsellinven(year_p, month_p, "폐기");
                dataGridView5.DataSource = dt_t.Tables[0];

                
                DataSet dt_b = DBconnect.Dbgetbuyinven(year_p, month_p);
                dataGridView6.DataSource = dt_b.Tables[0];


                //판매 데이터 가져오기
                DataTable datatable1 = DBconnect.Dbgetchart(year_p, month_p, "판매");

                // 폐기 데이터 가져오기
                DataTable datatable2 = DBconnect.Dbgetchart(year_p, month_p, "폐기");

                //구매 데이터
                
                DataTable datatable3 = DBconnect.Dbgetbuychart(year_p, month_p);

                // 차트 데이터 바인딩 설정
                chart1.Series.Clear();
                chart1.ChartAreas[0].AxisY.Title = "금액";

                // 판매 시리즈 추가
                chart1.Series.Add("판매");
                chart1.Series["판매"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
 
                chart1.Series["판매"].Points.DataBind(datatable1.DefaultView, "일", "금액", string.Empty);

                // 폐기 시리즈 추가
                chart1.Series.Add("폐기");
                chart1.Series["폐기"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        
                chart1.Series["폐기"].Points.DataBind(datatable2.DefaultView, "일", "금액", string.Empty);

                //구매 시리즈
                chart1.Series.Add("구매");
                chart1.Series["구매"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                chart1.Series["구매"].Points.DataBind(datatable3.DefaultView, "일", "금액", string.Empty);



            }
            else
            {
                MessageBox.Show("검색하실 날짜를 선택해주세요");
            }
        }
    }
}
