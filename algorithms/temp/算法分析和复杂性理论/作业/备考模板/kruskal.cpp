//POJ 1679 The Unique MST

#include <iostream>
#include <vector>
#include <memory.h>
#include <algorithm>

#define MAXN 110 //����
#define MAXM 20000 //����

using namespace std;
int F[MAXN]; //���鼯
vector <int> edge_id;

struct Edge
{
    int u, v, w;
}edge[MAXM];

int num_edge;
void addedge(int u, int v, int w)
{
    edge[num_edge].u = u;
    edge[num_edge].v = v;
    edge[num_edge++].w = w;
}

bool cmp (Edge a, Edge b)
{
    return a.w < b.w;
}

int find(int x)
{
    if(F[x] == -1)
        return x;
    else
        return F[x] = find(F[x]);
}
void kruskal_init()
{
    memset(F, -1, sizeof(F));
    edge_id.clear();
    num_edge = 0;
}
int kruskal (int n)
{
    /*
    * �Ա߽��б������ʺ�ϡ��ͼ
    * ����˵����n�ǽڵ���
    * ����ֵ��ans����С��������Ȩֵ��-1��ʾ����ͨ
    * edge_id����С�������бߵ�id��ע���id�������ġ�
    * �㷨���̣�
    * ��ÿ���ߴ�С��������
    * �õ�ÿ���ߵ���ֹ�㣬���ݲ��鼯�ж��Ƿ���һ�������У�
    * ���ǵĻ����䲢�鼯�ڵ�ϲ���֮���޸�ans��cnt����󽫱ߵ�id���뵽vector��
    * �ж�cnt�Ƿ�Ϊn-1��������ǰ�˳�
    *
    */
    sort(edge, edge+num_edge, cmp);
//    for(int i =0; i<num_edge; i++)
//    {
//        cout<<edge[i].u << "-->"<<edge[i].v<<": "<<edge[i].w<<endl;
//    }
    int cnt =0;
    int ans =0;
    for(int i=0; i<num_edge; i++)
    {
        int u =edge[i].u;
        int v =edge[i].v;
        int w =edge[i].w;
        int t1 = find(u);
        int t2 = find(v);
        if(t1!=t2)
        {
            edge_id.push_back(i); //��¼�ߵ����
            ans +=w;
            F[t1] = t2;
            cnt ++;
        }
        if(cnt == n-1) break;
    }
    if(cnt < n-1) return -1; //����ͨ
    else return ans;
}

int kruskal_b (int n, int id) //������ģ�壬�����ڱ������
{
    memset(F, -1, sizeof(F));
    int cnt =0;
    int ans =0;
    for(int i=0; i<num_edge; i++)
    {
        if( i == id) continue;
        int u =edge[i].u;
        int v =edge[i].v;
        int w =edge[i].w;
        int t1 = find(u);
        int t2 = find(v);
        if(t1!=t2)
        {
            ans +=w;
            F[t1] = t2;
            cnt ++;
        }
        if(cnt == n-1) break;
    }
    if(cnt < n-1) return -1; //����ͨ
    else return ans;
}

int main()
{
    int t;
    int n,m;
    int u, v, w;
    int ans, ans_b, id; //���У�����ֻ��һ������ͼ�����Ҳ���㣬��������
    cin>>t;
    while(t--)
    {
        cin>>n>>m;
        bool unique_tag = true; //���ں�����
        kruskal_init();
        for(int i = 0; i<m; i++)
        {
            cin >>u >> v >> w;
            addedge(u, v, w);
        }
        ans = kruskal(n); //Ĭ��һ��������С������
        for(int i = 0; i<edge_id.size(); i++)
        {
            id = edge_id[i];
            ans_b = kruskal_b(n, id);
//            cout << id <<' '<<ans_b<<endl;
            if(ans_b == ans)
            {
                unique_tag = false;
                break;
            }
        }
        if(unique_tag == true) cout<<ans<<endl;
        else cout<<"Not Unique!"<<endl;
    }
}
