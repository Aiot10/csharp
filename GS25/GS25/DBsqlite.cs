using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace GS25
{
    public class DBsqlite : IDB
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySQLiteDB"].ConnectionString;

        public void DbAddproduct(string name, int cost, int price, int expiry)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 등록
                    var insertProduct = $"INSERT INTO product_info (p_name,p_cost,p_price,p_expiry) VALUES('{name}', {cost}, {price}, {expiry})";
                    
                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);
                    insertProductcmd.ExecuteNonQuery();
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        public void Dbdiscard_save(string today_p, string yester_p)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //폐기, 재고
                    var insertProduct = "INSERT INTO product_record (p_name,p_num,p_date,p_type,p_stock) "
                                     + "SELECT A.p_name,A.p_num-B.p_num,@today,CASE WHEN A.p_stock = @today THEN '폐기' ELSE '재고' END,A.p_stock "
                                     + "FROM product_record AS A "
                                     + "INNER JOIN product_record AS B ON A.p_name=B.p_name AND A.p_stock=B.p_stock AND B.p_type='판매' AND A.p_type='재고' AND A.p_date=@yesterday AND B.p_date=@yesterday "
                                     + "WHERE (A.p_num-B.p_num)!=0";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);


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

        public bool Dbexistinven(string today_p, string name_p)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var Get = "SELECT * FROM product_record AS A "
                                  +"INNER JOIN product_info AS B ON A.p_name=B.p_name " +
                                  "WHERE A.p_name=@name AND A.p_stock=DATE(@today, '+' || B.p_expiry || ' DAY') AND A.p_type='재고'";

                    SQLiteCommand cmd = new SQLiteCommand(Get, connection);

                    cmd.Parameters.AddWithValue("@name", name_p);
                    cmd.Parameters.AddWithValue("@today", today_p);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
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
                    return false;

                }
            }
        }

        public DataTable Dbgetbuychart(string year_p, string month_p)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var Get = "SELECT B.p_date AS '일', SUM(A.p_cost * B.p_num) AS '금액' FROM product_record AS B " +
                              "INNER JOIN product_info AS A ON A.p_name = B.p_name " +
                              "WHERE B.p_type = '재고' AND strftime('%Y', B.p_date) = CAST(@year AS INT) AND strftime('%m', B.p_date) = CAST(@month AS INT) " +
                              "AND DATE(B.p_date, '+' || A.p_expiry || ' day') = B.p_stock " +
                              "GROUP BY B.p_date";

                    SQLiteCommand cmd = new SQLiteCommand(Get, connection);
                    cmd.Parameters.AddWithValue("@year", year_p);
                    cmd.Parameters.AddWithValue("@month", month_p);
          
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
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

        public DataSet Dbgetbuyinven(string year_p, string month_p)
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = "SELECT A.p_name as '품명', B.p_cost as '원가', A.p_stock as '유통기한', B.p_price as '가격', A.p_date as '구매날짜', A.p_num as '개수' FROM product_record AS A "
                                  + "INNER JOIN product_info AS B ON A.p_name = B.p_name WHERE A.p_type = '재고' AND strftime('%Y', A.p_date) = CAST(@year AS INT) AND strftime('%m', A.p_date) = CAST(@month AS INT) " +
                                  "AND DATE(A.p_date, '+' || B.p_expiry || ' day') = A.p_stock";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);

                    insertProductcmd.Parameters.AddWithValue("@year", year_p);
                    insertProductcmd.Parameters.AddWithValue("@month", month_p);
  

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(insertProductcmd))
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

        public DataTable Dbgetchart(string year_p, string month_p, string type_p)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var Get = "SELECT B.p_date AS '일', SUM(A.p_price * B.p_num) AS '금액' FROM product_record AS B " +
                              "INNER JOIN product_info AS A ON A.p_name = B.p_name " +
                              "WHERE B.p_type = @type AND strftime('%Y', B.p_date) = CAST(@year AS INT) AND strftime('%m', B.p_date) = CAST(@month AS INT) " +
                              "GROUP BY B.p_date";

                    SQLiteCommand cmd = new SQLiteCommand(Get, connection);
                    cmd.Parameters.AddWithValue("@year", year_p);
                    cmd.Parameters.AddWithValue("@month", month_p);
                    cmd.Parameters.AddWithValue("@type", type_p);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
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

        public string DbgetDate()
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //날짜 불러오기
                    var  Getdate= @"SELECT p_date FROM product_record ORDER BY p_date DESC LIMIT 1";

                    SQLiteCommand Datecmd = new SQLiteCommand(Getdate, connection);

                    using(SQLiteDataReader reader = Datecmd.ExecuteReader())
                    {
                        if(reader.Read())
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

        public DataSet Dbgetinven(string today_p)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = "SELECT A.p_name as '품명',B.p_cost as '원가',B.p_price as '가격',A.p_stock as '유통기한',A.p_num as '개수' FROM product_record AS A "
                                       + "INNER JOIN product_info AS B ON A.p_name=B.p_name WHERE A.p_type='재고' AND A.p_date=@today";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);

                    insertProductcmd.Parameters.AddWithValue("@today", today_p);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(insertProductcmd))
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
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //금액 불러오기
                    var Getdate = @"SELECT m_cash FROM money_record ORDER BY m_idx DESC LIMIT 1";

                    SQLiteCommand Datecmd = new SQLiteCommand(Getdate, connection);

                    using (SQLiteDataReader reader = Datecmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //데이터가 있을때
                            return reader.GetInt32(0).ToString();
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

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = @"SELECT p_name as '품명',p_cost as '원가',p_price as '가격',p_expiry as '유통기한' FROM product_info";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(insertProductcmd))
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
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 불러오기
                    var insertProduct = "SELECT A.p_name as '품명', B.p_cost as '원가', A.p_stock as '유통기한', B.p_price as '가격', A.p_date as '판매날짜', A.p_num as '개수' FROM product_record AS A "
                                  + "INNER JOIN product_info AS B ON A.p_name = B.p_name WHERE A.p_type = @type AND strftime('%Y', A.p_date) = CAST(@year AS INT) AND strftime('%m', A.p_date) = CAST(@month AS INT)";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);

                    insertProductcmd.Parameters.AddWithValue("@year", year_p);
                    insertProductcmd.Parameters.AddWithValue("@month", month_p);
                    insertProductcmd.Parameters.AddWithValue("@type", type_p);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(insertProductcmd))
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

        public void DbrecordSave(string name_p, int number_p, string today_p, string type_p)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 등록
                    var insertProduct = "INSERT INTO product_record (p_name,p_num,p_date,p_type,p_stock) "
                                        + "SELECT @productname,@productnum,@today,@producttype,DATE(@today, '+' || A.p_expiry || ' DAY') "
                                        + "FROM product_info as A WHERE A.p_name=@productname";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);

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

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //랜덤 수 판매
                    var insertProduct = "INSERT INTO product_record (p_name,p_num,p_date,p_type,p_stock) "
                                        + "SELECT p_name,(abs(random()) % (p_num))+1,@today,'판매',p_stock "
                                        + "FROM product_record WHERE p_date=@today AND p_type='재고'";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);

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
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //금액 불러오기
                    var Getmoney = "SELECT SUM(A.p_num*B.p_price) FROM product_record AS A "
                                  + "INNER JOIN product_info AS B ON A.p_name=B.p_name WHERE A.p_date=@yesterday AND A.p_type='판매'";

                    SQLiteCommand moneycmd = new SQLiteCommand(Getmoney, connection);

                    moneycmd.Parameters.AddWithValue("@yesterday", yester_p);

                    using (SQLiteDataReader reader = moneycmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //데이터가 있을때
                            return reader.GetInt32(0).ToString();
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

        public void DbsetMoney(int money, string today_m, string type_m, int money_)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //상품 정보 등록
                    var insertProduct = $"INSERT INTO money_record (m_cash,m_date,m_difference,m_type) VALUES({money}, '{today_m}',{money_},'{type_m}')";

                    SQLiteCommand insertProductcmd = new SQLiteCommand(insertProduct, connection);
                    insertProductcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void DbTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 테이블 삭제
                    var dropTableSql = @"DROP TABLE IF EXISTS product_info;
                                 DROP TABLE IF EXISTS product_record;
                                 DROP TABLE IF EXISTS money_record;";
                    SQLiteCommand dropTableCmd = new SQLiteCommand(dropTableSql, connection);
                    dropTableCmd.ExecuteNonQuery();

                    // 테이블을 다시 만들기 위한 SQL 문 생성
                    var createTableSql = @"CREATE TABLE IF NOT EXISTS product_info 
                                   (p_name TEXT PRIMARY KEY, 
                                    p_cost INT,
                                    p_price INT,
                                    p_expiry INT);
                                    CREATE TABLE IF NOT EXISTS product_record
                                    (p_idx INTEGER PRIMARY KEY AUTOINCREMENT,
                                     p_name TEXT,
                                     p_num INT,
                                     p_date TEXT,
                                     p_type TEXT,
                                     p_stock TEXT);
                                    CREATE TABLE IF NOT EXISTS money_record
                                    (m_idx INTEGER PRIMARY KEY AUTOINCREMENT,
                                     m_difference INT,
                                     m_cash INT,
                                     m_type TEXT,
                                     m_date TEXT);";
                    SQLiteCommand createTableCmd = new SQLiteCommand(createTableSql, connection);
                    createTableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
