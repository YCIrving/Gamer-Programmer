// poj1384 ǡ��װ��+��ȫ����+��С

/*
* ��ȫ������01��������ֻ��һ����ͬ��������˳��������������ȫ��������һ������˳�򼴿�
* ǡ��װ��������01��������ȫ����������ͻ��ǡ��װ��Ҫ���ڳ�ʼ��dp����ʱ��ֻ��dp[0]��ʼ��Ϊ0
* ����������������С����Ϊ�������������
* �籾��������С�����Ծ�����Ϊ�������ʾ�����ڣ�һ���и�С��ֵ�����ܸ��¡�
* ��֮������󣬾�����Ϊ������
*/

#include <iostream>
#include <algorithm>

#define INF 1000000
#define MAXN 600
#define MAXM 11000

using namespace std;

struct good
{
    int value;
    int weight;
}goods[MAXN];

int dp[MAXM];

int main()
{
    int t;
    int n,m;
    cin>>t;
    while(t--)
    {
        cin>>n>>m;
        m=m-n;
        cin>>n;
        for(int i=0; i<n; i++)
        {
            cin>>goods[i].value>>goods[i].weight;
        }
        for(int i=1; i<=m; i++)
        {
            dp[i] =INF; //�����1����ʼ��
        }
        dp[0] = 0;

        for(int i=0; i<n; i++)
        {
            int weight_temp = goods[i].weight;
            for(int j=weight_temp; j<=m; j++) //�����2���������
            {
                dp[j] = min(dp[j], dp[j-weight_temp] + goods[i].value);
            }
        }
        if(dp[m]==INF) cout<<"This is impossible."<<endl;
        else cout<<"The minimum amount of money in the piggy-bank is "<<dp[m]<<'.'<<endl;
    }
}
