/*
���������һ���Լ�Ū���׵��㷨�⣬��Ȼ���ڻ������ر����ף����ǶԼ򵥵��������
����������ʶ���ݹ���õĹ���Ҳ�ܺ���⡣
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
void dfs(int tag,int cnt,int sum)//���������tag�ǵ�ǰ�±꣬cnt�ǵ�ǰ�Ѿ��ۼ����ĸ�����
//sum�ǵ�ǰ��
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
