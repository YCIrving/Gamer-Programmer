#include<iostream>
using namespace std;
int main()
{
    int a[3001][2];
    int n,ans=0,temp=0;
    cin>>n;
    for(int i=0;i<n;i++)
    {
        cin>>a[i][0]>>a[i][1];
        temp+=a[i][0]+a[i][1]-8;//重点理解即可
        ans+=temp;
    }
    cout<<ans;
    return 0;
}
