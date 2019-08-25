// poj 1160 Post Office

#include <iostream>
#include <algorithm>
#include <memory.h>
#define MAXV 310
#define MAXP 310
#define INF 100000000
using namespace std;

int v, p;
int sum[MAXV][MAXP];
int dp[MAXV][MAXP];
int pos[MAXV];
int main()
{
    while(cin>>v>>p) //����û��˵�������ж��У�����ʵ�Ƕ��еģ�����ֻ��һ�λ�wa
    {
        for(int i=1; i<=v; i++) //�����1����Ŵ�1��ʼ��v���������ܴ�0��ʼ���������sum��ܸ���
        {
            cin>>pos[i];
        }

        memset(sum, 0, sizeof(sum));

        for(int i =1; i<v; i++) //�����2������sum����
        {
            for(int j=i+1; j<=v; j++)
            {
                sum[i][j] = sum[i][j-1] + pos[j] - pos[(i+j)/2]; // �ڴ�ׯi��j֮�佨��һ���ʾ���������ٿ���
                                                                 // Ϊi��j-1�Ŀ������������һ���㵽�յ�Ŀ���
            }
        }

        for(int i=1; i<=v; i++) //�����3����ʼ��dp����
        {
            dp[i][i] = 0; // i����ׯ����i���ʾ֣�����Ϊ0
            dp[i][1] =sum[1][i]; // i����ׯ����һ���ʾ֣��͵���sum[1][i]������һ����ׯ����i����ׯ�����ٿ���
                                 // ע�����ﲻ��sum[i][1]�� sum�±��dp�±����ű�������һ��Ҫ���ֿ���sumĬ���ǽ���һ���ʾ�
        }

        for(int j=2; j<=p; j++) //�����4������dp���飬�����������±��෴���ȱ����ڶ�ά����jΪ�ʾ���
        {
            for(int i=j+1; i<=v; i++) // iΪ��ׯ�� ��Ϊ�Ϸ���dp����Ҫ���һά�±������ڵ��ڵڶ�ά
                                      // ������ʱ�Ѿ����������������ֻ���Ǳȵڶ�ά������
            {
                dp[i][j] = INF;
                for(int k = j-1; k<i; k++) // �����ʾ���һ�����������п��ܵķ�λ��k��kҪ��i����ׯ�ֳ����ݣ���1��k��k+1��i
                                           // ���ʾ���jҲ���ֳ�j-1��1������1��k����Ҫ����j-1����ׯ����k����Ϊj-1
                {
                    dp[i][j] = min(dp[i][j], dp[k][j-1] + sum[k+1][i]);
                }
            }
        }
        cout<<dp[v][p]<<endl;
    }
    return 0;
}
