//poj 3624 Charm Bracelet

/*
* ������������򵥵�һ�֣��ȳ�ʼ��Ϊ0��֮���ÿ����Ʒ���б���
* ÿ����Ʒ�У��������ڵ��ڸ���Ʒ��������������Ҫ���ű���
* ����ת�Ʒ��̣��޸�dpֵ��dp[j] = max(dp[j], dp[j-goods[i].weight] + goods[i].value)
*/

#include <iostream>
#include <algorithm>
#include <memory.h>

#define MAXM 13000
#define MAXN 3500

using namespace std;

struct good
{
    int value;
    int weight;
}goods[MAXN];
int dp[MAXM];
int main()
{
    int n,m;
    cin>>n>>m;
    for(int i=0; i<n; i++)
    {
        cin>>goods[i].weight>>goods[i].value;
    }
    memset(dp, 0, sizeof(dp)); //�����1����ʼ��

    for(int i=0; i<n; i++) //�����2��������Ʒ
    {
        int weight_temp = goods[i].weight;
        for(int j=m; j>=weight_temp; j--) //�����3������������ڸ���Ʒ������ÿ��һ����
        {
            dp[j] = max(dp[j], dp[j-weight_temp] + goods[i].value);
        }
    }
    cout<<dp[m]<<endl;
    return 0;
}
