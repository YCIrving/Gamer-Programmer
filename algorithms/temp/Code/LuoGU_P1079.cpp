/*
ע���Сд���ж�
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string k,C;
    int i,j;
    cin>>k>>C;
    string M=C;
    for(i=0;i<k.length();i++)
    {
        if(k[i]>='A'&&k[i]<='Z')
            k[i]+='a'-'A';
    }
    for(i=0,j=0;i<C.length();i++,j++)
    {
        if(j==k.length())
            j=0;
        M[i]=C[i]-k[j]+'a';
        //����ط����׺���
        if(C[i]>='a'&&C[i]<='z')
        {
            if(M[i]<'a')
                M[i]+=26;
        }
        else
        {
            if(M[i]<'A')
                M[i]+=26;
        }
    }
    cout<<M;
    return 0;
}
