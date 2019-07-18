#include<iostream>
#include<iomanip>
using namespace std;
int ans=0;//定义有解牌型数
int cnt=0;//定义总牌型数
double probability=0;//定义最终概率
char symbol[4]={'+','-','*','/'};//定义符号
int calculateOutput(int a,char ch,int b);//计算两个数做一次运算的结果
bool judgeGroup(int a,int b,int c,int d);//计算一组指定顺序的数是否有解
bool judge(int a,int b,int c,int d);//计算一组数是否有解（不限定顺序）

bool judge(int a,int b,int c,int d)
{
    //四个数，忽略是否存在相同的数，有4!=24种
    //其间插入三个符号，每个符号有四种情况，共4^3=64种（忽略运算顺序，默认从左向右计算）
    //所以一共需要遍历24*64=1536种情况
    int arr[4]={a,b,c,d};
    for(int i=0;i<4;i++)
    {
        for(int j=0;j<4;j++)
        {
            for(int k=0;k<4;k++)
            {
                for(int l=0;l<4;l++)
                {
                    if(i!=j&&i!=k&&i!=l&&j!=k&&j!=l&&k!=l)//保证四个数取到不同的数字
                    {
                        if(judgeGroup(arr[i],arr[j],arr[k],arr[l]))
                            return true;
                    }
                }
            }
        }
    }
    //cout<<a<<' '<<b<<' '<<c<<' '<<d<<endl;输出所有无解组合，检验用
            return false;

}
bool judgeGroup(int a,int b,int c,int d)
{
    int output=0;
    for(int i=0;i<4;i++)
    {
        for(int j=0;j<4;j++)
        {
            for(int k=0;k<4;k++)
            {
                if(i!=3||(a%b==0))//这一步保证做除法运算时能够整除
                {
                    output=calculateOutput(a,symbol[i],b);
                }
                if(j!=3||(output%c==0))
                {
                    output=calculateOutput(output,symbol[j],c);
                }
                if(k!=3||(output%d==0))
                {
                    output=calculateOutput(output,symbol[k],d);
                }
                if(output==24)
                {
                    //cout<<a<<symbol[i]<<b<<symbol[j]<<c<<symbol[k]<<d<<"=24"<<endl;
                    return true;
                }
                output=0;//重置output
            }
        }
    }
    return false;
}
int calculateOutput(int a,char ch,int b)//简单的四则运算
{
    int output=0;
    switch (ch)
    {
    case '+':
        output =a+b;
        break;
    case '-':
        output=a-b;
        break;
    case '*':
        output=a*b;
        break;
    case '/':
        output=a/b;
    }

    return output;
}
int main()
{
    int a[36];
    for(int i=0,j=1;i<36;i+=4,j++)//初始化牌组
    {
        a[i]=a[i+1]=a[i+2]=a[i+3]=j;
    }
//    int b,c,d,e;
//    cin>>b>>c>>d>>e;
//    judge(b,c,d,e);
//    for(int i=0;i<36;i++)
//        cout<<a[i]<<' ';
    for(int i=0;i<36-3;i++)
    {
        for(int j=i+1;j<36-2;j++)
        {
            for(int k=j+1;k<36-1;k++)
            {
                for(int l=k+1;l<36;l++)
                {
                    cnt++;//C(36,4)组合数
                    if(judge(a[i],a[j],a[k],a[l]))
                    ans++;
                }
            }
        }
    }
    cout<<"Find "<<ans<<" solutions "<<"in "<<cnt<<" cases."<<endl;
    probability=(double)ans/cnt;
    cout<<fixed<<setprecision(2)<<"The probability is about "<<probability<<'.'<<endl;
    //cout<<judgeCase(5,'/',2);
    return 0;

}
