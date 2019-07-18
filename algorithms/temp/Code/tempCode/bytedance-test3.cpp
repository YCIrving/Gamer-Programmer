//马里奥跳板

#include <iostream>
#include <queue>

#define MAX_INT 10010
using namespace std;

struct Node
{
    int index;
    int distance;
    int step;
}nodes[MAX_INT];

int main()
{
    for (int i=1; i<MAX_INT; i++)
    {
        nodes[i].index = i;
        nodes[i].distance = -1;
        nodes[i].step = -1;
    }
    int n, p;
    cin>>n>>p;
    for(int i=1; i<=n; i++)
    {
        cin>>nodes[i].distance;
    }
    nodes[p].step=0;

    queue<Node> nodeQueue;
    Node nodeTemp;

    nodeQueue.push(nodes[p]);

    while(!nodeQueue.empty())
    {
        nodeTemp = nodeQueue.front();
        nodeQueue.pop();

        // 能否跳到终点

        if(nodeTemp.distance + nodeTemp.index >= n+1)
        {
            cout<<nodeTemp.step+1;
            return 0;
        }

        // 将能达到且未访问过的点加入队列

        for(int i=1; i<=nodeTemp.distance; i++)
        {
            int curPosLeft = nodeTemp.index - i;
            int curPosRight = nodeTemp.index + i;
            if(curPosLeft >=1 && nodes[curPosLeft].distance!= 0 && nodes[curPosLeft].step == -1)
            {
                nodes[curPosLeft].step = nodeTemp.step + 1;
                nodeQueue.push(nodes[curPosLeft]);
            }

            if(curPosRight <=n && nodes[curPosRight].distance!= 0 && nodes[curPosRight].step == -1)
            {
                nodes[curPosRight].step = nodeTemp.step + 1;
                nodeQueue.push(nodes[curPosRight]);
            }
        }
    }
    cout<<-1<<endl;
    return 0;


}
