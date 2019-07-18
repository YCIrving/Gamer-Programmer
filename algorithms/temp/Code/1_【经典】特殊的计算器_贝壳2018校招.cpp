//OneNote ������Ŀ->����ļ�����
#include<iostream>
#include<queue>
#include<set>
#define MAX_QUEUE_LENGTH 10^9+1
#define MIN(a, b) ((a<b)?a:b)

using namespace std;

//�����������

int bfs1(int n, int m)
{
    //�������������ж�������
    //����1�����n��m����ֻ��ͨ��-1�ﵽm
    if (n>m)
        return n-m;
    int ans=0,cur=0,cnt=1;
    queue <int> q;
    //������Լ�¼���ʹ��Ľڵ㣬�ٶ�Ӧ�û���죬����Ŀ��m���ֵΪ10^9�������޷��򵥽���һ������������������Ŀ��
    //��ˣ����ǽ���set�����ϼ�����ʹ���Ԫ��
    set<int> visited;
    q.push(n);
    while (1)
    {
        //��Ҫ��¼����Ķ��нڵ������Ա���ȷ�жϱ��������������ans++
        while(cnt>0)
        {
            cur=q.front();
            q.pop();
            if(cur==m)
            {
                return ans;
            }
            //�����Ԫ��û�б����ʹ�
            if(visited.find(cur)==visited.end())
            {
                visited.insert(cur);
                //����2�������ǰcur�Ѿ���m�����ٶ������*2����
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

//�Ľ���bfs
//������Լ�¼���ʹ��Ľڵ㣬�ٶ�Ӧ�û���죬����Ŀ��m���ֵΪ10^9�������޷��򵥽���һ������������������Ŀ��
//��ˣ����ǽ���set�����ϼ�����ʹ���Ԫ�أ�
//ͬʱ��ʹ��������������������push����������ʡȥ���ʹ��Ľڵ㣬�ٶȸ���

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

//�����飬�����ݽϴ������£�������������ļ���ʱ����Ȼ�ܳ�������˼����û�п���ֱ�ӵó����۵ķ���
//�ܵ�֮ǰn��m��ʱ������������˼��n�ﵽm�ļ��ֲ�ͬ��ʽ��
//1.n��m��ʱ���Ѿ�˵����ֻ��ͨ��-1�ﵽm��
//2.n��mСʱ����������ִﵽm�ķ�ʽ��
//  2.1��*2֮�󣬲���-1��
//  2.2����-1��m��һ�룬��*2�������������еĻ���Ҫ-1������4->5��
//���Ը������ϼ���������ֱ�ӶԺ�������������
//���ǣ��������Ǵ�ģ������̫���ˣ���򵥵����ӣ�3 8�����ٲ���Ӧ����3�������öδ������Ϊ4
//��Ϊ�����У�n�ﵽ��ֵ��һ����m��1/2��Ҳ������1/4��1/8�ȣ�����nֵӦ�ñ任Ϊ�����Լ������������λ�㣬Ȼ��Ƚϲ�����С
int calculate_wrong(int n, int m)
{
    if (n==m)
        return 0;
    //��Ӧ��1�����
    else if(n>m)
        return n-m;
    else
    {
        int ans=0;
        int cur=n;
        int tag=m%2;//(�ж���ż��)
        int m_mid=(m+1)/2;//����1/2��λ�㣬����ż��������
        //���nû��m��һ�����ϣ�������*2
        while(cur<m_mid)
        {
            cur*=2;
            ans++;
        }
        //����m��һ�����Ϻ��ж���������µ����ٲ���
        //��һ������-1��m��1/2�㣬֮��*2��������*2����Ҫ-1
        //�ڶ�������*2��֮������-1��m
        int dis_m_mid,dis_m;
        //��Ӧ��һ�ֲ�������+1�� ��ʾ*2����+tag����ʾ��������½��е�-1����
        dis_m_mid=cur-m_mid+1+tag;
        //��Ӧ�ڶ��ֲ�����"1"��ʾ*2
        dis_m=1+cur*2-m;
        //���ض���֮�н�С�Ĳ���
        return(MIN(dis_m_mid,dis_m)+ans);

    }
}

int _calculate_mindis(int n, int m)
{
    if(n==m)
        return 0;
    //n�ȿ��ܱ�m��
    else
        return MIN(2*n-m+1,n-(m+1)/2+1+m%2);
        //n����Ϊ��m����С����
}
int calculate(int n, int m)
{
    if (n==m)
        return 0;
    else if(n>m)
        return n-m;
    else
    {
        //��Ϊm���Ϊ10^9=(10^3)^3=1024^3=(2^10)^3=2^30
        int fractile[31]={0};
        int cur = m, i=0, j=0, ans=0, cnt=0;
        while (cur>1)
        {
            fractile[i]=cur;
            cur = (cur+1)/2;
            i++;
        }
        //���һ����λ��
        fractile[i]=1;

        for (j=0;j<=i;j++)
        {
            //ע��Ⱥų���������
            if (fractile[j]>=n&&fractile[j+1]<n)
                break;
        }
        //�ҵ�������λ�㣬fractile[j]���ڵ���n��fractile[j+1]С��n

        //Ŀ�꣬���n����λ��fractile[j]����С���룬֮�����������λ��֮��ľ��룬ֱ�����մﵽm
        ans=_calculate_mindis(n,fractile[j]);
        for (cnt=j;cnt>0;cnt--)
        {
            ans+=_calculate_mindis(fractile[cnt],fractile[cnt-1]);
        }
        return ans;
    }
}

//�ս᣺����һ�㣬nһ������ͨ��-1������������ķ�λ�㣬֮���ٲ��ϳ�2������m��
//��10һ����5*2�����ģ�������12-1-1
//�о������յ�������...
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
