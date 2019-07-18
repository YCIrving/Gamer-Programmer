//OneNote ◊÷∑˚¥Æ->≈–∂œ≤Â»Î

#include <iostream>
using namespace std;

bool strCheck(string s1, string s2)
{
    int i=0, pos=0, tag=1;
    if(s1.length()>s2.length())
        return false;
    while(i<s2.length())
    {
        if(s1[i]!=s2[i])
        {
            pos=i;
            tag=0;
            break;
        }
        i++;
    }
    if(tag==1)
        return true;
    for(i=s2.length()-s1.length()+pos;i<s2.length();i++,pos++)
    {
        if(s1[pos]!=s2[i])
        {
            return false;
        }
    }
    return true;

}
int main()
{
    int n,k,i,j,tag=0;
    string M[100000];
    string D[100];
    cin>>n;
    for (i=0;i<n;i++)
    {
        cin>>M[i];
    }
    cin>>k;
    for(i=0;i<k;i++)
    {
        cin>>D[i];
    }


    for(j=0;j<k;j++)
    {
        tag=0;
        for(i=0;i<n;i++)
        {
            if(strCheck(M[i], D[j])==true)
            {
                tag=1;
                cout<<"YES"<<endl;
                break;
            }
        }
        if(tag==false)
        {
            cout<<"NO"<<endl;
        }

    }
    return 0;
}
