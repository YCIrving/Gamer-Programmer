/*
�����Ŀ���Ƽ򵥣���ʵ�кܶ�ֵ��ע��ĵ�
1.������cin.ignore()���÷������Ժ��Ե����뻺���е��Ǹ����з�;
���ﲹ��һ�£�������ΪʲôҪʹ��ignore������ԭ����getline������ӻ������ж�ȡ\n֮ǰ�������ַ���
��cin��ȡʱ�����\n֮ǰ����\n���ڻ������У���������ʹ��cin֮������getline�ͻ����һ�����С�
��Ҳ������Ҫ��ignore��ԭ����Ҫ�������������е�\n����ignore��ʵ������������(int n,char c),
��һ�������Ǻ��Եĳ��ȣ��ڶ����Ƕ������ַ�ʱignore��ֹ��cҲ�������������Ӳ���Ĭ��Ϊ(1,EOF)��
��������һ���ַ�������Ϳ�������������������ʣ�µ��Ǹ����з�����
2.��λ������n�п��ܴ���26����������λ֮ǰ��Ҫ��ģ26����������׺��� ;
3.������ascii��ֵ�������127����Ч��(ʵ�����ǻص���-128)��
Ҳ����˵���ַ��Ѿ�������������ó��淽�������ж��ˡ�
�����ڶ��ַ����в���ʱ��Ҫ���ж���asciiֵ�Ƿ�����Ч��Χ�ڣ�֮���ٽ��в�����
�����һ���������̣�
*/
//��������
//#include<iostream>
//using namespace std;
//int main()
//{
//    char c='z';
//    cout<<c<<endl;
//    c+=6;
//    cout<<c<<endl;
//    if(c>'z')//�ж�ʧЧ
//    c-=6;
//    cout<<c<<(int)c<<endl;
//    return 0;
//}
//����Դ��
#include<iostream>
#include<string>
using namespace std;
int main()
{
    int n,ascii;
    string str;
    cin>>n;
    n%=26;
    cin.ignore();
    //getline(cin,str);
    getline(cin,str);
    for(int i=0;i<str.length();i++)
    {
        if(str[i]>='a'&&str[i]<='z')
        {
        ascii=str[i];
        ascii+=n;
        if(ascii>'z')
            ascii-=26;
        str[i]=ascii;
        }
    }
    cout<<str;
    return 0;
}

