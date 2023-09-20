using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Xml.Linq;
using ZstdSharp.Unsafe;

namespace GS25
{
    public class DBmysql:IDB
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MysqlDB"].ConnectionString;

        public void DbAddproduct(string name, int cost, int price, int expiry)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 등록
                    var insertProduct = $"INSERT INTO product_info (p_name,p_cost,p_price,p_expiry) VALUES('{name}', {cost}, {price}, {expiry})";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);
                    insertProductcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //재고 폐기
        public void Dbdiscard_save(string today_p, string yester_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //랜덤 수 판매
                    var insertProduct = "INSERT INTO product_record (p_name,p_num,p_date,p_type,p_stock) "
                                     + "SELECT A.p_name,A.p_num-B.p_num,@today,IF(A.p_stock=@today,'폐기','재고'),A.p_stock "
                                     + "FROM product_record AS A "
                                     + "INNER JOIN product_record AS B ON A.p_name=B.p_name AND A.p_stock=B.p_stock AND B.p_type='판매' AND A.p_type='재고' AND A.p_date=@yesterday AND B.p_date=@yesterday "
                                     + "WHERE (A.p_num-B.p_num)!=0";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);


                    insertProductcmd.Parameters.AddWithValue("@today", today_p);
                    insertProductcmd.Parameters.AddWithValue("@yesterday", yester_p);



                    insertProductcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }


        public string DbgetDate()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //날짜 불러오기
                    var Getdate = @"SELECT p_date FROM product_record ORDER BY p_date DESC LIMIT 1";

                    MySqlCommand Datecmd = new MySqlCommand(Getdate, connection);

                    using (MySqlDataReader reader = Datecmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //데이터가 있을때
                            return reader.GetDateTime(0).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            //데이터가 없을때
                            return null;
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;

                }

            }
        }

        public DataSet Dbgetinven(string today_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = "SELECT A.p_name as '품명',B.p_cost as '원가',B.p_price as '가격',A.p_stock as '유통기한',A.p_num as '개수' FROM product_record AS A "
                                      + "INNER JOIN product_info AS B ON A.p_name=B.p_name WHERE A.p_type='재고' AND A.p_date=@today";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);

                    insertProductcmd.Parameters.AddWithValue("@today", today_p);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(insertProductcmd))
                    {
                        DataSet productdata = new DataSet();
                        adapter.Fill(productdata);
                        return productdata;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }

            }
        }

        public string DbgetMoney()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //금액 불러오기
                    var Getdate = @"SELECT m_cash FROM money_record ORDER BY m_idx DESC LIMIT 1";

                    MySqlCommand Datecmd = new MySqlCommand(Getdate, connection);

                    using (MySqlDataReader reader = Datecmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //데이터가 있을때
                            return reader.GetString(0);
                        }
                        else
                        {
                            //데이터가 없을때
                            return null;
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;

                }

            }
        }

        public DataSet Dbgetproduct()
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = @"SELECT p_name as '품명',p_cost as '원가',p_price as '가격',p_expiry as '유통기한' FROM product_info";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);
                    using(MySqlDataAdapter adapter = new MySqlDataAdapter(insertProductcmd))
                    {
                        DataSet productdata = new DataSet();
                        adapter.Fill(productdata);
                        return productdata;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                
            }
        }

        public DataSet Dbgetsellinven(string year_p, string month_p, string type_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = "SELECT A.p_name as '품명',B.p_cost as '원가',A.p_stock as '유통기한',B.p_price as '가격',A.p_date as '처리날짜',A.p_num as '개수' FROM product_record AS A "
                                      + "INNER JOIN product_info AS B ON A.p_name=B.p_name WHERE A.p_type=@type AND YEAR(A.p_date)=@year AND MONTH(A.p_date)=@month";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);

                    insertProductcmd.Parameters.AddWithValue("@year", year_p);
                    insertProductcmd.Parameters.AddWithValue("@month", month_p);
                    insertProductcmd.Parameters.AddWithValue("@type", type_p);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(insertProductcmd))
                    {
                        DataSet productdata = new DataSet();
                        adapter.Fill(productdata);
                        return productdata;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }

            }
        }

        public DataSet Dbgetbuyinven(string year_p, string month_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = "SELECT A.p_name as '품명',B.p_cost as '원가',A.p_stock as '유통기한',B.p_price as '가격',A.p_date as '구매날짜',A.p_num as '개수' FROM product_record AS A "
                                      + "INNER JOIN product_info AS B ON A.p_name=B.p_name WHERE A.p_type='재고' AND YEAR(A.p_date)=@year AND MONTH(A.p_date)=@month AND DATE_ADD(A.p_date, INTERVAL B.p_expiry DAY) = A.p_stock";


                    

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);

                    insertProductcmd.Parameters.AddWithValue("@year", year_p);
                    insertProductcmd.Parameters.AddWithValue("@month", month_p);
               

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(insertProductcmd))
                    {
                        DataSet productdata = new DataSet();
                        adapter.Fill(productdata);
                        return productdata;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }

            }
        }

        //발주 기록
        public void DbrecordSave(string name_p, int number_p, string today_p, string type_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 등록
                    var insertProduct = "INSERT INTO product_record (p_name,p_num,p_date,p_type,p_stock)"
                                        + "SELECT @productname,@productnum,@today,@producttype,DATE_ADD(@today,INTERVAL A.p_expiry DAY)"
                                        + "FROM product_info as A WHERE A.p_name=@productname";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);

                    insertProductcmd.Parameters.AddWithValue("@productname", name_p);
                    insertProductcmd.Parameters.AddWithValue("@productnum", number_p);
                    insertProductcmd.Parameters.AddWithValue("@today", today_p);
                    insertProductcmd.Parameters.AddWithValue("@producttype", type_p);

                    insertProductcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void DbrecordSell(string today_p)
        {
 
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //랜덤 수 판매
                    var insertProduct = "INSERT INTO product_record (p_name,p_num,p_date,p_type,p_stock) "
                                        +"SELECT p_name,FLOOR(RAND()*(p_num))+1,@today,'판매',p_stock "
                                        + "FROM product_record WHERE p_date=@today AND p_type='재고' ";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);

              
                    insertProductcmd.Parameters.AddWithValue("@today", today_p);
                  
       

                    insertProductcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        public string Dbsellmoney(string yester_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //금액 불러오기
                    var Getmoney = "SELECT SUM(A.p_num*B.p_price) FROM product_record AS A "
                                  + "INNER JOIN product_info AS B ON A.p_name=B.p_name WHERE A.p_date=@yesterday AND A.p_type='판매'";

                    MySqlCommand moneycmd = new MySqlCommand(Getmoney, connection);

                    moneycmd.Parameters.AddWithValue("@yesterday", yester_p);

                    using (MySqlDataReader reader = moneycmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //데이터가 있을때
                            return reader.GetString(0);
                        }
                        else
                        {
                            //데이터가 없을때
                            return null;
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;

                }

            }
        }

        //변동금액 저장 인덱스,차액,현재금액,타입,날짜.
        //MONEY 총 금액 MONEY_ 차액
        public void DbsetMoney(int money,string today_m, string type_m,int money_)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 등록
                    var insertProduct = $"INSERT INTO money_record (m_cash,m_date,m_difference,m_type) VALUES({money}, '{today_m}',{money_},'{type_m}')";

                    MySqlCommand insertProductcmd = new MySqlCommand(insertProduct, connection);
                    insertProductcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public bool Dbexistinven(string today_p, string name_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                 
                    var Get = "SELECT * FROM product_record AS A "
                                  + "INNER JOIN product_info AS B ON A.p_name=B.p_name " +
                                  "WHERE A.p_name=@name AND A.p_stock=DATE_ADD(@today,interval B.p_expiry DAY) AND A.p_type='재고'";

                    MySqlCommand cmd = new MySqlCommand(Get, connection);

                    cmd.Parameters.AddWithValue("@name", name_p);
                    cmd.Parameters.AddWithValue("@today", today_p);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //데이터가 있을때
                            return true;
                        }
                        else
                        {
                            //데이터가 없을때
                            return false;
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return true;

                }

            }
        }
        public void DbTable()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // 테이블 삭제
                    var dropTableSql = @"DROP TABLE IF EXISTS product_info;
                                         DROP TABLE IF EXISTS product_record;
                                         DROP TABLE IF EXISTS money_record;";
                    MySqlCommand dropTableCmd = new MySqlCommand(dropTableSql, connection);
                    dropTableCmd.ExecuteNonQuery();

         
                    //  테이블을 다시 만들기 위한 SQL 문 생성
                    var createTableSql = @"CREATE TABLE product_info 
                                           (p_name VARCHAR(20) PRIMARY KEY, 
                                            p_cost INT,
                                            p_price INT,
                                            p_expiry INT);
                                            CREATE TABLE product_record
                                            (p_idx INT AUTO_INCREMENT PRIMARY KEY,
                                             p_name VARCHAR(20),
                                             p_num INT,
                                             p_date DATE,
                                             p_type VARCHAR(20),
                                             p_stock DATE);
                                            CREATE TABLE money_record
                                            (m_idx INT AUTO_INCREMENT PRIMARY KEY,
                                             m_difference INT,
                                             m_cash INT,
                                             m_type VARCHAR(20),
                                             m_date DATE);";
                    MySqlCommand createTableCmd = new MySqlCommand(createTableSql, connection);
                    createTableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public DataTable Dbgetchart(string year_p, string month_p, string type_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var Get = "SELECT B.p_date AS '일',SUM(A.p_price*B.p_num) AS '금액' FROM product_record AS B "
                             + "INNER JOIN product_info AS A ON A.p_name=B.p_name " +
                             "WHERE B.p_type=@type AND YEAR(B.p_date)=@year AND MONTH(B.p_date)=@month " +
                             "GROUP BY B.p_date";

                    MySqlCommand cmd = new MySqlCommand(Get, connection);
                    cmd.Parameters.AddWithValue("@year", year_p);
                    cmd.Parameters.AddWithValue("@month", month_p);
                    cmd.Parameters.AddWithValue("@type", type_p);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var dt=new DataTable();
                        dt.Load(reader);
                        return dt;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;

                }

            }
        }

        public DataTable Dbgetbuychart(string year_p, string month_p)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var Get = "SELECT B.p_date AS '일',SUM(A.p_cost*B.p_num) AS '금액' FROM product_record AS B "
                             + "INNER JOIN product_info AS A ON A.p_name=B.p_name " +
                             "WHERE B.p_type='재고' AND YEAR(B.p_date)=@year AND MONTH(B.p_date)=@month " +
                             "AND DATE_ADD(B.p_date, INTERVAL A.p_expiry DAY) = B.p_stock " +
                             "GROUP BY B.p_date";

                    MySqlCommand cmd = new MySqlCommand(Get, connection);
                    cmd.Parameters.AddWithValue("@year", year_p);
                    cmd.Parameters.AddWithValue("@month", month_p);
            

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        return dt;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;

                }

            }
        }
    }
}
