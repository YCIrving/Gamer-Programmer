#include<iostream>
#define N 1000000007

using namespace std;

long long int dp[2010] = {0};
int main()
{
    int n;
    long long int ans = 0;
    cin>>n;
    if(n==0 || n%2==1)
    {
        cout<<0<<endl;
        return 0;
    }
    dp[0] = 1;
    dp[2] = 1;
    for(int i=4; i<=n; i++)
    {
        ans = 0;
        for(int j=2; j<=n; j+=2)
        {
            //cout<<ans<<endl;
            ans += (dp[j-2]%N*dp[i-j]%N) %N;
            ans%=N;
        }
        dp[i] = ans;
    }
    cout<<dp[n]<<endl;
    return 0;
}
