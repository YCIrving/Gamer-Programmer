//OneNote 经典题目->特殊的计算器
#include<iostream>
#include<queue>
#include<set>
#define MAX_QUEUE_LENGTH 10^9+1
#define MIN(a, b) ((a<b)?a:b)

using namespace std;

//广度优先搜索

int bfs1(int n, int m)
{
    //加入两个特殊判断条件：
    //条件1：如果n比m大，则只能通过-1达到m
    if (n>m)
        return n-m;
    int ans=0,cur=0,cnt=1;
    queue <int> q;
    //如果可以记录访问过的节点，速度应该会更快，但题目中m最大值为10^9，所以无法简单建立一个布尔型数组来满足目标
    //因此，我们借助set来不断加入访问过的元素
    set<int> visited;
    q.push(n);
    while (1)
    {
        //需要记录当层的队列节点数，以便正确判断遍历当层结束，对ans++
        while(cnt>0)
        {
            cur=q.front();
            q.pop();
            if(cur==m)
            {
                return ans;
            }
            //如果该元素没有被访问过
            if(visited.find(cur)==visited.end())
            {
                visited.insert(cur);
                //条件2：如果当前cur已经比m大，则不再对其进行*2操作
                if (cur<m)
                    q.push(cur*2);
                q.push(cur-1);
            }
            cnt--;
        }
        ans++;
        cnt=q.size();
    }
    return ans;
}

//改进版bfs
//如果可以记录访问过的节点，速度应该会更快，但题目中m最大值为10^9，所以无法简单建立一个布尔型数组来满足目标
//因此，我们借助set来不断加入访问过的元素，
//同时，使用两个队列来反复互相push，这样可以省去访问过的节点，速度更快

int bfs2(int n, int m)
{
    if (n>m)
        return n-m;
    int ans=0,cur=0,tag=0;
//    bool visited_set[MAX_QUEUE_LENGTH]=false;
    queue <int> q0,q1;
    set<int> visited;
    q0.push(n);
    while (1)
    {
        if (tag==0)
        {
            while(!q0.empty())
            {
                cur=q0.front();
                q0.pop();
                if(visited.find(cur)==visited.end())
                {
                    if(cur==m)
                    {
                        return ans;
                    }
                    else
                    {
                        visited.insert(cur);
                        q1.push(cur-1);
                        if(cur<m)
                            q1.push(cur*2);
                    }
                }

            }
        }
        else
        {
            while(!q1.empty())
            {
                cur=q1.front();
                q1.pop();
                if(visited.find(cur)==visited.end())
                {
                    if(cur==m)
                    {
                        return ans;
                    }
                    else
                    {
                        visited.insert(cur);
                        q0.push(cur-1);
                        if(cur<m)
                            q0.push(cur*2);
                    }
                }

            }
        }
        ans++;
        tag=1-tag;
    }
}

//经试验，在数据较大的情况下，宽度优先搜索的计算时间仍然很长，所以思考有没有可以直接得出结论的方法
//受到之前n比m大时的启发，我们思考n达到m的几种不同方式：
//1.n比m大时，已经说过，只能通过-1达到m；
//2.n比m小时，最后有两种达到m的方式：
//  2.1是*2之后，不断-1；
//  2.2是先-1到m的一半，再*2，对于奇数，有的还需要-1（比如4->5）
//所以根据以上几个条件，直接对号入座，计算结果
//但是，这个结果是错的！！想得太简单了，最简单的例子，3 8，最少步数应该是3步，而该段代码输出为4
//因为计算中，n达到的值不一定是m的1/2，也可以是1/4，1/8等，所以n值应该变换为距离自己最近的两个分位点，然后比较步数大小
int calculate_wrong(int n, int m)
{
    if (n==m)
        return 0;
    //对应第1种情况
    else if(n>m)
        return n-m;
    else
    {
        int ans=0;
        int cur=n;
        int tag=m%2;//(判断奇偶性)
        int m_mid=(m+1)/2;//计算1/2分位点，奇数偶数均适用
        //如果n没到m的一半以上，则连续*2
        while(cur<m_mid)
        {
            cur*=2;
            ans++;
        }
        //到达m的一半以上后，判断两种情况下的最少步数
        //第一种是先-1到m的1/2点，之后*2，奇数在*2后还需要-1
        //第二种是先*2，之后连续-1到m
        int dis_m_mid,dis_m;
        //对应第一种步数，“+1” 表示*2，“+tag”表示奇数情况下进行的-1操作
        dis_m_mid=cur-m_mid+1+tag;
        //对应第二种步数，"1"表示*2
        dis_m=1+cur*2-m;
        //返回二者之中较小的步数
        return(MIN(dis_m_mid,dis_m)+ans);

    }
}

int _calculate_mindis(int n, int m)
{
    if(n==m)
        return 0;
    //n比可能比m大
    else
        return MIN(2*n-m+1,n-(m+1)/2+1+m%2);
        //n到分为点m的最小距离
}
int calculate(int n, int m)
{
    if (n==m)
        return 0;
    else if(n>m)
        return n-m;
    else
    {
        //因为m最大为10^9=(10^3)^3=1024^3=(2^10)^3=2^30
        int fractile[31]={0};
        int cur = m, i=0, j=0, ans=0, cnt=0;
        while (cur>1)
        {
            fractile[i]=cur;
            cur = (cur+1)/2;
            i++;
        }
        //最后一个分位点
        fractile[i]=1;

        for (j=0;j<=i;j++)
        {
            //注意等号成立的条件
            if (fractile[j]>=n&&fractile[j+1]<n)
                break;
        }
        //找到两个分位点，fractile[j]大于等于n，fractile[j+1]小于n

        //目标，求出n到分位点fractile[j]的最小距离，之后求出各个分位点之间的距离，直到最终达到m
        ans=_calculate_mindis(n,fractile[j]);
        for (cnt=j;cnt>0;cnt--)
        {
            ans+=_calculate_mindis(fractile[cnt],fractile[cnt-1]);
        }
        return ans;
    }
}

//终结：明白一点，n一定是先通过-1操作到达最近的分位点，之后再不断乘2，到达m；
//即10一定是5*2得来的，而不是12-1-1
//感觉智商收到了侮辱...
int calculate_better(int n, int m)
{
    if (n>m)
        return n-m;
    int ans=0;
    while(n<m)
    {
        ans=ans+1+m%2;
        m=(m+1)/2;
    }
    return ans+n-m;
}
int main()
{
    int n, m;
    int cur, ans1, ans2, ans3, ans4, ans5;
    cin>>n>>m;
//    ans1=bfs1(n,m);
//    ans2=bfs2(n,m);
    ans3=calculate_wrong(n,m);
    ans4=calculate(n,m);
    ans5=calculate_better(n,m);
    cout<<ans1<<endl<<ans2<<endl<<ans3<<endl<<ans4<<endl<<ans5;
}
