// �����÷��ͺ�����:
//http://blog.sina.com.cn/s/blog_6a4aa98201012fhn.html
#include<iostream>
#include<list>//����
using namespace std;
bool compare(int a,int b)
{
    return a>b;
}
int main()
{
    list <int> l;
    list<int>::iterator i;//�������������
    list<int>::reverse_iterator ri;//�������������
    l.push_front(1);
    l.push_front(2);
    l.push_back(5);//ȫ��������Ϊ215
    cout<<l.front()<<l.back()<<endl;
    l.sort(compare);//��������
    cout<<l.front()<<l.back()<<endl;
    for(i=l.begin();i!=l.end();i++)
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    for(ri=l.rbegin();ri!=l.rend();ri++)//�������
    {
        cout<<*ri<<' ';
    }
    cout<<endl;
    l.reverse();//��תl�е�Ԫ��
    for(i=l.begin();i!=l.end();i++)
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    l.remove(1);//ɾ����һ��Ԫ��
        for(i=l.begin();i!=l.end();i++)
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    return 0;
}
