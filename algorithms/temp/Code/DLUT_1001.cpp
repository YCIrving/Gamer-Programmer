/*�������������һ���ַ�����ɾ�����еġ�CAMBRIDGE���ַ�*/

#include<iostream>
#include<string>
using namespace std;
bool find(char c,string s)
{
    for(int i=0;i<s.length();i++)
    {
        if(c==s[i])
            return true;
    }
    return false;
}
int main()
{
    string in,cl,out;
    cin>>in;
    out=in;
    cl="CAMBRIDGE";
    for(int i=0;i<in.length();i++)
    {
        if(find(out[i],cl))
        {
            out.erase(i,1);
            i--;
        }
    }
    cout<<out;
    return 0;
}
