/*
����month���鶨����һ�����㣻
��Σ�������ģ7����֮�������յ������ᱻ����ans[0]�У����������Ҫע��һ�£�
���˵һ�±����˼·�����ȵõ�һ�����֮����Ҫ֪����1��1�������ڼ���
    ������week�������棬���㷽��������һ���weekֵ+��һ�������������Ҫ���������ƽ�꣩��
            ֮��ģ7��
    Ȼ�����week��ֵ����ÿ���µ�һ�������ڼ����������ƣ������ϸ��µ�����ģ7��
    ����ж������13�����ܼ����ɡ�
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
