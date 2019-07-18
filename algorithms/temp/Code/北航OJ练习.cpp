/*
问题描述：
输入：n，表示n位数
操作：每一位只有1和0两个数，而且连续的两位不允许同时出现0
输出：这个n位数有几种情况，输出情况总数%100007的结果

Hint：
将问题抽象为连续n次选择，每次选择只有两个选项0或者1，
而且最多只能连续两次选择0。我们假设当有n-1次选择时的全部选择方案
数目是ans[n-1],其中以1结尾的选择方案数目为cnt[n-1], 而第n次选择即在
上述全部选择方案的后面各增加一个选择。其中以0为结尾的选择方案数目
后只能添加选择1，而已1结尾的方案后可以增加选择0或1，故第n次选择的
总方案数目为asn[n]= (ans[n-1]-cnt[n-1])+cnt[n-1]*2,式中ans[n-1]-cnt[n-1]即代表第n-1次
选择的方案中已以0结尾的方案，cnt[n-1]为以1结尾的方案。而且cnt[n] =ans[n-1];
*/
#include<iostream>
#include<math.h>
using namespace std;

int main()
{
    int ans=2,cnt=1,temp,n;
    while(cin>>n)
    {
        ans=2;
        cnt=1;
    for(int i=1;i<n;i++)
    {
        /*实际就是对下面的两个式子的表达。
        ans[n]=ans[n-1]+cnt[n-1]
        cnt[n]=ans[n-1]
        */
        temp=ans;
        ans=ans+cnt;
        cnt=temp;
        if(ans>100007)
            ans%=100007;

    }
    cout<<ans<<endl;;
    }
}
