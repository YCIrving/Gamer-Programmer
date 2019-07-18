/*
首先month数组定义是一个亮点；
其次，进行了模7运算之后，星期日的天数会被存在ans[0]中，所以这个需要注意一下；
最后说一下本题的思路，首先得到一个年份之后，需要知道它1月1日是星期几，
    所以用week变量来存，计算方法就是上一年的week值+上一年的天数（这里要区分闰年和平年），
            之后模7；
    然后根据week的值计算每个月第一天是星期几，方法类似，加上上个月的天数模7；
    最后判断这个月13号是周几即可。
*/

#include<iostream>
#include<string>
using namespace std;
bool judgeYear(int year)
{
    if(year%400==0||(year%4==0&&year%100!=0))
        return true;
    else
        return false;
}
int main()
{
    int monthR[13]={366,31,29,31,30,31,30,31,31,30,31,30,31};
    int monthP[13]={365,31,28,31,30,31,30,31,31,30,31,30,31};
    int ans[7]={0};
    int week=1,week_temp,year,month;
    int n;
    cin>>n;
    for(year=1900;year<1900+n;year++)
    {
        week_temp=week;
        if(judgeYear(year))
        {
            for(month=1;month<=12;month++)
            {
                ans[(week_temp+5)%7]++;
                week_temp=(week_temp+monthR[month])%7;
            }
            week=(week+monthR[0])%7;
        }
        else
        {
            for(month=1;month<=12;month++)
            {
                ans[(week_temp+5)%7]++;
                week_temp=(week_temp+monthP[month])%7;
            }
            week=(week+monthP[0])%7;
        }
    }
    cout<<ans[6]<<' '<<ans[0]<<' ';
    for(int i=1;i<6;i++)
        cout<<ans[i]<<' ';
    return 0;
}
