#include <iostream>
#include <limits>
#include <math.h>
#include <memory.h>
#include <string>

#define MAXNUM 35
#define INF 1000000000

using namespace std;
double graph[MAXNUM][MAXNUM];
bool valid[MAXNUM][MAXNUM];

string currency[MAXNUM];
int main()
{
    int n, m, cnt=1, tag=0;
    string temp1, temp2;
    int id1, id2;
    double rate;
    while(cin >>n && n )
    {
        tag = 0;
        memset(graph, INF, sizeof(graph));
        memset(valid, false, sizeof(valid));
        for(int i=0; i<n; i++)
        {
            graph[i][i]=0;
            valid[i][i]=true;
        }
        for(int i=0; i<n; i++)
        {
            cin>>currency[i];
        }
        cin>>m;
        for(int i=0; i<m; i++)
        {
            cin>>temp1>>rate>>temp2;
            rate = - log(rate);
            id1=-1, id2=-1;
            for(int j=0; j<n; j++)
            {
                if(id1 !=-1 && id2 != -1)
                {
                    break;
                }
                if(id1 == -1 && currency[j] == temp1)
                {
                    id1=j;
                }
                if(id2 == -1 && currency[j] == temp2)
                {
                    id2=j;
                }
            }
            graph[id1][id2] = rate;
            valid[id1][id2] = true;
//            cout<<id1<<' '<<id2<<':'<<rate<<endl;
        }

        for(int q=0; q<n; q++)
        {
            for(int i=0; i<n; i++)
            {
                for(int j=0; j<n; j++)
                {
                    if(valid[i][q] && valid[q][j] && graph[i][q]+graph[q][j]<graph[i][j])
                    {
                        graph[i][j]=graph[i][q]+graph[q][j];
                        valid[i][j]=true;
                    }
                }
            }
        }
        for(int i =0; i<n; i++)
        {
            if(graph[i][i]<0)
            {
                cout<<"Case "<<(cnt++)<<": Yes"<<endl;
                tag = 1;
                break;
            }
        }
        if(tag == 0)
        {
            cout<<"Case "<<(cnt++)<<": No"<<endl;
        }
        getline(cin, temp1);
    }
    return 0;
}
