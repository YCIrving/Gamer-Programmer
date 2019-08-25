//poj1094 Sorting It All Out

#include <iostream>
#include <stdio.h>
#include <memory.h>
#include <vector>
#include <queue>


#define MAXN 26

using namespace std;


int in[MAXN], temp_in[MAXN];
vector <int> e[MAXN]; //��¼�ڵ���������Ľڵ�
queue <int> q; //�����洢���Ϊ0�Ľڵ�
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
    * ����˵����n�ڵ���
    * ����ֵ˵��������0ʱ����ʾ�����������򣬵���Ψһ��ans��¼һ����������
    ����1ʱ����ʾ����Ψһ��������ans��¼��������
    ����-1ʱ����ʾ�������������򣬼�ͼ�д��ڻ�

    * �㷨���̣�
    * ����ret��edge[]�����Ÿ������������ĵ㣬�����������
    * ��ʼʱ����ÿ�������һ�飬�ҵ����Ϊ0�ĵ㣬�ŵ�������
    * ֮��ѭ����������в��գ��Ҷ�����Ԫ�ظ�������1���򲻴���Ψһ��������
    * �Ӷ����е���һ��Ԫ�أ�����������������-1�������ȱ�Ϊ0�������
    * ����ѭ������¼����Ԫ�صĸ����������󵯳�Ԫ��С��n����˵���е�Ԫ��û����ӣ������ڻ�
    * �������Ԫ�ص���n����˵�������������򣬴�ʱ����ret���ɡ�
    */
    int ret=1; //�˴���ʼ�����ܶ�
    while(q.size()>0) q.pop(); //ֻ��������ն���
    memcpy(temp_in, in , sizeof(temp_in));
    for(int i=0; i<n; i++)
    {
        if(temp_in[i] == 0) q.push(i);
    }
    int cnt = 0;
    while(!q.empty())
    {
        if(q.size()>1)  ret = 0; // �޷�ȷ������ʱ���ܷ��أ���Ϊì�ܺͲ�ȷ���ȷ���ì�ܡ���ì��Ҫ���ж������������������ȿ���
        int cur = q.front(); //���Ϊ0�ĵ�
        q.pop(); //ע�ⵯ��
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
            if(flag) continue; //flag Ϊ1��-1ʱ��ֱ������
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
