/*�о������������ѧ������ֱ�Ӹ���n�������ж�
Ŀǰ������򻹱Ƚϼ򵥣��õ��Ǳ���������*/

#include<iostream>
using namespace std;
int main()
{
    int n,x,ans=0;
    cin>>n>>x;
    for(int i=n;i>0;i--)
    {
        int j=i;
        while(j!=0)
        {
            if(j%10==x)
                ans++;
            j/=10;
        }
    }
    cout<<ans;
    return 0;
}
/*���⸽�Ͻ�����ת��Ϊ�ַ����Ĵ��룬��ʱ��*/
//#include<iostream>
//#include<sstream>//�����õ�ͷ�ļ�
//using namespace std;
//int main()
//{
//    int n=0,ans=0;
//    char x;
//    cin>>n>>x;
//    string str;
//    for(int i=1;i<=n;i++)
//    {
//        stringstream ss;//��һ��
//        ss<<i;//�ڶ���
//        str=ss.str();//������
//        for(int j=0;j<str.length();j++)
//        {
//            if(x==str[j])
//                ans++;
//        }
//    }
//    cout<<ans;
//    return 0;
//}
