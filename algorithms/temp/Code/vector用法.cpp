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
    vec.push_back(5);//vectorֻ�����β������
    vector<int>::iterator i;
    vector<int>::reverse_iterator ri;
    for(i=vec.begin();i!=vec.end();i++)//�������
    {
        cout<<*i<<' ';
    }
    cout<<endl;
    for(ri=vec.rbegin();ri!=vec.rend();ri++)//�������
    {
        cout<<*ri<<' ';
    }
    cout<<endl;
    vec.insert(vec.begin(),10);//����
    vec.erase(vec.begin()+vec.size()-1);//ɾ��
    for(int j=vec.size()-1;j>=0;j--)
    {
        cout<<vec[vec.size()-1-j]<<' ';//ͨ���±����
    }
    cout<<endl;
    reverse(vec.begin(),vec.end());//ע��vector��list������sort��reverse�ϵ�����
    cout<<vec[0]<<endl;
    sort(vec.begin(),vec.end(),compare);//��������algorithmͷ�ļ�
    cout<<vec[0];
    cout<<endl;
}
