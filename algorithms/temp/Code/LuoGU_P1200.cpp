/*
用scanf一次性读取：
scanf("%s\n%s",s1,s2);
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string s1,s2;
    int a=1,b=1;
    cin>>s1>>s2;
    for(int i=0;i<=s1.length();i++)
    {
        a*=(s1[i]-'A'+1);
    }
    for(int i=0;i<=s2.length();i++)
    {
        b*=(s2[i]-'A'+1);
    }
    if(a%47==b%47)
        cout<<"GO";
    else
        cout<<"STAY";
    return 0;
}
