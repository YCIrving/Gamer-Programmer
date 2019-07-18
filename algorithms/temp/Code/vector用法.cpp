#include<iostream>
#include<vector>
#include<algorithm>
using namespace std;
bool compare(int a,int b)
{
    return a>b;
}
int main()
{
    vector<int> vec;
    vec.push_back(2);
    vec.push_back(1);
    vec.push_back(5);//vector只允许从尾部插入
    vector<int>::iterator i;
    vector<int>::reverse_iterator ri;
    for(i=vec.begin();i!=vec.end();i++)//正序输出
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    for(ri=vec.rbegin();ri!=vec.rend();ri++)//逆序输出
    {
        cout<<*ri<<' ';
    }
    cout<<endl;
    vec.insert(vec.begin(),10);//插入
    vec.erase(vec.begin()+vec.size()-1);//删除
    for(int j=vec.size()-1;j>=0;j--)
    {
        cout<<vec[vec.size()-1-j]<<' ';//通过下标访问
    }
    cout<<endl;
    reverse(vec.begin(),vec.end());//注意vector和list两者在sort和reverse上的区别
    cout<<vec[0]<<endl;
    sort(vec.begin(),vec.end(),compare);//必须引入algorithm头文件
    cout<<vec[0];
    cout<<endl;
}
