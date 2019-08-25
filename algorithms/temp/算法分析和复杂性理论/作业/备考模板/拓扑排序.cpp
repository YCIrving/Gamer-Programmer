//poj1094 Sorting It All Out

#include <iostream>
#include <stdio.h>
#include <memory.h>
#include <vector>
#include <queue>


#define MAXN 26

using namespace std;


int in[MAXN], temp_in[MAXN];
vector <int> e[MAXN]; //记录节点出边所连的节点
queue <int> q; //用来存储入度为0的节点
char ans[MAXN];

void init(int n)
{
    for(int i =0; i<n; i++)
    {
        in[i] = 0;
        e[i].clear();
    }
}

int topSort(int n)
{
    /*
    * 参数说明：n节点数
    * 返回值说明：返回0时，表示存在拓扑排序，但不唯一，ans记录一种拓扑排序
    返回1时，表示存在唯一拓扑排序，ans记录拓扑排序
    返回-1时，表示不存在拓扑排序，即图中存在环

    * 算法流程：
    * 定义ret，edge[]数组存放各个顶点相连的点，用来减少入度
    * 开始时，将每个点遍历一遍，找到入度为0的点，放到队列中
    * 之后循环，如果队列不空，且队列中元素个数大于1，则不存在唯一拓扑排序
    * 从队列中弹出一个元素，将其所连顶点的入度-1，如果入度变为0，则入队
    * 不断循环并记录弹出元素的个数，如果最后弹出元素小于n，则说明有的元素没有入队，即存在环
    * 如果弹出元素等于n，则说明存在拓扑排序，此时返回ret即可。
    */
    int ret=1; //此处初始化不能丢
    while(q.size()>0) q.pop(); //只能这样清空队列
    memcpy(temp_in, in , sizeof(temp_in));
    for(int i=0; i<n; i++)
    {
        if(temp_in[i] == 0) q.push(i);
    }
    int cnt = 0;
    while(!q.empty())
    {
        if(q.size()>1)  ret = 0; // 无法确定，此时不能返回，因为矛盾和不确定先返回矛盾。即矛盾要比有多种拓扑排序的情况优先考虑
        int cur = q.front(); //入度为0的点
        q.pop(); //注意弹出
        ans[cnt++] = cur+'A';
        for(int i=0; i<e[cur].size(); i++)
        {
            int j =e[cur][i];
            temp_in[j] -- ;
            if(temp_in[j] == 0) q.push(j);
        }
    }
    if(cnt < n) return -1;
    ans[cnt] = '\0';
    return ret;
}

int main()
{
    char a, b, op;
    int flag = 0;
    int n, m;
    while(scanf("%d%d", &n, &m))
    {
        if(n == 0 && m ==0) return 0;
        init(n);
        flag = 0;
        for(int i=0; i<m; i++)
        {
            getchar();
            scanf("%c<%c", &a, &b);
            if(flag) continue; //flag 为1或-1时，直接跳过
            a -='A';
            b -='A';
            e[a].push_back(b);
            in[b]++;
            flag = topSort(n);
            if(flag == 1)
                printf("Sorted sequence determined after %d relations: %s.\n", i + 1, ans);
            if(flag == -1)
                printf("Inconsistency found after %d relations.\n", i + 1);
        }
        if(flag == 0) printf("Sorted sequence cannot be determined.\n");
    }


}
