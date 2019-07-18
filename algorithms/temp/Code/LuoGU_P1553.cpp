/*
    这道题是之前数字翻转的升级版，主要注意的地方有两个：
    1.本题采用了之前提到的数学方法，简化了很多步骤，但是这种方法也有局限性，
    就是可以用来处理的字符串不能太长，否则long long就存不下了；
    2.本题还引入了对小数的处理，由于小数跟整数不一样，所以需要在程序中找到
    第一个不为零的数字所在的位置（即程序中的pos2），之后再处理就行了。
*/
#include<iostream>
#include<string>
using namespace std;
void decimal(string str,int pos)
{
    int ans1=0,ans2=0;
    int pos2=0;
    for(pos2=pos+1;pos2<str.length();pos2++)//找到pos2是重点
    {
        if(str[pos2]!='0')
            break;
    }
    for(int i=pos-1;i>=0;i--)
    {
        ans1=ans1*10+(str[i]-'0');
    }
    for(int i=str.length()-1;i>=pos2;i--)
    {
            ans2=ans2*10+(str[i]-'0');
    }
    cout<<ans1<<'.'<<ans2;
}
void fraction(string str,int pos)
{
    int ans1=0,ans2=0;
    for(int i=pos-1;i>=0;i--)
    {
        ans1=ans1*10+(str[i]-'0');
    }
    for(int i=str.length()-1;i>pos;i--)
    {
            ans2=ans2*10+(str[i]-'0');
    }
    cout<<ans1<<'/'<<ans2;
}
void percentage(string str,int pos)
{
    long long ans=0;
    for(int i=pos-1;i>=0;i--)
    {
        ans=ans*10+(str[i]-'0');
    }
    cout<<ans<<'%';
}
void integer(string str)
{
    long long  ans=0;
    for(int i=str.length()-1;i>=0;i--)
    {
        ans=ans*10+(str[i]-'0');
    }
    cout<<ans;
}
int main()
{
    string str;
    int pos=0;
    getline(cin,str);
    if(str.find('.')!=-1)
    {
        pos=str.find('.');
        decimal(str,pos);
    }
    else if(str.find('/')!=-1)
    {
        pos=str.find('/');
        fraction(str,pos);
    }
    else if(str.find('%')!=-1)
    {
        pos=str.find('%');
        percentage(str,pos);
    }
    else
    {
        integer(str);
    }
    return 0;
}
