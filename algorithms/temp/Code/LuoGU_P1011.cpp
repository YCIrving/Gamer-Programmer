/*
这道题过的好蠢，就是普通模拟，自己也分析过题意，但是竟然没看出来是斐波那契数列，
僵了好久，还好数据量不大，如果大的话，肯定过不了。
自己一开始的代码注释了，写一个新的出来


总结一下规律：
1.第i站下车人数等于第i-1站的上车人数，所以中间的上下车部分不用管就好
，关键是求第n-1站的上车人数，下车不用管，需要留意第1站上车的那个a和第2站下车的b；
2.再看一下上车规律：
设fibo[]={0,1,1,2,3,5 ...}
第1站：上车1a + 0b;
第2站：上车0a + 1b;
第3站：上车1a + 1b;
第4站：上车1a + 2b;
第5站：上车2a + 3b；
第6站：上车3a + 5b；
……
第n-1站：上车fibo(n-2)*a+fibo(n-1)*b
车上共有人数：a-b+fibo(n-1)*a+fibo(n)*b=m
则第x站出发时的人数为第1站的a和第x站上车的人数之和,同理也可求
*/

#include<iostream>
using namespace std;
int a,n,m,x;
int ans=0;
int b=0;
int fibo[21]={0};
int get_b()
{
    fibo[2]=1;
    for(int i=3;i<=n;i++)
    {
        fibo[i]=fibo[i-2]+fibo[i-1];
    }
    b=(m-a*(fibo[n-2]+1))/(fibo[n-1]-1);
}
int get_ans()
{
    ans=a-b+fibo[x-1]*a+fibo[x]*b;
}
int main()
{
    cin>>a>>n>>m>>x;
    if(n==x)
    {
        cout<<0;
        return 0;
    }
    get_b();
    get_ans();
    cout<<ans;
    return 0;
}
/*
#include<iostream>
using namespace std;
int a,n,m,x,ans=0;
int y=0;
int up[21]={0};
int down[21]={0};
int fac_a=1,fac_y=0;
int factor_a_up[21]={0},factor_y_up[21]={0};
int factor_a_down[21]={0},factor_y_down[21]={0};
int getFactor()
{

    factor_a_up[1]=1;
    factor_y_up[2]=1;
    factor_y_down[2]=1;
    for(int i=3;i<n;i++)
    {
        factor_a_up[i]=factor_a_up[i-2]+factor_a_up[i-1];
        factor_y_up[i]=factor_y_up[i-2]+factor_y_up[i-1];
        factor_a_down[i]=factor_a_up[i-1];
        factor_y_down[i]=factor_y_up[i-1];
        //cout<<factor_a_up[i]<<' '<<factor_y_up[i]<<' '<<factor_a_down[i]<<' '<<factor_y_down[i]<<endl;
        fac_a+=(factor_a_up[i]-factor_a_down[i]);
        fac_y+=(factor_y_up[i]-factor_y_down[i]);
    }
    factor_a_up[n]=0;
    factor_y_up[n]=0;
    return 0;
}
int solve()
{
    y=(m-fac_a*a)/fac_y;
    fac_a=fac_y=0;
    for(int i=1;i<=x;i++)
    {
        fac_a+=(factor_a_up[i]-factor_a_down[i]);
        fac_y+=(factor_y_up[i]-factor_y_down[i]);
    }
    ans=fac_a*a+fac_y*y;
}
int main()
{

    cin>>a>>n>>m>>x;
    if(x==n)
    {
        cout<<0;
        return 0;
    }
    getFactor();
    solve();
    cout<<ans;
    return 0;
}
*/
