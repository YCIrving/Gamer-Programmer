//poj 1125

#include <iostream>
#include <algorithm>
#define MAXN 200
#define INF 100000000

using namespace std;

bool valid[MAXN][MAXN];
int cost[MAXN][MAXN];

void floyd_init()
{
    for(int i=0; i<MAXN; i++)
    {
        for(int j=0; j<MAXN; j++)
        {
            if(i == j) { valid[i][j] = true; cost[i][j] = 0;}
            else { valid[i][j] = false; cost[i][j] = INF;}
        }
    }
}
void floyd (int n)
{
    /*
    * 参数说明：n是节点数目
    */
    for(int k=0; k<n; k++)
    {
        for(int i=0; i<n; i++)
        {
            for(int j=0; j<n; j++)
            {
                if(valid[i][k] && valid [k][j] && cost[i][k] + cost[k][j] < cost [i][j])
                {
                    cost[i][j] = cost[i][k] + cost[k][j];
                    valid[i][j] = true;
                }
            }
        }
    }
}

int main()
{
    int n;
    int nn, t, tt;
    int ans, ans_id, disjoint_cnt, ans_temp;
    bool disjoint_tag;
    while(cin >> n && n)
    {
        floyd_init(); //初始化
        ans =INF;
        disjoint_cnt = 0;
        for(int i=0; i<n; i++) //建图
        {
            cin>>nn;
            for(int j=0; j<nn; j++)
            {
                cin>>t>>tt;
                cost[i][t-1] = min(tt, cost[i][t-1]);
                valid[i][t-1] = true;
            }
        }

        floyd(n); //求解

        for(int i=0; i<n; i++) //遍历距离矩阵的每一行，找到最大值，最后选择所有最大值中最小的那个
        {
            disjoint_tag = false;
            ans_temp =0;
            for(int j=0; j<n; j++)
            {
                if(valid[i][j]==true)
                {
                    if(ans_temp < cost[i][j]) ans_temp = cost[i][j]; //记录对应行最大值
                }
                else
                {
                    disjoint_tag=true;
                    break;
                }
            }
            if(disjoint_tag)
            {
                disjoint_cnt++;
                continue;
            }
            else
            {
                if(ans > ans_temp)
                {
                    ans = ans_temp;
                    ans_id = i+1;
                }
            }
        }
        if(disjoint_cnt == n) //如果有一个点与其他所有点不连通，则输出disjoint
        {
            cout<< "disjoint"<<endl;
        }
        else //这里默认如果多人有相同解，则输出编号最小的
        {
            cout<<ans_id<<' '<<ans<<endl;
        }
    }

}
