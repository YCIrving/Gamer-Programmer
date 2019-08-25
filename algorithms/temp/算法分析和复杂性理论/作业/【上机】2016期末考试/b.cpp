#include <iostream>
#include <queue>
#include <stdio.h>
#include <string>

#define MAXN 35

using namespace std;

char maze[MAXN][MAXN][MAXN];
bool vis[MAXN][MAXN][MAXN]; //init
int minutes[MAXN][MAXN][MAXN];

int l,r,c;
int sl,sr,sc,el,er,ec;
int go[6][3]={0,0,1, 0,0,-1, 0,1,0, 0,-1,0, 1,0,0, -1,0,0};
string temp;

struct Point
{
    int k, i, j;
};

int BFS()
{
    int ans =-1;
    Point temp,temp2;
    queue<Point> q;
    for(int k=0; k<l; k++)
    {
        for(int i=0; i<r; i++)
        {
            for(int j=0; j<c; j++)
            {
                vis[k][i][j] =false;
                minutes[k][i][j] = -1;
            }
        }
    }
    vis[sl][sr][sc] = true;
    minutes[sl][sr][sc] = 0;
    temp.k=sl; temp.i=sr; temp.j = sc;
    q.push(temp);
    while(!q.empty())
    {
        temp = q.front();
        q.pop();
        for(int cnt =0; cnt <6; cnt++)
        {
            int k_now = temp.k + go[cnt][0];
            int i_now = temp.i + go[cnt][1];
            int j_now = temp.j + go[cnt][2];
            if(!vis[k_now][i_now][j_now] && k_now>=0 && k_now<l && i_now>=0 && i_now<r && j_now>=0 && j_now<c && maze[k_now][i_now][j_now] != '#')
            {
//                cout <<k_now<< ' ' << i_now<< ' ' <<j_now<<endl;
                temp2.k =k_now;
                temp2.i =i_now;
                temp2.j =j_now;
                q.push(temp2);
                vis[k_now][i_now][j_now] = true;
                minutes[k_now][i_now][j_now] = minutes[temp.k][temp.i][temp.j] +1;
            }

        }
    }
    ans = minutes[el][er][ec];
    return ans;
}
int main()
{
    while(scanf("%d%d%d", &l,&r,&c))
    {
        if(l==0 && r==0 && c==0) return 0;
        getchar();
        for(int k=0; k<l; k++)
        {
            for(int i=0; i<r; i++)
            {
                for(int j=0; j<c; j++)
                {
                    scanf("%c", &maze[k][i][j]);
                    if(maze[k][i][j] == 'S') {sl = k; sr = i; sc = j;}
                    if(maze[k][i][j] == 'E') {el = k; er = i; ec = j;}
                }
                getchar();
            }
            getline(cin, temp);
        }
//        for(int k=0; k<l; k++)
//        {
//            for(int i=0; i<r; i++)
//            {
//                for(int j=0; j<c; j++)
//                {
//                    cout<<maze[k][i][j];
//                }
//                cout<<endl;
//            }
//            cout<<endl;
//        }
//        cout<<sl<<' '<<sr<<' '<<sc<<endl;
//        cout<<el<<' '<<er<<' '<<ec<<endl;

        int ans =BFS();
        if(ans == -1) cout<<"Trapped!"<<endl;
        else cout<<"Escaped in "<<ans<<" minute(s)."<<endl;
    }
}

