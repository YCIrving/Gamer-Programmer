#include <iostream>
#include <memory.h>

#define MAXN 10

using namespace std;

char maze[MAXN][MAXN];
bool vis[MAXN];

int n, k;
int ans;

void DFS(int row, int num)
{
    if(num == k) { ans ++; return;}
    if(row >n) return;
    for(int j=1; j<=n; j++) //��ÿ�н��б���
    {
        if(!vis[j] && maze[row][j] == '#')
        {
            vis[j] = true;
            DFS(row+1, num+1);
            vis[j] = false;
        }
    }
    DFS(row+1, num); //ע���ⲽ����ʡ������Ϊ��ǰ��Ҳ�п��ܲ��ڷ�����
}
int main()
{
    while(cin >> n>>k)
    {
        if(n == -1 && k == -1) return 0;
        for(int i=1; i <= n; i++)
        {
            cin>>maze[i]+1;
        }
        ans = 0;
        memset(vis, false, sizeof(vis));
        DFS(1, 0);
        cout<<ans<<endl;
//        for(int i=1; i<=n; i++)
//        {
//            for(int j=1; j<=n; j++)
//            {
//                cout<<maze[i][j];
//            }
//            cout<<endl;
//        }
    }
    return 0;

}
