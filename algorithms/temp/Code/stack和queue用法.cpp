//http://www.cnblogs.com/mfryf/archive/2012/08/09/2629992.html
//�����ܽ�úܺ�

#include<iostream>
#include<stack>
#include<queue>
using namespace std;
struct T
{
    char a[10];
    bool b;
    int c;
};
bool operator <(const T &t1,T t2)
{
    return t1.c<t2.c;//�������ȶ��У���������
}
int main()
{
    //ջ
    stack<int> sta;
    sta.push(10);
    sta.push(5);
    sta.push(1);
    cout<<sta.top()<<endl;
    sta.pop();//��Ҫע��ľ�������pop������������Ҫ������Ҳû�з���ֵ;
    cout<<sta.top()<<endl;

    queue<int> que;
    que.push(10);
    que.push(5);
    que.push(1);
    cout<<que.front()<<endl;
    cout<<que.back()<<endl;
    que.pop();
    cout<<que.front()<<endl;
     //���ȶ���
    priority_queue<T> p_que;
    T t1={"gyc",1,5};
    T t2={"YCIrving",1,10};
    T t3={"kobe",1,2};
    p_que.push(t1);
    p_que.push(t2);
    p_que.push(t3);
    cout<<p_que.top().a<<' '<<p_que.top().b<<' '<<p_que.top().c<<' '<<endl;
    p_que.pop();
    cout<<p_que.top().a<<' '<<p_que.top().b<<' '<<p_que.top().c<<' '<<endl;
    return 0;
}
