//�ַ������ò�������ɾ�Ĳ�+�Ӵ���Ӧ�ù�����
//���http://www.cppblog.com/lmlf001/archive/2006/04/19/5883.html

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string s1="abcdefg",s2;
    s1.insert(3,1,'c');
    cout<<s1<<endl;
    cin>>s2;
    s1+=s2;
    cout<<s1<<endl;
    s1.erase(3,1);
    cout<<s1<<endl;
    s1.replace(7,3,"abc");//ɾ����7���ַ���ʼ��3���ַ�����abc����
    cout<<s1<<endl;
    s1.replace(7,3,3,'z');//ɾ����7���ַ���ʼ��3���ַ�����3��z����
    cout<<s1<<endl;
    cout<<s1.substr(2,5)<<endl;
    cout<<s1.find('z',8)<<endl;
    cout<<s1.find("zzz",5)<<endl;
    cout<<s1.find_first_of("bzzz",0)<<endl;
    /*ע�⣬��find�������ǣ�find_first_of������ȫƥ�䣬
    ֻ�貿��ƥ�伴�ɣ���������ӣ���abcdefgzzz����bzzz������ƥ��b��1��λ�ã�
    zzz�Ͳ�ƥ����*/
    cout<<s1.find_last_of('z')<<endl;
    cout<<s1.rfind("zzz")<<endl;

    return 0;
}
