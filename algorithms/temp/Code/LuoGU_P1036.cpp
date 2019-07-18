/*
洛谷上面又一道自己弄明白的算法题，虽然现在还不是特别明白，但是对简单的深度优先
搜索有了认识，递归调用的过程也很好理解。
*/
#include<iostream>
#include<math.h>
using namespace std;
int ans=0;
int n,k;
int a[20]={0};
bool judge(int n)
{
    for(int i=2;i*i<n;i++)
    {
        if(n%i==0)
            return false;
    }
    return true;
}
void dfs(int tag,int cnt,int sum)//深度搜索，tag是当前下标，cnt是当前已经累加数的个数，
//sum是当前和
{
    if(cnt==k&&judge(sum))
        ans++;
    for(int i=tag;i<n;i++)
    {
        dfs(i+1,cnt+1,sum+a[i]);
    }
}
int main()
{
    cin>>n>>k;
    for(int i=0;i<n;i++)
    {
        cin>>a[i];
    }
    dfs(0,0,0);
    cout<<ans;
    return 0;

}
