//OneNote ��ѧ->��������

#include<iostream>
using namespace std;
int main()
{
    int n,i,j,ans=0;
    int a[50000]={0};
    cin>>n;
    for(i=0;i<n;i++)
    {
        cin>>a[i];
    }
    for(i=0;i<n-1;i++)
    {
        for(j=i+1;j<n;j++)
        {
            if(a[i]>a[j])
            {
                ans++;
            }
        }
    }
    cout<<ans<<endl;
}
