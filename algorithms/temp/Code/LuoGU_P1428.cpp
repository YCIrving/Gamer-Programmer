/*
两个数组有点浪费，直接判断完输出即可
据说用树可以优化算法
*/
#include<iostream>
using namespace std;
int main()
{
    int a[101]={0};
    int ans[101]={0};
    int n,temp;
    cin>>n;
    for(int i=0;i<n;i++)
    {
        cin>>a[i];
    }
    for(int i=0;i<n;i++)
    {
        int j=i-1;
        while(j>=0)
        {
            if(a[j]<a[i])
                ans[i]++;
            j--;
        }
    }
    for(int i=0;i<n;i++)
    {
        cout<<ans[i]<<' ';
    }
    return 0;
}
