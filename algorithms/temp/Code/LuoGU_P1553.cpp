/*
    �������֮ǰ���ַ�ת�������棬��Ҫע��ĵط���������
    1.���������֮ǰ�ᵽ����ѧ���������˺ܶಽ�裬�������ַ���Ҳ�о����ԣ�
    ���ǿ�������������ַ�������̫��������long long�ʹ治���ˣ�
    2.���⻹�����˶�С���Ĵ�������С����������һ����������Ҫ�ڳ������ҵ�
    ��һ����Ϊ����������ڵ�λ�ã��������е�pos2����֮���ٴ�������ˡ�
*/
#include<iostream>
#include<string>
using namespace std;
void decimal(string str,int pos)
{
    int ans1=0,ans2=0;
    int pos2=0;
    for(pos2=pos+1;pos2<str.length();pos2++)//�ҵ�pos2���ص�
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
