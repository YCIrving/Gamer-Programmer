// poj 1979 Red and Black

#include <iostream>
#include <string>
#include <queue>
#include <memory.h>

#define MAXN 25

using namespace std;

struct Point
{
    int r, c;
};
char maze[MAXN][MAXN];
bool vis[MAXN][MAXN];

int go[][2] = {0,1,0,-1,-1,0,1,0};

int BFS(Point start, int w, int h)
{
    /*
    * ����ֳ������֣����ȳ�ʼ��
    * ֮�������ӣ���Ӻ󼴸���ans��vis��
    * ���ѭ���壺���ӣ�������Ԫ���ܵ�������λ�ã����Ƿ�����ӣ���Ӻ������ı�vis��ans��
    */
    memset(vis, 0, sizeof(vis));
    Point p, p_temp;
    int ans=1;
    int r,c;
    int new_r, new_c;
    queue <Point> q;

    // ��������
    q.push(start);
    vis[start.r][start.c] =1;

    while(!q.empty())
    {
        p=q.front();
        q.pop();
        r = p.r;
        c = p.c;
        for(int j=0; j<4; j++)
        {
            new_r = r+go[j][0];
            new_c = c+go[j][1];
            if(new_r >=1 && new_r <=h && new_c >=1 && new_c <=w && !vis[new_r][new_c] && maze[new_r][new_c]=='.') //ÿһ�����������ĵ�
            {
                p_temp.r = new_r;
                p_temp.c = new_c;
                q.push(p_temp); //���Ӻ󼴸ı�ans��vis
                ans ++;
                vis[new_r][new_c] =1;
            }
        }
    }
    return ans;
}

int main()
{
    int w, h;
    char c;
    Point start;
    while(cin >> w>>h)
    {
        if(w==0 && h==0) return 0;
//        getchar();
        for(int i =1; i<=h; i++)
        {
            for(int j=1; j<=w; j++)
            {
                cin>>c;
                if(c=='@')
                {
                    start.r = i;
                    start.c = j;
                }
                maze[i][j] = c;
            }
        }
        int ans = BFS(start, w, h);
        cout<<ans<<endl;
    }
}
