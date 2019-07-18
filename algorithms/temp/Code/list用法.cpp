// 更多用法和函数见:
//http://blog.sina.com.cn/s/blog_6a4aa98201012fhn.html
#include<iostream>
#include<list>//链表
using namespace std;
bool compare(int a,int b)
{
    return a>b;
}
int main()
{
    list <int> l;
    list<int>::iterator i;//声明正向迭代器
    list<int>::reverse_iterator ri;//声明反向迭代器
    l.push_front(1);
    l.push_front(2);
    l.push_back(5);//全部插入完为215
    cout<<l.front()<<l.back()<<endl;
    l.sort(compare);//降序排列
    cout<<l.front()<<l.back()<<endl;
    for(i=l.begin();i!=l.end();i++)
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    for(ri=l.rbegin();ri!=l.rend();ri++)//反向输出
    {
        cout<<*ri<<' ';
    }
    cout<<endl;
    l.reverse();//反转l中的元素
    for(i=l.begin();i!=l.end();i++)
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    l.remove(1);//删除第一个元素
        for(i=l.begin();i!=l.end();i++)
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    return 0;
}
