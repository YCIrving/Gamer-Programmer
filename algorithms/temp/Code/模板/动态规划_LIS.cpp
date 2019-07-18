// ���� �ϳ�����

/*
* ����������������С���ÿ���������ȳ�ʼ��dp[i] =1��
* ֮������ҵ�ǰ�����С����j���Ƚ�dp[i]��dp[j]+1�Ĵ�С��ȡ��󼴿ɡ�

*һ��ϸ�������������������±�Ķ�Ӧ������ans-1�Լ�n-ans
*/
#include <iostream>
#include <algorithm>

#define MAXN 110

using namespace std;

int height[MAXN];
int heightRev[MAXN];
int dp[MAXN];
int dpRev[MAXN];



int solve(int n)
{
    int ans = 0;

    // init
    for(int i=0; i<n; i++)
    {
        dp[i] = 1;
        dpRev[i] =1; //�����1����ʼ��
        for(int j=i-1; j>=0; j--)
        {
            if(height[i] > height[j])
            {
                dp[i] = max(dp[i], 1 + dp[j]); //�����2��max�����1+dp[j]��������max(1, 1+dp[j])
            }
            if(heightRev[i] > heightRev[j])
            {
                dpRev[i] = max(dpRev[i], 1 + dpRev[j]);
            }
        }
    }
    int p=0;
    for(int i=0; i<n; i++)
    {
        if(ans < dp[i] + dpRev[n-i-1])
        {
            ans = dp[i] + dpRev[n-i-1];
            p=i;
        }
    }
//    cout<<endl;
//    for(int i=0; i<n; i++)
//    {
//        cout<<dp[i]<<' ';
//    }
//    cout<<endl;
//    cout<<p<<' '<<ans-1<<endl;
//    cout<<dp[p]<<' '<<dpRev[n-p-1]<<endl;
    return ans-1; //������-1����Ȼ����ᱻ��������

}

int main()
{
    int n;
    cin>>n;
    for(int i=0; i<n; i++)
    {
        cin>>height[i];
        heightRev[n-1-i] = height[i];
    }
//    for(int i=0; i<n; i++)
//    {
//        cout<<height[i]<<' ';
//    }
//    cout<<endl;
//    for(int i=0; i<n; i++)
//    {
//        cout<<heightRev[i]<<' ';
//    }
    int ans = solve(n);
    cout<<n - ans<<endl; //�״��

}
