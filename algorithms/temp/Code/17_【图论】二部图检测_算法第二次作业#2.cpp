//OneNote Í¼ÂÛ->¶þ²¿Í¼¼ì²â

#include <iostream>
#include <stdio.h>
#include <vector>
#include <queue>
#include <algorithm>

using namespace std;
int graph[1000][1000];
int graph_class[1000];
void BFS(int n)
{
    int visited[n]={0};
    queue<int> visiting;
    for (int cnt=0;cnt<n;cnt++)
    {
        if(visited[cnt]==0)
        {
            visiting.push(cnt);
            visited[cnt] = 1;
            while(!visiting.empty())
            {
                int index = visiting.front();
                for(int i=0;i<n;i++)
                {
                    if(visited[i]==0 && graph[index][i]!=-1)
                    {
                        if(graph[index][i]==0)
                        {
                            graph_class[i]=graph_class[index];
                            visiting.push(i);
                            visited[i]=1;
                        }
                        else// if (graph[index][i]==1)
                        {
                            graph_class[i]=1 - graph_class[index];
                            visiting.push(i);
                            visited[i]=1;
                        }
                    }
                }
                visited[index]=2;
                visiting.pop();
            }
        }
    }
    return;
}

bool check_incons(vector <pair <pair <int, int>, int > > edge_list, int n)
{
    int s,e,r;
    for (int i=0; i<edge_list.size(); i++)
    {
        s=edge_list[i].first.first;
        e=edge_list[i].first.second;
        r=edge_list[i].second;
        if(r==0 && graph_class[s]!=graph_class[e])
        {
            return false;
        }
        else if(r==1 && graph_class[s]==graph_class[e])
        {
            return false;
        }
    }
    return true;
}
int main()
{
    std::ios::sync_with_stdio(false);
    int n,m,s,e,r;
    while(cin>>n>>m)
    {
        vector <pair <pair <int, int>, int > > edge_list;
        for(int i=0;i<n;i++)
        {
            graph_class[i]=0;
            for(int j=0;j<n;j++)
            {
                graph[i][j]=-1;
            }
        }
        for(int i=0;i<m;i++)
        {
            cin>>s>>e>>r;
            graph[s][e]=r;
            edge_list.push_back(make_pair(make_pair(s,e),r));
        }
        BFS(n);
//        for(int i=0;i<n;i++)
//        {
//            cout<<graph_class[i]<<' ';
//        }
//        cout<<endl;
        if(check_incons(edge_list, n))
        {
            printf("YES\n");
        }
        else
        {
            printf("NO\n");
        }

    }
    return 0;
}
