/*无脑遍历*/
#include<iostream>
using namespace std;
int main()
{
    int road[10001]={0};
    int m=0,l=0,x1,x2;
    int ans=0;
    cin>>l>>m;
    for(int i=0;i<=l;i++)
    {
        road[i]=1;
    }
    for(int i=0;i<m;i++)
    {
        cin>>x1>>x2;
        for(int j=x1;j<=x2;j++)
        {
            road[j]=0;
        }
    }
        for(int i=0;i<=l;i++)
    {
        if(road[i]==1)
        {
            ans++;
        }
    }
    cout<<ans;
    return 0;
}


