//Onenote 动态规划：股票买卖

#include <iostream>
#include <vector>
#include <algorithm>
#include <math.h>
#include <stdio.h>

int arr[100002]={0};
int f1[100002]={0};
int g1[100002]={0};
int f2[100002]={0};

using namespace std;
void calculateF1(int n)
{
    int p_min=arr[1];
    f1[1]=0;
    for(int i=2;i<=n;i++)
    {
        if(arr[i]<p_min)
        {
            p_min=arr[i];
        }
        f1[i]=arr[i]-p_min;
    }
}

void calculateG1(int n)
{
    g1[1]=0;
    for(int i=2;i<=n;i++)
    {
//        g1[i]=f1[i];
//        for(int j=i-1;j>=1;j--)
//        {
//            g1[i]=max(g1[i],f1[j]);//求最大值的好方法
//        }
        g1[i]=max(g1[i-1],f1[i]);
    }

}

void calculateF2(int n)
{
    f2[1]=0;
    for(int i=2;i<=n;i++)
    {
        f2[i]=max(g1[i-1],f2[i-1]+arr[i]-arr[i-1]);
    }
}

int getans(int n)
{
    int ans=f2[1];
    for(int i=2;i<=n;i++)
    {
        ans=max(ans,f2[i]);
    }
    return ans;
}

int main()
{
    int t,n;
    scanf("%d",&t);
    //cin>>t;
    for(int i=0;i<t;i++)
    {
        //cin>>n;
        scanf("%d",&n);
        for(int j=1;j<=n;j++)
        {
            //cin>>arr[j];
            scanf("%d",&arr[j]);
        }
        calculateF1(n);
        calculateG1(n);
        calculateF2(n);
//        for(int cnt=1;cnt<=n;cnt++)
//        {
//            cout<<f1[cnt]<<" ";
//        }
//        cout<<endl;
//        for(int cnt=1;cnt<=n;cnt++)
//        {
//            cout<<g1[cnt]<<" ";
//        }
//        cout<<endl;
//        for(int cnt=1;cnt<=n;cnt++)
//        {
//            cout<<f2[cnt]<<" ";
//        }
//        cout<<endl;
        printf("%d\n",getans(n));
        //cout<<getans(n)<<endl;
    }
    return 0;
}
