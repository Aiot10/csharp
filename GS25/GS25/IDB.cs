using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS25
{
    internal interface IDB
    {  
        //데베 테이블생성
        void DbTable();
        //데베 상품정보 저장
        void DbAddproduct(string name,int cost,int price,int expiry);

        //데베에서 상품정보 불러오기
        DataSet Dbgetproduct();

        //데베에서 날짜 불러오기
        string DbgetDate();

        //record 테이블 발주 기록 저장 이름 갯수 ,오늘날짜 ,type = 재고,폐기일?
        void DbrecordSave(string name_p,int number_p,string today_p, string type_p);

        //record 판매 기록 저장

        void DbrecordSell(string today_p);

        //데베에서 금액 불러오기
        string DbgetMoney();

        //데베 최근금액 insert
        void DbsetMoney(int money, string today_m, string type_m, int money_);

        //폐기, 재고 처리
        void Dbdiscard_save(string today_p,string yester_p);

        //판매 금액
        string Dbsellmoney(string yester_p);


        //재고 목록 출력
        DataSet Dbgetinven(string today_p);

        //판매 목록,폐기 목록 출력
        DataSet Dbgetsellinven(string year_p,string month_p,string type_p);

        //같은날 같은상품이 구매되었는지
        bool Dbexistinven(string today_p, string name_p);

        //차트 gruop
        DataTable Dbgetchart(string year_p, string month_p, string type_p);

        //차트 BUY
        DataTable Dbgetbuychart(string year_p, string month_p);

        //구매 기록
        DataSet Dbgetbuyinven(string year_p, string month_p);



    }
}
