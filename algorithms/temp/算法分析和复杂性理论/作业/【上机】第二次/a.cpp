//OneNote STL->动态中位数

#include <queue>
#include <iostream>
#include <vector>
#include <stdio.h>
using namespace std;

void my_insert(priority_queue<int> &q_l, priority_queue < int ,vector<int>, greater<int> > &q_r, int num)
{
    int size_l=q_l.size(), size_r=q_r.size();
    int top_l, top_r;
    if (size_l == 0 && size_r == 0)
    {
        q_l.push(num);
        return ;
    }
    else if(size_l == 0)
    {
        top_r = q_r.top();
        if (num <= top_r)
        {
            q_l.push(num);
        }
        else
        {
            q_l.push(q_r.top());
            q_r.pop();
            q_r.push(num);
            return;
        }
    }
    else if (size_r == 0)
    {
        top_l = q_l.top();
        if (num >= top_l)
        {
            q_r.push(num);
        }
        else
        {
            q_r.push(q_l.top());
            q_l.pop();
            q_l.push(num);
            return;
        }
    }
    else
    {
        top_l = q_l.top(), top_r = q_r.top();
        int flag = 0;
        if(num <= top_l)
        {
            q_l.push(num);
            size_l++;
        }
        else if (num>= top_r)
        {
            q_r.push(num);
            size_r++;
        }
        else
        {
            if(size_l<=size_r)
            {
                q_l.push(num);
            }
            else
            {
                q_r.push(num);
            }
            flag = 1;
        }
        if (flag == 0)
        {
            if(size_l-size_r > 1)
            {
                q_r.push(q_l.top());
                q_l.pop();
            }
            if (size_r - size_l >1)
            {
                q_l.push(q_r.top());
                q_r.pop();
            }
        }
    }
}

void my_query(priority_queue <int> &q_l, priority_queue < int ,vector<int>, greater<int> > &q_r)
{
    int size_l=q_l.size(), size_r=q_r.size();
    if(size_l>=size_r)
    {
        // 使用cout超时
//        cout<<q_l.top()<<'\n';
        printf("%d\n",q_l.top());
    }
    else
    {
//        cout<<q_r.top()<<'\n';
        printf("%d\n",q_r.top());
    }
}

void my_delete(priority_queue<int> &q_l, priority_queue < int ,vector<int>, greater<int> > &q_r)
{
    int size_l=q_l.size(), size_r=q_r.size();
    if(size_l>=size_r)
    {
        q_l.pop();

    }
    else
    {
        q_r.pop();
    }
}

int main()
{
    std::ios::sync_with_stdio(false);
    int n;
    cin>>n;
    for (int k=0;k<n;k++)
    {
        priority_queue <int ,vector<int>, less<int> > q_l;
        // priority_queue <int> q_l;
        // 默认大顶堆
        priority_queue <int ,vector<int>, greater<int> > q_r;
        // 使用greater， 小顶堆
        int m;
        char op;
        int num;
        cin>>m;
        for (int i=0;i<m;i++)
        {
            cin>>op;
            switch (op)
            {
                case 'I':
                    cin>> num;
                    my_insert(q_l, q_r, num);
                    break;
                case 'Q':
                    my_query(q_l,q_r);
                    break;
                case 'D':
                    my_delete(q_l,q_r);
            }

        }
    }
    return 0;
}
