// poj 1088 ��ѩ
// ���仯����������ÿ�ζ����µ������㣬ʹ��vis�����ʾ��ǰdpֵ�Ƿ������µġ�
/*
* �㷨���̣�
* ��ʼ��vis��dp���飬dp����ȫΪ1��visȫΪfalse
* DFS�У�����һ���㣬������Ѿ������ʹ�����ֱ�ӷ���dpֵ
* ���û�б����ʣ�����Ҫ���������ܵĴ������Ľڵ㣬�õ���dpֵ��Ȼ��ȡ����һ������ȥ
*/



#include <iostream>
#include <algorithm>
#include <memory.h>

#define MAXN 110

using namespace std;

int maze[MAXN][MAXN];
int dp[MAXN][MAXN];
int vis[MAXN][MAXN];
int go[][2] = {0,1, 0,-1, 1,0, -1,0};
int r, c;

int DFS(int pos_r, int pos_c) //�����1��DFS�㷨��Ҫ�з���ֵ
{
    if(vis[pos_r][pos_c]) return dp[pos_r][pos_c];
    int new_r, new_c;
    for(int i=0; i<4; i++)
    {
        new_r = pos_r + go[i][0];
        new_c = pos_c + go[i][1];
        if(new_r>=0 && new_r <r && new_c >=0 && new_c <c && maze[new_r][new_c] > maze[pos_r][pos_c] )
        {
            dp[pos_r][pos_c] = max(dp[pos_r][pos_c], DFS(new_r, new_c) +1); //�����2��������DFS(new_r, new_c) ������dp[new_r][new_c]
        }
    }
    vis[pos_r][pos_c] = true; //�����3���޸�visֵ
    return dp[pos_r][pos_c]; //�����4���������յ�dpֵ
}

int main()
{
    cin>>r>>c;
    for(int i=0; i<r; i++)
    {
        for(int j=0; j<c; j++)
        {
            cin>>maze[i][j];
            dp[i][j] =1; //memset ֻ�ܸ�ֵ0��-1��������ʹ�ñ�������ֵ
        }
    }
    memset(vis, 0, sizeof(vis));
    int ans =0;
    for(int i=0; i<r; i++)
    {
        for(int j=0; j<c; j++)
        {
            ans = max(ans, DFS(i, j)); //����Ҫ��ÿ�������һ��DFS���������ܱ�֤����ͼ��dp����ȫ�����������
                                       //��������һ�£������һ���������ľ���ȫ����С�㣬����ֻ�ܵõ��Լ���dpֵ
        }
    }
    cout<<ans<<endl;
}
