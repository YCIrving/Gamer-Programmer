/*据说是贪心*/

#include<iostream>
using namespace std;
int getmin(int a[][3],int n)
{
    int i=0;
    int min;
    int cnt;
    for(;i<n;i++)
    {
        if(a[i][2]==0)
        {
            min=a[i][1];
            cnt=i;
            break;
        }
    }
    if(i==n)
        return -1;
    for(int j=i;j<n;j++)
    {
        if(a[j][2]==0&&a[j][1]<min)
        {
            min=a[j][1];
            cnt=j;
        }
    }
    //cout<<cnt<<endl;
    return cnt;
}
int main()
{
    int n,s;
    int a,b;
    int ans=0;
    int array[5000][3];
    cin>>n>>s>>a>>b;
    for(int i=0;i<n;i++)
    {
        cin>>array[i][0]>>array[i][1];
        array[i][2]=0;//0表示没有摘过
    }
    while(1)
    {
        int i=getmin(array,n);
        if(i==-1)
            break;
        if(a+b>=array[i][0])
        {
            if((s-array[i][1])>=0)
            {
                s-=array[i][1];
                ans++;
        //cout<<i+1<<endl;
                //cout<<i<<endl;
            }
            else
                break;
        }
        array[i][2]=1;
    }
    cout<<ans;
    return 0;
}
